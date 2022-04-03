using System;
using Core.EventArgs;

namespace Core.Interfaces
{
    public interface IPriceSource
    {
        event Action<PriceUpdateEventArgs> PriceUpdate;
        
        decimal CurrentPrice { get; }
        
        decimal CurrentPriceVariation { get; }
    }
}