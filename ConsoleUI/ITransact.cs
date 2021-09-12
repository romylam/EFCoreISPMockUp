using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface ITransact
    {
        DateOnly Date { get; set; }
        string PayeeId { get; set; }
        decimal Amount { get; set; }
        string AccountId { get; set; }
        Payee Payee { get; set; }
        Account Account { get; set; }

    }
}
