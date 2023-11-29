namespace Core.Strategies
{
    using System;
    using Core.EventArgs;
    using Core.Interfaces;
    using Core.PriceActionDetectors;

    public class FiftyPercentStrategy : IStrategy
    {
        private ExtremumDetector detector;
        private IPriceSource priceSource;
        public event Action<StrategyUpdateEventArgs> StateChanged;

        public FiftyPercentStrategy(IPriceSource priceSource)
        {
            detector = new(priceSource);
            this.priceSource = priceSource;
        }

        public void Pause()
        {
            this.priceSource.PriceUpdate -= OnPriceUpdate;
        }

        public void Run()
        {
            this.priceSource.PriceUpdate += OnPriceUpdate;
        }

        public void Stop() => Pause();

        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            if(detector.LastHigh != 0 && detector.LastLow != 0)
            {
                var delta = detector.LastHigh - detector.LastLow;
                var ratio = (e.NewPrice - detector.LastLow)/delta;

                // Si le prix a retracé les 23.6% de Fibonacci à la hausse sans dépasser les 38.2%
                if (ratio >= 0.236m && ratio < 0.382m)
                {
                    StateChanged?.Invoke(new StrategyUpdateEventArgs()
                    {
                        State = Enums.StrategyState.Long
                    });
                }

                ratio = (detector.LastHigh - e.NewPrice)/delta;
                // Si le prix a retracé les 23.6% de Fibonacci à la baisse sans dépasser les 38.2%
                if (ratio >= 0.236m && ratio < 0.382m)
                {
                    StateChanged?.Invoke(new StrategyUpdateEventArgs()
                    {
                        State = Enums.StrategyState.Short
                    });
                }
            }
        }
    }
}