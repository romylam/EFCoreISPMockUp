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
    public class TransactDetail : Common
    {
        public int TransactId { get; set; }
        public int Order { get; set; }
        public decimal Amount { get; set; }
        [NotMapped]
        public virtual string OnDisplay { get; }
    }
    public class GeneralTransactDetail : TransactDetail
    {
        public string Category { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Amount:c2} on {Category}" : $"Debited {Amount:c2} on {Category}"; }
        }
    }
    public class TransferTransactDetail : TransactDetail
    {
        public int TransferId { get; set; }
        public Account Transfer { get; set; }
        public override string OnDisplay
        {
            get { return Amount > 0 ? $"Credited {Amount:c2}" : $"Debited {Amount:c2}"; }
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
        public DateTime PriceDate { get; set; }
        public int TradingId { get; set; }
        public TradingAccount TradingAccount { get; set; }
        public override string OnDisplay
        {
            get { return Unit > 0 ? $"Bought {Unit:n0} unit(s) @{Price:c2} valued {Amount:c2}" : $"Sold {Unit:n0} unit(s) @{Price:c2} valued {Amount:c2}"; }
        }
    }
}
