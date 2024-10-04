using Accounting.DataLayer.Context;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Business
{
    public class Account
    {
        public static ReportViewModel ReportFormMain()
        {
            ReportViewModel rp = new ReportViewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

                var recive = db.AccountingRepository.Get(a => a.TypeID == 1 && a.DataTitle >= startDate && a.DataTitle <= endDate).Select(a => a.Amount).ToList();
                var pay = db.AccountingRepository.Get(a => a.TypeID == 2 && a.DataTitle >= startDate && a.DataTitle <= endDate).Select(a => a.Amount).ToList();

                rp.Recive = (int)recive.Sum();
                rp.Pay = (int)pay.Sum();
                rp.AccountBalance = ((int)recive.Sum() - (int)pay.Sum()); 
            }

            return rp;
        }
    }
}
