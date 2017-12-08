﻿using System;
using Newtonsoft.Json.Linq;


namespace CryptoAnalysator {
    class BittrexMarket : BasicCryptoMarket {
        public BittrexMarket(string url = "https://bittrex.com/api/v1.1/public/", string command = "getmarketsummaries",
            decimal feeTaker = (decimal)0.0025, decimal feeMaker = (decimal)0.0025) : base(url, command, feeTaker, feeMaker) {
        }

        protected override void ProcessResponse(string response) {
            var responseJSON = JObject.Parse(response)["result"];

            foreach (JObject pair in responseJSON) {
                ExchangePair exPair = new ExchangePair();
                exPair.Pair = (string)pair["MarketName"];
                exPair.PurchasePrice = (decimal)pair["Ask"] * (1 + _feeTaker);
                exPair.SellPrice = (decimal)pair["Bid"] * (1 - _feeMaker);
                exPair.StockExchangeSeller = "Bittrex";

                _pairs.Add(exPair);
                CheckAddUsdtPair(exPair);
            }

            CreateCrossRates();
            Console.WriteLine("[INFO] BittrexMarket is ready");
        }
    }
}
