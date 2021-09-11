﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class MasterKey
    {
        [Key]
        public string Id { get; set; }
        public string Prefix { get; set; }
        public bool TwoFactor { get; set; }
        public int NextTerm { get; set; }
        public int NextKey { get; set; }
    }
}
