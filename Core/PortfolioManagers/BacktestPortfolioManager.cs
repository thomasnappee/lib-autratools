using System;
using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;
using Core.OrderManagers;

namespace Core.PortfolioManagers
{
    public class BacktestPortfolioManager : IPortfolioManager
    {
        private double balance = 100000;

        public event Action<PortfolioUpdateEventArgs> PortfolioUpdate;

        public double Balance => balance;

        public void Debit(double v)
        {
            if (balance - v < 0) throw new("Crédit insuffisant");
            balance -= v;
            PortfolioUpdate?.Invoke(new PortfolioUpdateEventArgs()
            {
                Type = TransactionType.Debit,
                Value = v,
            });
        }

        public void Credit(double v)
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
