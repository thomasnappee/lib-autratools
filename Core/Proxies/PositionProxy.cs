using System;
using Binance.Net.Objects.Futures.FuturesData;
using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.Proxies
{
    public class PositionProxy : IPosition
    {
        public PositionProxy(BinancePositionBase binancePositionBase)
        {
            
        }

        public IOrder Order { get; set; }
        public decimal Variation { get; set; }
        public PositionSide Side { get; set; }
        public decimal PositionCost { get; set; }
        public decimal Profit { get; }

        public decimal ComputeProfit(decimal currentPrice)
        {
            return 0;
        }

        public event Action<PositionUpdateEventArgs> PositionUpdate;
    }
}