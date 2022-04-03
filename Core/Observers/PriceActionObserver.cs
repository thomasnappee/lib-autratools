using Core.Enums;
using Core.EventArgs;
using Core.Interfaces;
using Core.PriceActionDetectors;
using System;
using System.Collections.Generic;

namespace Core.Observers
{
    public class PriceActionObserver
    {
        private List<decimal> prices = new List<decimal>();
        public event Action<PriceActionState> StateUpdate;

        private TrendDetector trendDetector = new();
        private ExtremumTrendDetector extremumTrendDetector = new();
        private VolatilityDetector volatilityDetector = new();
        
        private int trend_time = 50;
        private int trend_time_counter = 0;
        private int last_trend_state = 0;

        private bool isAnalyzingTrend = true;
        private bool isUpTrend = false;
        private bool isDownTrend = false;

        public decimal LastHigherLow => this.extremumTrendDetector.c;

        public IEnumerable<decimal> LastBuyLevels => levels;
        private Stack<decimal> levels = new();
        private IPriceSource priceSource;

        public PriceActionObserver(IPriceSource priceSource)
        {
            priceSource.PriceUpdate += OnPriceUpdate;
            this.priceSource = priceSource;
        }

        public void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            this.prices.Add(e.NewPrice);
            
            if(this.volatilityDetector.Process(e.NewPrice) == 0)
            {
                levels.Push(e.NewPrice);
            }

            if(this.extremumTrendDetector.Process(e.NewPrice) == 1)
            {
                StateUpdate?.Invoke(PriceActionState.NewHigherLow);
            }

            var trend_state = this.trendDetector.State;

            if (isAnalyzingTrend)
            {
                if(trend_state == 3)
                {
                    StateUpdate?.Invoke(PriceActionState.ConfirmedUpTrend);
                    isAnalyzingTrend = false;
                    isUpTrend = true;
                }

                else if (trend_state == 4)
                {
                    StateUpdate?.Invoke(PriceActionState.ConfirmedDownTrend);
                    isAnalyzingTrend = false;
                    isDownTrend = true;
                }

            }

            if (trend_state == 5)
            {
                if (isUpTrend)
                {
                    StateUpdate?.Invoke(PriceActionState.BrokenUpTrend);
                    isUpTrend = false;
                    isAnalyzingTrend = true;
                }

                else if(isDownTrend)
                {
                    StateUpdate?.Invoke(PriceActionState.BrokenDownTrend);
                    isDownTrend = false;
                    isAnalyzingTrend = true;
                }
            }

        }
    }
}
