using Binance.Net.Enums;
using Binance.Net.Objects.Futures.FuturesData;
using Core.Interfaces;
using PositionSide = Core.Enums.PositionSide;

namespace Core.Proxies
{
    public class OrderProxy : IOrder
    {
        public OrderProxy(BinanceFuturesOrder binanceFuturesOrder)
        {
            Side = binanceFuturesOrder.Side == OrderSide.Buy ? PositionSide.Buy : PositionSide.Sell;
            EntryPrice = (double) binanceFuturesOrder.Price;
            StopPrice = (double) (binanceFuturesOrder.StopPrice ?? 0);
            HasStopLoss = StopPrice != 0;
            HasStopLimit = false;
            LimitPrice = 0;
            Quantity = (double) binanceFuturesOrder.Quantity;
            QuantityFilled = (double) binanceFuturesOrder.QuantityFilled;
        }

        public PositionSide Side { get; }
        public double EntryPrice { get; }
        public bool HasStopLoss { get; }
        public double StopPrice { get; }
        public bool HasStopLimit { get; }
        public double LimitPrice { get; }
        public double Quantity { get; }
        
        public double QuantityFilled { get; }

        public IOrder StopLimitOrder => throw new System.NotImplementedException();

        public IOrder StopLossOrder => throw new System.NotImplementedException();

        public int CompareTo(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}