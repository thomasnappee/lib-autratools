using System;
using Core.EventArgs;
using Core.Interfaces;
using Core.Structs;

public class IchimokuStrategy : IStrategy
{
    private Ichimoku ichimoku;

    public IchimokuStrategy(IProcessedPriceSource<Candlestick> priceSource)
    {
        this.ichimoku = new();
        priceSource.PriceUpdate += OnPriceUpdate;
    }

    private void OnPriceUpdate(Candlestick candle)
    {
        
    }

    public event Action<StrategyUpdateEventArgs> StateChanged;

    public void Pause()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}