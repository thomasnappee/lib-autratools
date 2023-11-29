using System;
using Core.Interfaces;
using Core.Class;
using Core.EventArgs;

namespace Core.Indicators
{
    public class PivotPoints : ITechnicalIndicator<PivotPointsLevels>
    {
        private IPriceSource priceSource;

        public PivotPointsLevels Value { get; }

        public event Action<PivotPointsLevels> ValueChanged;

        public PivotPoints(IPriceSource priceSource)
        {
            this.priceSource = priceSource;
            this.priceSource.PriceUpdate += OnNewPrice;
        }

        private void OnNewPrice(PriceUpdateEventArgs e)
        {
            
        }
    }
}