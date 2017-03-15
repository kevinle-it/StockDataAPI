using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class StockController : ApiController
    {
        [HttpGet]
        public List<Stock> GetStockList()
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            return db.Stocks.ToList();
        }

        [HttpGet]
        public Stock GetStock(string ticker)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            return db.Stocks.FirstOrDefault(x => x.Ticker == ticker);
        }

        [HttpPost]
        public bool InsertNewStock(string ticker, string equityName, 
                                    decimal price, decimal prevClosePrice)
        {
            try
            {
                DBStockTrainerDataContext db = new DBStockTrainerDataContext();

                Stock stock = new Stock();
                stock.Ticker = ticker;
                stock.EquityName = equityName;
                stock.Price = price;
                stock.PrevClosePrice = prevClosePrice;

                db.Stocks.InsertOnSubmit(stock);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPut]
        public bool UpdateStock(string ticker, string equityName,
                                    decimal price, decimal prevClosePrice)
        {
            try
            {
                DBStockTrainerDataContext db = new DBStockTrainerDataContext();

                Stock stock = db.Stocks.FirstOrDefault(x => x.Ticker == ticker);
                if (stock == null) return false;
                
                stock.EquityName = equityName;
                stock.Price = price;
                stock.PrevClosePrice = prevClosePrice;

                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete]
        public bool DeleteStock(string ticker)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            Stock stock = db.Stocks.FirstOrDefault(x => x.Ticker == ticker);
            if (stock == null) return false;

            db.Stocks.DeleteOnSubmit(stock);
            db.SubmitChanges();
            return true;
        }
    }
}
