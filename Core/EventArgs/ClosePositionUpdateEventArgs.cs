using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventArgs
{
    public class ClosePositionUpdateEventArgs : PortfolioUpdateEventArgs
    {
        public double Profit { get; set; }
        public double PositionSize { get; set; }
    }
}
