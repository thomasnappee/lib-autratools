using System;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core.EventArgs;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface permettant de gérer des ordres
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Liste des positions prises
        /// </summary>
        IList<IPosition> Positions { get; }

        event Action<PositionUpdateEventArgs> PositionUpdated;

        /// <summary>
        /// Ferme toute les positions
        /// </summary>
        /// <returns>Le profit réalisé</returns>
        decimal CloseAllPositions();

        /// <summary>
        /// Fait un achat au prix du marché
        /// </summary>
        /// <param name="quantity">La quantité à acheter</param>
        void OpenLongPosition(decimal quantity);

        /// <summary>
        /// Fait une vente à découvert au prix du marché
        /// </summary>
        /// <param name="quantity">La quantité à vendre</param>
        void OpenShortPosition(decimal quantity);
    }
}