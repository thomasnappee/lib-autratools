using Core.EventArgs;
using System;

namespace Core.Interfaces
{
    public interface IPortfolioManager
    {
        decimal Balance { get; }

        public event Action<PortfolioUpdateEventArgs> PortfolioUpdate;

        void Debit(decimal v);
        void Credit(decimal v);
    }
}