using Core.Interfaces;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Core.EventArgs;

namespace Core.PriceGenerators
{
    public class SimplePriceSource : IPriceSource
    {
        private int index = 0;

        PriceUpdateEventArgs priceUpdateEventArgs = new(0);

        public event Action<PriceUpdateEventArgs> PriceUpdate;

        public double CurrentPrice { get; protected set; }
        double IPriceSource.CurrentPrice => Closes[index];

        public double CurrentPriceVariation { get; protected set; }
        double IPriceSource.CurrentPriceVariation => throw new NotImplementedException();

        public int Time { get; protected set; }

        public List<double> Closes { get; set; }

        public bool EndOfFileReached;

        public SimplePriceSource(string path)
        {
            TextReader txtReader = new StreamReader(path);
            CsvReader reader = new(txtReader, new CultureInfo("en-US"));

            Closes = new List<double>();

            reader.Read();
            reader.ReadHeader();
            string val;
            while (reader.Read())
            {
                val = reader.GetField("Close").Replace(".", ",");
                if(double.TryParse(val, out var value))
                {
                    Closes.Add(value);
                }
            }

            txtReader.Close();
            Time = 0;
        }
        public double ProcessNextPrice()
        {
            if (++Time == Closes.Count)
            {
                Time = 0;
                EndOfFileReached = true;
            }
            var realCurrentPrice = Closes.ElementAt(Time);
            CurrentPriceVariation = realCurrentPrice / CurrentPrice - 1;
            CurrentPrice = realCurrentPrice;

            priceUpdateEventArgs.NewPrice = realCurrentPrice;
            PriceUpdate?.Invoke(priceUpdateEventArgs);

            return realCurrentPrice;
        }

        public void ResetTime()
        {
            Time = 0;
            CurrentPrice = 0;
            EndOfFileReached = false;
        }
    }
}