using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class PortfolioController : ApiController
    {
        [HttpGet]
        public List<PortfolioTabModel> GetPortfoliosByUsername(string username)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            //return db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
            //if ((from a in db.GetTable<Account>()
            //    where (a.Username == username)
            //    select a.Username).ToString() == username)
            //{

            //}
            return (from p in db.GetTable<Portfolio>()
                    from s in db.GetTable<Stock>()
                    where ((p.Username == username) && (p.Ticker == s.Ticker))
                    select new PortfolioTabModel
                    {
                        Ticker = s.Ticker,
                        EquityName = s.EquityName,
                        Price = s.Price,
                        Cost = p.Cost,
                        GainLossMoney = (s.Price - p.Cost) * p.NumStocks,
                        NumStocks = p.NumStocks,
                        ChangeMoney = s.Price - p.Cost,
                        Value = s.Price * p.NumStocks,
                        ChangePercent = (s.Price - p.Cost) / p.Cost
                    }).ToList();
        }

        [HttpGet]
        public PortfolioTabModel GetPortfolioByUsernameAndTicker(string username, string ticker)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            //return db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
            return (from p in db.GetTable<Portfolio>()
                    from s in db.GetTable<Stock>()
                    where ((p.Username == username) && (p.Ticker == ticker) && (p.Ticker == s.Ticker))
                    select new PortfolioTabModel
                    {
                        Ticker = s.Ticker,
                        EquityName = s.EquityName,
                        Price = s.Price,
                        Cost = p.Cost,
                        GainLossMoney = 0,
                        NumStocks = p.NumStocks,
                        ChangeMoney = 0,
                        Value = s.Price * p.NumStocks,
                        ChangePercent = 0
                    }).FirstOrDefault();
        }

        [HttpPost]
        public bool InsertNewPortfolio(string username, string ticker, decimal cost,
                                        int numStocks)
        {
            try
            {
                DBStockTrainerDataContext db = new DBStockTrainerDataContext();

                Portfolio portfolio = new Portfolio();
                portfolio.Username = username;
                portfolio.Ticker = ticker;
                portfolio.Cost = cost;
                portfolio.NumStocks = numStocks;

                db.Portfolios.InsertOnSubmit(portfolio);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //[HttpPut]
        //public bool UpdatePortfolio(int accountID, string ticker, double cost,
        //                                int num)
        //{
        //    try
        //    {
        //        DBStockTrainerDataContext db = new DBStockTrainerDataContext();

        //        Portfolio portfolio = db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
        //        if (portfolio == null) return false;

        //        portfolio.Ticker = ticker;
        //        portfolio.Cost = cost;
        //        portfolio.Num = num;

        //        db.SubmitChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [HttpDelete]
        public bool DeletePortfolioByUsername(string username)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            var match = (from p in db.GetTable<Portfolio>()
                         where (p.Username == username)
                         select p).ToList();

            // Portfolio portfolio = db.Portfolios.FirstOrDefault(x => x.Username == username);
            //if (portfolio == null) return false;

            //db.Portfolios.DeleteOnSubmit(portfolio);

            if (match == null) return false;

            db.Portfolios.DeleteAllOnSubmit(match);
            db.SubmitChanges();
            return true;
        }

        [HttpDelete]
        public bool DeletePortfolioByUsernameAndTicker(string username, string ticker)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            var match = (from p in db.GetTable<Portfolio>()
                         where ((p.Username == username) && (p.Ticker == ticker))
                         select p).SingleOrDefault();

            Portfolio portfolio = db.Portfolios.FirstOrDefault(x => x.Username == username);
            //if (portfolio == null) return false;

            //db.Portfolios.DeleteOnSubmit(portfolio);

            if (match == null) return false;

            db.Portfolios.DeleteOnSubmit(match);
            db.SubmitChanges();
            return true;
        }
    }
}
