namespace Core.EventArgs
{
    public class PortfolioUpdateEventArgs : System.EventArgs
    {
        public object Type { get; internal set; }
        public double Value { get; internal set; }
    }
}