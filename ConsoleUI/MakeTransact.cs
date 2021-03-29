using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMakeTransact
    {
        void MakeTransact(dbContext context, Transact transact);
    }
    public class MakeGeneralTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, Transact transact)
        {
            Account account = context.Accounts.Find(transact.AccountId);
            if (transact.Amount > 0)
                account.Credit += transact.Amount;
            else
                account.Debit += transact.Amount;
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
    public class MakeTradingTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, Transact transact)
        {
            TradingAccount account = context.Accounts.OfType<TradingAccount>().FirstOrDefault(x => x.Id.Equals(transact.AccountId));
            account.Price = transact.Price;
            if (DateTime.Compare(account.PriceDate, transact.PriceDate) > 0)
                account.PriceDate = transact.PriceDate;
            if (transact.Unit > 0)
                account.Credit += transact.Unit;
            else
                account.Debit += transact.Unit;
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
}
