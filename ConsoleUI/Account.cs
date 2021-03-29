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
        public string Name { get; set; }
        public decimal Open { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public decimal Transfer { get; set; } = 0;
        [NotMapped]
        public decimal Balance { get { return Open + Debit + Credit + Transfer; } }
        [NotMapped]
        public virtual decimal Worth { get { return Balance; } }
        [NotMapped]
        public virtual string OnDisplay { get { return $"{Worth:c2}"; } }
    }
    public class CreditAccount : Account
    {
        public decimal Limit { get; set; }
        public override string OnDisplay { get { return $"{Worth:c2} against limit of {Limit:c2}"; } }
    }
    public class TradingAccount : Account
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceDate { get; set; }
        [NotMapped]
        public override decimal Worth { get { return Balance * Price; } }
        [NotMapped]
        public override string OnDisplay {  get { return $"{Balance:n0} unit(s) @{Price:c2} value {Worth:c2}"; } }
    }
}
