using Core.Class;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.EventArgs;
using System;

namespace Core.OrderManagers
{
    public class BacktestOrderManager : IOrderManager
    {
        private decimal lastPrice;

        private IPortfolioManager portfolioManager;

        public event Action<PositionUpdateEventArgs> PositionUpdated;

        public BacktestOrderManager(
            IPriceSource priceGenerator,
            IPortfolioManager portfolioManager)
        {
            orders = new();
            Positions = new List<IPosition>();
            priceGenerator.PriceUpdate += this.OnPriceUpdate;
            this.portfolioManager = portfolioManager;
        }

        public IList<IPosition> Positions { get; }
        public IList<IOrder> Orders => orders.ToList();

        private SortedSet<IOrder> orders { get; }

        public decimal Profit => Positions.Sum(p => p.Profit);

        public void CloseAllOrders()
        {
            orders.Clear();
        }

        /// <summary>
        /// <inheritdoc cref="IPosition"/>
        /// </summary>
        public void CancelMultipleOrders(IEnumerable<IOrder> orders)
        {
            foreach (IOrder o in orders)
            {
                this.orders.Remove(o);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IPosition"/>
        /// </summary>
        public void CancelOrder(IOrder order)
        {
            orders.Remove(order);
        }

        /// <summary>
        /// <inheritdoc cref="IPosition"/>
        /// </summary>
        public decimal CloseAllPositions()
        {
            if (Positions.Count == 0) return 0;

            var profits = Positions.Sum(x => x.Profit);
            var aggregatedPositionSize = Positions.Sum(x => x.Order.Quantity * x.Order.EntryPrice);
            var s = profits > 0 ? "positive" : "negative";

            portfolioManager.Credit(aggregatedPositionSize + profits);

            Positions.Clear();

            return profits;
        }
       
        /// <summary>
        /// <inheritdoc cref="IPosition"/>
        /// </summary>
        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            IEnumerable<IOrder> touchedOrders;

            if (e.NewPrice == lastPrice) return;

            if(e.NewPrice > lastPrice)
            {
                touchedOrders = orders.Where(x => lastPrice < x.EntryPrice && x.EntryPrice <= e.NewPrice).ToList();
            }

            else
            {
                touchedOrders = orders.Where(x => lastPrice < x.EntryPrice && x.EntryPrice >= e.NewPrice).ToList();
            }

            ProcessOrders(touchedOrders);
            ProcessPositions(e.NewPrice);

            lastPrice = e.NewPrice;
        }

        /// <summary>
        /// <inheritdoc cref="IPosition"/>
        /// </summary>
        public decimal CloseAllPositions(decimal closePrice)
        {
            decimal sum = 0;
            decimal profit;
            foreach (IPosition p in Positions)
            {
                profit = p.ComputeProfit(closePrice);
                sum += profit;
                PositionUpdated?.Invoke(new()
                {
                    Position = p,
                    State = profit > 0 ? PositionState.Winning : PositionState.Losing,
                });
            }

            Positions.Clear();
            return sum;
        }

        public void PostOrder(IOrder o)
        {
            orders.Add(o);
        }

        private void ProcessOrders(IEnumerable<IOrder> touchedOrders)
        {

        }

        private void ProcessPositions(decimal newPrice)
        {
            decimal profit;
            foreach(var position in Positions)
            {
                profit = position.ComputeProfit(newPrice);
                PositionUpdated?.Invoke(new()
                {
                    Position = position,
                    State = profit > 0 ? PositionState.Winning : PositionState.Losing,
                });
            }
        }

        public void OpenLongPosition(decimal quantity)
        {
            OpenPosition(PositionSide.Buy, quantity);
        }

        public void OpenShortPosition(decimal quantity)
        {
            OpenPosition(PositionSide.Sell, quantity);
        }

        private void OpenPosition(PositionSide side, decimal quantity)
        {
            try
            {
                portfolioManager.Debit(quantity * lastPrice);
                var p = new Position(new Order(side, lastPrice, quantity));
                Positions.Add(p);
                PositionUpdated?.Invoke(new()
                {
                    Position = p,
                    State = PositionState.Created,
                });
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
