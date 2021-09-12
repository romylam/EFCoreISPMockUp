using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Transact : ITransact
    {
        [Key]
        public string Id { get; set; }
        public DateOnly Date { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public List<TransactDetail> TransactDetail { get; set; }
    }
    //public class GeneralTransact : Transact, ITransact
    //{
    //    public override string OnDisplay
    //    {
    //        get { return Amount > 0 ? $"Credited {Amount:c2} into {Account.Name}" : $"Debited {Amount:c2} from { Account.Name}"; }
    //    }
    //}
    //public class ForexTransact : Transact, ITransact
    //{
    //    public string ForexCurrency { get; set; }
    //    public decimal ForexAmount { get; set; }
    //    public override string OnDisplay
    //    {
    //        get { return Amount > 0 ? $"Bought {ForexAmount:n2} of {ForexCurrency} valued {Amount:c2}" : $"Sold {ForexAmount:n2} of {ForexCurrency} valued {Amount:c2}"; }
    //    }
    //}
    //public class TradingTransact : Transact, ITransact
    //{
    //    public decimal Unit { get; set; }
    //    public decimal Price { get; set; }
    //    public DateTime PriceDate { get; set; }
    //    public override string OnDisplay
    //    {
    //        get { return Unit > 0 ? $"Bought {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}" : $"Sold {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}"; }
    //    }
    //}
    public class TransactDetail
    {
        [Key]
        public string Id { get; set; }
        public string TransactId { get; set; }
        public string Order { get; set; }
        public decimal Amount { get; set; }
        [NotMapped]
        public virtual string OnDisplay { get; }
    }
    public class GeneralTransactDetail : TransactDetail
    {
        public string Category { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Amount:c2} to {Category}" : $"Debited {Amount:c2} from {Category}"; }
        }
    }
    public class CreditTransactDetail : GeneralTransactDetail
    {
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Amount:c2} from {Category}" : $"Charged {Amount:c2} on {Category}"; }
        }
    }
    public class TransferTransactDetail : TransactDetail
    {
        public string TransferId { get; set; }
        public Account Transfer { get; set; }
        public string LinkId { get; set; }
        public string LinkOrder { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Transfered {Amount:c2} from {Transfer.Name}" : $"Transfered {Amount:c2} to {Transfer.Name}"; }
        }
    }
    public class ForexTransactDetail : TransactDetail
    {
        public string ForexCurrency { get; set; }
        public decimal ForexAmount { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Bought {ForexAmount:n2} of {ForexCurrency} valued {Amount:c2}" : $"Sold {ForexAmount:n2} of {ForexCurrency} valued {Amount:c2}"; }
        }
    }
    public class TradingTransactDetail : TransactDetail
    {
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        public DateOnly PriceDate { get; set; }
        public string TradingId { get; set; }
        public TradingAccount TradingAccount { get; set; }
        public override string OnDisplay
        {
            get { return Unit > 0 ? $"Bought {Unit:n0} unit(s) @{Price:c2} valued {Amount:c2}" : $"Sold {Unit:n0} unit(s) @{Price:c2} valued {Amount:c2}"; }
        }
    }
}
