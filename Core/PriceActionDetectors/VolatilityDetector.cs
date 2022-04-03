using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PriceActionDetectors
{
    class VolatilityDetector
    {
        private int currentState;
        public int State => currentState;

        private decimal average_period = 20;
        private List<decimal> averages = new();
        private decimal lastPrice;
        private decimal last_average;

        private List<int> bools = new();

        public int Process(decimal price)
        {
            averages.Add((price / lastPrice - 1)*100);
            if (averages.Count > average_period) averages.RemoveAt(0);

            lastPrice = price;
            
            var average = averages.Sum() / averages.Count;
            var toReturn = 0;

            if(average < last_average)
            {
                bools.Add(1);
                if (bools.Count > average_period) bools.RemoveAt(0);
            }

            last_average = average;

            if(bools.Sum() / average_period > 0.3M)
            {
                return 1;
            }

            return 0;
        }

        public VolatilityDetector(decimal init)
        {
            currentState = 0;
            lastPrice = init;
        }

        public VolatilityDetector()
        {
        }
    }
}
