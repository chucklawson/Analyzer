using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using System;
using System.Collections.Generic;

namespace Analyzer
{
    public class SimpleMovingAverages
    {
        public static List<DataPoint> calculatedAverages = new List<DataPoint>();

        public static List<DataPoint> buildSimpleMovingAverages(DateTime originalStartDate,
                                                                EodResponseInfo[] eodResponseInfo,                                                                
                                                                long numberOfDaystoLookBack)
        {
            calculatedAverages.Clear();

            // feed in EodResponseInfo[]

            try
            {
                //Console.WriteLine("Entries to evaluate from eodResponseInfo {0} ", eodResponseInfo.Length);

                List<DataPoint> dataPoints = GenerateTheDataPointsSimpleMovingAverage(numberOfDaystoLookBack,
                                                                                           eodResponseInfo);

                //Console.WriteLine("originalStartDate {0} ", originalStartDate.ToString());

                //Console.WriteLine( "SimpleMovingAverages dataPoints: {0} ", dataPoints.Count);

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
            } catch (Exception ex)
            { 
                Console.Write("runGetRequest Exception: " + ex.Message);
            }

            return calculatedAverages;
        }

        private static List<DataPoint> GenerateTheDataPointsSimpleMovingAverage(long numberOfDaystoLookBack,
                                                                                 EodResponseInfo[] eodResponseInfo)
        {
            if ((long)eodResponseInfo.Length < numberOfDaystoLookBack)
                return new List<DataPoint>();

            List<DataPoint> dataPoints = new List<DataPoint>();

            // this generates an up to the date average

            for (long i = (int)numberOfDaystoLookBack; i < (eodResponseInfo.Length); ++i)
            {
                double tempDouble = GenerateOneDataPoint(i, numberOfDaystoLookBack, eodResponseInfo);
                DataPoint aDataPoint = new DataPoint(eodResponseInfo[(int)i].date, tempDouble);
                dataPoints.Add(aDataPoint);
                //Console.WriteLine("Added aDataPoint: {0}", aDataPoint.ToString());
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
                summedCloses += Convert.ToDouble((eodResponseInfo[(int)i].close));
            }

            double devisor = (double)numberOfDaystoLookBack;
            return (summedCloses / devisor);
        }
    }
}
