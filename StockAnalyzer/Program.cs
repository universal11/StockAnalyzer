using Newtonsoft.Json.Linq;
using StockAnalyzer.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<StockListItem> stockListItems = Client.getStockListItems();
            Dictionary<int, Dictionary<int, StockDate>> dates = Client.getUniqueStockDates(stockListItems);//.ToDictionary(sd=>sd, x=>true);


            
            foreach(StockListItem stockListItem in stockListItems) {
                StockDate stockDate = stockListItem.getStockDate();
                if(dates[stockDate.month][stockDate.day] != null) {
                    if(!stockListItem.isProfit())
                    {
                        System.Console.WriteLine($"No Profit | Month: {dates[stockDate.month][stockDate.day].month} | Day: {dates[stockDate.month][stockDate.day].day} | Year: {dates[stockDate.month][stockDate.day].year} | Open: {stockListItem.open} | Close: {stockListItem.close}");
                        dates[stockDate.month][stockDate.day] = null;
                        
                    }
                }
                
            }

            foreach(KeyValuePair<int, Dictionary<int, StockDate>> month in dates)
            {
                foreach(KeyValuePair<int, StockDate> date in month.Value)
                {
                    if(date.Value != null) {
                    
                        System.Console.WriteLine($"Month: {date.Value.month} | Day: {date.Value.day}");
                    }
                    
                }
            }


            System.Console.WriteLine("Complete!");

            System.Console.ReadKey();
        }
    }
}
