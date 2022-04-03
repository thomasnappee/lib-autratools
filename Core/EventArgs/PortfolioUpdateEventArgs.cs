namespace Core.EventArgs
{
    public class PortfolioUpdateEventArgs : System.EventArgs
    {
        public object Type { get; internal set; }
        public decimal Value { get; internal set; }
    }
}