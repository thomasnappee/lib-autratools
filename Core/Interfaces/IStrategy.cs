using Core.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IStrategy
    {
        event Action<StrategyUpdateEventArgs> StateChanged;
        void Pause();
        void Run();
        void Stop();
    }
}
