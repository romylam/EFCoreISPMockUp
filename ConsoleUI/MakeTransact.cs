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
        void MakeTransact(Account account, Transact transact, TransactDetail transactDetail, Account transferAccount = null);
    }
    public class PostTransact    
    {
        public void MakeTransact(dbContext context, int transactId)
        {
            IPostTransact postGeneralTransact = new PostGeneralTransact();
            IPostTransact postTransferTransact = new PostTransferTransact();
            IPostTransact postTradingTransact = new PostTradingTransact();
            GeneralTransactDetail generalTransactDetail = new GeneralTransactDetail();
            TransferTransactDetail transferTransactDetail = new TransferTransactDetail();
            TradingTransactDetail tradingTransactDetail = new TradingTransactDetail();
            //Transact transact = context.Transacts.Find(transactId);
            //Transact transferTransact = new Transact();
            //Account account = context.Accounts.Find(transact.AccountId);
            //Account transferAccount = new Account();
            //List<TransactDetail> transactDetails = context.TransactDetails.Where(t => t.TransactId.Equals(transactId)).ToList();
            //transact.Amount = 0;
            //string transactType = string.Empty;
            //foreach (var item in transactDetails)
            //{
            //    transactType = item.GetType().Name;
            //    Console.WriteLine($"Processing {transactType}... ");
            //    switch (transactType)
            //    {
            //        case "GeneralTransactDetail":
            //            generalTransactDetail = (GeneralTransactDetail)item;
            //            postGeneralTransact.MakeTransact(account, transact, generalTransactDetail);
            //            break;
            //        case "TransferTransactDetail":
            //            transferTransactDetail = (TransferTransactDetail)item;
            //            transferAccount = context.Accounts.Find(transferTransactDetail.TransferId);

            //            transferTransact = new Transact { Date = transact.Date, Payee = transact.Payee, AccountId = transferAccount.Id };
            //            context.Transacts.Add(transferTransact);

            //            postTransferTransact.MakeTransact(account, transact, transferTransactDetail, transferAccount);
            //            context.Accounts.Update(transferAccount);
            //            break;
            //        default:
            //            Console.WriteLine("Default");
            //            break;
            //    }
            //}
            //context.Transacts.Update(transact);
            //context.Accounts.Update(account);
            //context.SaveChanges();
        }
    }
    public class PostGeneralTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, TransactDetail transactDetail, Account transferAccount)
        {
            transact.Amount += transactDetail.Amount;
            if (transactDetail.Amount > 0)
                account.Credit += transactDetail.Amount;
            else
                account.Debit += transactDetail.Amount;
        }
    }
    public class PostTransferTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, TransactDetail transactDetail, Account transferAccount)
        {
            transact.Amount += transactDetail.Amount;
            if (transactDetail.Amount > 0)
            {
                account.Transfer += transactDetail.Amount;
                transferAccount.Transfer -= transactDetail.Amount;
            }
            else
            {
                account.Transfer += transactDetail.Amount;
                transferAccount.Transfer -= transactDetail.Amount;
            }
        }
    }
    public class PostTradingTransact : IPostTransact
    {
        public void MakeTransact(Account account, Transact transact, TransactDetail transactDetail, Account transferAccount)
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
