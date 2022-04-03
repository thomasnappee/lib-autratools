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
            EntryPrice = (decimal) binanceFuturesOrder.Price;
            StopPrice = (decimal) (binanceFuturesOrder.StopPrice ?? 0);
            HasStopLoss = StopPrice != 0;
            HasStopLimit = false;
            LimitPrice = 0;
            Quantity = (decimal) binanceFuturesOrder.Quantity;
            QuantityFilled = (decimal) binanceFuturesOrder.QuantityFilled;
        }

        public PositionSide Side { get; }
        public decimal EntryPrice { get; }
        public bool HasStopLoss { get; }
        public decimal StopPrice { get; }
        public bool HasStopLimit { get; }
        public decimal LimitPrice { get; }
        public decimal Quantity { get; }
        
        public decimal QuantityFilled { get; }

        public IOrder StopLimitOrder => throw new System.NotImplementedException();

        public IOrder StopLossOrder => throw new System.NotImplementedException();

        public int CompareTo(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}