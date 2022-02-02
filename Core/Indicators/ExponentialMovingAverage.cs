using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Indicators
{
    public class ExponentialMovingAverage : ITechnicalIndicator<double>
    {
        private RotationList<double> prices;

        private double alpha;

        public double Value { get; private set; }

        public event Action<double> ValueChanged;

        public ExponentialMovingAverage(int period, IPriceSource priceGenerator)
        {
            prices = new(period);
            alpha = 2 / (period + 1);
            priceGenerator.PriceUpdate += OnPriceUpdate;
        }

        private void OnPriceUpdate(PriceUpdateEventArgs obj)
        {
            prices.Add(obj.NewPrice);
            Value = 0;
            double coef = alpha;
            for(int i = prices.Count - 1; i >= 0; i--)
            {
                Value += coef * prices[0];
                coef *= 1 - alpha;
            }

            ValueChanged?.Invoke(Value);
        }
    }
}
