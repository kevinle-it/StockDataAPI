using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class TransactionController : ApiController
    {
        const string BUY = "Buy";

        [HttpGet]
        public List<TransactionsTabModel> GetTransactionsByUserName(string username)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            return (from t in db.GetTable<Transaction>()
                    from a in db.GetTable<Account>()
                    where ((t.Username == username) && (t.Username == a.Username))
                    select new TransactionsTabModel
                    {
                        Ticker = t.Ticker,
                        EquityName = t.EquityName,
                        Date = t.Date,
                        Type = t.Type,
                        NumStocks = t.NumStocks,
                        Price = t.Price,
                        GainLossMoney = t.GainLossMoney,
                        GainLossPercent = t.GainLossPercent
                    }).ToList();
        }

        [HttpPost]
        public bool InsertNewTransaction(string ticker, string equityName, 
                                         DateTime date, string type,
                                         long numStocks, decimal price,
                                         decimal gainLossMoney,
                                         decimal gainLossPercent)
        {
            try
            {
                DBStockTrainerDataContext db = new DBStockTrainerDataContext();

                Transaction transaction = new Transaction();
                transaction.Ticker = ticker;
                transaction.EquityName = equityName;
                transaction.Date = date;
                transaction.Type = type;
                transaction.NumStocks = numStocks;
                transaction.Price = price;
                transaction.GainLossMoney = gainLossMoney;
                transaction.GainLossPercent = gainLossPercent;

                db.Transactions.InsertOnSubmit(transaction);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public bool Buy(string username, string ticker, long numStocks)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            var matchedStock = (from s in db.GetTable<Stock>()
                                where s.Ticker == ticker
                                select s).SingleOrDefault();

            if (matchedStock != null)
            {
                try
                {
                    Transaction transaction = new Transaction();
                    transaction.Username = username;
                    transaction.Ticker = matchedStock.Ticker;
                    transaction.EquityName = matchedStock.EquityName;
                    transaction.Date = DateTime.Now;
                    transaction.Type = BUY;
                    transaction.NumStocks = numStocks;
                    transaction.Price = matchedStock.Price;

                    var matchedPortfolio = (from p in db.GetTable<Portfolio>()
                                            where p.Ticker == ticker
                                            select p).SingleOrDefault();
                    if (matchedPortfolio == null)
                    {
                        Portfolio portfolio = new Portfolio();

                        portfolio.Username = username;
                        portfolio.Ticker = matchedStock.Ticker;
                        portfolio.Cost = matchedStock.Price;
                        portfolio.NumStocks = numStocks;

                        db.Portfolios.InsertOnSubmit(portfolio);
                    }
                    else
                    {
                        matchedPortfolio.Cost = ((matchedPortfolio.NumStocks * matchedPortfolio.Cost) + (numStocks * matchedStock.Price)) / (matchedPortfolio.NumStocks + numStocks);
                        matchedPortfolio.NumStocks += numStocks;
                    }

                    var matchedUser = (from a in db.GetTable<Account>()
                                       where a.Username == username
                                       select a).SingleOrDefault();

                    matchedUser.AvailableCash -= (matchedStock.Price * numStocks);
                    ++matchedUser.TotalTrans;

                    db.Transactions.InsertOnSubmit(transaction);
                    db.SubmitChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
