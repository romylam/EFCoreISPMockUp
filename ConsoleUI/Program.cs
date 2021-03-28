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
            //accountDataService.Add(new GeneralAccount { Name = "Acme Checking" });
            //accountDataService.Add(new TradingAccount { Name = "Acme Shares", Symbol = "ACME", Open = 125, Price = 1.48M });
            //Console.WriteLine("Accounts created.");

            //dbService<Transact> transactDataService = new dbService<Transact>(context);
            //List<Transact> transacts = transactDataService.GetAll();
            //transactDataService.DeleteRange(transacts);

            //transacts = context.Transacts.Include(h => h.House).Include(r => r.Resident).ToList();
            //foreach (var item in transacts)
            //{
            //    Console.WriteLine($"{item.House.Address} {item.ActualTransact} to {item.Resident.FirstName} for {item.Amount:c2}");
            //}

            accounts = accountDataService.GetAll();
            foreach (var item in accounts)
            {
                Console.WriteLine($"{item.Class} account {item.Name} have a balance of {item.OnDisplay}");
            }
        }
    }
}
