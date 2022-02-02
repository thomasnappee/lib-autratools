using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Linq;

namespace Core.Indicators
{
    public class BollingerBands : ITechnicalIndicator<double[]>
    {
        private RotationList<double> prices;

        /// <summary>
        /// Value[0] => Bande inférieure
        /// Value[1] => Bande centrale
        /// Value[2] => Bande supérieure
        /// </summary>
        public double[] Value { get; }

        public event Action<double[]> ValueChanged;

        public double Delta { get; }

        public BollingerBands(int period, double delta = 2)
        {
            prices = new(period);
            Value = new double[3];
            Delta = delta;
        }

        public BollingerBands(int period, IPriceSource priceGenerator, double delta = 2) : this(period, delta)
        {
            priceGenerator.PriceUpdate += OnPriceUpdate;
        }

        public void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            prices.Add(e.NewPrice);
            var mu = prices.Sum() / prices.Count;
            var sigma = Math.Sqrt(
                prices.Sum(x => Math.Pow(x - mu, 2)
                /
                prices.Count));

            Value[0] = mu - Delta * sigma;
            Value[1] = mu;
            Value[2] = mu + Delta * sigma;
        }
    }
}
