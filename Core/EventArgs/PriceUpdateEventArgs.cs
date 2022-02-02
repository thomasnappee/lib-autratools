namespace Core.EventArgs
{
    public class PriceUpdateEventArgs : System.EventArgs
    {
        public double NewPrice { get; set; }

        public PriceUpdateEventArgs(double newPrice) => NewPrice = newPrice;
    }
}