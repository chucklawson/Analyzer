using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Analyzer.MiddleWare.TopOfBook
{
    public class GetTopOfBookForEodEntry
    {
        public static RequestFromClient? requestFromClient { get; set; }
        public static string currentValue = "";

        // https://api.tiingo.com/iex/?tickers=aapl,spy&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3"
        //private static final String URL_LEFT = "https://api.tiingo.com/iex/?tickers=";
        //private static final String URL_RIGHT = "&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3";

        public static async void HTTP_GET()
        {
            string Url_Left = "https://api.tiingo.com/iex/?tickers=";
            string Url_Right = "&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3&format=json";

            using (var client = new HttpClient())
            {
                string urlToGet = "";
                urlToGet = string.Concat(urlToGet, Url_Left.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, requestFromClient.ticker.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);                

                urlToGet = string.Concat(urlToGet, Url_Right.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                Console.WriteLine("UrlToGet: " + urlToGet);

                using (var request = new HttpRequestMessage(HttpMethod.Get, urlToGet))
                {
                    using (var response = await client.SendAsync(request))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            //Console.WriteLine("responseData" + responseData.ToString());
                            TopOfBookResponse[] topOfBookResponses = JsonConvert.DeserializeObject<TopOfBookResponse[]>(responseData);

                            Console.WriteLine("topOfBookResponses.Length: " + topOfBookResponses.Length);
                            Console.WriteLine("topOfBookResponses[0].tngoLast: " + topOfBookResponses[0].tngoLast);
                            currentValue = topOfBookResponses[0].tngoLast;
                            foreach (TopOfBookResponse topOfBookResponse in topOfBookResponses)
                            {
                                Console.WriteLine("topOfBookResponse for EOD: {0} ", topOfBookResponse.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Request Failed");
                        }
                    }
                }
            }
        }
    }
}
