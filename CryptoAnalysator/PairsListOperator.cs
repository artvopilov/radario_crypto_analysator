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

        // Analyses loaded pairs of currencies from different crypto markets and saves actual.
        public void analyse_pairs(BasicCryptoMarket[] marketsArray)
        {
            for (int i = 0; i < marketsArray.Length - 1; i++)
            {
                foreach (ExchangePair thatMarketPair in marketsArray[i].get_all_pairs_info())
                {
                    ExchangePair maxSellPricePair = thatMarketPair;
                    ExchangePair minPurchasePricePair = thatMarketPair;
                    ExchangePair actualPair = new ExchangePair();
                    
                    for (int j = i + 1; j < marketsArray.Length; j++)
                    {
                        string name = thatMarketPair.pair;

                        ExchangePair anotherMarketPair = marketsArray[j].get_pair_by_name(name) ??
                            marketsArray[j].get_pair_by_name(name.Substring(name.IndexOf('-') + 1) + '-' + name.Substring(0, name.IndexOf('-'))) ??
                            thatMarketPair;

                        if (maxSellPricePair.sellPrice < anotherMarketPair.sellPrice)
                        {
                            maxSellPricePair = anotherMarketPair;
                        }
                        if (minPurchasePricePair.purchasePrice > anotherMarketPair.purchasePrice)
                        {
                            minPurchasePricePair = anotherMarketPair;
                        }

                        if (anotherMarketPair != thatMarketPair)
                        {
                            marketsArray[j].delete_pair_by_name(anotherMarketPair.pair);
                        }
                        
                    }
                    
                    if (minPurchasePricePair.purchasePrice < maxSellPricePair.sellPrice)
                    {
                        decimal diff = (maxSellPricePair.sellPrice - minPurchasePricePair.purchasePrice) / maxSellPricePair.sellPrice;

                        actualPair.pair = diff > (decimal)0.1 ? "[WARN] " + minPurchasePricePair.pair : minPurchasePricePair.pair;
                        actualPair.purchasePrice = minPurchasePricePair.purchasePrice;
                        actualPair.sellPrice = maxSellPricePair.sellPrice;
                        actualPair.stockExchangeBuyer = maxSellPricePair.stockExchangeSeller;
                        actualPair.stockExchangeSeller = minPurchasePricePair.stockExchangeSeller;

                        actualPairs.Add(actualPair);
                    }
                }
            }
        }

        // Shows actual pairs of currencies. If [WARN] label, actual pair maybe wrong or is really good.
        public void show_actual_pairs()
        {
            foreach (ExchangePair pair in actualPairs)
            {
                Console.WriteLine($"{pair.pair}: {pair.stockExchangeSeller} >> {pair.stockExchangeBuyer}   {pair.purchasePrice} >> {pair.sellPrice}");
            }
        }
    }
}
