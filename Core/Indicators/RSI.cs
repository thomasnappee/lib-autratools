using Core.EventArgs;
using Core.Interfaces;
using Core.Utils;
using System;
using System.Linq;

namespace Core.Indicators
{
    public class RSI : ITechnicalIndicator<decimal>
    {
        public decimal Value { get; private set; }

        public event Action<decimal> ValueChanged;

        RotationList<decimal> rises;
        RotationList<decimal> falls;


        public RSI(int period, IPriceSource priceGenerator)
        {
            rises = new(period);
            falls = new(period);

            priceGenerator.PriceUpdate += OnPriceUpdate;
        }

        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            if(e.NewPrice >= 0)
            {
                rises.Add(e.NewPrice);
                falls.Add(0);
            }
            else
            {
                falls.Add(e.NewPrice);
                rises.Add(0);
            }

            var h = rises.Sum(x => x);
            Value = h / (h + falls.Sum(x => x)) * 100;

            ValueChanged?.Invoke(Value);
        }
    }
}
