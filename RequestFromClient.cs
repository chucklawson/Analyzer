using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BackEndReactWebApplication
{
    //{"operation":"OBTAIN_TICKER_VALUES","ticker":"nvda","startDate":"2023-04-28","endDate":"2023-01-30"}
       
    public class RequestFromClient
    {
        public string operation { get; set; }
        public string ticker { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        [JsonConstructor]
        public RequestFromClient() { }

        public RequestFromClient(string operationIn, string tickerIn, string startDateIn, string endDateIn)
        {
            operation = operationIn;
            ticker = tickerIn;
            startDate = startDateIn;
            endDate = endDateIn;
        }

    public string ToString()
        {
            StringBuilder stringOut = new StringBuilder("");
            stringOut.Append("operation: " + operation);
            stringOut.Append(", ticker: " + ticker);
            stringOut.Append(", startDate: " + startDate);
            stringOut.Append(", endDate: " + endDate);

            return stringOut.ToString();
        }
    }
}
