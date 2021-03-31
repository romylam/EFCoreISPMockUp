using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Account : Common, IAccount
    {
        public string Name { get; set; }
        public decimal Open { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public decimal Transfer { get; set; } = 0;
        [NotMapped]
        public decimal Balance { get { return Open + Debit + Credit + Transfer; } }
        [NotMapped]
        public virtual decimal Worth { get; }
        [NotMapped]
        public virtual string OnDisplay { get; }
    }
    public class GeneralAccount : Account, IAccount
    {
        public override decimal Worth { get { return Balance; } }
        public override string OnDisplay { get { return $"{Worth:c2}"; } }
    }
    public class CreditAccount : Account, IAccount
    {
        public decimal Limit { get; set; }
        public override decimal Worth { get { return Balance; } }
        public override string OnDisplay { get { return $"{Worth:c2} against limit of {Limit:c2}"; } }
    }
    public class TradingAccount : Account, IAccount
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceDate { get; set; }
        public override decimal Worth { get { return Balance * Price; } }
        public override string OnDisplay {  get { return $"{Balance:n0} unit(s) @{Price:c2} value {Worth:c2}"; } }
    }
}
