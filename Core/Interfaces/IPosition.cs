using System;
using Core.Enums;
using Core.EventArgs;

namespace Core.Interfaces
{
    /// <summary>
    /// Position
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Ordre exécuté à l'origine de la position
        /// </summary>
        public IOrder Order { get; }

        public double Variation { get; }

        public PositionSide Side { get; }

        public double PositionCost { get; }

        public double Profit { get; }

        public double ComputeProfit(double currentPrice);

        public event Action<PositionUpdateEventArgs> PositionUpdate;

    }
}
