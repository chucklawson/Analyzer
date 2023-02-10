using System.Collections.Generic;
using System.Security.Cryptography;

namespace Analyzer.MiddleWare.Csv
{
    public class CsvTickerResponse
    {
        public string ticker { get; set; }       
        public string companyName { get; set; }
        public string costBasis { get; set; }

        public CsvTickerResponse()
        {
            ticker = "";
            costBasis = "";
            companyName = "";
        }

        public string ToString()
        {
            return "CsvTicker: " + "ticker: " + ticker + ", companyName: " + companyName + ", costBasis: " + costBasis;
        }

    }
}
