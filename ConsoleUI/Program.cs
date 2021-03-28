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

            dbService<Resident> residentDataService = new dbService<Resident>(context);
            List<Resident> residents = residentDataService.GetAll();
            residentDataService.DeleteRange(residents);
            residentDataService.Add(new Owner { FirstName = "Nick", LastName = "Morland" });
            residentDataService.Add(new Tenant { FirstName = "Jessie", LastName = "Clark" });
            Console.WriteLine("Residents created.");

            dbService<House> houseDataService = new dbService<House>(context);
            List<House> houses = houseDataService.GetAll();
            houseDataService.DeleteRange(houses);
            houseDataService.Add(new TownHouse { Address = "1 Elm Street", IsForSale = true, IsForRent = false });
            houseDataService.Add(new SemiDetachedHouse { Address = "5 York Street", IsForSale = false, IsForRent = true });
            houseDataService.Add(new ShopHouse { Address = "10 Main Street", IsForSale = true, IsForRent = true });
            Console.WriteLine("Houses created.");

            dbService<Transact> transactDataService = new dbService<Transact>(context);
            List<Transact> transacts = transactDataService.GetAll();
            transactDataService.DeleteRange(transacts);
            IMakeTransact makeTransact = new BuyAHouse();
            transactDataService.Add(makeTransact.TransactHouse(1, 1, 500000M));
            House house = houseDataService.Get(1);
            Resident resident = residentDataService.Get(1);
            house.Residents = new List<Resident>();
            house.Residents.Add(resident);
            resident.Houses = new List<House>();
            resident.Houses.Add(house);
            houseDataService.Update(house);
            residentDataService.Update(resident);
            transactDataService.Add(makeTransact.TransactHouse(3, 1, 1500000M));
            house = houseDataService.Get(3);
            resident = residentDataService.Get(1);
            house.Residents = new List<Resident>();
            house.Residents.Add(resident);
            resident.Houses.Add(house);
            houseDataService.Update(house);
            residentDataService.Update(resident);
            makeTransact = new RentAHouse();
            transactDataService.Add(makeTransact.TransactHouse(1, 2, 8500M));
            house = houseDataService.Get(1);
            resident = residentDataService.Get(2);
            house.Residents.Add(resident);
            resident.Houses = new List<House>();
            resident.Houses.Add(house);
            houseDataService.Update(house);
            residentDataService.Update(resident);
            transactDataService.Add(makeTransact.TransactHouse(2, 2, 4000M));
            house = houseDataService.Get(2);
            resident = residentDataService.Get(2);
            house.Residents = new List<Resident>();
            house.Residents.Add(resident);
            resident.Houses.Add(house);
            houseDataService.Update(house);
            residentDataService.Update(resident);

            transacts = context.Transacts.Include(h => h.House).Include(r => r.Resident).ToList();
            foreach (var item in transacts)
            {
                Console.WriteLine($"{item.House.Address} {item.ActualTransact} to {item.Resident.FirstName} for {item.Amount:c2}");
            }

            residents = context.Residents.Include(h => h.Houses).ToList();
            foreach (var item in residents)
            {
                Console.WriteLine($"{item.FirstName} have {item.Houses.Count} house(s)");
            }

            houses = context.Houses.Include(r => r.Residents).ToList();
            foreach (var item in houses)
            {
                Console.WriteLine($"{item.Address} have {item.Residents.Count} resident(s)");
            }
        }
    }
}
