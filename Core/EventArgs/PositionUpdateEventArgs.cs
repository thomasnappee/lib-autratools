using System.Runtime.CompilerServices;
using Core.Enums;
using Core.Interfaces;

namespace Core.EventArgs
{
    public class PositionUpdateEventArgs : System.EventArgs
    {
        public IPosition Position;
        public PositionState State;
    }
}