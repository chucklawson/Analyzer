namespace Analyzer.TopOfBook
{
    public class TopOfBookResponse
    {
        public string ticker { get; set; }
        public string timestamp { get; set; }
        public string quoteTimestamp { get; set; }
        public string lastSaleTimeStamp { get; set; }
        public string last { get; set; }
        public string lastSize { get; set; }
        public string tngoLast { get; set; }
        public string prevClose { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string mid { get; set; }
        public string volume { get; set; }
        public string bidSize { get; set; }
        public string bidPrice { get; set; }
        public string askSize { get; set; }
        public string askPrice { get; set; }

        public TopOfBookResponse()
        {
            ticker = "";
            timestamp = "";
            quoteTimestamp = "";
            lastSaleTimeStamp = "";
            last = "";
            lastSize = "";
            tngoLast = "";
            prevClose = "";
            open = "";
            high = "";
            low = "";
            mid = "";
            volume = "";
            bidSize = "";
            bidPrice = "";
            askSize = "";
            askPrice = "";
        }

        public string ToString()
        {
            return "TopOfBookResponse: \n" + "ticker: " + ticker
                    + ", timestamp: " + timestamp + ", quoteTimestamp: "
                    + quoteTimestamp + ", lastSaleTimeStamp: "
                    + lastSaleTimeStamp + ", last: " + last
                    + ", lastSize: " + lastSize + ", tngoLast: "
                    + tngoLast + ", prevClose: " + prevClose
                    + ", open: " + open + ", high: " + high
                    + ", low: " + low + ", mid: " + mid
                    + ", volume: " + volume + ", bidSize: "
                    + bidSize + ", bidPrice: " + bidPrice
                    + ", askSize: " + askSize + ", askPrice: "
                    + askPrice;
        }
    }
}
