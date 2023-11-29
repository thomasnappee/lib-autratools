namespace Core.PriceActionDetectors
{
    using Core.EventArgs;
    using Core.Interfaces;

    public class ExtremumDetector
    {
        public ExtremumDetector(
            IPriceSource priceSource)
        {
            priceSource.PriceUpdate += OnPriceUpdate;
        }

        public decimal LastLow;
        public decimal LastHigh;

        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            if(e.NewPrice > LastHigh)
            {
                LastHigh = e.NewPrice;
            }

            else if(e.NewPrice < LastLow)
            {
                LastLow = e.NewPrice;
            }
        }
    }
}