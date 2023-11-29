using System;
using System.Threading.Tasks;
using Core.EventArgs;
using Core.Interfaces;

namespace Core.PriceGenerators
{
    public class BrownianMotionPriceSource : IPriceSource
    {
        private decimal price;
        
        private decimal last_price;
        
        private Task task;

        private bool taskRunning;

        private decimal mu;
 
        private Func<decimal> muFunction;

        private decimal sigma;

        Func<decimal> sigmaFunction;

        Func<decimal> weinerProcess;

        Action nextPrice;

        public BrownianMotionPriceSource(
            decimal init_price,
            Func<decimal> mu,
            Func<decimal> sigma,
            Func<decimal> weinerProcess)
        {
            price = init_price;
            last_price = init_price;

            this.muFunction = mu;
            this.sigmaFunction = sigma;
            this.weinerProcess = weinerProcess;
            
            nextPrice = NextPrice;
        }

        public BrownianMotionPriceSource(
            decimal init_price,
            decimal mu,
            decimal sigma,
            Func<decimal> weinerProcess)
        {
            price = init_price;
            last_price = init_price;

            this.mu = mu;
            this.sigma = sigma;
            this.weinerProcess = weinerProcess;
            
            nextPrice = NextPriceWithConstants;
        }

        public decimal CurrentPrice => price;

        public decimal CurrentPriceVariation => price/last_price - 1;

        public event Action<PriceUpdateEventArgs> PriceUpdate;

        public void Run()
        {
            taskRunning = true;

            task = new Task(() => 
            {
                while(taskRunning)
                {
                    nextPrice();
                }
            });

            task.Start();
        }

        public void Stop()
        {
            taskRunning = false;
        }

        private void NextPrice()
        {
            price += muFunction()*price + sigmaFunction()*weinerProcess();
        }

        private void NextPriceWithConstants()
        {
            price += mu*price + sigma*weinerProcess();
        }
    }
}