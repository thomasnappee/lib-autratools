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

        public decimal Variation { get; }

        public PositionSide Side { get; }

        public decimal PositionCost { get; }

        public decimal Profit { get; }

        public decimal ComputeProfit(decimal currentPrice);

        public event Action<PositionUpdateEventArgs> PositionUpdate;

    }
}
