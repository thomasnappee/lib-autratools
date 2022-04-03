using Core.EventArgs;
using System;
using System.Collections.Generic;

namespace Core.PriceActionDetectors
{
    public class TrendDetector
    {
        private decimal lastHigh;
        private decimal lastLow;
        private decimal lastLastHigh;
        private decimal lastLastLow;
        private decimal lastPrice;
        private int currentState;

        public event Action<int> StateChanged;

        private List<decimal> lastHighs = new();
        private List<decimal> lastLows = new();

        public string Text => currentState == 3 ? $"{(int)lastLastLow} {(int)lastLastHigh} {(int)lastLow} {(int)lastHigh}" : currentState == 4 ? $"{(int)lastLastHigh} {(int)lastLastLow} {(int)lastHigh} {(int)lastLow}" : "";

        public int State => currentState;

        private void ResetLastValues(decimal value)
        {
            lastHigh = lastLow = lastLastHigh = lastLastLow = value;
        }

        public void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            var price = e.NewPrice;


        }

        public void OnPriceUpdateObsolete(PriceUpdateEventArgs e)
        {
            var price = e.NewPrice;
            switch (currentState)
            {
                // Si je suis à l'état initial
                case 0:

                    // Si le prix fait un nouveau plus haut
                    if (price > lastLastHigh)
                    {
                        lastHigh = price;

                        // Je suis à l'état 1er nouveau plus haut
                        StateChanged?.Invoke(1);
                    }

                    // Si le prix fait un nouveau plus bas
                    else if (price < lastLastLow)
                    {
                        lastLow = price;
                        // Je suis à l'état 1er nouveau plus bas
                        StateChanged?.Invoke(2);
                    }

                    // Sinon
                    else
                    {
                        // L'état ne change pas
                        StateChanged?.Invoke(currentState);
                    }
                    break;

                // Si je suis à au premier nouveau plus haut
                case 1:

                    if (price >= lastLastHigh)
                    {
                        if (price < lastHigh)
                        {
                            lastLow = price;
                            StateChanged?.Invoke(currentState);
                        }

                        else if (price > lastHigh)
                        {
                            lastLastHigh = lastHigh;
                            lastLastLow = lastLow;
                            lastHigh = price;
                            StateChanged?.Invoke(3);
                        }

                        else
                        {
                            StateChanged?.Invoke(currentState);
                        }
                    }

                    else
                    {
                        ResetLastValues(price);
                        StateChanged?.Invoke(5);
                    }
                    break;

                // Si je suis à l'état premier nouveau plus bas
                case 2:

                    if (price <= lastLastLow)
                    {
                        if (price > lastLow)
                        {
                            lastHigh = price;
                            StateChanged?.Invoke(currentState);
                        }

                        else if (price < lastLow)
                        {
                            lastLastLow = lastLow;
                            lastLastHigh = lastHigh;
                            lastLow = price;
                            StateChanged?.Invoke(4);
                        }

                        else
                        {
                            StateChanged?.Invoke(currentState);
                        }
                    }

                    // Si le prix fait un nouveau plus bas
                    else
                    {
                        ResetLastValues(price);
                        StateChanged?.Invoke(5);
                    }
                    break;

                // Si je suis à l'état deuxième nouveau plus haut
                case 3:

                    if (price < lastLastHigh)
                    {
                        ResetLastValues(price);
                        StateChanged?.Invoke(5);
                    }

                    else if (price < lastHigh)
                    {
                        lastLow = price;
                        StateChanged?.Invoke(currentState);
                    }

                    else if (price > lastHigh)
                    {
                        lastLastHigh = lastHigh;
                        lastLastLow = lastLow;
                        lastHigh = price;
                        StateChanged?.Invoke(currentState);
                    }

                    else
                    {
                        StateChanged?.Invoke(currentState);
                    }
                    break;

                // Si je suis à l'état deuxième nouveau plus bas
                case 4:

                    if (price > lastLastLow)
                    {
                        ResetLastValues(price);
                        StateChanged?.Invoke(5);
                    }

                    else if (price > lastLow)
                    {
                        lastHigh = price;
                        StateChanged?.Invoke(currentState);
                    }

                    else if (price < lastLow)
                    {
                        lastLastLow = lastLow;
                        lastLastHigh = lastHigh;
                        lastLow = price;
                        StateChanged?.Invoke(currentState);
                    }

                    else
                    {
                        StateChanged?.Invoke(currentState);
                    }
                    break;

                case 5:
                    StateChanged?.Invoke(0);
                    break;

                default:
                    StateChanged?.Invoke(currentState);
                    break;
            }
            lastPrice = price;
        }

        public TrendDetector(decimal init)
        {
            this.Init(init);
        }

        public TrendDetector()
        {
        }

        public void Init(decimal init)
        {
            this.lastLastHigh = init;
            this.lastLastLow = init;
            this.lastHigh = init;
            this.lastLow = init;
            this.lastPrice = init;
            this.currentState = 0;
        }


    }
}
