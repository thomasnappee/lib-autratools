using Core.Class;
using Core.Enums;
using Core.EventArgs;
using Core.Indicators;
using Core.Interfaces;

public class Looper
{
    private int t = 0;

    public bool IsSubscribed;

    private event Action _execute;
    public event Action Execute
    {
        add
        {
            IsSubscribed = true;
            _execute += value;
        }

        remove
        {
            IsSubscribed = false;
            _execute -= value;
        }
    }

    public int Offset { get; set; }

    public void OnTick()
    {
        if(++t >= Offset)
        {
            _execute?.Invoke();
            t = 0;
        }
    }
}

public class BotBase : IBot
{
    public bool IsRunning { get; private set; }
    public bool IsStopped { get; private set; }
    public IPriceSource PriceSource { get; set; }
    public IOrderManager OrderManager { get; set; }
    public IPortfolioManager PortfolioManager { get; set; }
    public IStrategy Strategy { get; set; }

    BollingerBands bollingerBands;

    Module module;

    int n = 0;
    int time = 0;

    public BotBase(
        IPriceSource priceSource,
        IOrderManager orderManager,
        IPortfolioManager portfolioManager,
        IStrategy strategy)
    {
        PriceSource =  priceSource;
        PriceSource.PriceUpdate += e =>
        {
            time++;
        };
        bollingerBands = new(PriceSource, 4);
        bollingerBands.ValueChanged += OnBBUpdate;

        OrderManager = orderManager;

        PortfolioManager = portfolioManager;
        Strategy = strategy;
        Strategy.StateChanged += OnStrategyStateChanged;

        module = new(PriceSource, OrderManager, portfolioManager);
    }

    private void OnBBUpdate(decimal[] obj)
    {
        OrderManager.PostOrder(new Order(PositionSide.Buy, obj[0], PortfolioManager.Balance / 16));
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
        if (e.State == StrategyState.Long && !module.IsRunning)
        {
            module.Start(StrategyState.Long, 0.01m);
        }
        else if(e.State == StrategyState.Short && !module.IsRunning)
        {
            module.Start(StrategyState.Short, 0.01m);
        }
        else if(e.State == StrategyState.ClosePositions)
        {
            Close();
            module.Stop();
        }
    }

    private void Long()
    {
        var position = PortfolioManager.Balance/10;
        OrderManager.OpenLongPosition(position);
        Console.WriteLine($"[{++n}] {DateTimeOffset.FromUnixTimeSeconds(1240347600 + (time + 1) * (86400 + 24685))} Long at {PriceSource.CurrentPrice}");
    }

    private void Short()
    {
        var position = PortfolioManager.Balance/10;
        OrderManager.OpenShortPosition(position);
        Console.WriteLine($"[{++n}] {DateTimeOffset.FromUnixTimeSeconds(1240347600 + (time + 1) * (86400 + 24685))} Short at {PriceSource.CurrentPrice}");
    }

    private void Close()
    {
        OrderManager.CloseAllPositions();
        Console.WriteLine($"{DateTimeOffset.FromUnixTimeSeconds(1240347600 + (time + 1) * (86400 + 24685))} Close at {PriceSource.CurrentPrice} --> {PortfolioManager.Balance}");
    }

    public class Module
    {
        private decimal percentage;
        private decimal previousPrice;
        private StrategyState positionSide;
        private IOrderManager orderManager;
        private IPriceSource priceSource;
        private IPortfolioManager portfolioManager;

        public bool IsRunning;

        public Module(IPriceSource priceSource, IOrderManager orderManager, IPortfolioManager portofolio)
        {
            this.orderManager = orderManager;
            this.priceSource = priceSource;
            this.portfolioManager = portofolio;
        }

        public void Start(StrategyState side, decimal percent)
        {
            positionSide = side;
            percentage = percent;
            previousPrice = priceSource.CurrentPrice;
            priceSource.PriceUpdate += OnPriceUpdate;
            IsRunning = true;
            OnPriceUpdate(new PriceUpdateEventArgs(priceSource.CurrentPrice));
        }

        public void Stop()
        {
            priceSource.PriceUpdate -= OnPriceUpdate;
            IsRunning = false;
        }

        private void OnPriceUpdate(PriceUpdateEventArgs e)
        {
            if (positionSide == StrategyState.Long)
            {
                if(e.NewPrice / previousPrice - 1 >= percentage)
                {
                    orderManager.OpenLongPosition(portfolioManager.Balance/10);
                    Console.WriteLine($"Long at {e.NewPrice}");
                    previousPrice = e.NewPrice;
                }
            }
            else if (positionSide == StrategyState.Short)
            {
                if (-e.NewPrice / previousPrice + 1 >= percentage)
                {
                    orderManager.OpenShortPosition(portfolioManager.Balance/10);
                    Console.WriteLine($"Short at {e.NewPrice}");
                    previousPrice = e.NewPrice;
                }
            }

        }
    }
}
