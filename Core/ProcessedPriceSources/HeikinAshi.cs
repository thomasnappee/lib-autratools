namespace Core.ProcessedPriceSources
{
    using Core.Interfaces;
    using Core.ProcessedPriceSources;
    using Core.Structs;
    using System;

    public class HeikinAshiSource : IProcessedPriceSource<Candlestick>
    {
        public event Action<Candlestick> PriceUpdate;
    }
}