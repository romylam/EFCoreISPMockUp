using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Transact : Common
    {
        public string Class { get; set; }
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceDate { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        [NotMapped]
        public string OnDisplay
        {
            get
            {
                switch (Class)
                {
                    case "General":
                        if (Amount > 0)
                            return $"Credited {Amount:c2} into {Account.Name}";
                        else
                            return $"Debited {Amount:c2} from {Account.Name}";
                    case "Trading":
                        if (Unit > 0)
                            return $"Bought {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}";
                        else
                            return $"Sold {Unit:n0} unit(s) of {Account.Name} @{Price:c2} valued {Amount:c2}";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
