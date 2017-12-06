using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CryptoAnalysator
{
    class BittrexMarket : BasicCryptoMarket
    {
        public BittrexMarket(string url = "https://bittrex.com/api/v1.1/public/", string command = "getmarketsummaries",
            decimal feeTaker = (decimal)0.0025, decimal feeMaker = (decimal)0.0025) : base(url, command, feeTaker, feeMaker)
        {
        }

        protected override void process_response(string response)
        {
            var responseJSON = JObject.Parse(response)["result"];

            foreach (JObject pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair["MarketName"];
                exPair.purchasePrice = (decimal)pair["Ask"] * (1 + feeTaker);
                exPair.sellPrice = (decimal)pair["Bid"] * (1 - feeMaker);
                exPair.stockExchangeSeller = "Bittrex";

                pairs.Add(exPair);
                check_add_usdt_pair(exPair);
            }

            create_crossRates();
            Console.WriteLine("[INFO] BittrexMarket is ready");
        }
    }
}
