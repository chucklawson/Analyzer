using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace Analyzer.MiddleWare.ObtainTickerData
{
    public class GetOneSetOfData
    {
        public static WebSocket? webSocket { get; set; }
        public static RequestFromClient? requestFromClient { get; set; }

        public static async void HTTP_GET()
        {
            string Url_Left = "https://api.tiingo.com/tiingo/daily/";
            string Url_Prices = "/prices";
            string Url_StartDate = "?startDate=";
            string Url_EndDate = "&endDate=";
            string Url_Right = "&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3&format=json";

            // Example of what we are building up
            var Url = string.Format("https://api.tiingo.com/tiingo/daily/voo/prices?startDate=2021-1-31&endDate=2022-1-1&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3&format=json");
            //var Url = string.Format("https://api.tiingo.com/tiingo/daily/voo/prices?startDate=2021-1-31&endDate=2022-1-1&token=d8f860c1c310308deb65f4ecdcd4d2d94711d6e3&format=json&resampleFreq=daily");
            // different vendor
            var Url_2 = string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=voo&outputsize=compact&apikey=2DL6BOCVXMLKJH3X");

            using (var client = new HttpClient())
            {
                //Console.WriteLine(Url_Left.Trim());
                //Console.WriteLine(requestFromClient.ticker.Trim());
                //Console.WriteLine(Url_Right.Trim());

                string urlToGet = "";
                urlToGet = string.Concat(urlToGet, Url_Left.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, requestFromClient.ticker.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, Url_Prices);
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, Url_StartDate);
                //Console.WriteLine("UrlToGet: " + urlToGet);

                DateTime originalStartDate = DateTime.Parse(requestFromClient.startDate.Trim());
                int yearsToLookBack = 1;
                string startingDateToUse = BuildDateToStartFrom(yearsToLookBack, requestFromClient.startDate.Trim());
                //Console.WriteLine("startingDateToUse: {0}", startingDateToUse);
                urlToGet = string.Concat(urlToGet, startingDateToUse);
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, Url_EndDate);
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, requestFromClient.endDate.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                urlToGet = string.Concat(urlToGet, Url_Right.Trim());
                //Console.WriteLine("UrlToGet: " + urlToGet);

                //Console.WriteLine("UrlToGet: " + urlToGet);

                using (var request = new HttpRequestMessage(HttpMethod.Get, urlToGet))
                {
                    using (var response = await client.SendAsync(request))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            EodResponseInfo[] eodResponseInfos = JsonConvert.DeserializeObject<EodResponseInfo[]>(responseData);

                            foreach (EodResponseInfo eodResponseInfo in eodResponseInfos)
                            {
                                eodResponseInfo.SetDateTime();
                            }
                            Array.Sort(eodResponseInfos);
                            foreach (EodResponseInfo eodResponseInfo in eodResponseInfos)
                            {
                                //Console.WriteLine("eodResponseInfo: {0} ", eodResponseInfo.ToString());
                            }

                            // Console.WriteLine("eodResponseInfos count: " + eodResponseInfos.Count());

                            CalculatePrices calculatedPrices = new CalculatePrices(originalStartDate, eodResponseInfos);

                            TickerPackageForClient aPackageForClient = new TickerPackageForClient(calculatedPrices.getChartData(), "OBTAIN_TICKER_VALUES");

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

        private static string BuildDateToStartFrom(int yearsToLookBack, string referenceDateIn)
        {
            //Console.WriteLine("referenceDateIn: {0}", referenceDateIn);

            DateTime referenceDate = DateTime.Parse(referenceDateIn);

            //DateTime dateToUseAsDate = referenceDate.AddDays((-1 * daysToGoBack));
            DateTime dateToUseAsDate = referenceDate.AddYears(-yearsToLookBack);
            //Console.WriteLine("dateToUseAsDate: {0}", dateToUseAsDate.ToString("yyyy-MM-dd"));
            return dateToUseAsDate.ToString("yyyy-MM-dd");
        }
    }
}
