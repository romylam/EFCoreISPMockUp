using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleUI
{
    public class Group
    {
        [Key]
        public string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
    }
    public class Type
    {
        [Key]
        public string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
    }
    public class Currency
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
    public class Payee
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Category
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Tag
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
