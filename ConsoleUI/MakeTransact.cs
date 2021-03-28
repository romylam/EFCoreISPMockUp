using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMakeTransact
    {
        Transact TransactHouse(int houseId, int residentId, decimal amount);
    }
    public class BuyAHouse : IMakeTransact
    {
        public Transact TransactHouse(int houseId, int residentId, decimal amount)
        {
            Transact buyHouse = new BuyHouse();
            buyHouse.HouseId = houseId;
            buyHouse.ResidentId = residentId;
            buyHouse.BuyOrRent = "Buy";
            buyHouse.Amount = amount;
            return buyHouse;
        }
    }
    public class RentAHouse : IMakeTransact
    {
        public Transact TransactHouse(int houseId, int residentId, decimal amount)
        {
            Transact rentHouse = new RentHouse();
            rentHouse.HouseId = houseId;
            rentHouse.ResidentId = residentId;
            rentHouse.BuyOrRent = "Rent";
            rentHouse.Amount = amount;
            return rentHouse;
        }
    }
}
