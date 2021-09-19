using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleUI
{
    public class Group : iKey
    {
        public int Order { get; set; }
        public string Name { get; set; }
    }
    public class Type : iKey
    {
        public int Order { get; set; }
        public string Name { get; set; }
    }
    public class Currency : iKey
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public decimal Rate { get; set; }
    }
    public class Payee : iKey
    {
        public string Name { get; set; }
    }
    public class Category : iKey
    {
        public string Name { get; set; }
        public List<Subcategory> Subcategory { get; set; }
    }
    public class Subcategory : iKey
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
    public class Tag : iKey
    {
        public string Name { get; set; }
    }
}
