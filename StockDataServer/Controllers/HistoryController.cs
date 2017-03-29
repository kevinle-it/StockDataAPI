using StockDataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StockDataServer.Controllers
{
    public class HistoryController : ApiController
    {
        [HttpGet]
        public decimal GetPreviousClosePrice(string ticker)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            return (from s in db.GetTable<Stock>()
                    where s.Ticker == ticker
                    select s.PrevClosePrice).Single();
        }

        [HttpGet]
        public List<HistoryPriceModel> GetHistoryPrice(string ticker, string historyType)
        {
            DBStockTrainerDataContext db = new DBStockTrainerDataContext();

            var now = DateTime.Now;
            var timeMark = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            switch (historyType)
            {
                case "1w":
                    var oneWeek = DateTime.Now.AddDays(-7);
                    timeMark = new DateTime(oneWeek.Year, oneWeek.Month, oneWeek.Day, 0, 0, 0);
                    break;

                case "1m":
                    var oneMonth = DateTime.Now.AddMonths(-1);
                    timeMark = new DateTime(oneMonth.Year, oneMonth.Month, oneMonth.Day, 0, 0, 0);
                    break;

                case "3m":
                    var threeMonths = DateTime.Now.AddMonths(-3);
                    timeMark = new DateTime(threeMonths.Year, threeMonths.Month, threeMonths.Day, 0, 0, 0);
                    break;

                case "6m":
                    var sixMonths = DateTime.Now.AddMonths(-6);
                    timeMark = new DateTime(sixMonths.Year, sixMonths.Month, sixMonths.Day, 0, 0, 0);
                    break;

                case "1y":
                    var oneYear = DateTime.Now.AddYears(-1);
                    timeMark = new DateTime(oneYear.Year, oneYear.Month, oneYear.Day, 0, 0, 0);
                    break;

                case "2y":
                    var twoYears = DateTime.Now.AddYears(-2);
                    timeMark = new DateTime(twoYears.Year, twoYears.Month, twoYears.Day, 0, 0, 0);
                    break;

                case "5y":
                    var fiveYears = DateTime.Now.AddYears(-5);
                    timeMark = new DateTime(fiveYears.Year, fiveYears.Month, fiveYears.Day, 0, 0, 0);
                    break;

                case "max":
                    timeMark = new DateTime();
                    break;

                default:
                    break;
            }

            return (from h in db.GetTable<History>()
                    where ((h.Ticker == ticker) && (h.Time >= timeMark))
                    orderby h.Time ascending
                    select new HistoryPriceModel
                    {
                        Time = h.Time,
                        Price = h.HistoryPrice
                    }).ToList();
        }
    }
}
