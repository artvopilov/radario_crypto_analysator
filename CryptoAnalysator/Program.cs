using System;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace CryptoAnalysator
{
    class PairsListOperator
    {
        List<ExchangePair> actualPairs = new List<ExchangePair>();

        public void delete_all_pairs()
        {
            actualPairs.Clear();
        }

        public void add_pair(ExchangePair pair)
        {
            actualPairs.Add(pair);
        }

        public void analyse_add_pairs(PoloniexApiRequester poloniex, BittrexApiRequester bittrex)
        {

        }
    }

    class ExchangePair
    {
        public string pair;
        public string stockExchangeSeller;
        public string stockExchangeBuyer;
        public decimal purchasePrice;
        public decimal sellPrice;
    }

    class PoloniexApiRequester
    {
        static string basicURL = "https://poloniex.com/public?command=";
        List<ExchangePair> pairs = new List<ExchangePair>();

        public PoloniexApiRequester()
        {
            load_pairs();
        }

        public void load_pairs()
        {
            pairs.Clear();

            string response = Program.get_request(basicURL + "returnTicker");
            var responseJSON = JObject.Parse(response);
            foreach (var pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair.Key.Replace('_', '-');
                Console.WriteLine(exPair.pair);
                exPair.purchasePrice = (decimal)pair.Value["lowestAsk"];
                exPair.sellPrice = (decimal)pair.Value["highestBid"];

                pairs.Add(exPair);
            }
        }

        public ExchangePair get_pair_by_name(string name)
        {
            return pairs.Find(pairEx => pairEx.pair == name);
        }
    }

    class BittrexApiRequester
    {
        string basicUrl = "https://bittrex.com/api/v1.1/public/";
        List<ExchangePair> pairs = new List<ExchangePair>();

        public BittrexApiRequester()
        {
            load_pairs();
        }

        public void load_pairs()
        {
            pairs.Clear();

            string response = Program.get_request(basicUrl + "getmarketsummaries");
            var responseJSON = JObject.Parse(response)["result"];

            foreach (JObject pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = (string)pair["MarketName"];
                exPair.purchasePrice = (decimal)pair["Ask"];
                exPair.sellPrice = (decimal)pair["Bid"];

                pairs.Add(exPair);
            }
        }

        public ExchangePair get_pair_by_name(string name)
        {
            return pairs.Find(pairEx => pairEx.pair == name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            PoloniexApiRequester poloniex = new PoloniexApiRequester();
            //BittrexApiRequester bittrex = new BittrexApiRequester();

            Console.WriteLine(poloniex.get_pair_by_name("ETH-GAS"));

            Console.ReadKey();
        }

        public static string get_request(string url) //async
        {
            //using (HttpClient client = new HttpClient())
            //{

            //    using (HttpResponseMessage response = await client.GetAsync(url))
            //    {
            //        using (HttpContent content = response.Content)
            //        {
            //            string myContent = await content.ReadAsStringAsync();
            //            //var myContentJson = JObject.Parse(myContent);
            //            //var myContentJson2 = JsonConvert.DeserializeObject<dynamic>(myContent);                      
            //            //string attr = myContentJson2.ETH_GAS.id;
            //            return myContent;
            //        }
            //    }
            //}
            using (var webClient = new WebClient())
            {
                // Выполняем запрос по адресу и получаем ответ в виде строки
                string response = webClient.DownloadString(url);
                return response;
            }
        }
    }
}
