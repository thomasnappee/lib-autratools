using Core.OrderManagers;
using Core.PortfolioManagers;
using Core.PriceGenerators;
using Core.Strategies;
using Core.Utils;

SimplePriceSource bond = new(HistoricalDatas.IT10YMinusDE10Y);
SimplePriceSource eurusd = new(HistoricalDatas.EURUSD);
BacktestPortfolioManager portfolio = new();
BacktestOrderManager orderManager = new BacktestOrderManager(eurusd, portfolio);
MatthieuStrategy strategy = new MatthieuStrategy(eurusd, bond);
BotBase bot = new BotBase(eurusd, orderManager, portfolio, strategy);

bot.Run();

while (!bond.EndOfFileReached)
{
    eurusd.ProcessNextPrice();
    bond.ProcessNextPrice();
}