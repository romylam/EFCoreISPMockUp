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

            dbContextFactory factory = new dbContextFactory();
            dbContext context = factory.CreateDbContext();

            Group group = new Group();
            List<Group> groups = new List<Group>();

            Type type = new Type();
            List<Type> types = new List<Type>();

            Currency currency = new Currency();
            List<Currency> currencies = new List<Currency>();

            Payee payee = new Payee();
            List<Payee> payees = new List<Payee>();

            Category category = new Category();
            List<Category> categories = new List<Category>();

            Subcategory subcategory = new Subcategory();
            List<Subcategory> subcategories = new List<Subcategory>();

            MasterKey masterKey = new MasterKey();
            List<MasterKey> masterKeys = new List<MasterKey>();

            Account savingsAccount = new GeneralAccount();
            Account checkingAccount = new GeneralAccount();
            Account foreign1Account = new GeneralAccount();
            Account foreign2Account = new GeneralAccount();
            Account creditAccount = new CreditAccount();
            Account tradingAccount = new TradingAccount();
            List<Account> accounts = new List<Account>();

            List<Transact> transacts = new List<Transact>();
            List<TransactDetail> transactDetails = new List<TransactDetail>();

            dbService<MasterKey> masterKeyDataService = new dbService<MasterKey>(context);
            dbService<Account> accountDataService = new dbService<Account>(context);
            dbService<Transact> transactDataService = new dbService<Transact>(context);
            dbService<TransactDetail> transactDetailDataService = new dbService<TransactDetail>(context);

            PurgeTransactDetails(transactDetailDataService, transactDetails);
            PurgeTransact(transactDataService, transacts);
            PurgeAccount(accountDataService, accounts);
            PurgeSystemSupport(context, groups, types, currencies, payees, categories, subcategories);
            PurgeMasterKey(masterKeyDataService, masterKeys);

            CreateMasterKey(masterKeyDataService, masterKey);
            CreateSystemSuppport(context, group, type, currency, payee, category, subcategory);
            CreateAccount(context, accountDataService, savingsAccount, checkingAccount, foreign1Account, foreign2Account, creditAccount, tradingAccount);

            accounts = accountDataService.GetAll();
            ListAccount(accounts);

            savingsAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Savings"));
            checkingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Checking"));
            foreign1Account = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Foreign 1"));
            foreign2Account = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Foreign 2"));
            creditAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Credit"));
            tradingAccount = context.Accounts.FirstOrDefault(n => n.Name.Equals("Acme Shares"));

            Transact t1 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                                            PayeeId = getPayee(context, "Zen Company"), AccountId = savingsAccount.Id, Note = "" };
            transactDataService.Add(t1);
            TransactDetail t1n1 = new GeneralTransactDetail { Id = t1.Id + "-0001", TransactId = t1.Id, Order = 1, Amount = 1500, 
                CategoryId = getCategory(context, "Income"), SubcategoryId = getSubcategory(context, "Salary") };
            transactDetailDataService.Add(t1n1);
            TransactDetail t1n2 = new GeneralTransactDetail { Id = t1.Id + "-0002", TransactId = t1.Id, Order = 2, Amount = -150, 
                CategoryId = getCategory(context, "Tax"), SubcategoryId = getSubcategory(context, "Withholding Tax") };
            transactDetailDataService.Add(t1n2);

            Transact t2 = new Transact { Id = NextKey(context, "Transact"), Date = t1.Date, Payee = t1.Payee, AccountId = checkingAccount.Id, Note = "" };
            transactDataService.Add(t2);
            TransactDetail t1n3 = new TransferTransactDetail { Id = t1.Id + "-0003", TransactId = t1.Id, Order = 3, Amount = -1000, TransferId = checkingAccount.Id,
                                                                LinkId = t2.Id, LinkOrder = 1 };
            transactDetailDataService.Add(t1n3);
            TransactDetail t2n1 = new TransferTransactDetail { Id = t2.Id + "-0001", TransactId = t2.Id, Order = 1, Amount = t1n3.Amount * -1, TransferId = t1.AccountId,
                                                               LinkId = t1.Id, LinkOrder = t1n3.Order };
            transactDetailDataService.Add(t2n1);

            Transact t3 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                                            PayeeId = getPayee(context, "Organic Store"), AccountId = creditAccount.Id, Note = "" };
            transactDataService.Add(t3);
            TransactDetail t3n1 = new CreditTransactDetail { Id = t3.Id + "-0001", TransactId = t3.Id, Order = 1, Amount = -15, 
                CategoryId = getCategory(context, "Food"), SubcategoryId = getSubcategory(context, "Bread") };
            transactDetailDataService.Add(t3n1);
            TransactDetail t3n2 = new CreditTransactDetail { Id = t3.Id + "-0002", TransactId = t3.Id, Order = 2, Amount = -20, 
                CategoryId = getCategory(context, "Food"), SubcategoryId = getSubcategory(context, "Fruits")
            };
            transactDetailDataService.Add(t3n2);
            TransactDetail t3n3 = new CreditTransactDetail { Id = t3.Id + "-0003", TransactId = t3.Id, Order = 3, Amount = -60, 
                CategoryId = getCategory(context, "Household") };
            transactDetailDataService.Add(t3n3);

            Transact t4 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                                            PayeeId = getPayee(context, "Sure Exchange"), AccountId = savingsAccount.Id, Note = "" };
            transactDataService.Add(t4);
            Transact t5 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                                            PayeeId = getPayee(context, "Sure Exchange"), AccountId = foreign1Account.Id, Note = "" };
            transactDataService.Add(t5);
            Decimal originalAmount = 100;
            Decimal foreignAmount = originalAmount * foreign1Account.Currency.Rate;
            TransactDetail t4n1 = new ForexTransactDetail { Id = t4.Id + "-0001", TransactId = t4.Id, Order = 1, Amount = -originalAmount, 
                TransferId = foreign1Account.Id, ForexAmount = -foreignAmount, LinkId = t5.Id, LinkOrder = 1 };
            transactDetailDataService.Add(t4n1);
            TransactDetail t5n1 = new ForexTransactDetail { Id = t5.Id + "-0001", TransactId = t5.Id, Order = 1, Amount = foreignAmount, 
                TransferId = savingsAccount.Id, ForexAmount = originalAmount, LinkId = t4.Id, LinkOrder = 1 };
            transactDetailDataService.Add(t5n1);

            Transact t6 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                PayeeId = getPayee(context, "Win Shares"), AccountId = tradingAccount.Id, Note = "" };
            transactDataService.Add(t6);
            Transact t7 = new Transact { Id = NextKey(context, "Transact"), Date = DateOnly.FromDateTime(DateTime.Today), 
                PayeeId = getPayee(context, "Win Shares"), AccountId = savingsAccount.Id, Note = "" };
            transactDataService.Add(t7);
            Decimal tradingUnit = 150;
            Decimal tradingPrice = 1.75M;
            TransactDetail t6n1 = new TradingTransactDetail { Id = t6.Id + "-0001", TransactId = t6.Id, Order = 1, Amount = tradingUnit, Price = tradingPrice, 
                TransferId = savingsAccount.Id, LinkId = t7.Id, LinkOrder = 1 };
            transactDetailDataService.Add(t6n1);
            TransactDetail t7n1 = new TradingFromTransactDetail { Id = t7.Id + "-0001", TransactId = t7.Id, Order = 1, Amount = tradingUnit * tradingPrice * -1, 
                TransferId = tradingAccount.Id, CategoryId = getCategory(context, "Shares"), SubcategoryId = getSubcategory(context, "Buy"), 
                LinkId = t6.Id, LinkOrder = 1 };
            transactDetailDataService.Add(t7n1);

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
            postTransact.PostTransact(context, t1, t1n3);
            Console.WriteLine($"{t1n3.OnDisplay} on {t1.Date}");
            postTransact.ReverseTransact(context, t1, t1n3);
            Console.WriteLine($"Reversed {t1n3.OnDisplay} on {t1.Date}");
            t1n3.Amount = -500;
            t2n1.Amount = t1n3.Amount * -1;
            postTransact.PostTransact(context, t1, t1n3);
            Console.WriteLine($"{t1n3.OnDisplay} on {t1.Date}");
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

            postTransact = new PostForexTransact();
            postTransact.PostTransact(context, t4, t4n1);
            Console.WriteLine($"{t4n1.OnDisplay} on {t4.Date}");
            postTransact.ReverseTransact(context, t4, t4n1);
            Console.WriteLine($"Reversed {t4n1.OnDisplay} on {t4.Date}");
            originalAmount = 250;
            foreignAmount = originalAmount * foreign2Account.Currency.Rate;
            t4n1.Amount = -originalAmount;
            ((ForexTransactDetail)t4n1).TransferId = foreign2Account.Id;
            ((ForexTransactDetail)t4n1).ForexAmount = -foreignAmount;
            t5.AccountId = foreign2Account.Id;
            t5n1.Amount = foreignAmount;
            ((ForexTransactDetail)t5n1).ForexAmount = originalAmount;
            transactDetailDataService.Update(t4n1);
            postTransact.PostTransact(context, t4, t4n1);
            Console.WriteLine($"{t4n1.OnDisplay} on {t4.Date}");
            Console.WriteLine("");

            postTransact = new PostTradingTransact();
            postTransact.PostTransact(context, t6, t6n1);
            Console.WriteLine($"{t6n1.OnDisplay} on {t6.Date}");
            postTransact.ReverseTransact(context, t6, t6n1);
            Console.WriteLine($"Reversed {t6n1.OnDisplay} on {t6.Date}");
            tradingUnit = 200;
            tradingPrice = 2.15M;
            t6n1.Amount = tradingUnit;
            ((TradingTransactDetail)t6n1).Price = tradingPrice;
            t7n1.Amount = tradingUnit * tradingPrice * -1;
            postTransact.PostTransact(context, t6, t6n1);
            Console.WriteLine($"{t6n1.OnDisplay} on {t6.Date}");
            Console.WriteLine("");

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
                Console.Write($"On {item.Date:d} account {item.Account.Name} {item.Payee.Name} ");
                foreach (var detail in item.TransactDetail)
                {
                    Console.WriteLine($"{detail.OnDisplay} ");
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
        private static void PurgeSystemSupport(dbContext context, List<Group> groups, List<Type> types, List<Currency> currencies, List<Payee> payees, 
            List<Category> categories, List<Subcategory> subcategories)
        {
            groups = context.Groups.ToList();
            context.Groups.RemoveRange(groups);
            Console.WriteLine("Account Groups purged.");
            Console.WriteLine("");

            types = context.Types.ToList();
            context.Types.RemoveRange(types);
            Console.WriteLine("Account Types purged.");
            Console.WriteLine("");

            currencies = context.Currencies.ToList();
            context.Currencies.RemoveRange(currencies);
            Console.WriteLine("Currencies purged.");
            Console.WriteLine("");

            payees = context.Payees.ToList();
            context.Payees.RemoveRange(payees);
            Console.WriteLine("Payees purged.");
            Console.WriteLine("");

            subcategories = context.Subcategories.ToList();
            context.Subcategories.RemoveRange(subcategories);
            Console.WriteLine("Subcategories purged.");
            Console.WriteLine("");

            categories = context.Categories.ToList();
            context.Categories.RemoveRange(categories);
            Console.WriteLine("Categories purged.");
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
            masterKey = new MasterKey { Id = "Subcategory", Prefix = "SC", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            masterKey = new MasterKey { Id = "Tag", Prefix = "TG", TwoFactor = false, NextTerm = 0, NextKey = 0 };
            masterKeyDataService.Add(masterKey);
            Console.WriteLine("Master Keys created.");
            Console.WriteLine("");
        }
        private static void CreateSystemSuppport(dbContext context, Group group, Type type, Currency currency, Payee payee, Category category, Subcategory subcategory)
        {
            group = new Group { Id = NextKey(context, "Group"), Order = 1, Name = "Core" };
            context.Groups.Add(group);
            group = new Group { Id = NextKey(context, "Group"), Order = 2, Name = "Credit" };
            context.Groups.Add(group);
            group = new Group { Id = NextKey(context, "Group"), Order = 3, Name = "Finance" };
            context.Groups.Add(group);
            Console.WriteLine("Account Groups created.");
            Console.WriteLine("");

            type = new Type { Id = NextKey(context, "Type"), Order = 1, Name = "Cash" };
            context.Types.Add(type);
            type = new Type { Id = NextKey(context, "Type"), Order = 2, Name = "Bank" };
            context.Types.Add(type);
            type = new Type { Id = NextKey(context, "Type"), Order = 3, Name = "Credit Card" };
            context.Types.Add(type);
            type = new Type { Id = NextKey(context, "Type"), Order = 4, Name = "Investment" };
            context.Types.Add(type);
            Console.WriteLine("Account Types created.");
            Console.WriteLine("");

            currency = new Currency { Id = "USD", Name = "United States Dollar", Prefix = "$", Rate = 1 };
            context.Currencies.Add(currency);
            currency = new Currency { Id = "CNY", Name = "Chinese Yuan", Prefix = "¥", Rate = 6.47M };
            context.Currencies.Add(currency);
            currency = new Currency { Id = "EUR", Name = "EUR", Prefix = "€", Rate = 0.85M };
            context.Currencies.Add(currency);
            Console.WriteLine("Currencies created.");
            Console.WriteLine("");

            payee = new Payee { Id = NextKey(context, "Payee"), Name = "Zen Company" };
            context.Payees.Add(payee);
            payee = new Payee { Id = NextKey(context, "Payee"), Name = "Organic Store" };
            context.Payees.Add(payee);
            payee = new Payee { Id = NextKey(context, "Payee"), Name = "Sure Exchange" };
            context.Payees.Add(payee);
            payee = new Payee { Id = NextKey(context, "Payee"), Name = "Win Shares" };
            context.Payees.Add(payee);
            Console.WriteLine("Payees created.");
            Console.WriteLine("");

            category = new Category { Id = NextKey(context, "Category"), Name = "Income" };
            context.Categories.Add(category);
            category = new Category { Id = NextKey(context, "Category"), Name = "Tax" };
            context.Categories.Add(category);
            category = new Category { Id = NextKey(context, "Category"), Name = "Food" };
            context.Categories.Add(category);
            category = new Category { Id = NextKey(context, "Category"), Name = "Household" };
            context.Categories.Add(category);
            category = new Category { Id = NextKey(context, "Category"), Name = "Shares" };
            context.Categories.Add(category);
            Console.WriteLine("Categories created.");
            Console.WriteLine("");

            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Salary", CategoryId = getCategory(context, "Income") };
            context.Subcategories.Add(subcategory);
            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Withholding Tax", CategoryId = getCategory(context, "Tax") };
            context.Subcategories.Add(subcategory);
            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Bread", CategoryId = getCategory(context, "Food") };
            context.Subcategories.Add(subcategory);
            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Fruits", CategoryId = getCategory(context, "Food") };
            context.Subcategories.Add(subcategory);
            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Buy", CategoryId = getCategory(context, "Shares") };
            context.Subcategories.Add(subcategory);
            subcategory = new Subcategory { Id = NextKey(context, "Subcategory"), Name = "Sell", CategoryId = getCategory(context, "Shares") };
            context.Subcategories.Add(subcategory);
            Console.WriteLine("Subcategories created.");
            Console.WriteLine("");
        }

        private static void CreateAccount(dbContext context, dbService<Account> accountDataService, 
            Account savingsAccount, Account checkingAccount, Account foreignAccount1, Account foreignAccount2, Account creditAccount, Account tradingAccount)
        {
            savingsAccount = new GeneralAccount { Id = NextKey(context,"Account"), GroupId = getGroup(context, "Core"), TypeId = getType(context, "Bank"),
                                                    CurrencyId = "USD", Name = "Acme Savings", Open = 1000 };
            checkingAccount = new GeneralAccount { Id = NextKey(context, "Account"), GroupId = getGroup(context, "Core"), TypeId = getType(context, "Bank"),
                                                    CurrencyId = "USD", Name = "Acme Checking", Open = 0 };
            foreignAccount1 = new GeneralAccount { Id = NextKey(context, "Account"), GroupId = getGroup(context, "Finance"), TypeId = getType(context, "Bank"),
                                                    CurrencyId = "CNY", Name = "Acme Foreign 1", Open = 5000 };
            foreignAccount2 = new GeneralAccount { Id = NextKey(context, "Account"), GroupId = getGroup(context, "Finance"), TypeId = getType(context, "Bank"),
                                                    CurrencyId = "EUR", Name = "Acme Foreign 2", Open = 2500 };
            creditAccount = new CreditAccount { Id = NextKey(context, "Account"), GroupId = getGroup(context, "Credit"), TypeId = getType(context, "Credit Card"), 
                                                    CurrencyId = "USD", Name = "Acme Credit", Open = 0, Limit = -5000 };
            tradingAccount = new TradingAccount { Id = NextKey(context, "Account"), GroupId = getGroup(context, "Finance"), TypeId = getType(context, "Investment"),
                                                    CurrencyId = "USD", Name = "Acme Shares", Symbol = "ACME", Open = 100, Price = 1.5M, 
                                                    PriceDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(-1) };
            accountDataService.Add(savingsAccount);
            accountDataService.Add(checkingAccount);
            accountDataService.Add(foreignAccount1);
            accountDataService.Add(foreignAccount2);
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
        private static string getGroup(dbContext context, string name)
        {
            Group group = context.Groups.FirstOrDefault(n => n.Name.Equals(name));
            return group == null ? "Group Not Found" : group.Id;
        }
        private static string getType(dbContext context, string name)
        {
            Type type = context.Types.FirstOrDefault(n => n.Name.Equals(name));
            return type == null ? "Type Not Found" : type.Id;
        }
        private static string getPayee(dbContext context, string name)
        {
            Payee payee = context.Payees.FirstOrDefault(n => n.Name.Equals(name));
            return payee == null ? "Payee Not Found" : payee.Id;
        }
        private static string getCategory(dbContext context, string name)
        {
            Category category = context.Categories.FirstOrDefault(n => n.Name.Equals(name));
            return category == null ? "Category Not Found" : category.Id;
        }
        private static string getSubcategory(dbContext context, string name)
        {
            Subcategory subcategory = context.Subcategories.FirstOrDefault(n => n.Name.Equals(name));
            return subcategory == null ? "Subcategory Not Found" : subcategory.Id;
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
