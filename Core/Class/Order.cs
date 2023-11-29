using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class Order : IOrder, IComparable
    {
        public PositionSide Side { get; }

        public decimal EntryPrice { get; }

        public decimal Quantity { get; }

        public OrderType OrderType { get; }

        public Order(PositionSide side, decimal entryPrice, decimal quantity)
        {
            Side = side;
            EntryPrice = entryPrice;
            Quantity = quantity;
        }

        public override string ToString() => $"[{Side}] | "
            + $"ENTRY@{EntryPrice}"
            + $" | Qty: {Quantity}";

        public int CompareTo(object obj)
        {
            if(obj is IOrder o)
            {   
                return this.EntryPrice > o.EntryPrice ? 1 : (this.EntryPrice == o.EntryPrice ? 0 : -1);
            }

            throw new Exception("WTFEXCEPTION");
        }
    }
}
