using Core.Orders.OrderManagers;
using Core.PortfolioManagers;
using Core.PriceGenerators;
using Core.Utils;

SimplePriceSource eurusd = new(HistoricalDatas.EURUSD);
BacktestPortfolioManager portfolio = new();
BacktestOrderManager orderManager = new BacktestOrderManager(eurusd, portfolio);
BotBase bot = new BotBase(eurusd, orderManager, portfolio, strategy);

bot.Run();

while (!eurusd.EndOfFileReached)
{
    eurusd.ProcessNextPrice();
}