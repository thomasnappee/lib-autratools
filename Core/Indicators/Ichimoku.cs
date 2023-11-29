using System;
using System.Linq;
using Core.Interfaces;
using Core.Structs;

public class Ichimoku : ITechnicalIndicator<IchimokuData>
{
    private IchimokuData value = new();

    public IchimokuData Value => value;

    public event Action<IchimokuData> ValueChanged;

    public Ichimoku()
    {
        
    }

    private void OnPriceUpdate(Candlestick candle)
    {
        candlesticks.Add(candle);
        value.Kijun = (candlesticks.TakeLast(26).Sum(x => x.Low) + candlesticks.TakeLast(26).Sum(x => x.High))/2.0m;
        value.Tenkan = (candlesticks.TakeLast(8).Sum(x => x.Low) + candlesticks.TakeLast(8).Sum(x => x.High))/2.0m;
    }
}