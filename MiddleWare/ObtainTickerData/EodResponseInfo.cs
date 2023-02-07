using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.MiddleWare.ObtainTickerData
{
    public class EodResponseInfo : IComparable
    {
        public string date { get; set; }
        public string close { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string open { get; set; }
        public string volume { get; set; }
        public string adjClose { get; set; }
        public string adjHigh { get; set; }
        public string adjLow { get; set; }
        public string adjOpen { get; set; }
        public string adjVolume { get; set; }
        public string divCash { get; set; }
        public string splitFactor { get; set; }

        public DateTime itemDate { get; set; }

        public EodResponseInfo()
        {
            date = "";
            close = "";
            high = "";
            low = "";
            open = "";
            volume = "";
            adjClose = "";
            adjHigh = "";
            adjLow = "";
            adjOpen = "";
            adjVolume = "";
            divCash = "";
            splitFactor = "";

            itemDate = DateTime.Now;
        }

        int IComparable.CompareTo(object obj)
        {
            EodResponseInfo itemComparingTo = (EodResponseInfo)obj;

            if (itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return 0;
            }
            if (itemDate.Date > itemComparingTo.itemDate.Date)
            {
                return 1;
            }
            return -1;
        }

        public void SetDateTime()
        {
            //Console.WriteLine("Need to convert {0}", this.date);
            DateTime tempDateTime = Convert.ToDateTime(date.Substring(0, date.IndexOf("T", 0)));
            itemDate = new DateTime(tempDateTime.Year, tempDateTime.Month, tempDateTime.Day, 0, 0, 0);
            //Console.WriteLine("Ending up with {0}", this.itemDate);
        }

        public void ReSetDateTimeBasedOnDateTimeIn(DateTime DateTimeToAdjust)
        {
            itemDate = new DateTime(DateTimeToAdjust.Year, DateTimeToAdjust.Month, DateTimeToAdjust.Day, 0, 0, 0);            
        }

        public bool AreTheseEqual(EodResponseInfo itemComparingTo)
        {
            if (itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return true;
            }
            return false;
        }

        public bool AreTheseGreaterOrEqual(EodResponseInfo itemComparingTo)
        {
            if (itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return true;
            }
            if (itemDate.Date > itemComparingTo.itemDate.Date)
            {
                return true;
            }
            return false;
        }

        public static long StaticBinarySearchForEodResponseEntry(List<EodResponseInfo> EodResponseInfoList,
                                                                            EodResponseInfo aEodResponseInfoEntryToFind,
                                                                            long startAddress,
                                                                            long endAddress)
        {
            if (EodResponseInfoList.Count < 1)
                return -1;

            EodResponseInfo startEodResponseInfoItem = EodResponseInfoList[(int)startAddress];

            if (startEodResponseInfoItem.AreTheseEqual(aEodResponseInfoEntryToFind))
            {
                return startAddress;
            }

            long middle = endAddress >= startAddress ? (endAddress - startAddress) / 2 + startAddress : (startAddress - endAddress) / 2 + startAddress;

            if (middle < 0 || middle >= endAddress)
                return -1;

            EodResponseInfo middleEodResponseInfoItem = EodResponseInfoList[(int)middle];


            if (middleEodResponseInfoItem.AreTheseGreaterOrEqual(aEodResponseInfoEntryToFind))
            {
                return StaticBinarySearchForEodResponseEntry(EodResponseInfoList,
                                                        aEodResponseInfoEntryToFind,
                                                        startAddress,
                                                        middle);
            }
            else
            {
                return StaticBinarySearchForEodResponseEntry(EodResponseInfoList,
                                                        aEodResponseInfoEntryToFind,
                                                        middle + 1,
                                                        endAddress);
            }
        }

        public string ToString()
        {
            StringBuilder stringOut = new StringBuilder("");
            stringOut.Append("date: " + date.Substring(0, date.IndexOf("T", 0)));

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime aDate = DateTime.ParseExact(date.Substring(0, date.IndexOf("T", 0)), "yyyy-MM-dd", provider);
            //Console.WriteLine("aDate: {0}", aDate.ToString());

            stringOut.Append(", close: " + close);
            stringOut.Append(", high: " + high);
            stringOut.Append(", low: " + low);
            stringOut.Append(", open: " + open);
            stringOut.Append(", volume: " + volume);
            stringOut.Append(", adjClose: " + adjClose);
            stringOut.Append(", adjHigh: " + adjHigh);
            stringOut.Append(", adjLow: " + adjLow);
            stringOut.Append(", adjOpen: " + adjOpen);
            stringOut.Append(", adjVolume: " + adjVolume);
            stringOut.Append(", divCash: " + divCash);
            stringOut.Append(", splitFactor: " + splitFactor);
            stringOut.Append(", DateTime: " + itemDate);

            return stringOut.ToString();
        }
    }
}

/*
 * 
 * { "date":"2012-01-31T00:00:00.000Z","close":60.02,"high":61.0,"low":57.84,"open":58.45,"volume":8812200,"adjClose":97.287582971,"adjHigh":98.8760839925,"adjLow":93.7539786578,"adjOpen":94.7427394977,"adjVolume":4406100,"divCash":0.0,"splitFactor":1.0}
 * 
 * 
[{"date":"2021-01-29T00:00:00.000Z","close":340.18,"high":354.645,"low":335.37,"open":345.02,"volume":67318058,"adjClose":330.0645637696,"adjHigh":344.0994391735,"adjLow":325.3975917202,"adjOpen":334.7606437526,"adjVolume":67318058,"divCash":0.0,"splitFactor":1.0},{"date":"2021-02-26T00:00:00.000Z","close":349.59,"high":362.37,"low":341.4,"open":343.63,"volume":65076646,"adjClose":339.1947523317,"adjHigh":351.5947321217,"adjLow":331.2482864099,"adjOpen":333.4119761542,"adjVolume":65076646,"divCash":0.0,"splitFactor":1.0},{"date":"2021-03-31T00:00:00.000Z","close":364.3,"high":366.05,"low":341.915,"open":354.55,"volume":115909237,"adjClose":354.6932773101,"adjHigh":356.1731943606,"adjLow":331.7479726065,"adjOpen":344.0072640499,"adjVolume":115909237,"divCash":1.2625,"splitFactor":1.0},{"date":"2021-04-30T00:00:00.000Z","close":383.57,"high":386.74,"low":366.03,"open":366.2,"volume":77401939,"adjClose":373.4551204442,"adjHigh":376.5415263983,"adjLow":356.3776565847,"adjOpen":356.5431736232,"adjVolume":77401939,"divCash":0.0,"splitFactor":1.0},{"date":"2021-05-31T00:00:00.000Z","close":386.13,"high":388.68,"low":372.13,"open":385.56,"volume":89691613,"adjClose":375.9476123188,"adjHigh":378.430367897,"adjLow":362.3167973796,"adjOpen":375.3926434248,"adjVolume":89691613,"divCash":0.0,"splitFactor":1.0},{"date":"2021-06-30T00:00:00.000Z","close":393.52,"high":394.45,"low":382.52,"open":388.5,"volume":68852165,"adjClose":384.4416745883,"adjHigh":385.0385790993,"adjLow":372.4328093238,"adjOpen":378.2551145621,"adjVolume":68852165,"divCash":1.3329,"splitFactor":1.0},{"date":"2021-07-30T00:00:00.000Z","close":403.15,"high":406.13,"low":387.93,"open":394.3,"volume":87866050,"adjClose":393.8495149173,"adjHigh":396.7607676879,"adjLow":378.9806333173,"adjOpen":385.203680347,"adjVolume":87866050,"divCash":0.0,"splitFactor":1.0},{"date":"2021-08-31T00:00:00.000Z","close":415.05,"high":416.56,"low":400.92,"open":404.78,"volume":81058621,"adjClose":405.4749873904,"adjHigh":406.9501523849,"adjLow":391.6709599917,"adjOpen":395.441911567,"adjVolume":81058621,"divCash":0.0,"splitFactor":1.0},{"date":"2021-09-30T00:00:00.000Z","close":394.4,"high":417.44,"low":394.34,"open":416.05,"volume":103248307,"adjClose":386.5633979185,"adjHigh":407.8098511896,"adjLow":386.4443820311,"adjOpen":406.4519178503,"adjVolume":103248307,"divCash":1.3084,"splitFactor":1.0},{"date":"2021-10-29T00:00:00.000Z","close":422.16,"high":422.515,"low":391.96,"open":396.2262,"volume":93894175,"adjClose":413.7718155813,"adjHigh":414.1197618446,"adjLow":384.1718799395,"adjOpen":388.3533119075,"adjVolume":93894175,"divCash":0.0,"splitFactor":1.0},{"date":"2021-11-30T00:00:00.000Z","close":419.06,"high":435.41,"low":418.63,"open":423.19,"volume":100053866,"adjClose":410.7334115916,"adjHigh":426.7585423116,"adjLow":410.3119555543,"adjOpen":414.7813498102,"adjVolume":100053866,"divCash":0.0,"splitFactor":1.0},{"date":"2021-12-31T00:00:00.000Z","close":436.57,"high":440.36,"low":412.77,"open":424.47,"volume":142126854,"adjClose":429.4363354138,"adjHigh":433.164405852,"adjLow":404.5683918834,"adjOpen":416.0359166188,"adjVolume":142126854,"divCash":1.5329,"splitFactor":1.0},{"date":"2022-01-31T00:00:00.000Z","close":413.69,"high":441.26,"low":386.84,"open":437.93,"volume":209277200,"adjClose":406.9302004199,"adjHigh":434.0496996236,"adjLow":380.5189362335,"adjOpen":430.7741126686,"adjVolume":209277200,"divCash":0.0,"splitFactor":1.0}]
*/
