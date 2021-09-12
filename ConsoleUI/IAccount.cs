using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IAccount
    {
        string Name { get; set; }
        string GroupId { get; set; }
        string TypeId { get; set; }
        string CurrencyId { get; set; }
        decimal Open { get; set; }
        decimal Debit { get; set; }
        decimal Credit { get; set; }
        decimal Transfer { get; set; }
        string Status { get; set; }
        bool Default { get; set; }
        Group Group { get; set; }
        Type Type { get; set; }
        Currency Currency { get; set; }
        decimal Balance { get; }
        decimal Worth { get; }
        string OnDisplay { get; }
    }
}
