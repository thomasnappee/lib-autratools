using Core.Enums;
using System;

namespace Core.Interfaces
{
    public interface IOrder : IComparable
    {
        PositionSide Side { get; }

        decimal EntryPrice { get; }

        decimal Quantity { get; }
    }
}
