namespace Analyzer
{
    public class PackageForClient
    {
        public ChartData[] calculatedPrices { get; set; }

        public PackageForClient(ChartData[] calculatedPricesIn)
        {
            this.calculatedPrices = calculatedPricesIn;
        }
    }
}
