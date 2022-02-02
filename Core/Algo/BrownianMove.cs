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
        private double Mu;
        private double Sigma;

        private double price;
        private double lastPrice;

        private NormalDistribution NormalDistribution;

        private double initPrice;

        private List<double> prices = new();

        public int Time { get; protected set; }

        public double CurrentPrice => price;
        public double CurrentPriceVariation => price/lastPrice - 1;


        public event Action<PriceUpdateEventArgs> PriceUpdate;

        BrownianMove(double initState)
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
        public BrownianMove(double initState, double mu, double sigma)
        {
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

            List<double> closes = new List<double>();

            while (creader.Read())
                closes.Add(double.Parse(creader.GetField("Close")));

            for(int i = 0; i<closes.Count-1; i++)
            {
                Mu += (closes.ElementAt(i+1) - closes.ElementAt(i))/ closes.ElementAt(i);
            } Mu /= (closes.Count-1);

            double sum = 0;

            for (int i = 0; i < closes.Count - 1; i++)
            {
                sum += Math.Pow((closes.ElementAt(i + 1) - closes.ElementAt(i)) / closes.ElementAt(i) - Mu, 2);
            } Sigma = Math.Sqrt(sum/(closes.Count-1));

            treader.Close();
            treader.Dispose();
            creader.Dispose();

            initPrice = lastPrice = price = closes[0];
            NormalDistribution = new NormalDistribution(Mu, Sigma);
        }

        public double GetNextPrice()
        {
            lastPrice = price;
            price = initPrice * Math.Exp(Mu - Sigma * Sigma / 2) + Sigma * NormalDistribution.Generate();
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
