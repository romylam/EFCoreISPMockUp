using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            dbContextFactory factory = new dbContextFactory();
            dbContext context = factory.CreateDbContext();

            dbService<Account> accountDataService = new dbService<Account>(context);
            List<Account> accounts = accountDataService.GetAll();
            //accountDataService.DeleteRange(accounts);
            //accountDataService.Add(new GeneralAccount { Name = "Acme Savings", Open = 1000 });
            //accountDataService.Add(new GeneralAccount { Name = "Acme Checking", Open = 0 });
            //accountDataService.Add(new CreditAccount { Name = "Acme Credit", Open = 0, Limit = -5000 });
            //accountDataService.Add(new TradingAccount { Name = "Acme Shares", Symbol = "ACME", Open = 100, Price = 1.5M, PriceDate = DateTime.Today });
            //Console.WriteLine("Accounts created.");
            //Console.WriteLine("");

            accounts = accountDataService.GetAll();
            ListAccount(accounts);

            Account savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            Account creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            Account tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));

            dbService<Transact> transactDataService = new dbService<Transact>(context);
            List<Transact> transacts = transactDataService.GetAll();
            //transactDataService.DeleteRange(transacts);

            //Transact t1 = new GeneralTransact { Date = DateTime.Today, Payee = "Salary", Amount = 2500, AccountId = savingsAccount.Id };
            //transactDataService.Add(t1);
            //IMakeTransact makeTransact = new MakeGeneralTransact();
            //makeTransact.MakeTransact(context, t1);

            //Transact t2 = new GeneralTransact { Date = DateTime.Today, Payee = "Travel", Amount = -375, AccountId = creditAccount.Id };
            //transactDataService.Add(t2);
            //makeTransact = new MakeGeneralTransact();
            //makeTransact.MakeTransact(context, t2);

            //Transact t3 = new TradingTransact { Date = DateTime.Today, Payee = "NYSE", Unit = 150, Price = 1.75M, PriceDate = DateTime.Today.AddDays(1), AccountId = tradingAccount.Id };
            //transactDataService.Add(t3);
            //makeTransact = new MakeTradingTransact();
            //makeTransact.MakeTransact(context, t3);

            transacts = context.Transacts.Include(a => a.Account).ToList();
            ListTransact(transacts);

            accounts = accountDataService.GetAll();
            ListAccount(accounts);
        }
        private static void ListAccount(List<Account> accounts)
        {
            foreach (var item in accounts)
            {
                Console.WriteLine($"Account {item.Name} have a balance of {item.OnDisplay}");
            }
            Console.WriteLine("");
        }
        public delegate void ShowTransactDetails(ITransact transact);
        private void TestDelegate(ShowTransactDetails showTransactDetails)
        {
            Transact item = new Transact();
            showTransactDetails(item);
        }
        private static void ListTransact(List<Transact> transacts)
        {
            foreach (var item in transacts)
            {
                Console.WriteLine($"On {item.Date} {item.OnDisplay} for {item.Payee}");
            }
            Console.WriteLine("");
        }
    }
}
