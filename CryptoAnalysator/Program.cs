using System;
using System.Net;


namespace CryptoAnalysator {

    class Program {
        static void Main(string[] args) {

            PoloniexMarket poloniex = new PoloniexMarket();
            BittrexMarket bittrex = new BittrexMarket();
            // Uncomment the line under that to see how much time it will take to load info abt Yobit
            // YobitMarket yobit = new YobitMarket();
            ExmoMarket exmo = new ExmoMarket();

            Console.WriteLine("\n----- Calculating -----\n");

            BasicCryptoMarket[] marketsArray = { poloniex, bittrex, exmo };
            PairsListOperator CA = new PairsListOperator();
            CA.AnalysePairs(marketsArray);
            CA.ShowActualPairs();

            Console.ReadKey();
        }

        public static string get_request(string url) {//async
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
            using (var webClient = new WebClient()) {
                string response = webClient.DownloadString(url);
                return response;
            }
        }
    }
}
