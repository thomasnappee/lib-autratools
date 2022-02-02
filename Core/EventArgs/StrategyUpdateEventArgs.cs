using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventArgs
{
    public class StrategyUpdateEventArgs : System.EventArgs
    {
        public StrategyState State;
        public string Message;
    }
}
