using System.Globalization;

namespace BackEndReactWebApplication
{
    public class CalculatePrices
    {

        private List<ChartData> chartData = new List<ChartData>();

        public CalculatePrices() { }

        public CalculatePrices(EodResponseInfo[] eodResponseInfos)
        {
            loadDailyPrices(eodResponseInfos);
        }

        private void loadDailyPrices(EodResponseInfo[] eodResponseInfos)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberDecimalDigits = 2;
            
            foreach (EodResponseInfo eodResponseInfo in eodResponseInfos)
            {
                ChartData aChartDataEntry = new ChartData();

                aChartDataEntry.name = eodResponseInfo.date.Substring(0, eodResponseInfo.date.IndexOf("T", 0));
                aChartDataEntry.dailyPrices = Convert.ToDouble(eodResponseInfo.adjClose, provider);
                aChartDataEntry.simpleMovingAverage = aChartDataEntry.dailyPrices - 10;
                aChartDataEntry.expMovingAverage = aChartDataEntry.dailyPrices - 20;
                chartData.Add(aChartDataEntry);
            }

            Console.WriteLine("chartData.Count {0}", chartData.Count);
        }

        public ChartData[] getChartData()
        {
            return chartData.ToArray();
        }

    }
}
