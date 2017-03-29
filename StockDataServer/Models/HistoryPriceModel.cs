using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockDataServer.Models
{
    public class HistoryPriceModel
    {
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
    }
}