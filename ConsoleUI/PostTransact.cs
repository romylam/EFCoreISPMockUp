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
        void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail);
        void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail);
    }
    public class PostGeneralTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            GeneralTransactDetail generalTransactDetail = (GeneralTransactDetail)transactDetail;
            transact.Amount += generalTransactDetail.Amount;
            if (generalTransactDetail.Amount > 0)
                account.Credit += generalTransactDetail.Amount;
            else
                account.Debit += generalTransactDetail.Amount;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            GeneralTransactDetail generalTransactDetail = (GeneralTransactDetail)transactDetail;
            transact.Amount -= generalTransactDetail.Amount;
            if (generalTransactDetail.Amount > 0)
                account.Credit -= generalTransactDetail.Amount;
            else
                account.Debit -= generalTransactDetail.Amount;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
    public class PostCreditTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            CreditTransactDetail creditTransactDetail = (CreditTransactDetail)transactDetail;
            transact.Amount += creditTransactDetail.Amount;
            if (creditTransactDetail.Amount > 0)
                account.Credit += creditTransactDetail.Amount;
            else
                account.Debit += creditTransactDetail.Amount;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            CreditTransactDetail creditTransactDetail = (CreditTransactDetail)transactDetail;
            transact.Amount -= creditTransactDetail.Amount;
            if (creditTransactDetail.Amount > 0)
                account.Credit -= creditTransactDetail.Amount;
            else
                account.Debit -= creditTransactDetail.Amount;
            context.Transacts.Update(transact);
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
    public class PostTransferTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            TransferTransactDetail transferTransactDetail = (TransferTransactDetail)transactDetail;
            transact.Amount += transferTransactDetail.Amount;
            transferTransactDetail.Link.Amount -= transferTransactDetail.Amount;
            if (transferTransactDetail.Amount > 0)
            {
                account.Transfer += transferTransactDetail.Amount;
                transferTransactDetail.Transfer.Transfer -= transferTransactDetail.Amount;
            }
            else
            {
                account.Transfer += transferTransactDetail.Amount;
                transferTransactDetail.Transfer.Transfer -= transferTransactDetail.Amount;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(transferTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(transferTransactDetail.Transfer);
            context.SaveChanges();
        }
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            TransferTransactDetail transferTransactDetail = (TransferTransactDetail)transactDetail;
            transact.Amount -= transferTransactDetail.Amount;
            transferTransactDetail.Link.Amount += transferTransactDetail.Amount;
            if (transferTransactDetail.Amount > 0)
            {
                account.Transfer -= transferTransactDetail.Amount;
                transferTransactDetail.Transfer.Transfer += transferTransactDetail.Amount;
            }
            else
            {
                account.Transfer -= transferTransactDetail.Amount;
                transferTransactDetail.Transfer.Transfer += transferTransactDetail.Amount;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(transferTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(transferTransactDetail.Transfer);
            context.SaveChanges();
        }
    }
    public class PostTradingTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
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
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {

        }
    }
}
