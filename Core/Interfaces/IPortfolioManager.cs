using Core.EventArgs;
using System;

namespace Core.Interfaces
{
    public interface IPortfolioManager
    {
        double Balance { get; }

        public event Action<PortfolioUpdateEventArgs> PortfolioUpdate;

        void Debit(double v);
        void Credit(double v);
    }
}