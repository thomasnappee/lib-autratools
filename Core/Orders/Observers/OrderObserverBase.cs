using System;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.Orders.Observers
{
    public class OrderObserverBase : IDisposable, IOrderObserver
    {
        IPriceSource priceSource;

        public OrderObserverBase(
            IPriceSource priceSource)
        {
            this.priceSource = priceSource;
            this.priceSource.PriceUpdate += this.OnPriceUpdate;
        }

        public void Dispose()
        {
            this.priceSource.PriceUpdate -= this.OnPriceUpdate;
        }

        protected virtual void OnPriceUpdate(PriceUpdateEventArgs obj)
        {
        }
    }
}
