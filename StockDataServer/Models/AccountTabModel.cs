using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockDataServer.Models
{
    public class AccountTabModel
    {
        public string Username { get; set; }
        public decimal StartingInvestment { get; set; }
        public decimal StocksValue { get; set; }
        public decimal AvailableCash { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Position { get; set; }
        public long TotalTrans { get; set; }
        public long PositiveTrans { get; set; }
        public long NegativeTrans { get; set; }
    }
}