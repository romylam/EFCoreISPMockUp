using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IPostTransact
    {
        void MakeTransact(Account account, Transact transact, decimal transactAmount);
    }
    public class PostTransact    
    {
        public void MakeTransact(dbContext context, int transactId)
        {
            IPostTransact postGeneralTransact = new PostGeneralTransact();
            IPostTransact postTransferTransact = new PostTransferTransact();
            IPostTransact postTradingTransact = new PostTradingTransact();
            Transact transact = context.Transacts.Find(transactId);
            Account account = context.Accounts.Find(transact.AccountId);
            List<TransactDetail> transactDetails = context.TransactDetails.Where(t => t.TransactId.Equals(transactId)).ToList();
            transact.Amount = 0;
            string transactType = string.Empty;
            foreach (var item in transactDetails)
            {
                transactType = item.GetType().Name;
                Console.WriteLine($"Processing {transactType}... ");
                switch (transactType)
                {
                    case "GeneralTransactDetail":
                        postGeneralTransact.MakeTransact(account, transact, item.Amount);
                        break;
                    case "TransferTransactDetail":
                        Console.WriteLine("Transfer");
                        break;
                    default:
                        Console.WriteLine("Default");
                        break;
                }
            }
            //context.Transacts.Update(transact);
            //context.Accounts.Update(account);
            //context.SaveChanges();
        }
    }
    public class PostGeneralTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, decimal transactAmount)
        {
            transact.Amount += transactAmount;
            if (transactAmount > 0)
                account.Credit += transactAmount;
            else
                account.Debit += transactAmount;
        }
    }
    public class PostTransferTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, decimal transactAmount)
        {
            transferAccount = context.Accounts.Find(item.TransferId);
            transact.Amount += transactAmount;
            if (transactAmount > 0)
            {
                account.Credit += transactAmount;
                transferAccount.Debit -= transactAmount;
            }
            else
            {
                account.Debit += transactAmount;
                transferAccount.Credit -= transactAmount;
            }
            context.Accounts.Update(transferAccount);
            //Transact transact = context.Transacts.Find(transactId);
            //Account account = context.Accounts.Find(transact.AccountId);
            //Account transferAccount;
            //List<TransferTransactDetail> transactDetails = context.TransactDetails.OfType<TransferTransactDetail>().Where(t => t.TransactId.Equals(transact.Id)).ToList();
            //decimal totalAmount = 0;
            //foreach (var item in transactDetails)
            //{
            //    transferAccount = context.Accounts.Find(item.TransferId);
            //    totalAmount += item.Amount;
            //    if (item.Amount > 0)
            //    {
            //        account.Credit += item.Amount;
            //        transferAccount.Debit -= item.Amount;
            //    }
            //    else
            //    {
            //        account.Debit += item.Amount;
            //        transferAccount.Credit -= item.Amount;
            //    }
            //    context.Accounts.Update(transferAccount);
            //}
            //transact.Amount = totalAmount;
            //context.Transacts.Update(transact);
            //context.Accounts.Update(account);
            //context.SaveChanges();
        }
    }
    public class PostTradingTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, decimal transactAmount)
        {
            //Transact transact = context.Transacts.Find(transactId);
            //TradingAccount account = context.Accounts.OfType<TradingAccount>().FirstOrDefault(x => x.Id.Equals(transact.AccountId));
            //Account tradingAccount;
            //List<TradingTransactDetail> transactDetails = context.TransactDetails.OfType<TradingTransactDetail>().Where(t => t.TransactId.Equals(transact.Id)).ToList();
            //decimal totalUnit = 0;
            //decimal totalAmount = 0;
            //foreach (var item in transactDetails)
            //{
            //    totalUnit += item.Unit;
            //    totalAmount = item.Price * item.Unit;
            //    tradingAccount = context.Accounts.Find(item.TradingId);
            //    account.Price = item.Price;
            //    if (DateTime.Compare(account.PriceDate, item.PriceDate) < 0)
            //        account.PriceDate = item.PriceDate;
            //    if (item.Unit > 0)
            //    {
            //        account.Credit += item.Unit;
            //        tradingAccount.Debit -= totalAmount;
            //    }
            //    else
            //    {
            //        account.Debit += item.Unit;
            //        tradingAccount.Credit -= totalAmount;
            //    }
            //    context.Accounts.Update(tradingAccount);
            //}
            //transact.Amount = totalUnit;
            //context.Transacts.Update(transact);
            //context.Accounts.Update(account);
            //context.SaveChanges();
        }
    }
}
