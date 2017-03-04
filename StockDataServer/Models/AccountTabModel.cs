using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockDataServer.Models
{
    public class AccountTabModel
    {
        public string Username { get; set; }
        public string  Password { get; set; }
        public double Investment { get; set; }
        public double AvailableCash { get; set; }
        public long TotalTrans { get; set; }
        public long PositiveTrans { get; set; }
        public long NegativeTrans { get; set; }
    }
}