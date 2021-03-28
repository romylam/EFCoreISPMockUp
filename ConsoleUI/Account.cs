using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Account : Common
    {
        public string Class { get; set; }
        public decimal Open { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public decimal Transfer { get; set; } = 0;
        public string Symbol { get; set; }
        public decimal Price { get; set; } = 0;
        public DateTime DateOfPrice { get; set; }
        [NotMapped]
        public decimal Balance { get { return Open + Debit + Credit + Transfer; } }
        [NotMapped]
        public decimal Worth { get; set; }
        [NotMapped]
        public string OnDisplay { get; set; }
    }
    public class GeneralAccount : Account
    {
        public GeneralAccount()
        {
            Class = "General";
            Worth = Balance;
            OnDisplay = $"{Worth:c2}";
        }
    }
    public class TradingAccount : Account
    {
        public TradingAccount()
        {
            Class = "Trading";
            Worth = Balance * Price;
            OnDisplay = $"{Balance} share(s) worth {Worth:c2}";
        }
    }
}
