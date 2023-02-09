using System.Globalization;
using System.Reflection.PortableExecutable;
using System;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Text;

namespace Analyzer.MiddleWare.Csv
{
    public class GetCsvData
    {
        public static WebSocket? webSocket { get; set; }
        public static RequestFromClient? requestFromClient { get; set; }

        public static async void parseACsvFile()
        {
            string path = @"D:\Market\Holdings.csv";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int linesProcessed = 0;
                    List<CsvTickerResponse> csvTickerResponseList = new List<CsvTickerResponse>();
                    while (sr.Peek() >= 0)
                    {
                        string theLine = sr.ReadLine();
                        if (linesProcessed >= 3) { 
                            string ticker= theLine.Substring(0,theLine.IndexOf(','));
                            if (ticker.Length>0)
                            {
                                CsvTickerResponse aCsvTickerResponse = new CsvTickerResponse();
                                aCsvTickerResponse.ticker = ticker;
                                Console.WriteLine(aCsvTickerResponse.ToString());
                                csvTickerResponseList.Add(aCsvTickerResponse);
                            }
                        }
                        ++linesProcessed;
                    }
                    CsvTickerPackageForClient aCsvTickerPackageForClient = new CsvTickerPackageForClient(csvTickerResponseList.ToArray(), "OBTAIN_CSV_TICKER_DATA");
                    Console.WriteLine("Ticker count: {0}",aCsvTickerPackageForClient.csvTickerResponses.Length);
                    // Generate json from the array of objects EodResponseInfo[]
                    string jsonOut = JsonConvert.SerializeObject(aCsvTickerPackageForClient);

                    // Sending json back to the client right here
                    // Turn int back into byte data to send as json
                    byte[] data = Encoding.ASCII.GetBytes($"{jsonOut}");
                    await webSocket.SendAsync(data, WebSocketMessageType.Text,
                        true, CancellationToken.None);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

    }
}
