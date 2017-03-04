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
    }
}
