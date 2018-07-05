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

            /*
            foreach(StockListItem stockListItem in stockListItems) {
                System.Console.WriteLine($"Year: {stockListItem.dateTime.Year} | Date: {stockListItem.dateTime.ToShortDateString()} | Open: {stockListItem.open} | Close: {stockListItem.close} | High: {stockListItem.high} | Low: {stockListItem.low} | Volume: {stockListItem.volume}");
            }
            */

            foreach(StockDate stockDate in Client.getStockDates(stockListItems)) {
                System.Console.WriteLine($"Month: {stockDate.month} | Day: {stockDate.day}");
            }

            System.Console.ReadKey();
        }
    }
}
