using Newtonsoft.Json.Linq;
using StockAnalyzer.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockAnalyzer
{
    class Program
    {
        static void getStockResults(string symbol) {
            List<StockListItem> stockListItems = Client.getStockListItems(symbol);
            Dictionary<int, Dictionary<int, List<StockListItem>>> dates = Client.getUniqueStockDates(stockListItems);//.ToDictionary(sd=>sd, x=>true);

            foreach(StockListItem stockListItem in stockListItems)
            {
                StockDate stockDate = stockListItem.getStockDate();
                if(dates[stockDate.month][stockDate.day] != null)
                {
                    if(!stockListItem.isProfit())
                    {
                        //System.Console.WriteLine($"No Profit | Symbol: {symbol} | Month: {dates[stockDate.month][stockDate.day].month} | Day: {dates[stockDate.month][stockDate.day].day} | Year: {dates[stockDate.month][stockDate.day].year} | Open: {stockListItem.open} | Close: {stockListItem.close}");
                        dates[stockDate.month][stockDate.day] = null;

                    }
                }

            }

            foreach(KeyValuePair<int, Dictionary<int, List<StockListItem>>> month in dates)
            {
                
                foreach(KeyValuePair<int, List<StockListItem>> date in month.Value)
                {
                    if(date.Value != null)
                    {
                        foreach(StockListItem stockListItem in date.Value) {
                            StockDate stockDate = stockListItem.getStockDate();
                            System.Console.WriteLine($"Symbol: {symbol} | Year: {stockDate.year} | Date: {stockDate.month}/{stockDate.day} | Open: {stockListItem.open} | Close: {stockListItem.close} | Change: {stockListItem.getChange()}");
                        }
                        
                    }

                }
            }

        }

        static void Main(string[] args)
        {
            
            string filePath = null;

            for(int i=0; i < args.Length; i++) {
                string arg = args[i];
                switch(arg) {
                    case "--source":
                        if( (i + 1) <= args.Length )
                        {
                            filePath = args[i + 1];
                        }
                        break;
                    default:
                        break;
                }
            }

            if(filePath != null && File.Exists(filePath))
            {
                using(StreamReader reader = new StreamReader(filePath))
                {
                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] fields = line.Split(',');
                        if(fields[0].Trim() != "")
                        {
                            System.Console.WriteLine($"Analyzing Symbol: {fields[0].Trim()}");
                            Program.getStockResults(fields[0].Trim());
                            Thread.Sleep(15000);
                        }


                    }
                }
                System.Console.WriteLine("Complete!");
            }
            else {
                System.Console.WriteLine("Please provide a file path!");
            }
            
            


            

            System.Console.ReadKey();
        }
    }
}
