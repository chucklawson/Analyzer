using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Analyzer
{
    public class GetTopOfBook
    {
        public static WebSocket? webSocket { get; set; }
        public static RequestFromClient? requestFromClient { get; set; }

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
                            foreach (TopOfBookResponse topOfBookResponse in topOfBookResponses)
                            {
                                Console.WriteLine("topOfBookResponse: {0} ", topOfBookResponse.ToString());
                            }

                            TopOfBookPackageForClient aPackageForClient = new TopOfBookPackageForClient(topOfBookResponses, "OBTAIN_TOP_OF_BOOK");

                            // Generate json from the array of objects EodResponseInfo[]
                            string jsonOut = JsonConvert.SerializeObject(aPackageForClient);

                            // Sending json back to the client right here
                            // Turn int back into byte data to send as json
                            byte[] data = Encoding.ASCII.GetBytes($"{jsonOut}");
                            await webSocket.SendAsync(data, WebSocketMessageType.Text,
                                true, CancellationToken.None);
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
