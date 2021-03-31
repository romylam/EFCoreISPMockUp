using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    class Program
    {
        public delegate void ShowTransactDetails(ITransact transact);
        public delegate void ManageTransact(ITransact transact);
        static void Main(string[] args)
        {
            dbContextFactory factory = new dbContextFactory();
            dbContext context = factory.CreateDbContext();

            dbService<Account> accountDataService = new dbService<Account>(context);
            List<Account> accounts = accountDataService.GetAll();
            accountDataService.DeleteRange(accounts);
            Account savingsAccount = new GeneralAccount { Name = "Acme Savings", Open = 1000 };
            Account checkingAccount = new GeneralAccount { Name = "Acme Checking", Open = 0 };
            Account creditAccount = new CreditAccount { Name = "Acme Credit", Open = 0, Limit = -5000 };
            Account tradingAccount = new TradingAccount { Name = "Acme Shares", Symbol = "ACME", Open = 100, Price = 1.5M, PriceDate = DateTime.Today };
            accountDataService.Add(savingsAccount);
            accountDataService.Add(checkingAccount);
            accountDataService.Add(creditAccount);
            accountDataService.Add(tradingAccount);

            Console.WriteLine("Accounts created.");
            Console.WriteLine("");

            accounts = accountDataService.GetAll();
            ListAccount(accounts);

            savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));

            dbService<Transact> transactDataService = new dbService<Transact>(context);
            List<Transact> transacts = transactDataService.GetAll();
            transactDataService.DeleteRange(transacts);

            dbService<TransactDetail> transactDetailDataService = new dbService<TransactDetail>(context);
            List<TransactDetail> transactDetails = transactDetailDataService.GetAll();
            transactDetailDataService.DeleteRange(transactDetails);

            Transact t1 = new Transact { Date = DateTime.Today, Payee = "Acme Company", AccountId = savingsAccount.Id };
            transactDataService.Add(t1);
            TransactDetail t1n1 = new GeneralTransactDetail { Order = 1, Amount = 2500, Category = "Salary", TransactId = t1.Id };
            transactDetailDataService.Add(t1n1);
            IMakeTransact makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t1.Id);

            Transact t2 = new Transact { Date = DateTime.Today, Payee = "Acme Store", AccountId = creditAccount.Id };
            transactDataService.Add(t2);
            TransactDetail t2n1 = new GeneralTransactDetail { Order = 1, Amount = -25, Category = "Milk", TransactId = t2.Id };
            transactDetailDataService.Add(t2n1);
            TransactDetail t2n2 = new GeneralTransactDetail { Order = 2, Amount = -30, Category = "Fruits", TransactId = t2.Id };
            transactDetailDataService.Add(t2n2);
            makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t2.Id);

            Transact t3 = new Transact { Date = DateTime.Today, Payee = "Acme Forex", AccountId = checkingAccount.Id };
            transactDataService.Add(t3);
            TransactDetail t3n1 = new ForexTransactDetail { Order = 1, Amount = 1350, ForexCurrency = "USD", ForexAmount = 1000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n1);
            TransactDetail t3n2 = new ForexTransactDetail { Order = 2, Amount = -865.92M, ForexCurrency = "HKD", ForexAmount = 5000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n2);
            TransactDetail t3n3 = new ForexTransactDetail { Order = 3, Amount = 1020, ForexCurrency = "AUD", ForexAmount = 1000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n3);
            makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t3.Id);

            Transact t4 = new Transact { Date = DateTime.Today, Payee = "Acme Exchange", AccountId = tradingAccount.Id };
            transactDataService.Add(t4);

            TransactDetail t4n1 = new TradingTransactDetail { Order = 1, Unit = 150, Price = 1.75M, PriceDate = DateTime.Today.AddDays(1), TradingId = savingsAccount.Id, TransactId = t4.Id };
            transactDetailDataService.Add(t4n1);
            makeTransact = new MakeTradingTransact();
            makeTransact.MakeTransact(context, t4.Id);

            transacts = context.Transacts.Include(a => a.Account).Include(t => t.TransactDetail).ToList();
            ListTransact(transacts);

            accounts = accountDataService.GetAll();
            ListAccount(accounts);
        }
        private static void ListAccount(List<Account> accounts)
        {
            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.GetType().Name} {item.Name} has a balance of {item.OnDisplay}");
            }
            Console.WriteLine("");
        }
        private static void ListTransact(List<Transact> transacts)
        {
            foreach (var item in transacts)
            {
                Console.Write($"On {item.Date:d} {item.Payee} ");
                foreach (var detail in item.TransactDetail)
                {
                    Console.Write($"{detail.OnDisplay} ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
