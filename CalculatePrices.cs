using Analyzer;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Analyzer
{
    public class CalculatePrices
    {

        private List<ChartData> chartData = new List<ChartData>();
        private List<DataPoint> simpleMovingAverageData= new List<DataPoint>();


        public CalculatePrices() { }

        public CalculatePrices(DateTime originalStartDate,EodResponseInfo[] eodResponseInfos)
        {
            simpleMovingAverageData=SimpleMovingAverages.buildSimpleMovingAverages(originalStartDate,eodResponseInfos, 33);
            loadDailyPrices(originalStartDate,eodResponseInfos);
        }

        private void loadDailyPrices(DateTime originalStartDate,EodResponseInfo[] eodResponseInfos)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberDecimalDigits = 2;
            
            foreach (EodResponseInfo eodResponseInfo in eodResponseInfos)
            {
                ChartData aChartDataEntry = new ChartData();

                DateTime thisItmesDateTime = Convert.ToDateTime(eodResponseInfo.date);

                //if (DateTime.Compare(thisItmesDateTime,originalStartDate.AddDays(-1))>=0)
                if (DateTime.Compare(eodResponseInfo.itemDate, originalStartDate) >= 0)
                    {
                    aChartDataEntry.name = eodResponseInfo.itemDate.ToShortDateString();
                    aChartDataEntry.dailyPrices = Convert.ToDouble(eodResponseInfo.adjClose, provider);
                    DataPoint dataPointToFind = new DataPoint(eodResponseInfo.itemDate);
                    long addressOfMatchingSimpleMovingAverageDataPointEntry = DataPoint.StaticBinarySearchForDataPointEntry(simpleMovingAverageData,
                                                                            dataPointToFind,
                                                                            0L,
                                                                            simpleMovingAverageData.Count-1);

                    DataPoint dataPointToUse = new DataPoint(eodResponseInfo.itemDate);
                    if (addressOfMatchingSimpleMovingAverageDataPointEntry >= 0)
                    {
                        dataPointToUse = simpleMovingAverageData.ElementAt((int)addressOfMatchingSimpleMovingAverageDataPointEntry);
                    }

                    aChartDataEntry.simpleMovingAverage = dataPointToUse.calculatedValue;
                    aChartDataEntry.expMovingAverage = aChartDataEntry.dailyPrices - 20;
                    chartData.Add(aChartDataEntry);
                }
            }

            //Console.WriteLine("chartData.Count {0}", chartData.Count);
        }

        public ChartData[] getChartData()
        {
            return chartData.ToArray();
        }

    }
}
