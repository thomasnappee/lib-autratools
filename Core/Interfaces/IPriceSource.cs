using System;
using Core.EventArgs;

namespace Core.Interfaces
{
    public interface IPriceSource
    {
        event Action<PriceUpdateEventArgs> PriceUpdate;
        
        double CurrentPrice { get; }
        
        double CurrentPriceVariation { get; }
    }
}