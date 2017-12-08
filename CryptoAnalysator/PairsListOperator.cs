using System;
using System.Collections.Generic;

namespace CryptoAnalysator {
    class PairsListOperator {
        List<ExchangePair> _actualPairs;

        public PairsListOperator() {
            _actualPairs = new List<ExchangePair>();
        }

        public void DeleteAllActualPairs() {
            _actualPairs.Clear();
        }

        public void AnalysePairs(BasicCryptoMarket[] marketsArray) {
            for (int i = 0; i < marketsArray.Length - 1; i++) {
                foreach (ExchangePair thatMarketPair in marketsArray[i].Pairs) {
                    ExchangePair maxSellPricePair = thatMarketPair;
                    ExchangePair minPurchasePricePair = thatMarketPair;
                    ExchangePair actualPair = new ExchangePair();

                    for (int j = i + 1; j < marketsArray.Length; j++) {
                        string name = thatMarketPair.Pair;
                        ExchangePair anotherMarketPair = marketsArray[j].GetPairByName(name) ??
                            marketsArray[j].GetPairByName(name.Substring(name.IndexOf('-') + 1) + '-' + name.Substring(0, name.IndexOf('-'))) ??
                            thatMarketPair;

                        if (maxSellPricePair.SellPrice < anotherMarketPair.SellPrice) {
                            maxSellPricePair = anotherMarketPair;
                        }
                        if (minPurchasePricePair.PurchasePrice > anotherMarketPair.PurchasePrice) {
                            minPurchasePricePair = anotherMarketPair;
                        }

                        if (anotherMarketPair != thatMarketPair) {
                            marketsArray[j].DeletePairByName(anotherMarketPair.Pair);
                        }

                    }

                    if (minPurchasePricePair.PurchasePrice < maxSellPricePair.SellPrice) {
                        decimal diff = (maxSellPricePair.SellPrice - minPurchasePricePair.PurchasePrice) / maxSellPricePair.SellPrice;

                        actualPair.Pair = diff > (decimal)0.1 ? "[WARN] " + minPurchasePricePair.Pair : minPurchasePricePair.Pair;
                        actualPair.PurchasePrice = minPurchasePricePair.PurchasePrice;
                        actualPair.SellPrice = maxSellPricePair.SellPrice;
                        actualPair.StockExchangeBuyer = maxSellPricePair.StockExchangeSeller;
                        actualPair.StockExchangeSeller = minPurchasePricePair.StockExchangeSeller;

                        _actualPairs.Add(actualPair);
                    }
                }

                foreach (ExchangePair thatMarketPair in marketsArray[i].СrossRates) {
                    ExchangePair maxSellPricePair = thatMarketPair;
                    ExchangePair minPurchasePricePair = thatMarketPair;
                    ExchangePair actualPair = new ExchangePair();

                    for (int j = i + 1; j < marketsArray.Length; j++) {
                        string name = thatMarketPair.Pair;
                        ExchangePair anotherMarketPair = marketsArray[j].GetCrossByName(name) ??
                            marketsArray[j].GetCrossByName(name.Substring(name.IndexOf('-') + 1) + '-' + name.Substring(0, name.IndexOf('-'))) ??
                            thatMarketPair;

                        if (maxSellPricePair.SellPrice < anotherMarketPair.SellPrice) {
                            maxSellPricePair = anotherMarketPair;
                        }
                        if (minPurchasePricePair.PurchasePrice > anotherMarketPair.PurchasePrice) {
                            minPurchasePricePair = anotherMarketPair;
                        }

                        if (anotherMarketPair != thatMarketPair) {
                            marketsArray[j].DeleteCrossByName(anotherMarketPair.Pair);
                        }

                    }

                    if (minPurchasePricePair.PurchasePrice < maxSellPricePair.SellPrice) {
                        decimal diff = (maxSellPricePair.SellPrice - minPurchasePricePair.PurchasePrice) / maxSellPricePair.SellPrice;

                        actualPair.Pair = diff > (decimal)0.1 ? "[CROSS][WARN] " + minPurchasePricePair.Pair : "[CROSS] " + minPurchasePricePair.Pair;
                        actualPair.PurchasePrice = minPurchasePricePair.PurchasePrice;
                        actualPair.SellPrice = maxSellPricePair.SellPrice;
                        actualPair.StockExchangeBuyer = maxSellPricePair.StockExchangeSeller;
                        actualPair.StockExchangeSeller = minPurchasePricePair.StockExchangeSeller;

                        _actualPairs.Add(actualPair);
                    }
                }
            }
        }

        public void ShowActualPairs() {
            foreach (ExchangePair pair in _actualPairs) {
                Console.WriteLine($"{pair.Pair}: {pair.StockExchangeSeller} >> {pair.StockExchangeBuyer}   {pair.PurchasePrice} >> {pair.SellPrice}");
            }
        }
    }
}
