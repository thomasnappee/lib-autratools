namespace Core.ProcessedPriceSources
{
    using Core.Enums;
    using Core.Interfaces;
    using Core.Structs;
    using System;

    public class CandlestickSource : IProcessedPriceSource<Candlestick>
    {
        private IPriceSource priceSource;
        private UnitTime unitTime;

        public event Action<Candlestick> PriceUpdate;

        public CandlestickSource(IPriceSource priceSource, UnitTime unitTime)
        {
            this.priceSource = priceSource;
            this.unitTime = unitTime;
        }
    }
}