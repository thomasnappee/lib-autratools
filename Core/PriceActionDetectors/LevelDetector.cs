using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PriceActionDetectors
{
    public class LevelDetector
    {
        public IEnumerable<decimal> Levels;
        private SortedDictionary<int, int> map = new();

        public void Process(decimal price)
        {
            int p = (int)price;
            if (!map.ContainsKey(p))
            {
                map.Add(p, 1);
            }

            else
            {
                map[p]++;
            }

            Levels = map.OrderBy(x => x.Value).Take(3).Select(x => (decimal)x.Key);
        }
    }
}
