using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Classes
{
    public class Client
    {

        public static List<StockDate> getStockDates(List<StockListItem> stockListItems) {
            //IDictionary<string, int> dict = new Dictionary<string, int>();
            List<StockDate> stockDates = new List<StockDate>();
            foreach(StockListItem stockListItem in stockListItems) {
                StockDate stockDate = new StockDate();
                stockDate.month = stockListItem.dateTime.Month;
                stockDate.day = stockListItem.dateTime.Day;
                stockDates.Add(stockDate);
            }
            return stockDates;
            //return stockDates.GroupBy(sd=> new { sd.month, sd.day }).ToList().OrderBy(sd=>sd.month).ThenBy(sd=>sd.day).ToList();
        }

        public static List<StockListItem> getStockListItems() {
            string url = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&apikey=IO5GL7XPD3B40CDZ&outputsize=full";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            List<StockListItem> stockListItems = new List<StockListItem>();
            try
            {
                response = client.GetAsync(url).Result;
                JToken token = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                token = token.SelectToken("['Time Series (Daily)']");
                foreach(var record in (JObject)token)
                {
                    JToken data = record.Value;

                    StockListItem stockListItem = new StockListItem();
                    stockListItem.dateTime = DateTime.Parse(record.Key);
                    stockListItem.open = Decimal.Parse(data.SelectToken("['1. open']").ToString());
                    stockListItem.close = Decimal.Parse(data.SelectToken("['4. close']").ToString());
                    stockListItem.high = Decimal.Parse(data.SelectToken("['2. high']").ToString());
                    stockListItem.low = Decimal.Parse(data.SelectToken("['3. low']").ToString());
                    stockListItem.volume = int.Parse(data.SelectToken("['5. volume']").ToString());

                    

                    stockListItems.Add(stockListItem);
                }
                //System.Diagnostics.Debug.WriteLine(token.ToString());
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            }
            return stockListItems;
        } 
    }
}
