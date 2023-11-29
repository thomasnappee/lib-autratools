using Core.Class;
using Core.Enums;

namespace Core.EventArgs
{
    public class StrategyUpdateEventArgs : System.EventArgs
    {
        public StrategyState State;
        public PositionInfo PositionInfo;
        public string Message;
    }
}
