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
        public double Variation { get; set; }
        public PositionSide Side { get; set; }
        public double PositionCost { get; set; }
        public double Profit { get; }

        public double ComputeProfit(double currentPrice)
        {
            return 0;
        }

        public event Action<PositionUpdateEventArgs> PositionUpdate;
    }
}