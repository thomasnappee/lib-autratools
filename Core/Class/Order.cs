using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class Order : IOrder
    {
        public PositionSide Side { get; }
        public double EntryPrice { get; }
        public double Quantity { get; }
        public Order(PositionSide side, double entryPrice, double quantity)
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
