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
            switch (typeof(string).Assembly.GetName().ProcessorArchitecture)
            {
                case System.Reflection.ProcessorArchitecture.X86:
                    Console.WriteLine("This is the Intel 32-bit Version");
                    break;
                case System.Reflection.ProcessorArchitecture.Amd64:
                    Console.WriteLine("This is the Intel 64-bit Version");
                    break;
                case System.Reflection.ProcessorArchitecture.Arm:
                    Console.WriteLine("This is the ARM 64-bit Version");
                    break;
                default:
                    Console.WriteLine("Unknown CPU Architecture");
                    break;
            }
            Console.WriteLine("");

            MasterKey masterKey = new MasterKey();
            List<MasterKey> masterKeys = new List<MasterKey>();

            Account savingsAccount = new GeneralAccount();
            Account checkingAccount = new GeneralAccount();
            Account creditAccount = new CreditAccount();
            Account tradingAccount = new TradingAccount();
            List<Account> accounts = new List<Account>();

            List<Transact> transacts = new List<Transact>();
            List<TransactDetail> transactDetails = new List<TransactDetail>();

            dbContextFactory factory = new dbContextFactory();
            dbContext context = factory.CreateDbContext();

            dbService<MasterKey> masterKeyDataService = new dbService<MasterKey>(context);
            dbService<Account> accountDataService = new dbService<Account>(context);
            dbService<Transact> transactDataService = new dbService<Transact>(context);
            dbService<TransactDetail> transactDetailDataService = new dbService<TransactDetail>(context);

            PurgeTransactDetails(transactDetailDataService, transactDetails);
            PurgeTransact(transactDataService, transacts);
            PurgeAccount(accountDataService, accounts);
            PurgeMasterKey(masterKeyDataService, masterKeys);

            CreateMasterKey(masterKeyDataService, masterKey);
            CreateAccount(context, accountDataService, savingsAccount, checkingAccount, creditAccount, tradingAccount);

            accounts = accountDataService.GetAll();
            ListAccount(accounts);

            savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));

            Transact t1 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), Payee = "Zen Company", AccountId = savingsAccount.Id };
            transactDataService.Add(t1);
            TransactDetail t1n1 = new GeneralTransactDetail { Id = t1.Id + "-0001", TransactId = t1.Id, Order = "0001", Amount = 1500, Category = "Salary" };
            transactDetailDataService.Add(t1n1);
            TransactDetail t1n2 = new GeneralTransactDetail { Id = t1.Id + "-0002", TransactId = t1.Id, Order = "0002", Amount = -150, Category = "Withholding Tax" };
            transactDetailDataService.Add(t1n2);

            Transact t2 = new Transact { Id = NextKey(context, "Transact"), Date = t1.Date, Payee = t1.Payee, AccountId = checkingAccount.Id };
            transactDataService.Add(t2);
            TransactDetail t1n4 = new TransferTransactDetail { Id = t1.Id + "-0003", TransactId = t1.Id, Order = "0003", Amount = -1000, TransferId = checkingAccount.Id,
                                                                LinkId = t2.Id, LinkOrder = "0001" };
            transactDetailDataService.Add(t1n4);
            TransactDetail t2n1 = new TransferTransactDetail { Id = t2.Id + "-0001", TransactId = t1.Id, Order = "0001", Amount = t1n4.Amount * -1, TransferId = t1.AccountId,
                                                               LinkId = t1.Id, LinkOrder = t1n4.Order };
            transactDetailDataService.Add(t2n1);

            Transact t3 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), Payee = "Organic Store", AccountId = creditAccount.Id };
            transactDataService.Add(t3);
            TransactDetail t3n1 = new CreditTransactDetail { Id = t3.Id + "-0001", TransactId = t3.Id, Order = "0001", Amount = -15, Category = "Food" };
            transactDetailDataService.Add(t3n1);
            TransactDetail t3n2 = new CreditTransactDetail { Id = t3.Id + "-0002", TransactId = t3.Id, Order = "0002", Amount = -20, Category = "Alcohol" };
            transactDetailDataService.Add(t3n2);
            TransactDetail t3n3 = new CreditTransactDetail { Id = t3.Id + "-0003", TransactId = t3.Id, Order = "0003", Amount = -60, Category = "Household" };
            transactDetailDataService.Add(t3n3);

            IPostTransact postTransact = new PostGeneralTransact();
            postTransact.PostTransact(context, t1, t1n1);
            Console.WriteLine($"{t1n1.OnDisplay} on {t1.Date} to {t1.Account.Name}");
            postTransact.PostTransact(context, t1, t1n2);
            Console.WriteLine($"{t1n2.OnDisplay} on {t1.Date} to {t1.Account.Name}");
            postTransact.ReverseTransact(context, t1, t1n2);
            Console.WriteLine($"Reversed {t1n2.OnDisplay} on {t1.Date} to {t1.Account.Name}");
            t1n2.Amount = -300;
            postTransact.PostTransact(context, t1, t1n2);
            Console.WriteLine($"{t1n2.OnDisplay} on {t1.Date} to {t1.Account.Name}");
            Console.WriteLine("");

            postTransact = new PostTransferTransact();
            postTransact.PostTransact(context, t1, t1n4);
            Console.WriteLine($"{t1n4.OnDisplay} on {t1.Date}");
            postTransact.ReverseTransact(context, t1, t1n4);
            Console.WriteLine($"Reversed {t1n4.OnDisplay} on {t1.Date}");
            t1n4.Amount = -500;
            t2n1.Amount = t1n4.Amount * -1;
            postTransact.PostTransact(context, t1, t1n4);
            Console.WriteLine($"{t1n4.OnDisplay} on {t1.Date}");
            Console.WriteLine("");

            postTransact = new PostCreditTransact();
            postTransact.PostTransact(context, t3, t3n1);
            Console.WriteLine($"{t3n1.OnDisplay} on {t3.Date} to {t3.Account.Name}");
            postTransact.PostTransact(context, t3, t3n2);
            Console.WriteLine($"{t3n2.OnDisplay} on {t3.Date} to {t3.Account.Name}");
            postTransact.ReverseTransact(context, t3, t3n2);
            Console.WriteLine($"Reversed {t3n2.OnDisplay} on {t3.Date} to {t3.Account.Name}");
            t3n2.Amount = -35;
            postTransact.PostTransact(context, t3, t3n2);
            Console.WriteLine($"{t3n2.OnDisplay} on {t3.Date} to {t3.Account.Name}");
            postTransact.PostTransact(context, t3, t3n3);
            Console.WriteLine($"{t3n3.OnDisplay} on {t3.Date} to {t3.Account.Name}");
            Console.WriteLine("");

            ////Transact t3 = new Transact { Date = DateTime.Today, Payee = "Sure Forex", AccountId = checkingAccount.Id };
            ////transactDataService.Add(t3);
            ////TransactDetail t3n1 = new ForexTransactDetail { Order = 1, Amount = 1350, ForexCurrency = "USD", ForexAmount = 1000, TransactId = t3.Id };
            ////transactDetailDataService.Add(t3n1);
            ////TransactDetail t3n2 = new ForexTransactDetail { Order = 2, Amount = -865.92M, ForexCurrency = "HKD", ForexAmount = 5000, TransactId = t3.Id };
            ////transactDetailDataService.Add(t3n2);
            ////TransactDetail t3n3 = new ForexTransactDetail { Order = 3, Amount = 1020, ForexCurrency = "AUD", ForexAmount = 1000, TransactId = t3.Id };
            ////transactDetailDataService.Add(t3n3);
            ////makeTransact = new MakeGeneralTransact();
            ////makeTransact.MakeTransact(context, t3.Id);

            ////Transact t4 = new Transact { Date = DateTime.Today, Payee = "Never Exchange", AccountId = tradingAccount.Id };
            ////transactDataService.Add(t4);
            ////TransactDetail t4n1 = new TradingTransactDetail { Order = 1, Unit = 150, Price = 1.75M, PriceDate = DateTime.Today.AddDays(1), TradingId = savingsAccount.Id, TransactId = t4.Id };
            ////transactDetailDataService.Add(t4n1);
            ////makeTransact = new MakeTradingTransact();
            ////makeTransact.MakeTransact(context, t4.Id);

            ////Transact t5 = new Transact { Date = DateTime.Today, Payee = "Internal Transfer", AccountId = savingsAccount.Id };
            ////transactDataService.Add(t5);
            ////TransactDetail t5n1 = new TransferTransactDetail { Order = 1, Amount = 750, TransferId = checkingAccount.Id, TransactId = t5.Id };
            ////transactDetailDataService.Add(t5n1);
            ////makeTransact = new MakeTransferTransact();
            ////makeTransact.MakeTransact(context, t5.Id);

            ////transacts = context.Transacts.Include(a => a.Account).Include(t => t.TransactDetail).ToList();
            ////ListTransact(transacts);

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
        private static void PurgeMasterKey(dbService<MasterKey> masterKeyDataService, List<MasterKey> masterKeys)
        {
            masterKeys = masterKeyDataService.GetAll();
            masterKeyDataService.DeleteRange(masterKeys);
            Console.WriteLine("Master Keys purged.");
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
        private static void PurgeTransactDetails(dbService<TransactDetail> transactDetailDataService, List<TransactDetail> transactDetails)
        {
            transactDetails = transactDetailDataService.GetAll();
            transactDetailDataService.DeleteRange(transactDetails);
            Console.WriteLine("Transaction Details purged.");
            Console.WriteLine("");
        }
        private static void CreateMasterKey(dbService<MasterKey> masterKeyDataService, MasterKey masterKey)
        {
            masterKey = new MasterKey { Id = "Account", Prefix = "AC", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Group", Prefix = "GP", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Type", Prefix = "TY", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Transact", Prefix = "TR", TwoFactor = true, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Payee", Prefix = "PY", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Category", Prefix = "CT", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Tag", Prefix = "TG", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            Console.WriteLine("Master Keys created.");
            Console.WriteLine("");
        }
        private static void CreateAccount(dbContext context, dbService<Account> accountDataService, Account savingsAccount, Account checkingAccount, Account creditAccount, Account tradingAccount)
        {
            savingsAccount = new GeneralAccount { Id = NextKey(context,"Account"), Name = "Acme Savings", Open = 1000 };
            checkingAccount = new GeneralAccount { Id = NextKey(context, "Account"), Name = "Acme Checking", Open = 0 };
            creditAccount = new CreditAccount { Id = NextKey(context, "Account"), Name = "Acme Credit", Open = 0, Limit = -5000 };
            tradingAccount = new TradingAccount { Id = NextKey(context, "Account"), Name = "Acme Shares", Symbol = "ACME", Open = 100, 
                Price = 1.5M, PriceDate = DateOnly.FromDateTime(DateTime.Today) };
            accountDataService.Add(savingsAccount);
            accountDataService.Add(checkingAccount);
            accountDataService.Add(creditAccount);
            accountDataService.Add(tradingAccount);
            Console.WriteLine("Accounts created.");
            Console.WriteLine("");
        }
        private static void ReadAccount(dbContext context, Account savingsAccount, Account checkingAccount, Account creditAccount, Account tradingAccount)
        {
            //savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            //checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            //creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            //tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));
        }
        public static string NextKey(dbContext context, string id)
        {
            MasterKey masterKey = context.MasterKeys.Find(id);
            string nextKey = string.Empty;
            if (masterKey == null)
                return "No Key";
            if (masterKey.TwoFactor)
            {
                if (masterKey.NextTerm != DateTime.Today.Year)
                {
                    masterKey.NextTerm = DateTime.Today.Year;
                    masterKey.NextKey = 0;
                }
                nextKey = masterKey.Prefix + (10000 + DateTime.Today.Year).ToString().Substring(1) + (1000001 + masterKey.NextKey).ToString().Substring(1);
            }
            else
            {
                nextKey = masterKey.Prefix + (1000001 + masterKey.NextKey).ToString().Substring(1);
            }
            masterKey.NextKey++;
            context.MasterKeys.Update(masterKey);
            context.SaveChanges();
            return nextKey;
        }
    }
}
