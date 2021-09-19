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
    public class PostForexTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            ForexTransactDetail forexTransactDetail = (ForexTransactDetail)transactDetail;
            transact.Amount += forexTransactDetail.Amount;
            forexTransactDetail.Link.Amount -= forexTransactDetail.ForexAmount;
            if (forexTransactDetail.Amount > 0)
            {
                account.Transfer += forexTransactDetail.Amount;
                forexTransactDetail.Transfer.Transfer -= forexTransactDetail.ForexAmount;
            }
            else
            {
                account.Transfer += forexTransactDetail.Amount;
                forexTransactDetail.Transfer.Transfer -= forexTransactDetail.ForexAmount;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(forexTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(forexTransactDetail.Transfer);
            context.SaveChanges();
        }
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            Account account = transact.Account;
            ForexTransactDetail forexTransactDetail = (ForexTransactDetail)transactDetail;
            transact.Amount -= forexTransactDetail.Amount;
            forexTransactDetail.Link.Amount += forexTransactDetail.ForexAmount;
            if (forexTransactDetail.Amount > 0)
            {
                account.Transfer -= forexTransactDetail.Amount;
                forexTransactDetail.Transfer.Transfer += forexTransactDetail.ForexAmount;
            }
            else
            {
                account.Transfer -= forexTransactDetail.Amount;
                forexTransactDetail.Transfer.Transfer += forexTransactDetail.ForexAmount;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(forexTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(forexTransactDetail.Transfer);
            context.SaveChanges();
        }
    }
    public class PostTradingTransact : IPostTransact
    {
        public void PostTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            TradingAccount account = (TradingAccount)transact.Account;
            TradingTransactDetail tradingTransactDetail = (TradingTransactDetail)transactDetail;
            transact.Amount += tradingTransactDetail.Amount;
            tradingTransactDetail.Link.Amount -= tradingTransactDetail.Amount * tradingTransactDetail.Price;
            if (account.PriceDate <  transact.Date)
            {
                account.Price = tradingTransactDetail.Price;
                account.PriceDate = transact.Date;
            }
            if (account.PriceDate == transact.Date && account.Price != tradingTransactDetail.Price)
            {
                account.Price = tradingTransactDetail.Price;
            }
            if (tradingTransactDetail.Amount > 0)
            {
                account.Credit += tradingTransactDetail.Amount;
                tradingTransactDetail.Transfer.Debit -= tradingTransactDetail.Amount * tradingTransactDetail.Price;
            }
            else
            {
                account.Credit -= tradingTransactDetail.Amount;
                tradingTransactDetail.Transfer.Debit += tradingTransactDetail.Amount * tradingTransactDetail.Price;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(tradingTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(tradingTransactDetail.Transfer);
            context.SaveChanges();
        }
        public void ReverseTransact(dbContext context, Transact transact, TransactDetail transactDetail)
        {
            TradingAccount account = (TradingAccount)transact.Account;
            TradingTransactDetail tradingTransactDetail = (TradingTransactDetail)transactDetail;
            transact.Amount -= tradingTransactDetail.Amount;
            tradingTransactDetail.Link.Amount += tradingTransactDetail.Amount * tradingTransactDetail.Price;
            if (tradingTransactDetail.Amount > 0)
            {
                account.Credit -= tradingTransactDetail.Amount;
                tradingTransactDetail.Transfer.Debit += tradingTransactDetail.Amount * tradingTransactDetail.Price;
            }
            else
            {
                account.Credit += tradingTransactDetail.Amount;
                tradingTransactDetail.Transfer.Debit -= tradingTransactDetail.Amount * tradingTransactDetail.Price;
            }
            context.Transacts.Update(transact);
            context.Transacts.Update(tradingTransactDetail.Link);
            context.Accounts.Update(account);
            context.Accounts.Update(tradingTransactDetail.Transfer);
            context.SaveChanges();
        }
    }
}
