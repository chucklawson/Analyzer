using System.Collections.Generic;
using System.Security.Cryptography;

namespace Analyzer.MiddleWare.Csv
{
    public class CsvTickerResponse
    {
        public string ticker { get; set; }

        public CsvTickerResponse()
        {
            ticker = "";
        }

        public string ToString()
        {
            return "CsvTicker: " + "ticker: " + ticker;
        }

    }
}
