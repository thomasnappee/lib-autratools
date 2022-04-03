namespace Core.EventArgs
{
    public class PriceUpdateEventArgs : System.EventArgs
    {
        public decimal NewPrice { get; set; }

        public PriceUpdateEventArgs(decimal newPrice) => NewPrice = newPrice;
    }
}