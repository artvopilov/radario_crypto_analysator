using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoAnalysator
{
    class YobitMarket : BasicCryptoMarket
    {
        public YobitMarket(string url = "https://yobit.net/api/3/", string command = "info") : base(url, command)
        {
        }

        protected override void process_response(string response)
        {
            var responseJSON = JObject.Parse(response)["pairs"].Value<JObject>();
            //string request = "";

            Console.Write("[INFO] YobitMarket is loading:  ");
            int count = 0;
            foreach (var pair in responseJSON)
            {
                ExchangePair exPair = new ExchangePair();
                exPair.pair = pair.Key.ToUpper().Replace('_', '-');
                var pairInfo = JObject.Parse(load_pair_info(pair.Key));
                exPair.purchasePrice = (decimal)pairInfo[pair.Key]["sell"];
                exPair.sellPrice = (decimal)pairInfo[pair.Key]["buy"];
                exPair.stockExchangeSeller = "Yobit";

                pairs.Add(exPair);

                count++;
                if (count % 10 == 0)
                {
                    Console.Write('>');
                }
                //request += pair.Key + '-';
            }
            //request = request.Remove(request.Length - 1, 1);
            //Console.WriteLine(load_pair_info(request));
            Console.Write('\n');
            Console.WriteLine("[INFO] YobitMarket is ready");
        }

        string load_pair_info(string name)
        {
            string response = Program.get_request(basicUrl + "ticker/" + name);
            return response;
        }
    }
}
