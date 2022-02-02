namespace Core.PriceActionDetectors
{
    public class TrendDetector
    {
        private double lastHigh;
        private double lastLow;
        private double lastLastHigh;
        private double lastLastLow;
        private double lastPrice;
        private int currentState;

        public string Text => currentState == 3 ? $"{(int)lastLastLow} {(int)lastLastHigh} {(int)lastLow} {(int)lastHigh}" : currentState == 4 ? $"{(int)lastLastHigh} {(int)lastLastLow} {(int)lastHigh} {(int)lastLow}" : "";

        public int State => currentState;

        private void ResetLastValues(double value)
        {
            lastHigh = lastLow = lastLastHigh = lastLastLow = value;
        }

        public int Process(double price)
        {
            currentState = ExecuteStep(price);
            lastPrice = price;
            return currentState;
        }

        private int ExecuteStep(double price)
        {
            switch (currentState)
            {
                // Si je suis à l'état initial
                case 0:

                    // Si le prix fait un nouveau plus haut
                    if (price > lastLastHigh)
                    {
                        lastHigh = price;

                        // Je suis à l'état 1er nouveau plus haut
                        return 1;
                    }

                    // Si le prix fait un nouveau plus bas
                    else if (price < lastLastLow)
                    {
                        lastLow = price;
                        // Je suis à l'état 1er nouveau plus bas
                        return 2;
                    }

                    // Sinon
                    else
                    {
                        // L'état ne change pas
                        return currentState;
                    }

                // Si je suis à au premier nouveau plus haut
                case 1:

                    if (price >= lastLastHigh)
                    {
                        if (price < lastHigh)
                        {
                            lastLow = price;
                            return currentState;
                        }

                        else if (price > lastHigh)
                        {
                            lastLastHigh = lastHigh;
                            lastLastLow = lastLow;
                            lastHigh = price;
                            return 3;
                        }

                        else
                        {
                            return currentState;
                        }
                    }

                    else
                    {
                        ResetLastValues(price);
                        return 5;
                    }


                // Si je suis à l'état premier nouveau plus bas
                case 2:

                    if (price <= lastLastLow)
                    {
                        if (price > lastLow)
                        {
                            lastHigh = price;
                            return currentState;
                        }

                        else if (price < lastLow)
                        {
                            lastLastLow = lastLow;
                            lastLastHigh = lastHigh;
                            lastLow = price;
                            return 4;
                        }

                        else
                        {
                            return currentState;
                        }
                    }

                    // Si le prix fait un nouveau plus bas
                    else
                    {
                        ResetLastValues(price);
                        return 5;
                    }

                // Si je suis à l'état deuxième nouveau plus haut
                case 3:

                    if (price < lastLastHigh)
                    {
                        ResetLastValues(price);
                        return 5;
                    }

                    else if(price < lastHigh)
                    {
                        lastLow = price;
                        return currentState;
                    }

                    else if (price > lastHigh)
                    {
                        lastLastHigh = lastHigh;
                        lastLastLow = lastLow;
                        lastHigh = price;
                        return currentState;
                    }

                    else
                    {
                        return currentState;
                    }

                // Si je suis à l'état deuxième nouveau plus bas
                case 4:

                    if (price > lastLastLow)
                    {
                        ResetLastValues(price);
                        return 5;
                    }

                    else if(price > lastLow)
                    {
                        lastHigh = price;
                        return currentState;
                    }

                    else if (price < lastLow)
                    {
                        lastLastLow = lastLow;
                        lastLastHigh = lastHigh;
                        lastLow = price;
                        return currentState;
                    }

                    else
                    {
                        return currentState;
                    }
                
                case 5 :
                    return 0;

                default:
                    return currentState;
            }
        }

        public TrendDetector(double init)
        {
            this.Init(init);
        }

        public TrendDetector()
        {
        }

        public void Init(double init)
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
