using System;

namespace Core.Interfaces
{
    public interface IProcessedPriceSource<T>
    {
        event Action<T> PriceUpdate;
    }
}