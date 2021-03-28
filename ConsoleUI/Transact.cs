using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Transact
    {
        public int Id { get; set; }
        public string BuyOrRent { get; set; }
        public decimal Amount { get; set; }
        public int ResidentId { get; set; }
        public Resident Resident { get; set; }
        public int HouseId { get; set; }
        public House House { get; set; }
        public string ActualTransact { get; set; }
    }
    public class BuyHouse : Transact
    {
        public BuyHouse()
        {
            ActualTransact = "Sold";
        }
    }
    public class RentHouse : Transact
    {
        public RentHouse()
        {
            ActualTransact = "Rent";
        }
    }
}
