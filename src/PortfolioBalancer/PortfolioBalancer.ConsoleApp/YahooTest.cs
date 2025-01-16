using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace PortfolioBalancer.ConsoleApp;

public class ETF
{
    public string Symbol { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
}

internal class YahooTest
{
    public void Run()
    {
        

            // Path to the CSV file
            var csvFilePath = "C:\\Users\\dledvinka\\Downloads\\extracted_data.csv";

            // Read the ETF data from the CSV
            List<ETF> portfolio = ReadCsv(csvFilePath);

            // Fetch exchange rates
            var exchangeRates = GetExchangeRates();

            // Calculate total value in EUR
            var totalValueInEUR = CalculateTotalValueInEUR(portfolio, exchangeRates);

            // Display the result
            Console.WriteLine($"Total Portfolio Value in EUR: {totalValueInEUR:F2}");
        }

        static List<ETF> ReadCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                    csv.Configuration.MissingFieldFound = null;
            return new List<ETF>(csv.GetRecords<ETF>());
            }
        }

        static Dictionary<string, decimal> GetExchangeRates()
        {
            var client = new RestClient("https://api.exchangerate-api.com/v4/latest/EUR");
            var request = new RestRequest("https://api.exchangerate-api.com/v4/latest/EUR", Method.Get);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var data = JObject.Parse(response.Content);
                var rates = data["rates"];

                return new Dictionary<string, decimal>
            {
                { "USD", rates["USD"].Value<decimal>() },
                { "GBP", rates["GBP"].Value<decimal>() },
                { "EUR", 1.0m } // EUR to EUR is always 1
            };
            }
            else
            {
                throw new Exception("Failed to fetch exchange rates.");
            }
        }

        static decimal CalculateTotalValueInEUR(List<ETF> portfolio, Dictionary<string, decimal> exchangeRates)
        {
            decimal totalValue = 0;

            foreach (var etf in portfolio)
            {
                if (exchangeRates.TryGetValue(etf.Currency, out var rate))
                {
                    var valueInEUR = etf.Price * etf.Quantity * rate;
                    totalValue += valueInEUR;
                }
                else
                {
                    throw new Exception($"Exchange rate for currency {etf.Currency} not found.");
                }
            }

            return totalValue;
        }
    }


