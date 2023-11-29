using System;
using System.Collections.Generic;
using Core.Class;
using Core.EventArgs;
using Core.Indicators;
using Core.Interfaces;
using Core.PriceGenerators;

namespace Core.Strategies
{
    public class TestStrategy : IStrategy
    {
        private BrownianMotionPriceSource priceSource;

        private BollingerBands bb2;

        private BollingerBands bb3;

        // 0 : En dessous de la bande
        // 1 : Au dessus de la bande inférieure et en dessous de la ligne médiane
        // 2 : Au dessus de la ligne médiane et en dessous de la ligne supérieure
        // 3 : Au dessus de la bande supérieure
        private int lastbb2State;
        private int lastbb3State;

        private List<Tuple<int, int>> statesAndOccurences = new();

        public event Action<StrategyUpdateEventArgs> StateChanged;

        public TestStrategy(IPriceSource priceSource)
        {
            this.priceSource = (BrownianMotionPriceSource)priceSource;
            this.bb2 = new(this.priceSource, 20, 2);
            this.bb3 = new(this.priceSource, 20, 3);
            this.priceSource.PriceUpdate += OnPriceUpdate;
        }

        public void Pause()
        {
        }

        public void Run()
        {
            priceSource.Run();
        }

        public void Stop()
        {
        }

        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            var newbb2State = this.GetBBState(bb2, e.NewPrice);

            if(this.lastbb2State == 0 && newbb2State >= 1)
            {
                this.StateChanged(new StrategyUpdateEventArgs()
                {
                    State = Enums.StrategyState.Long,
                    PositionInfo = new PositionInfo()
                    {
                        StopLoss = this.bb3.Value[0],
                        TakeProfit = this.bb3.Value[1],
                        TrailingStop = true
                    }
                });
            }

        }

        private int GetBBState(BollingerBands bb, decimal price)
        {
            if(price < bb.Value[0])
            {
                return 0;
            }
            else if(price > bb.Value[0] && price < bb.Value[1])
            {
                return 1;
            }
            else if(price > bb.Value[1] && price < bb.Value[2])
            {
                return 2;
            }
            else if(price > bb.Value[2])
            {
                return 3;
            }

            return -1;
        }
    }
}