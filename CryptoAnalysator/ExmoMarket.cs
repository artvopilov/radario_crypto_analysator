using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoAnalysator
{
    class ExmoMarket: BasicCryptoMarket
    {
        public ExmoMarket(string url = "https://api.exmo.com/v1/", string command = "ticker") : base(url, command)
        {
        }

        protected override void process_response(string response)
        {
            var responseJSON = JObject.Parse(response);

            foreach (var pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair.Key.Replace('_', '-');
                exPair.purchasePrice = (decimal)pair.Value["sell_price"];
                exPair.sellPrice = (decimal)pair.Value["buy_price"];
                exPair.stockExchangeSeller = "Exmo";

                pairs.Add(exPair);
                check_add_usdt_pair(exPair);
            }

            create_crossRates();
            Console.WriteLine("[INFO] ExmoMarket is ready");
        }
    }
}
