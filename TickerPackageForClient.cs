namespace Analyzer
{
    public class TickerPackageForClient
    {
        public ChartData[] calculatedPrices { get; set; }
        public string packageType { get; set; }
        public string firstReferenceClosingPrice { get; set; }
        public string lastReferenceClosingPrice { get; set; }

        public TickerPackageForClient(ChartData[] calculatedPricesIn, string firstReferenceClosingPriceIn, string lastReferenceClosingPriceIn, string packageTypeIn)
        {
            this.calculatedPrices = calculatedPricesIn;
            this.packageType = packageTypeIn;
            this.firstReferenceClosingPrice = firstReferenceClosingPriceIn;
            this.lastReferenceClosingPrice = lastReferenceClosingPriceIn;
        }
    }
}
