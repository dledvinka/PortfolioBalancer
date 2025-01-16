using PortfolioBalancer.ConsoleApp;
using YahooFinanceApi;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

new YahooTest().Run();


// VWCE ETF symbol
//var ticker = "VWCE.DE"; // Note: ".DE" indicates it's traded on a German exchange. (EUR)
//var ticker = "XD9U.AS"; // Note: ".DE" indicates it's traded on a Amsterdam exchange. (EUR)
//var ticker = "XD9U.L"; // Note: ".DE" indicates it's traded on a London exchange. (USD)

string[] cryptoTickers = { "BTC-USD", "ETH-USD", "SOL-USD" };
string[] cryptoTickersCzk = { "BTC-CZK", "ETH-CZK", "SOL-CZK" };

string forexTicker = "USDCZK=X"; // Forex ticker for EUR/USD exchange rate

var ticker = cryptoTickers[0];
ticker = forexTicker;

try
{
    var securities = await Yahoo.Symbols(ticker).Fields(Field.Symbol, Field.RegularMarketPrice).QueryAsync();
    var security = securities[ticker];
    var price = security[Field.RegularMarketPrice];
    Console.WriteLine($"The current price of {ticker} is {price} EUR.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error fetching ETF price: {ex.Message}");
}

//using System;
//using System.Threading.Tasks;
//using YahooFinanceApi;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string etfTicker = "XD9U.AS";    // XD9U ticker on Euronext Amsterdam
//        string forexTicker = "EURUSD=X"; // Forex ticker for EUR/USD exchange rate

//        try
//        {
//            // Fetch XD9U price in EUR
//            var securities = await Yahoo.Symbols(etfTicker, forexTicker)
//                                        .Fields(Field.Symbol, Field.RegularMarketPrice)
//                                        .QueryAsync();

//            // Get XD9U price in EUR
//            var etfPriceEur = securities[etfTicker][Field.RegularMarketPrice];

//            // Get EUR/USD exchange rate
//            var eurUsdRate = securities[forexTicker][Field.RegularMarketPrice];

//            // Convert to USD
//            var etfPriceUsd = etfPriceEur * eurUsdRate;

//            Console.WriteLine($"The current price of {etfTicker} is {etfPriceEur} EUR.");
//            Console.WriteLine($"The current EUR/USD exchange rate is {eurUsdRate}.");
//            Console.WriteLine($"The price of {etfTicker} in USD is approximately {etfPriceUsd} USD.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error fetching data: {ex.Message}");
//        }
//    }
//}
