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
        //[HttpGet]
        //public List<PortfolioTabModel> GetPortfolioByAccountID(int accountID)
        //{
        //    DBStockTrainerDataContext db = new DBStockTrainerDataContext();
        //    //return db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
        //    return (from p in db.GetTable<Portfolio>()
        //            from s in db.GetTable<Stock>()
        //            where ((p.AccountID == accountID) && (p.Ticker == s.Ticker))
        //            select new PortfolioTabModel {
        //                Ticker = s.Ticker,
        //                Name = s.Name,
        //                Price = s.Price,
        //                Cost = p.Cost,
        //                GainLossMoney = 0,
        //                NumStocks = p.Num,
        //                ChangeMoney = 0,
        //                Value = s.Price * p.Num,
        //                ChangePercent = 0
        //            }).ToList();
        //}

        //[HttpGet]
        //public PortfolioTabModel GetPortfolioStockByAccountIDAndTicker(int accountID, string ticker)
        //{
        //    DBStockTrainerDataContext db = new DBStockTrainerDataContext();
        //    //return db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
        //    return (from p in db.GetTable<Portfolio>()
        //            from s in db.GetTable<Stock>()
        //            where ((p.AccountID == accountID) && (p.Ticker == ticker) && (p.Ticker == s.Ticker))
        //            select new PortfolioTabModel
        //            {
        //                Ticker = s.Ticker,
        //                Name = s.Name,
        //                Price = s.Price,
        //                Cost = p.Cost,
        //                GainLossMoney = 0,
        //                NumStocks = p.Num,
        //                ChangeMoney = 0,
        //                Value = s.Price * p.Num,
        //                ChangePercent = 0
        //            }).FirstOrDefault();
        //}

        //[HttpPost]
        //public bool InsertNewPortfolio(int accountID, string ticker, double cost, 
        //                                int num)
        //{
        //    try
        //    {
        //        DBStockTrainerDataContext db = new DBStockTrainerDataContext();

        //        Portfolio portfolio = new Portfolio();
        //        portfolio.AccountID = accountID;
        //        portfolio.Ticker = ticker;
        //        portfolio.Cost = cost;
        //        portfolio.Num = num;

        //        db.Portfolios.InsertOnSubmit(portfolio);
        //        db.SubmitChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

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

        //[HttpDelete]
        //public bool DeletePortfolio(int accountID)
        //{
        //    DBStockTrainerDataContext db = new DBStockTrainerDataContext();

        //    Portfolio portfolio = db.Portfolios.FirstOrDefault(x => x.AccountID == accountID);
        //    if (portfolio == null) return false;

        //    db.Portfolios.DeleteOnSubmit(portfolio);
        //    db.SubmitChanges();
        //    return true;
        //}
    }
}
