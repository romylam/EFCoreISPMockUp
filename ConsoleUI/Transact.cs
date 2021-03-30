using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Transact : Common, ITransact
    {
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        [NotMapped]
        public virtual string OnDisplay { get; }
    }
    public class GeneralTransact : Transact, ITransact
    {
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Amount:c2} into {Account.Name}" : $"Debited {Amount:c2} from { Account.Name}"; }
        }
    }
    public class TradingTransact : Transact, ITransact
    {
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceDate { get; set; }
        public override string OnDisplay
        {
            get { return Unit > 0 ? $"Bought {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}" : $"Sold {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}"; }
        }

    }
}
