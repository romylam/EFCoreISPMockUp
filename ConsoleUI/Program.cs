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
            accountDataService.DeleteRange(accounts);
            accountDataService.Add(new Account { Name = "Acme Savings", Class = "General", Open = 1000 });
            accountDataService.Add(new Account { Name = "Acme Checking", Class = "General", Open = 0 });
            accountDataService.Add(new CreditAccount { Name = "Acme Credit", Class = "Credit", Open = 0, Limit = -5000 });
            accountDataService.Add(new TradingAccount { Name = "Acme Shares", Class = "Trading", Symbol = "ACME", Open = 100, Price = 1.5M, PriceDate = DateTime.Today });
            Console.WriteLine("Accounts created.");
            Console.WriteLine("");

            accounts = accountDataService.GetAll();
            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.Class} account {item.Name} have a balance of {item.OnDisplay}");
            }
            Console.WriteLine("");

            dbService<Transact> transactDataService = new dbService<Transact>(context);
            List<Transact> transacts = transactDataService.GetAll();
            transactDataService.DeleteRange(transacts);

            Account savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            Account creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            Account tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));

            Transact thisTransact = new Transact();
            thisTransact.Class = "General";
            thisTransact.AccountId = savingsAccount.Id;
            thisTransact.Date = DateTime.Today;
            thisTransact.Payee = "Salary";
            thisTransact.Amount = 2500;
            transactDataService.Add(thisTransact);
            IMakeTransact makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, thisTransact);

            thisTransact = new Transact();
            thisTransact.Class = "General";
            thisTransact.AccountId = creditAccount.Id;
            thisTransact.Date = DateTime.Today;
            thisTransact.Payee = "King's Hotel";
            thisTransact.Amount = -375;
            transactDataService.Add(thisTransact);
            makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, thisTransact);

            thisTransact = new Transact();
            thisTransact.Class = "Trading";
            thisTransact.AccountId = tradingAccount.Id;
            thisTransact.Date = DateTime.Today;
            thisTransact.Payee = "NYSE";
            thisTransact.Unit = 150;
            thisTransact.Price = 1.75M;
            thisTransact.PriceDate = DateTime.Today.AddDays(1);
            thisTransact.Amount = thisTransact.Unit * thisTransact.Price;
            transactDataService.Add(thisTransact);
            makeTransact = new MakeTradingTransact();
            makeTransact.MakeTransact(context, thisTransact);

            transacts = context.Transacts.Include(a => a.Account).ToList();
            foreach (var item in transacts)
            {
                Console.WriteLine($"On {item.Date} {item.OnDisplay} for {item.Payee}");
            }
            Console.WriteLine("");

            accounts = accountDataService.GetAll();
            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.Class} account {item.Name} have a balance of {item.OnDisplay}");
            }
            Console.WriteLine("");
        }
    }
}
