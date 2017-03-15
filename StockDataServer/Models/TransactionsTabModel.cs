using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockDataServer.Models
{
    public class TransactionsTabModel
    {
        public string Ticker { get; set; }
        public string EquityName { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public long NumStocks { get; set; }
        public decimal Price { get; set; }
        public decimal? GainLossMoney { get; set; } = 0;
        public decimal? GainLossPercent { get; set; } = 0;
    }
}