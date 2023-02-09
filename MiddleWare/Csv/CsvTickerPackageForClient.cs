using Analyzer.MiddleWare.TopOfBook;

namespace Analyzer.MiddleWare.Csv
{
    public class CsvTickerPackageForClient
    {
        public CsvTickerResponse[] csvTickerResponses { get; set; }
        public string packageType { get; set; }

        public CsvTickerPackageForClient(CsvTickerResponse[] csvTickerResponsesIn, string packageTypeIn)
        {
            csvTickerResponses = csvTickerResponsesIn;
            packageType = packageTypeIn;
        }
    }
}
