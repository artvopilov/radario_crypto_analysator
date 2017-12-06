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

        public BasicCryptoMarket(string url, string command)
        {
            pairs = new List<ExchangePair>();
            basicUrl = url;
            load_pairs(command);
        }
        
        public void load_pairs(string command)
        {
            pairs.Clear();

            string response = Program.get_request(basicUrl + command);

            process_response(response);
        }

        protected abstract void process_response(string response);

        public ExchangePair get_pair_by_name(string name)
        {
            return pairs.Find(pairEx => pairEx.pair == name);
        }

        public List<ExchangePair> get_all_pairs_info()
        {
            return pairs;
        }

        public void delete_pair_by_name(string name)
        {
            pairs.Remove(pairs.Where(pairEx => pairEx.pair == name).First());
            //pairs.Remove(pairs.Find(pairEx => pairEx.pair == name))
        }
    }
}
