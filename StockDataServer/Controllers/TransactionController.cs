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
        const string SELL = "Sell";

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
        public bool BuySell(string username, string ticker, long numStocks, string transactionType)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            var matchedStock = (from s in db.GetTable<Stock>()
                                where s.Ticker == ticker
                                select s).SingleOrDefault();

            var matchedUser = (from a in db.GetTable<Account>()
                               where a.Username == username
                               select a).SingleOrDefault();

            if (transactionType.Equals(BUY, StringComparison.InvariantCultureIgnoreCase))
            {
                if (matchedUser.AvailableCash - (matchedStock.Price * numStocks) < 0)       // Not Enough Available Cash for Buying.
                {
                    return false;
                }

                if (matchedStock != null && matchedUser != null)
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

                        db.Transactions.InsertOnSubmit(transaction);

                        var matchedPortfolio = (from p in db.GetTable<Portfolio>()
                                                where ((p.Ticker == ticker) && (p.Username == username))
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
                            matchedPortfolio.Cost = Math.Round(((matchedPortfolio.NumStocks * matchedPortfolio.Cost) + (numStocks * matchedStock.Price)) / (matchedPortfolio.NumStocks + numStocks), 3, MidpointRounding.AwayFromZero);
                            matchedPortfolio.NumStocks += numStocks;
                        }

                        //var matchedPortfolios = (from p in db.GetTable<Portfolio>()
                        //                         join s in db.GetTable<Stock>() on p.Ticker equals s.Ticker
                        //                         where p.Username == username
                        //                         select new { p, s }).ToList();

                        //foreach (var item in matchedPortfolios)
                        //{
                        //    matchedUser.StocksValue += item.s.Price * item.p.NumStocks;
                        //}

                        matchedUser.AvailableCash -= (matchedStock.Price * numStocks) + 10;
                        ++matchedUser.TotalTrans;

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
            if (transactionType.Equals(SELL, StringComparison.InvariantCultureIgnoreCase))
            {
                var matchedPortfolio = (from p in db.GetTable<Portfolio>()
                                        where ((p.Ticker == ticker) && (p.Username == username))
                                        select p).SingleOrDefault();

                if (matchedPortfolio.NumStocks < numStocks)     // Not Enough Available Shares for Selling.
                {
                    return false;
                }

                if (matchedStock != null && matchedUser != null && matchedPortfolio != null)
                {
                    try
                    {
                        Transaction transaction = new Transaction();
                        transaction.Username = username;
                        transaction.Ticker = matchedStock.Ticker;
                        transaction.EquityName = matchedStock.EquityName;
                        transaction.Date = DateTime.Now;
                        transaction.Type = SELL;
                        transaction.NumStocks = numStocks;
                        transaction.Price = matchedStock.Price;
                        transaction.GainLossMoney = Math.Round((matchedStock.Price - matchedPortfolio.Cost) * numStocks, 3, MidpointRounding.AwayFromZero);
                        transaction.GainLossPercent = Math.Round((matchedStock.Price - matchedPortfolio.Cost) / matchedPortfolio.Cost, 3, MidpointRounding.AwayFromZero);

                        db.Transactions.InsertOnSubmit(transaction);

                        matchedPortfolio.NumStocks -= numStocks;

                        if (matchedPortfolio.NumStocks == 0)
                        {
                            db.Portfolios.DeleteOnSubmit(matchedPortfolio);
                        }

                        matchedUser.AvailableCash += (matchedStock.Price * numStocks) - 10;
                        ++matchedUser.TotalTrans;
                        if (transaction.GainLossMoney > 0)
                        {
                            ++matchedUser.PositiveTrans;
                        }
                        else if (transaction.GainLossMoney < 0)
                        {
                            ++matchedUser.NegativeTrans;
                        }

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

            return false;
        }
    }
}
