using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace Analyzer
{
    public class ExponentialMovingAverages
    {
        public static List<DataPoint> calculatedAverages = new List<DataPoint>();

        public static List<DataPoint> buildExponetialMovingAverages(DateTime originalStartDate,
                                                                        EodResponseInfo[] eodResponseInfo,
                                                                        long numberOfDaystoLookBack)
        {
            calculatedAverages.Clear();

            // feed in EodResponseInfo[]
            try
            {
                //Console.WriteLine("Entries to evaluate from eodResponseInfo {0} ", eodResponseInfo.Length);

                List<DataPoint> dataPoints = GenerateTheDataPointsFormTwo_UpToDate(numberOfDaystoLookBack,
                                                                                           eodResponseInfo);

                //Console.WriteLine("originalStartDate {0} ", originalStartDate.ToString());

                //Console.WriteLine( "ExponentialMovingAverage dataPoints: {0} ", dataPoints.Count);

                foreach (DataPoint dataPointEntry in dataPoints)
                {
                    DateTime thisItmesDateTime = Convert.ToDateTime(dataPointEntry.date).AddDays(1);
                    //Console.WriteLine("first look at dataPointEntry {0}\n", dataPointEntry.ToString());

                    //Console.WriteLine("Comparing: {0} to: {1}", thisItmesDateTime.Date, originalStartDate.Date);
                    // these are the guys that should be getting sent back....

                    //Console.WriteLine("Comparing: {0} to: {1}, result: {2}", dataPointEntry.itemDate, originalStartDate.Date,
                    //                                                         DateTime.Compare(dataPointEntry.itemDate, originalStartDate.Date));

                    if (DateTime.Compare(dataPointEntry.itemDate, originalStartDate.Date) >= 0)
                    {
                        //Console.WriteLine("Adding thisItmesDateTime {0} ", thisItmesDateTime.ToString());
                        //Console.WriteLine("Adding itemDate {0} ", dataPointEntry.itemDate.ToShortDateString());
                        //Console.WriteLine("dataPointEntry {0}\n", dataPointEntry.ToString());
                        calculatedAverages.Add(dataPointEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("runGetRequest Exception: " + ex.Message);
            }

            return calculatedAverages;
        }

        private static List<DataPoint> GenerateTheDataPointsFormTwo_UpToDate(long howManyDaysInAverage,
                                                                              EodResponseInfo[] eodResponseInfo)
        {
            //Console.WriteLine("eodResponseInfo.Length: {0}, howManyDaysInAverage: {1}", eodResponseInfo.Length, howManyDaysInAverage);
            if ((long)eodResponseInfo.Length < howManyDaysInAverage)
                return new List<DataPoint>();

            List<DataPoint> dataPoints = new List<DataPoint>();

            // get the first point as a simple moving average.
            long referenceAddressForSimpleMovingAverage = howManyDaysInAverage;
            double theFirstValue = GenerateOneDataPoint(referenceAddressForSimpleMovingAverage,
                                                        howManyDaysInAverage,
                                                        eodResponseInfo);
            String theFirstDate = eodResponseInfo[(int)(howManyDaysInAverage - 1)].date;

            //Console.WriteLine("theFirstDate: {0}, theFirstValue: {1}", theFirstDate, theFirstValue);

            dataPoints.Add(new DataPoint(theFirstDate, theFirstValue));

            // geneate the rest of them
            DataPoint previousDataPoint = dataPoints.ElementAt(0);


            // this generates an up to the date average
            for (long i = howManyDaysInAverage; i < (eodResponseInfo.Length); ++i)
            {
                double tempDouble = GenerateExponentialDataPointFormTwo(i,
                                                                        howManyDaysInAverage,
                                                                        previousDataPoint,
                                                                        eodResponseInfo);
                DataPoint aDataPoint = new DataPoint(eodResponseInfo[(int)i].date, tempDouble);
                dataPoints.Add(aDataPoint);
                previousDataPoint = aDataPoint;
            }
            return dataPoints;
        }

        // this geneates a simple moving average value for one datapoint
        private static double GenerateOneDataPoint(long startAddress,
                                                   long numberOfDaystoLookBack,
                                                   EodResponseInfo[] eodResponseInfo)
        {
            if (numberOfDaystoLookBack <= 0)
                return 0.0;

            if (startAddress < numberOfDaystoLookBack)
                return 0.0;

            long theSizeOfTheVector = (long)eodResponseInfo.Length;

            // collect values up to the day you are evaluating
            double summedCloses = 0.0;
            for (long i = ((startAddress + 1) - numberOfDaystoLookBack); i < (startAddress + 1); ++i)
            {
                summedCloses += Convert.ToDouble(eodResponseInfo[(int)i].adjClose);
            }

            double devisor = (double)numberOfDaystoLookBack;
            return (summedCloses / devisor);
        }
        // Exponential Moving Average Calculation: form two

        //Exponential Moving Averages can be specified in two ways - as a percent-based EMA or as a period-based EMA. A percent-based EMA has a percentage as it's single parameter while a period-based EMA has a parameter that represents the duration of the EMA.

        //The formula for an exponential moving average is:

        //EMA(current) = ( (Price(current) - EMA(prev) ) x Multiplier) + EMA(prev)

        //For a percentage-based EMA, "Multiplier" is equal to the EMA's specified percentage.
        //For a period-based EMA, "Multiplier" is equal to 2 / (1 + N) where N is the specified number of periods.

        //For example, a 10-period EMA's Multiplier is calculated like this:

        //This means that a 10-period EMA is equivalent to an 18.18% EMA.

        // The second period  calculation is as follows for a table for Eastman Kodak.
        // For the first period's exponential moving average, the simple moving average was used as the previous period's exponential moving average.
        // Close 61.33, previous periods EMA 63.682, current periods ems 63.254
        //(C - P) = (61.33 - 63.682) = -2.352 
        //(C - P) x K = -2.352 x .181818 = -0.4276 
        //((C - P) x K) + P = -0.4276 + 63.682 = 63.254 

        // With a 15 day moving average:
        // currentAddressToEvaluate = Starts at 15 and goes up as this method constantly is called to gererate one point at a time.
        // lengthOfAverage = 15
        // previousDataPoint the last datapoint 
        // EodResponseInfo[] eodResponseInfo = from querry
        // Logger is the logger
        private static double GenerateExponentialDataPointFormTwo(long currentAddressToEvaluate,
                                                                  long lengthOfAverage,
                                                                  DataPoint previousDataPoint,
                                                                  EodResponseInfo[] eodResponseInfo)
        {
            if (lengthOfAverage <= 0)
                return 0.0;

            long theSizeOfTheVector = (long)eodResponseInfo.Length;
            if ((theSizeOfTheVector - 1) < currentAddressToEvaluate)
                return 0.0;

            double numberOfTimePeriods = (double)lengthOfAverage;
            double multiplier = (2.0 / (numberOfTimePeriods + 1.0));

            //EMA(current) = ( (Price(current) - EMA(prev) ) x Multiplier) + EMA(prev)
            double theCurrentDaysClose = Convert.ToDouble(eodResponseInfo[(int)currentAddressToEvaluate].adjClose);
            //double testValue=((theCurrentDaysClose-previousDataPoint)*multiplier);
            double currentEMA = (((theCurrentDaysClose - previousDataPoint.calculatedValue) * multiplier)
                    + previousDataPoint.calculatedValue);
            //previousDataPoint = currentEMA;
            return currentEMA;
        }



    }
}
