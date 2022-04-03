using Core.EventArgs;
using Core.Indicators;
using Core.Interfaces;
using System;

namespace Core.Strategies
{
    public class CrossingMovingAverageStrategy : IStrategy, IDisposable
    {
        IPriceSource priceSource;
        MovingAverage ma1, ma2;
        bool state;
        private bool ma1Changed;
        private bool ma2Changed;

        private int p1;
        public int Period1
        {
            get => p1;
            set
            {
                ma1?.Dispose();
                p1 = value;
                ma1 = new(value, priceSource);
            }
        }

        private int p2;
        public int Period2
        {
            get => p2;
            set
            {
                ma2?.Dispose();
                p2 = value;
                ma2 = new(value, priceSource);
            }
        }
        /// <summary>
        /// Appelé quand les deux moyennes moviles se croisent.
        /// Le paramètre est 1 si ma1 > ma2, 0 si inférieur ou égal
        /// </summary>
        public event Action<StrategyUpdateEventArgs> StateChanged;

        public CrossingMovingAverageStrategy(
            IPriceSource priceSource)
        {
            this.priceSource = priceSource;
        }

        /// <summary>
        /// Appelle <see cref="StateChanged"/> si l'état a changé.
        /// Le paramètre est 1 si ma1 > ma2, 0 si inférieur ou égal
        /// </summary>
        /// <param name="obj"></param>
        private void OnMa1ValueChanged(decimal value)
        {
            ma1Changed = true;
            if (ma1Changed && ma2Changed) OnValueChanged(value);
        }

        private void OnMa2ValueChanged(decimal value)
        {
            ma2Changed = true;
            if (ma1Changed && ma2Changed) OnValueChanged(value);
        }

        private void OnValueChanged(decimal obj)
        {
            ma1Changed = ma2Changed = false;

            if (ma1.Value > ma2.Value)
            {
                if (state == false)
                {
                    state = true;
                    StateChanged?.Invoke(new()
                    {
                        State = Enums.StrategyState.Long,
                    });
                }
            }

            else
            {
                if (state == true)
                {
                    state = false;
                    StateChanged?.Invoke(new()
                    {
                        State = Enums.StrategyState.Short,
                    });
                }
            }
        }

        public void Pause()
        {
            ma1.ValueChanged -= OnMa1ValueChanged;
            ma2.ValueChanged -= OnMa2ValueChanged;
        }

        public void Run()
        {
            ma1.ValueChanged += OnMa1ValueChanged;
            ma2.ValueChanged += OnMa2ValueChanged;
        }

        /// <summary>
        /// <see cref="Pause"/> Pause puis <see cref="Dispose"/>
        /// </summary>
        public void Stop()
        {
            Pause();
            Dispose();
        }

        /// <summary>
        /// Privilégier l'appel à <see cref="Stop"/>
        /// </summary>
        public void Dispose()
        {
            ma1.Dispose();
            ma2.Dispose();
        }
    }
}
