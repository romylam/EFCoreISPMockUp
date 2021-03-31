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
        decimal Open { get; set; }
        decimal Debit { get; set; }
        decimal Credit { get; set; }
        decimal Transfer { get; set; }
        decimal Balance { get; }
        decimal Worth { get; }
        string OnDisplay { get; }
    }
}
