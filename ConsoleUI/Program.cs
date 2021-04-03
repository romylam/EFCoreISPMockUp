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
            Account savingsAccount = new GeneralAccount();
            Account checkingAccount = new GeneralAccount();
            Account creditAccount = new CreditAccount();
            Account tradingAccount = new TradingAccount();
            List<Account> accounts = new List<Account>();

            List<Transact> transacts = new List<Transact>();
            List<TransactDetail> transactDetails = new List<TransactDetail>();

            dbContextFactory factory = new dbContextFactory();
            dbContext context = factory.CreateDbContext();

            dbService<Account> accountDataService = new dbService<Account>(context);
            //PurgeAccount(accountDataService, accounts);
            //CreateAccount(accountDataService, savingsAccount, checkingAccount, creditAccount, tradingAccount);

            accounts = accountDataService.GetAll();
            ListAccount(accounts);

            //ReadAccount(context, savingsAccount, checkingAccount, creditAccount, tradingAccount);
            savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));


            dbService<Transact> transactDataService = new dbService<Transact>(context);
            PurgeTransact(transactDataService, transacts);

            dbService<TransactDetail> transactDetailDataService = new dbService<TransactDetail>(context);
            transactDetails = transactDetailDataService.GetAll();
            transactDetailDataService.DeleteRange(transactDetails);

            Transact t1 = new Transact { Date = DateTime.Today, Payee = "Top Company", AccountId = savingsAccount.Id };
            transactDataService.Add(t1);
            TransactDetail t1n1 = new GeneralTransactDetail { Order = 1, Amount = 2500, Category = "Salary", TransactId = t1.Id };
            transactDetailDataService.Add(t1n1);
            IMakeTransact makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t1.Id);

            Transact t2 = new Transact { Date = DateTime.Today, Payee = "Excellent Store", AccountId = creditAccount.Id };
            transactDataService.Add(t2);
            TransactDetail t2n1 = new GeneralTransactDetail { Order = 1, Amount = -25, Category = "Milk", TransactId = t2.Id };
            transactDetailDataService.Add(t2n1);
            TransactDetail t2n2 = new GeneralTransactDetail { Order = 2, Amount = -30, Category = "Fruits", TransactId = t2.Id };
            transactDetailDataService.Add(t2n2);
            makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t2.Id);

            Transact t3 = new Transact { Date = DateTime.Today, Payee = "Sure Forex", AccountId = checkingAccount.Id };
            transactDataService.Add(t3);
            TransactDetail t3n1 = new ForexTransactDetail { Order = 1, Amount = 1350, ForexCurrency = "USD", ForexAmount = 1000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n1);
            TransactDetail t3n2 = new ForexTransactDetail { Order = 2, Amount = -865.92M, ForexCurrency = "HKD", ForexAmount = 5000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n2);
            TransactDetail t3n3 = new ForexTransactDetail { Order = 3, Amount = 1020, ForexCurrency = "AUD", ForexAmount = 1000, TransactId = t3.Id };
            transactDetailDataService.Add(t3n3);
            makeTransact = new MakeGeneralTransact();
            makeTransact.MakeTransact(context, t3.Id);

            Transact t4 = new Transact { Date = DateTime.Today, Payee = "Never Exchange", AccountId = tradingAccount.Id };
            transactDataService.Add(t4);
            TransactDetail t4n1 = new TradingTransactDetail { Order = 1, Unit = 150, Price = 1.75M, PriceDate = DateTime.Today.AddDays(1), TradingId = savingsAccount.Id, TransactId = t4.Id };
            transactDetailDataService.Add(t4n1);
            makeTransact = new MakeTradingTransact();
            makeTransact.MakeTransact(context, t4.Id);

            Transact t5 = new Transact { Date = DateTime.Today, Payee = "Internal Transfer", AccountId = savingsAccount.Id };
            transactDataService.Add(t5);
            TransactDetail t5n1 = new TransferTransactDetail { Order = 1, Amount = 750, TransferId = checkingAccount.Id, TransactId = t5.Id };
            transactDetailDataService.Add(t5n1);
            makeTransact = new MakeTransferTransact();
            makeTransact.MakeTransact(context, t5.Id);



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
        private static void PurgeAccount(dbService<Account> accountDataService, List<Account> accounts)
        {
            accounts = accountDataService.GetAll();
            accountDataService.DeleteRange(accounts);
            Console.WriteLine("Accounts purged.");
            Console.WriteLine("");
        }
        private static void PurgeTransact(dbService<Transact> transactDataService, List<Transact> transacts)
        {
            transacts = transactDataService.GetAll();
            transactDataService.DeleteRange(transacts);
            Console.WriteLine("Transactions purged.");
            Console.WriteLine("");
        }
        private static void CreateAccount(dbService<Account> accountDataService, Account savingsAccount, Account checkingAccount, Account creditAccount, Account tradingAccount)
        {
            savingsAccount = new GeneralAccount { Name = "Acme Savings", Open = 1000 };
            checkingAccount = new GeneralAccount { Name = "Acme Checking", Open = 0 };
            creditAccount = new CreditAccount { Name = "Acme Credit", Open = 0, Limit = -5000 };
            tradingAccount = new TradingAccount { Name = "Acme Shares", Symbol = "ACME", Open = 100, Price = 1.5M, PriceDate = DateTime.Today };
            accountDataService.Add(savingsAccount);
            accountDataService.Add(checkingAccount);
            accountDataService.Add(creditAccount);
            accountDataService.Add(tradingAccount);
            Console.WriteLine("Accounts created.");
            Console.WriteLine("");
        }
        private static void ReadAccount(dbContext context, Account savingsAccount, Account checkingAccount, Account creditAccount, Account tradingAccount)
        {
            savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));
        }
    }
}
