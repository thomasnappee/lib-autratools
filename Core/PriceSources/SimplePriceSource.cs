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
        PriceUpdateEventArgs priceUpdateEventArgs = new(0);

        public event Action<PriceUpdateEventArgs> PriceUpdate;

        public decimal CurrentPrice => Closes[Time];

        public decimal CurrentPriceVariation => throw new NotImplementedException();

        public int Time { get; protected set; }

        public List<decimal> Closes { get; set; }

        public bool EndOfFileReached;

        public SimplePriceSource(string path)
        {
            TextReader txtReader = new StreamReader(path);
            CsvReader reader = new(txtReader, new CultureInfo("en-US"));

            Closes = new List<decimal>();

            reader.Read();
            reader.ReadHeader();
            string val;
            while (reader.Read())
            {
                val = reader.GetField("close").Replace(".", ",");
                if(decimal.TryParse(val, out var value))
                {
                    Closes.Add(value);
                }
            }

            txtReader.Close();
            Time = 0;
        }
        public decimal ProcessNextPrice()
        {
            if (++Time == Closes.Count)
            {
                Time = 0;
                EndOfFileReached = true;
            }

            priceUpdateEventArgs.NewPrice = CurrentPrice;
            PriceUpdate?.Invoke(priceUpdateEventArgs);

            return CurrentPrice;
        }

        public void ResetTime()
        {
            Time = 0;
            EndOfFileReached = false;
        }
    }
}