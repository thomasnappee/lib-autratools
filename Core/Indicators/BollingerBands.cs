using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Linq;

namespace Core.Indicators
{
    public class BollingerBands : ITechnicalIndicator<decimal[]>
    {
        private RotationList<decimal> prices;
        private int v;

        /// <summary>
        /// Value[0] => Bande inférieure
        /// Value[1] => Bande centrale
        /// Value[2] => Bande supérieure
        /// </summary>
        public decimal[] Value { get; }

        public event Action<decimal[]> ValueChanged;

        public decimal Delta { get; }

        public BollingerBands(IPriceSource priceSource, int period , decimal delta = 2)
        {
            priceSource.PriceUpdate += OnPriceUpdate;
            prices = new(period);
            Value = new decimal[3];
            Delta = delta;
        }

        public void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            prices.Add(e.NewPrice);
            var mu = prices.Sum() / prices.Count;
            var sigma = (decimal)Math.Sqrt(
                prices.Sum(x => Math.Pow((double)(x - mu), 2)
                /
                prices.Count));

            Value[0] = mu - Delta * sigma;
            Value[1] = mu;
            Value[2] = mu + Delta * sigma;
        }
    }
}
