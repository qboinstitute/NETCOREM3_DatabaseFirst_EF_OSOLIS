﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst_EF.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public string  pwd { get; set; }
        public bool state { get; set; }



    }
}
