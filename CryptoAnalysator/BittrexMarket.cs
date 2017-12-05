using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CryptoAnalysator
{
    class BittrexMarket : BasicCryptoMarket
    {
        public BittrexMarket(string url = "https://bittrex.com/api/v1.1/public/", string command = "getmarketsummaries") : base(url, command)
        {
        }

        protected override void process_response(string response)
        {
            var responseJSON = JObject.Parse(response)["result"];

            foreach (JObject pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair["MarketName"];
                exPair.purchasePrice = (decimal)pair["Ask"];
                exPair.sellPrice = (decimal)pair["Bid"];
                exPair.stockExchangeSeller = "Bittrex";

                pairs.Add(exPair);
            }

            Console.WriteLine("[INFO] BittrexMarket is ready");
        }
    }
}
