using Accord.Statistics.Distributions.Univariate;
using Core.Interfaces;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Core.EventArgs;

namespace Core.Algo
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Geometric_Brownian_motion
    /// </summary>
    public class BrownianMove : IPriceSource
    {
        private decimal Mu;
        private decimal Sigma;

        private decimal price;
        private decimal lastPrice;

        private NormalDistribution NormalDistribution;

        private decimal initPrice;

        private List<decimal> prices = new();

        public int Time { get; protected set; }

        public decimal CurrentPrice => price;
        public decimal CurrentPriceVariation => price/lastPrice - 1;


        public event Action<PriceUpdateEventArgs> PriceUpdate;

        BrownianMove(decimal initState)
        {
            initPrice = lastPrice = price = initState;
            NormalDistribution = new NormalDistribution();
        }

        /// <summary>
        /// Créer un mouvement Brownien avec mu et sigma fixés
        /// </summary>
        /// <param name="initState">Valeur initiale</param>
        /// <param name="mu"></param>
        /// <param name="sigma">Pourcentage de volatilité</param>
        public BrownianMove(decimal initState, decimal mu, decimal sigma)
        {
            throw new Exception();
            Mu = mu;
            Sigma = sigma;

            initPrice = lastPrice = price = initState;
            NormalDistribution = new NormalDistribution();
        }

        /// <summary>
        /// Déduit mu et sigma en fonction d'une liste de prix
        /// </summary>
        /// <param name="path">Chemin vers des données CSV. Le header "Close" est utilisé</param>
        /// <param name="initState">Valeur initiale</param>
        public BrownianMove(String path)
        {
            TextReader treader = new StreamReader(path);
            CsvReader creader = new CsvReader(treader, new CultureInfo("en-US"));

            creader.Read();
            creader.ReadHeader();

            List<decimal> closes = new List<decimal>();

            while (creader.Read())
                closes.Add(decimal.Parse(creader.GetField("Close")));

            for(int i = 0; i<closes.Count-1; i++)
            {
                Mu += (closes.ElementAt(i+1) - closes.ElementAt(i))/ closes.ElementAt(i);
            } Mu /= (closes.Count-1);

            decimal sum = 0;

            for (int i = 0; i < closes.Count - 1; i++)
            {
//                sum += Math.Pow((closes.ElementAt(i + 1) - closes.ElementAt(i)) / closes.ElementAt(i) - Mu, 2);
            } //Sigma = Math.Sqrt(sum/(closes.Count-1));

            treader.Close();
            treader.Dispose();
            creader.Dispose();

            initPrice = lastPrice = price = closes[0];
            //NormalDistribution = new NormalDistribution(Mu, Sigma);
        }

        public decimal GetNextPrice()
        {
            lastPrice = price;
            //price = initPrice * Math.Exp(Mu - Sigma * Sigma / 2) + Sigma * NormalDistribution.Generate();
            prices.Add(price);
            Time++;
            return price;
        }

        /// <summary>
        /// Attention l'implémentation actuelle génèrera une nouvelle suite de prix
        /// </summary>
        public void ResetTime()
        {
            lastPrice = price = initPrice;
            Time = 0;
            prices.Clear();
        }
    }
}
