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

            if (password == match && !string.IsNullOrWhiteSpace(password))
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
                        StartingInvestment = a.StartingInvestment,
                        AvailableCash = a.AvailableCash,
                        TotalTrans = a.TotalTrans,
                        PositiveTrans = a.PositiveTrans,
                        NegativeTrans = a.NegativeTrans
                    }).SingleOrDefault();
        }

        [HttpPost]
        public bool SignUp(string username, string password, string fullname, string firstSecurityQuestion, string firstSecurityAnswer, string secondSecurityQuestion, string secondSecurityAnswer)
        {
            try
            {
                DBStockTrainerDataContext db = new DBStockTrainerDataContext();

                Account account = new Account();
                account.Username = username;
                account.Password = password;
                account.Fullname = fullname;
                account.FirstSecurityQuestion = firstSecurityQuestion;
                account.FirstSecurityAnswer = firstSecurityAnswer;
                account.SecondSecurityQuestion = secondSecurityQuestion;
                account.SecondSecurityAnswer = secondSecurityAnswer;
                account.StartingInvestment = 20000;
                account.AvailableCash = 20000;
                account.TotalTrans = 0;
                account.PositiveTrans = 0;
                account.NegativeTrans = 0;

                db.Accounts.InsertOnSubmit(account);
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
