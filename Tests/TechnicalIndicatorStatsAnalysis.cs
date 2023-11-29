using Core.PriceGenerators;
using Core.Utils;

public class TechnicalIndicatorStatsAnalysis
{
    private SimplePriceSource priceSource;

    public TechnicalIndicatorStatsAnalysis()
    {
        priceSource = new SimplePriceSource(HistoricalDatas.EURUSD);
        technicalIndicator = new Ichimoku();
    }
}