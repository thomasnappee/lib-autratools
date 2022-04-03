namespace Core.EventArgs
{
    public class TransactionWalletUpdateEventArgs : PortfolioUpdateEventArgs
    {
        public decimal Value { get; }

        public TransactionWalletUpdateEventArgs(decimal value) => Value = value;
    }
}