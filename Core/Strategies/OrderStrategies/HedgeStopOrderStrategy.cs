using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Strategies.OrderStrategies
{
    public class HedgeStopOrderStrategy
    {
        List<IOrderObserver> orderObservers = new();

        public HedgeStopOrderStrategy()
        {            
        }

        public void AddObserver(IOrderObserver observer)
        {
            this.orderObservers.Add(observer);
        }
    }
}