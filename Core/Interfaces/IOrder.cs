using Core.Enums;
using System;

namespace Core.Interfaces
{
    public interface IOrder : IComparable
    {
        PositionSide Side { get; }

        double EntryPrice { get; }

        double Quantity { get; }
    }
}
