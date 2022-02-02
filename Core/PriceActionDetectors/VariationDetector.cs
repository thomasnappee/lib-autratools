using Core.EventArgs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PriceActionDetectors
{
    public class VariationDetector
    {
        private Dictionary<(double,double), int> p;
        private double lastPrice;
        private double lastVar;
        private double coef;

        public event Action<KeyValuePair<(double,double), int>> StateChanged;

        public VariationDetector(IPriceSource priceSource, double precision)
        {
            coef = 1 / precision;
            priceSource.PriceUpdate += OnPriceUpdate;
            lastPrice = priceSource.CurrentPrice;
        }

        public void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            var k = (lastVar, (int)((e.NewPrice - lastPrice) / lastPrice + 1) * coef);
            if(!p.ContainsKey(k))
            {
                p.Add(k, 1);
            }
            else
            {
                p[k]++;
            }

            lastPrice = e.NewPrice;
            lastVar = k.Item2;

            StateChanged?.Invoke(p.Min(x => x));
        }
    }
}
