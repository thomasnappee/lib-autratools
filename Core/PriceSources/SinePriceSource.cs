using System;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.PriceGenerators
{
    class SinePriceSource : IPriceSource
    {
        public event Action<PriceUpdateEventArgs> PriceUpdate;
        public decimal CurrentPrice { get; protected set; }
        public decimal CurrentPriceVariation { get; protected set; }
        public int Time { get; protected set; }

        private int period;

        public SinePriceSource(int period)
        {
            this.period = period;
            Time = 0;
        }

        public decimal GetNextPrice()
        {
            throw new Exception();
            //CurrentPrice = 100.0 + 10 * Math.Cos(2 * Math.PI * Time / period);
            //CurrentPriceVariation = -(10 / period) * Math.Sin(2 * Math.PI * Time / period);
            Time++;
            PriceUpdate?.Invoke(new PriceUpdateEventArgs(CurrentPrice));

            return CurrentPrice;
        }

        public void ResetTime()
        {
            Time = 0;
        }
    }
}
