using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMakeTransact
    {
        void MakeTransact(dbContext context, ITransact transact);
    }
    public class MakeGeneralTransact : IMakeTransact
    {
        public void MakeTransact(dbContext context, ITransact transact)
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
        public void MakeTransact(dbContext context, ITransact transact)
        {
            TradingAccount account = context.Accounts.OfType<TradingAccount>().FirstOrDefault(x => x.Id.Equals(transact.AccountId));
            TradingTransact tt = (TradingTransact)transact;
            account.Price = tt.Price;
            if (DateTime.Compare(account.PriceDate, tt.PriceDate) < 0)
                account.PriceDate = tt.PriceDate;
            if (tt.Unit > 0)
                account.Credit += tt.Unit;
            else
                account.Debit += tt.Unit;
            context.Accounts.Update(account);
            context.SaveChanges();
        }
    }
}
