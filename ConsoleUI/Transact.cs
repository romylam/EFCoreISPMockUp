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
        public string Class { get; set; }
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
    public class GeneralTransact : Transact
    {
        public GeneralTransact()
        {
            Class = "General";
        }    
    }
    public class TradingTransact : Transact
    {
        public TradingTransact()
        {
            Class = "Trading";
        }
    }
}
