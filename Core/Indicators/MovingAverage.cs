using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Linq;

namespace Core.Indicators
{
    public class MovingAverage : ITechnicalIndicator<double>, IDisposable
    {
        /// <summary>
        /// Les derniers prix pour calculer la moyenne
        /// </summary>
        private RotationList<double> lastPrices;

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
        public double Value { get; protected set;}

        /// <summary>
        /// La variation de l'indicateur
        /// </summary>
        public double ValueVariation { get; protected set;}

        public event Action<double> ValueChanged;

        public MovingAverage(int period, IPriceSource priceGenerator)
        {
            lastPrices = new (period);
            for (int i = 0; i < period; i++) lastPrices.Add(priceGenerator.CurrentPrice);
            this.period = period;
            this.priceGenerator = priceGenerator;
            this.priceGenerator.PriceUpdate += this.OnPriceUpdate;
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

        private void Compute(double price)
        {
            lastPrices.Add(price);
            double newValue = lastPrices.Sum() / period;
            ValueVariation = (newValue - Value) / Value;
            Value = newValue;
        }

        public void Dispose()
        {
            this.priceGenerator.PriceUpdate -= this.OnPriceUpdate;
        }
    }
}
