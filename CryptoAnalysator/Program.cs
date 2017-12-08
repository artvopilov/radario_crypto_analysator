using System;


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
    }
}
