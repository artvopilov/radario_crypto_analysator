using System;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace CryptoAnalysator
{

    class Program
    {
        static void Main(string[] args)
        {

            PoloniexMarket poloniex = new PoloniexMarket();
            BittrexMarket bittrex = new BittrexMarket();
            YobitMarket yobit = new YobitMarket();
            ExmoMarket exmo = new ExmoMarket();

            Console.WriteLine("\n----- Calculating -----\n");

            BasicCryptoMarket[] marketsArray = { poloniex, bittrex, exmo };

            //Console.WriteLine(poloniex.get_pair_by_name("ETH-GAS")?.sellPrice);

            PairsListOperator CA = new PairsListOperator();
            
        
            CA.analyse_pairs(marketsArray);

            CA.show_actual_pairs();

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
