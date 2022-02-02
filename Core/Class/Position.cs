using System;
using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.Class
{
    public class Position : IPosition
    {
        public Position(IOrder o, double stop = 0, double limit = 0)
        {
            order = o;
        }

        private IOrder order;
        public IOrder Order { get => order; }
        public double Variation { get; set; }
        public PositionSide Side { get => order.Side; }
        public double PositionCost { get; set; }

        public double Profit { get; set; }
        public double ComputeProfit(double newPrice) => Profit = Order.Quantity * (Side == PositionSide.Buy ? 1 : -1) * (newPrice - Order.EntryPrice);

        public event Action<PositionUpdateEventArgs> PositionUpdate;

        public override string ToString()
        {
            return $"[{Side}]\t|\t{Variation}\t|\t@{Order.EntryPrice}\t|\tQty : {Order.Quantity}";
        }
    }
}
