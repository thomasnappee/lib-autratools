using Core.EventArgs;
using Core.Interfaces;

public class BotBase : IBot
{
    public bool IsRunning { get; private set; }
    public bool IsStopped { get; private set; }
    public IPriceSource PriceSource { get; set; }
    public IOrderManager OrderManager { get; set; }
    public IPortfolioManager PortfolioManager { get; set; }
    public IStrategy Strategy { get; set; }

    public BotBase(
        IPriceSource priceSource,
        IOrderManager orderManager,
        IPortfolioManager portfolioManager,
        IStrategy strategy)
    {
        PriceSource =  priceSource;

        OrderManager = orderManager;

        PortfolioManager = portfolioManager;

        Strategy = strategy;
        Strategy.StateChanged += OnStrategyStateChanged;
    }

    public void Pause()
    {
        Strategy.Pause();
    }

    public void Run()
    {
        Strategy.Run();
    }

    public void Stop()
    {
        Strategy.Stop();
    }
    private void OnStrategyStateChanged(StrategyUpdateEventArgs e)
    {
        OrderManager.CloseAllPositions();
        Console.WriteLine($"Enter {e.State} position --> {e.Message}");
        if(e.State == Core.Enums.StrategyState.Long)
        {
            OrderManager.OpenLongPosition(1);
        }
        else if(e.State == Core.Enums.StrategyState.Short)
        {
            OrderManager.OpenShortPosition(1);
        }
    }
}
