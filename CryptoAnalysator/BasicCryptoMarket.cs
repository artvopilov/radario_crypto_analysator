﻿using System.Collections.Generic;
using System.Linq;

namespace CryptoAnalysator {
    abstract class BasicCryptoMarket {
        protected readonly string _basicUrl;
        protected List<ExchangePair> _pairs;
        protected List<ExchangePair> _usdtPairs;
        protected List<ExchangePair> _crossRates;

        public List<ExchangePair> Pairs { get => _pairs; }
        public List<ExchangePair> СrossRates { get => _crossRates; }

        protected decimal _feeTaker;
        protected decimal _feeMaker;

        public BasicCryptoMarket(string url, string command, decimal feeTaker, decimal feeMaker) {
            _pairs = new List<ExchangePair>();
            _usdtPairs = new List<ExchangePair>();
            _crossRates = new List<ExchangePair>();
            _basicUrl = url;
            _feeTaker = feeTaker;
            _feeMaker = feeMaker;
            LoadPairs(command);
        }

        public void LoadPairs(string command) {
            _pairs.Clear();

            string response = Program.get_request(_basicUrl + command);

            ProcessResponse(response);
        }

        protected abstract void ProcessResponse(string response);

        protected void CheckAddUsdtPair(ExchangePair pair) {
            char[] signsSplit = { '-' };
            string[] splitedPair = pair.Pair.Split(signsSplit);

            if (splitedPair[0].ToUpper() == "USDT" || splitedPair[1].ToUpper() == "USDT") {
                _usdtPairs.Add(pair);
            }
        }

        protected void CreateCrossRates() {
            for (int i = 0; i < _usdtPairs.Count - 1; i++) {
                for (int j = i + 1; j < _usdtPairs.Count; j++) {
                    ExchangePair pair1 = _usdtPairs[i];
                    ExchangePair pair2 = _usdtPairs[j];

                    ExchangePair crossRatePair = new ExchangePair();
                    char[] signsSplit = { '-' };
                    string[] splitedPair = pair1.Pair.Split(signsSplit);
                    string[] splitedPair2 = pair2.Pair.Split(signsSplit);

                    if (splitedPair[0] == "USDT" && splitedPair2[0] == "USDT") {
                        crossRatePair.Pair = splitedPair[1] + '-' + splitedPair2[1];
                        crossRatePair.SellPrice = pair2.SellPrice / pair1.SellPrice;
                        crossRatePair.PurchasePrice = pair2.PurchasePrice / pair1.PurchasePrice;
                        crossRatePair.StockExchangeSeller = pair1.StockExchangeSeller;

                        _crossRates.Add(crossRatePair);
                    }
                }
            }
        }

        public ExchangePair GetPairByName(string name) {
            return _pairs.Find(pairEx => pairEx.Pair == name);
        }

        public ExchangePair GetCrossByName(string name) {
            return _crossRates.Find(pairEx => pairEx.Pair == name);
        }

        public void DeletePairByName(string name) {
            _pairs.Remove(_pairs.Find(pairEx => pairEx.Pair == name));
        }

        public void DeleteCrossByName(string name) {
            _crossRates.Remove(_crossRates.Where(pairEx => pairEx.Pair == name).First());
        }
    }
}
