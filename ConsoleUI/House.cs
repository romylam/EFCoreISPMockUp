using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class House
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public bool IsForSale { get; set; }
        public bool IsForRent { get; set; }
        public ICollection<Resident> Residents { get; set; }
    }
    public class TownHouse : House
    {
    }
    public class SemiDetachedHouse : House
    {
    }
    public class ShopHouse : House
    {
    }
}
