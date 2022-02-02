using System;

namespace Core.Interfaces
{
    public interface IBot
    {
        public IPriceSource PriceSource { get; set; }

        public IOrderManager OrderManager { get; set; }

        public IPortfolioManager PortfolioManager { get; set; }

        public bool IsRunning { get; }
        public bool IsStopped { get; }

        public void Run();

        public void Stop();

        public void Pause();


    }
}
