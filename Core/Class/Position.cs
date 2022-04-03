using System;
using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.Class
{
    public class Position : IPosition
    {
        public Position(IOrder o, decimal stop = 0, decimal limit = 0)
        {
            order = o;
        }

        private IOrder order;
        public IOrder Order { get => order; }
        public decimal Variation { get; set; }
        public PositionSide Side { get => order.Side; }
        public decimal PositionCost { get; set; }

        public decimal Profit { get; set; }
        public decimal ComputeProfit(decimal newPrice)
        {
            Profit = Order.Quantity * (Side == PositionSide.Buy ? 1 : -1) * (newPrice - Order.EntryPrice);
            Variation = (Order.EntryPrice - newPrice) / Order.EntryPrice - 1;
            return Profit;
        }

        public event Action<PositionUpdateEventArgs> PositionUpdate;

        public override string ToString()
        {
            return $"[{Side}]\t|\t{Math.Round(Variation*100, 2)}%\t|\t@{Order.EntryPrice}\t|\tQty : {Order.Quantity}";
        }
    }
}
