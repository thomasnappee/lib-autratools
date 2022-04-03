using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Linq;

namespace Core.Indicators
{
    public class MovingAverage : ITechnicalIndicator<decimal>, IDisposable
    {
        /// <summary>
        /// Les derniers prix pour calculer la moyenne
        /// </summary>
        private RotationList<decimal> lastPrices;

        /// <summary>
        /// Nombre de valeurs utilisées pour calculer le prix moyen
        /// </summary>
        private int period;

        /// <summary>
        /// Permet de recevoir les updates de prix
        /// </summary>
        public IPriceSource priceGenerator { get; protected set; }

        /// <summary>
        /// La valeur actuelle de l'indicateur
        /// </summary>
        public decimal Value { get; protected set;}

        /// <summary>
        /// La variation de l'indicateur
        /// </summary>
        public decimal ValueVariation { get; protected set;}

        public event Action<decimal> ValueChanged;

        public MovingAverage(int period, IPriceSource priceGenerator)
        {
            lastPrices = new (period);
            for (int i = 0; i < period; i++) lastPrices.Add(priceGenerator.CurrentPrice);
            this.period = period;
            this.priceGenerator = priceGenerator;
            this.priceGenerator.PriceUpdate += this.OnPriceUpdate;
            this.Value = priceGenerator.CurrentPrice;
        }

        /// <summary>
        /// Calcule la nouvelle valeur de l'indicateur à chaque update
        /// </summary>
        /// <param name="e"></param>
        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            Compute(e.NewPrice);
            ValueChanged?.Invoke(Value);
        }

        private void Compute(decimal price)
        {            
            lastPrices.Add(price);
            decimal newValue = lastPrices.Sum() / period;
            ValueVariation = (newValue - Value) / Value;
            Value = newValue;
        }

        public void Dispose()
        {
            this.priceGenerator.PriceUpdate -= this.OnPriceUpdate;
        }
    }
}
