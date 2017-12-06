using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoAnalysator
{
    abstract class BasicCryptoMarket
    {
        protected string basicUrl;
        protected List<ExchangePair> pairs;
        protected List<ExchangePair> usdtPairs;
        protected List<ExchangePair> crossRates;

        public BasicCryptoMarket(string url, string command)
        {
            pairs = new List<ExchangePair>();
            usdtPairs = new List<ExchangePair>();
            crossRates = new List<ExchangePair>();
            basicUrl = url;
            load_pairs(command);
        }
        
        // Requests crypto markets
        public void load_pairs(string command)
        {
            pairs.Clear();

            string response = Program.get_request(basicUrl + command);

            process_response(response);
        }

        protected abstract void process_response(string response);

        protected void check_add_usdt_pair(ExchangePair pair)
        {
            char[] signsSplit = { '-' };
            string[] splitedPair = pair.pair.Split(signsSplit);

            if (splitedPair[0].ToUpper() == "USDT" || splitedPair[1].ToUpper() == "USDT")
            {
                usdtPairs.Add(pair);
            }
        }

        protected void create_crossRates()
        {
            for (int i = 0; i < usdtPairs.Count - 1; i++)
            {
                for (int j = i + 1; j < usdtPairs.Count; j++)
                {
                    ExchangePair pair1 = usdtPairs[i];
                    ExchangePair pair2 = usdtPairs[j];

                    ExchangePair crossRatePair = new ExchangePair();
                    char[] signsSplit = { '-' };
                    string[] splitedPair = pair1.pair.Split(signsSplit);
                    string[] splitedPair2 = pair2.pair.Split(signsSplit);

                    //Console.WriteLine(splitedPair[0]);

                    if (splitedPair[0] == "USDT" && splitedPair2[0] == "USDT")
                    {
                        crossRatePair.pair = splitedPair[1] + '-' + splitedPair2[1];
                        crossRatePair.sellPrice = pair2.sellPrice / pair1.sellPrice;
                        crossRatePair.purchasePrice = pair2.purchasePrice / pair1.purchasePrice;
                        crossRatePair.stockExchangeSeller = pair1.stockExchangeSeller;

                        crossRates.Add(crossRatePair);
                    }
                }
            }
        }

        public ExchangePair get_pair_by_name(string name)
        {
            return pairs.Find(pairEx => pairEx.pair == name);
        }

        public ExchangePair get_cross_by_name(string name)
        {
            return crossRates.Find(pairEx => pairEx.pair == name);
        }

        public List<ExchangePair> get_all_pairs_info()
        {
            return pairs;
        }

        public List<ExchangePair> get_cross_rates_info()
        {
            return crossRates;
        }

        public void delete_pair_by_name(string name)
        {
            pairs.Remove(pairs.Where(pairEx => pairEx.pair == name).First());
            //pairs.Remove(pairs.Find(pairEx => pairEx.pair == name))
        }

        public void delete_cross_by_name(string name)
        {
            crossRates.Remove(crossRates.Where(pairEx => pairEx.pair == name).First());
        }
    }
}
