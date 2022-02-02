// See https://aka.ms/new-console-template for more information
using Core.OrderManagers;
using Core.PortfolioManagers;
using Core.PriceGenerators;
using Core.Strategies;
using Core.Utils;

BacktestEnvironment environment = new();

var priceSource = new SimplePriceSource(HistoricalDatas.APPLE);
var portfolioManager = new BacktestPortfolioManager();
var orderManager = new BacktestOrderManager(priceSource, portfolioManager);
var strategy = new CrossingMovingAverageStrategy(priceSource);
strategy.Period1 = 12;
strategy.Period2 = 24;

var bot = new BotBase(priceSource, orderManager, portfolioManager, strategy);

bot.Run();

while (!priceSource.EndOfFileReached)
{
    priceSource.ProcessNextPrice();
}

Console.WriteLine(bot.PortfolioManager.Balance);