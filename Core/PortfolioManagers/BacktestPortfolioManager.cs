using System;
using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.PortfolioManagers
{
    public class BacktestPortfolioManager : IPortfolioManager
    {
        private decimal balance = 10000;

        public event Action<PortfolioUpdateEventArgs> PortfolioUpdate;

        public decimal Balance => balance;

        public void Debit(decimal v)
        {
            if (balance - v < 0) throw new("Crédit insuffisant");
            balance -= v;
            PortfolioUpdate?.Invoke(new PortfolioUpdateEventArgs()
            {
                Type = TransactionType.Debit,
                Value = v,
            });
        }

        public void Credit(decimal v)
        {
            balance += v;
            PortfolioUpdate?.Invoke(new PortfolioUpdateEventArgs()
            {
                Type = TransactionType.Credit,
                Value = v,
            });
        }
    }
}
