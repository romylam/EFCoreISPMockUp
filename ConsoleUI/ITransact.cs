using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface ITransact
    {
        DateTime Date { get; set; }
        string Payee { get; set; }
        decimal Amount { get; set; }
        int AccountId { get; set; }
        Account Account { get; set; }
        string OnDisplay { get; }
    }
}
