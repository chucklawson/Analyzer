
namespace Analyzer
{
    public class DataPoint : IComparable
    {
        public string date { get; set; }
        public double calculatedValue { get; set; }
        public DateTime itemDate { get; set; }

        public DataPoint(string dateIn, double calculatedValueIn)
        {
            date = dateIn;
            calculatedValue = calculatedValueIn;
            DateTime tempDateTime = Convert.ToDateTime(this.date.Substring(0, this.date.IndexOf("T", 0)));
            //Console.WriteLine("DataPoint, date to convert {0}", this.date);
            this.itemDate = new DateTime(tempDateTime.Year, tempDateTime.Month, tempDateTime.Day, 0, 0, 0);
            //Console.WriteLine("DataPoint, Converted Date {0}", this.itemDate);
        }

        public DataPoint(DateTime dateTimeIn)
        {
            this.itemDate = dateTimeIn;
        }


        int IComparable.CompareTo(Object obj)
        {
            DataPoint itemComparingTo = (DataPoint)obj;

            if (this.itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return 0;
            }
            if (this.itemDate.Date > itemComparingTo.itemDate.Date)
            {
                return 1;
            }
            return -1;
        }

        public void SetDateTime()
        {
            //Console.WriteLine("Need to convert {0}", this.date);
            DateTime tempDateTime = Convert.ToDateTime(this.date.Substring(0, this.date.IndexOf("T", 0)));
            this.itemDate = new DateTime(tempDateTime.Year, tempDateTime.Month, tempDateTime.Day, 0, 0, 0);
            //Console.WriteLine("Ending up with {0}", this.itemDate);
        }

        public bool AreTheseEqual(DataPoint itemComparingTo)
        {
            if (this.itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return true;
            }
            return false;
        }

        public bool AreTheseGreaterOrEqual(DataPoint itemComparingTo)
        {
            if (this.itemDate.Date == itemComparingTo.itemDate.Date)
            {
                return true;
            }
            if (this.itemDate.Date > itemComparingTo.itemDate.Date)
            {
                return true;
            }
            return false;
        }

        public static long StaticBinarySearchForDataPointEntry(List<DataPoint> DataPointList,
                                                                            DataPoint aDataPointEntryToFind,
                                                                            long startAddress,
                                                                            long endAddress)
        {
            if (DataPointList.Count < 1)
                return -1;

            DataPoint startDataPointItem = (DataPoint)DataPointList[(int)startAddress];

            if (startDataPointItem.AreTheseEqual(aDataPointEntryToFind))
            {
                return startAddress;
            }

            long middle = (endAddress >= startAddress) ? ((endAddress - startAddress) / 2 + startAddress) : ((startAddress - endAddress) / 2 + startAddress);

            if ((middle < 0) || (middle >= endAddress))
                return -1;

            DataPoint middleDataPointItem = (DataPoint)DataPointList[(int)middle];


            if (middleDataPointItem.AreTheseGreaterOrEqual(aDataPointEntryToFind))
            {
                return StaticBinarySearchForDataPointEntry(DataPointList,
                                                        aDataPointEntryToFind,
                                                        startAddress,
                                                        middle);
            }
            else
            {
                return StaticBinarySearchForDataPointEntry(DataPointList,
                                                        aDataPointEntryToFind,
                                                        middle + 1,
                                                        endAddress);
            }
        }


        public string ToString()
        {
            return "DataPoint: " + "date: " + this.date + ", calculatedValue: " + this.calculatedValue + ", itemDate: " + this.itemDate;
        }
    }
}
