using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public bool Login(string username, string password)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            string match = (from a in db.GetTable<Account>()
                         where (a.Username == username)
                         select a.Password).SingleOrDefault();

            if (password == match)
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public AccountTabModel GetAccountInfoByUsername(string username)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();
            return (from a in db.GetTable<Account>()
                    where (a.Username == username)
                    select new AccountTabModel {
                        Username = a.Username,
                        Investment = a.Investment,
                        AvailableCash = a.AvailableCash,
                        TotalTrans = a.TotalTrans,
                        PositiveTrans = a.PositiveTrans,
                        NegativeTrans = a.NegativeTrans
                    }).SingleOrDefault();
        }
    }
}
