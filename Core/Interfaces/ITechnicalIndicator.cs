using Core.EventArgs;
using System;

namespace Core.Interfaces
{
    public interface ITechnicalIndicator<O>
    {
        O Value { get; }

        event Action<O> ValueChanged;
    }
}
