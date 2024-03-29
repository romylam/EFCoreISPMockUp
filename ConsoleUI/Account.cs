﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Account : IAccount
    {
        [Key]
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string TypeId { get; set; }
        public string CurrencyId { get; set; }
        public string Name { get; set; }
        public decimal Open { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public decimal Transfer { get; set; } = 0;
        public string Status { get; set; } = "Active";
        public bool Default { get; set; } = false;
        public Group Group { get; set; }
        public Type Type { get; set; }
        public Currency Currency { get; set; }
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
        public override string OnDisplay { get { return $"{Currency.Prefix}{Worth:n2}"; } }
    }
    public class CreditAccount : Account, IAccount
    {
        public decimal Limit { get; set; }
        public override decimal Worth { get { return Balance; } }
        public override string OnDisplay { get { return $"{Currency.Prefix}{Worth:n2} against limit of {Currency.Prefix}{Limit:n2}"; } }
    }
    public class TradingAccount : Account, IAccount
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateOnly PriceDate { get; set; }
        public override decimal Worth { get { return Balance * Price; } }
        public override string OnDisplay {  get { return $"{Balance:n0} unit(s) @{Currency.Prefix}{Price:n2} value {Currency.Prefix}{Worth:n2}"; } }
    }
}
