using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Transact : iKey, ITransact
    {
        public DateOnly Date { get; set; }
        public string PayeeId { get; set; }
        public decimal Amount { get; set; }
        public string AccountId { get; set; }
        public string Note { get; set; }
        public Payee Payee { get; set; }
        public Account Account { get; set; }
        public List<TransactDetail> TransactDetail { get; set; }
    }
    public class TransactDetail : iKey
    {
        public string TransactId { get; set; }
        public int Order { get; set; }
        public decimal Amount { get; set; }
        public Transact Transact { get; set; }
        [NotMapped]
        public virtual string OnDisplay { get; }
    }
    public class GeneralTransactDetail : TransactDetail
    {
        public string CategoryId { get; set; }
        public string SubcategoryId { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} to {Category.Name} {Subcategory.Name}" : 
                    $"Debited {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} from {Category.Name} {Subcategory.Name}"; }
        }
    }
    public class CreditTransactDetail : GeneralTransactDetail
    {
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} from {Category.Name} {Subcategory.Name}" : 
                    $"Charged {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} on {Category.Name} {Subcategory.Name}"; }
        }
    }
    public class TransferTransactDetail : TransactDetail
    {
        public string TransferId { get; set; }
        public Account Transfer { get; set; }
        public string LinkId { get; set; }
        public Transact Link { get; set; }
        public int LinkOrder { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Received {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} from {Transfer.Name}" : 
                    $"Transfered {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} to {Transfer.Name}"; }
        }
    }
    public class ForexTransactDetail : TransferTransactDetail
    {
        public decimal ForexAmount { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? 
                    $"Received {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} from {Transfer.Name} valued at {Transfer.Currency.Prefix}{Math.Abs(ForexAmount):n2}" : 
                    $"Transfered {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} to {Transfer.Name} valued at {Transfer.Currency.Prefix}{Math.Abs(ForexAmount):n2}"; }
        }
    }
    public class TradingTransactDetail : TransferTransactDetail
    {
        public decimal Price { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? 
                    $"Bought {Math.Abs(Amount):n0} unit(s) @{Transact.Account.Currency.Prefix}{Price:n2} valued {Transfer.Currency.Prefix}{Amount * Price:n2}" : 
                    $"Sold {Math.Abs(Amount):n0} unit(s) @{Transact.Account.Currency.Prefix}{Price:n2} valued {Transfer.Currency.Prefix}{Amount * Price:n2}"; }
        }
    }
    public class TradingFromTransactDetail : TransferTransactDetail
    {
        public string CategoryId { get; set; }
        public string SubcategoryId { get; set; }
        public Category Category { get; set; }
        public Subcategory Subcategory { get; set; }
        public override string OnDisplay
        {
            get
            {
                return Amount > 0 ? $"Credited {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} to {Category.Name} {Subcategory.Name}" :
                  $"Debited {Transact.Account.Currency.Prefix}{Math.Abs(Amount):n2} from {Category.Name} {Subcategory.Name}";
            }
        }
    }
}
