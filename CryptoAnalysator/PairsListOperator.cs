using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAnalysator
{
    class PairsListOperator
    {
        List<ExchangePair> actualPairs;

        public PairsListOperator()
        {
            actualPairs = new List<ExchangePair>();
        }

        public void delete_all_pairs()
        {
            actualPairs.Clear();
        }

        public void add_pair(ExchangePair pair)
        {
            actualPairs.Add(pair);
        }

        public void analyse_add_pairs(PoloniexMarket poloniex, BittrexMarket bittrex, ExmoMarket exmo)
        {
            actualPairs.Clear();

            foreach (ExchangePair poloniexPair in poloniex.get_all_pairs_info().OrderBy(i => i.pair).ToList())
            {

                ExchangePair bittrexPair = bittrex.get_pair_by_name(poloniexPair.pair) ?? poloniexPair;
                ExchangePair exmoPair = exmo.get_pair_by_name(poloniexPair.pair) ?? poloniexPair;
                Console.WriteLine(poloniexPair.pair);

                ExchangePair maxSellPricePair;
                ExchangePair minPurchasePricePair;
                ExchangePair actualPair = new ExchangePair();


                if (poloniexPair.purchasePrice < bittrexPair.purchasePrice)
                {
                    minPurchasePricePair = poloniexPair.purchasePrice < exmoPair.purchasePrice ? poloniexPair : exmoPair;
                }
                else
                {
                    minPurchasePricePair = bittrexPair.purchasePrice < exmoPair.purchasePrice ? bittrexPair : exmoPair;
                }

                if (poloniexPair.sellPrice > bittrexPair.sellPrice)
                {
                    maxSellPricePair = poloniexPair.sellPrice > exmoPair.sellPrice ? poloniexPair : exmoPair;
                }
                else
                {
                    maxSellPricePair = bittrexPair.sellPrice > exmoPair.sellPrice ? bittrexPair : exmoPair;
                }

                if (minPurchasePricePair.purchasePrice < maxSellPricePair.sellPrice)
                {
                    actualPair.pair = minPurchasePricePair.pair;
                    actualPair.purchasePrice = minPurchasePricePair.purchasePrice;
                    actualPair.sellPrice = maxSellPricePair.sellPrice;
                    actualPair.stockExchangeBuyer = maxSellPricePair.stockExchangeSeller;
                    actualPair.stockExchangeSeller = minPurchasePricePair.stockExchangeSeller;

                    actualPairs.Add(actualPair);
                }

                
            }
        }

        public void show_actual_pairs()
        {
            foreach (ExchangePair pair in actualPairs)
            {
                Console.WriteLine($"{pair.pair}: {pair.stockExchangeSeller} > {pair.stockExchangeBuyer}   {pair.purchasePrice} > {pair.sellPrice}");
            }
        }
    }
}
