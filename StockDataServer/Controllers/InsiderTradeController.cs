using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class InsiderTradeController : ApiController
    {
        [HttpGet]
        public List<InsiderTrade> GetAllInsiderTrades()
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            return db.InsiderTrades.ToList();
        }
    }
}
