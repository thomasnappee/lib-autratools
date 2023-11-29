using Core.Enums;
using System;

namespace Core.Interfaces
{
    public interface IOrder
    {
        PositionSide Side { get; }

        decimal EntryPrice { get; }

        decimal Quantity { get; }
    }
}
