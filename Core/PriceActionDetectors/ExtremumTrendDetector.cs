using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PriceActionDetectors
{
    public class ExtremumTrendDetector
    {
        public double a, b, c;

        private int state = 0;
        private double[] values = new double[20];
        private int index = 0;

        public int Process(double price, bool useSmoothing = false)
        {
            if (useSmoothing)
            {
                values[index++] = price;
                if (index == 20) index = 0;
                price = values.Sum() / 20;
            }


            if (a > c && b < c && price > c && state == 1)
            {
                a = b = c = price;
                state = 0;
                return 1; // New higher low confirmed
            }

            if (a < c && b > c && price < c && state == 1)
            {
                a = b = c = price;
                state = 0;
            }

            if (state == 0)
            {
                if (price > a)
                {
                    if (c == 0)
                    {
                        a = price;
                        b = price;
                    }

                    else
                    {
                        b = c = price;
                        state = 1;
                    }
                }

                else if (price < a)
                {
                    if (b == 0)
                    {
                        a = price;
                        c = price;
                    }

                    else
                    {
                        b = c = price;
                        state = 1;
                    }
                }
            }

            if(state == 1)
            {
                if (price < a)
                {
                    if (price > b && price < a)
                    {
                        c = price;
                    }
                    else
                    {
                        b = c = price;
                    }
                }

                else if (price > a)
                {
                    if (price < b && price > a)
                    {
                        c = price;
                    }
                    else
                    {
                        b = c = price;
                    }
                }

            }

            return 0;
        }
    }
}
