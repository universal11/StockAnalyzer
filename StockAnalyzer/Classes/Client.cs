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

        public static Dictionary<int, Dictionary<int, StockDate>> getUniqueStockDates(List<StockListItem> stockListItems) {
            Dictionary<int, Dictionary<int, StockDate>> dates = new Dictionary<int, Dictionary<int, StockDate>>();
            
            foreach(StockListItem stockListItem in stockListItems) {
                StockDate stockDate = stockListItem.getStockDate();
                /*
                StockDate stockDate = new StockDate();
                stockDate.month = stockListItem.dateTime.Month;
                stockDate.day = stockListItem.dateTime.Day;
                */
                if(!dates.ContainsKey(stockDate.month))
                {
                    dates[stockDate.month] = new Dictionary<int, StockDate>();
                }

                if(!dates[stockDate.month].ContainsKey(stockDate.day)) {
                    dates[stockDate.month][stockDate.day] = stockDate;
                }
            }

            return dates;
        }

        
        public static List<StockDate> getSortedStockDates(Dictionary<int, Dictionary<int, StockDate>> dates) {
            List<StockDate> stockDates = new List<StockDate>();
            foreach(KeyValuePair<int, Dictionary<int, StockDate>> month in dates)
            {
                foreach(KeyValuePair<int, StockDate> date in month.Value)
                {
                    stockDates.Add(date.Value);
                }
            }

            return stockDates.OrderBy(sd => sd.month).ThenBy(sd => sd.day).ToList();
        }
        

        public static List<StockListItem> getStockListItems() {
            string url = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=AAPL&apikey=IO5GL7XPD3B40CDZ&outputsize=full";

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
