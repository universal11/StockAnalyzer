using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.Classes
{
    public class StockListItem
    {
        public DateTime dateTime { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public int volume { get; set; }

        public bool isProfit() {
            return ( this.close > this.open );
        }

        public StockDate getStockDate() {
            StockDate stockDate = new StockDate();
            stockDate.month = this.dateTime.Month;
            stockDate.day = this.dateTime.Day;
            stockDate.year = this.dateTime.Year;
            return stockDate;
        }
    }
}
