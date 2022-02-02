using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PriceActionDetectors
{
    public class LevelDetector
    {
        public IEnumerable<double> Levels;
        private SortedDictionary<int, int> map = new();

        public void Process(double price)
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

            Levels = map.OrderBy(x => x.Value).Take(3).Select(x => (double)x.Key);
        }
    }
}
