using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class TransactionsController : ApiController
    {
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
                        AvgBuyPrice = t.AvgBuyPrice,
                        GainLossMoney = t.GainLossMoney,
                        GainLossPercent = t.GainLossPercent
                    }).ToList();
        }

        [HttpPost]
        public bool InsertNewTransaction(string ticker, string equityName, 
                                         DateTime date, string type,
                                         long numStocks, double price,
                                         double avgBuyPrice, double gainLossMoney,
                                         double gainLossPercent)
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
                transaction.AvgBuyPrice = avgBuyPrice;
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
    }
}
