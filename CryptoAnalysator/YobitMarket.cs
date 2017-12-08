using System;
using Newtonsoft.Json.Linq;

namespace CryptoAnalysator {
    class YobitMarket : BasicCryptoMarket {
        public YobitMarket(string url = "https://yobit.net/api/3/", string command = "info",
             decimal feeTaker = (decimal)0.0005, decimal feeMaker = (decimal)0.0005) : base(url, command, feeTaker, feeMaker) {
        }

        protected override void ProcessResponse(string response) {
            var responseJSON = JObject.Parse(response)["pairs"].Value<JObject>();
            //string request = "";

            Console.Write("[INFO] YobitMarket is loading:  ");
            int count = 0;
            foreach (var pair in responseJSON) {
                ExchangePair exPair = new ExchangePair();
                exPair.Pair = pair.Key.ToUpper().Replace('_', '-');
                var pairInfo = JObject.Parse(LoadPairsInfo(pair.Key));
                exPair.PurchasePrice = (decimal)pairInfo[pair.Key]["sell"] * (1 + _feeTaker);
                exPair.SellPrice = (decimal)pairInfo[pair.Key]["buy"] * (1 - _feeMaker);
                exPair.StockExchangeSeller = "Yobit";

                _pairs.Add(exPair);
                CheckAddUsdtPair(exPair);

                count++;
                if (count % 10 == 0) {
                    Console.Write('>');
                }
                //request += pair.Key + '-';
            }
            //request = request.Remove(request.Length - 1, 1);
            //Console.WriteLine(load_pair_info(request));
            CreateCrossRates();
            Console.Write('\n');
            Console.WriteLine("[INFO] YobitMarket is ready");
        }

        string LoadPairsInfo(string name) {
            string response = GetResponse(_basicUrl + "ticker/" + name);
            return response;
        }
    }
}
