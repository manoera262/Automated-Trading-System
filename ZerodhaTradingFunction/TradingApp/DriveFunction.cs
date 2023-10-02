using KiteConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp
{
    public class DriveFunction
    {
        private readonly CosmosDbService _cosmosService;
        private readonly KiteConnectClient _kiteConnect;

        public DriveFunction()
        {
            _cosmosService = new CosmosDbService();
            _kiteConnect = new KiteConnectClient();
        }

        public async Task ExecuteTradingLogicAsync()
        {
            // Assume you have a method to get trading symbols
            string[] tradingSymbols = GetTradingSymbols();

            // Get last traded prices from Kite Connect
            var lastTradedPrices = _kiteConnect.GetLastTradedPrice(tradingSymbols);

            // Apply mean reversion business logic
            var tradeData = ApplyMeanReversionLogic(lastTradedPrices);

            // Store the trade data in Cosmos DB
            await _cosmosService.AddItemAsync(tradeData);
        }

        private string[] GetTradingSymbols()
        {
            return new[] { "NSE:RELIANCE", "NSE:TCS" }; // example symbols
        }

        private dynamic ApplyMeanReversionLogic(Dictionary<string, LTP> lastTradedPrices)
        {
            var tradeDataList = new List<dynamic>();

            foreach (var kvp in lastTradedPrices)
            {
                var symbol = kvp.Key;
                var price = kvp.Value.LastPrice; // assuming LTP object has a LastPrice property

                // Fetch historical prices from database (simplified)
                var historicalPrices = (_cosmosService.GetItemAsync(Guid.Parse(symbol)).Result);

                if (historicalPrices != null)
                {
                    var meanPrice = 1230; // assignning random value;

                    // If current price is significantly below the mean, consider it a buying opportunity
                    if (price < meanPrice * 0.95m)
                    {
                        tradeDataList.Add(new { Symbol = symbol, Action = "Buy", Price = price });
                    }
                    // If current price is significantly above the mean, consider it a selling opportunity
                    else if (price > meanPrice * 1.05m)
                    {
                        tradeDataList.Add(new { Symbol = symbol, Action = "Sell", Price = price });
                    }
                }
            }

            return tradeDataList;
        }
    }
}
