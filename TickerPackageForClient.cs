namespace Analyzer
{
    public class TickerPackageForClient
    {
        public ChartData[] calculatedPrices { get; set; }
        public string packageType { get; set; }

        public TickerPackageForClient(ChartData[] calculatedPricesIn, string packageTypeIn)
        {
            this.calculatedPrices = calculatedPricesIn;
            packageType = packageTypeIn;
        }
    }
}
