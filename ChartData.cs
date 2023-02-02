namespace Analyzer
{
    public class ChartData
    {
        public string name { get; set; }
        public double dailyPrices { get; set; }
        public double simpleMovingAverage { get; set; }
        public double expMovingAverage { get; set; }

        public ChartData()
        {
            name = "";
            dailyPrices = 0.0;
            simpleMovingAverage = 0.0;
            expMovingAverage = 0.0;
        }
    }
}

