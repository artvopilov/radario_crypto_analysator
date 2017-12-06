using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CryptoAnalysator
{
    class PoloniexMarket : BasicCryptoMarket
    {
        public PoloniexMarket(string url = "https://poloniex.com/public?command=", string command = "returnTicker") : base(url, command)
        {
        }

        protected override void process_response(string response)
        {
            var responseJSON = JObject.Parse(response);

            foreach (var pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair.Key.Replace('_', '-');
                exPair.purchasePrice = (decimal)pair.Value["lowestAsk"];
                exPair.sellPrice = (decimal)pair.Value["highestBid"];
                exPair.stockExchangeSeller = "Poloniex";

                pairs.Add(exPair);
                check_add_usdt_pair(exPair);
            }

            create_crossRates();
            Console.WriteLine("[INFO] PoloniexMarket is ready");
        }
    }
}
