namespace Core.EventArgs
{
    public class TransactionWalletUpdateEventArgs : PortfolioUpdateEventArgs
    {
        public double Value { get; }

        public TransactionWalletUpdateEventArgs(double value) => Value = value;
    }
}