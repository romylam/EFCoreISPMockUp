using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMakeTransact
    {
        void MakeTransact(dbContext context, int transactId);
    }
    public class MakeGeneralTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, int transactId)
        {
            Transact transact = context.Transacts.Find(transactId);
            Account account = context.Accounts.Find(transact.AccountId);
            List<TransactDetail> transactDetails = context.TransactDetails.Where(t => t.TransactId.Equals(transact.Id)).ToList();
            decimal totalAmount = 0;
            foreach (var item in transactDetails)
            {
                totalAmount += item.Amount;
                if (item.Amount > 0)
                    account.Credit += item.Amount;
                else
                    account.Debit += item.Amount;
            }
            transact.Amount = totalAmount;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
    public class MakeTransferTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, int transactId)
        {

        }
    }
    public class MakeTradingTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, int transactId)
        {
            Transact transact = context.Transacts.Find(transactId);
            TradingAccount account = context.Accounts.OfType<TradingAccount>().FirstOrDefault(x => x.Id.Equals(transact.AccountId));
            Account tradingAccount;
            List<TradingTransactDetail> transactDetails = context.TransactDetails.OfType<TradingTransactDetail>().Where(t => t.TransactId.Equals(transact.Id)).ToList();
            decimal totalUnit = 0;
            decimal totalAmount = 0;
            foreach (var item in transactDetails)
            {
                totalUnit += item.Unit;
                totalAmount = item.Price * item.Unit;
                tradingAccount = context.Accounts.Find(item.TradingId);
                account.Price = item.Price;
                if (DateTime.Compare(account.PriceDate, item.PriceDate) < 0)
                    account.PriceDate = item.PriceDate;
                if (item.Unit > 0)
                {
                    account.Credit += item.Unit;
                    tradingAccount.Debit -= totalAmount;
                }
                else
                {
                    account.Debit += item.Unit;
                    tradingAccount.Credit -= totalAmount;
                }
            }
            transact.Amount = totalUnit;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
}
