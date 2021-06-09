using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitios.Models
{
    public class Login
    {
        [Required]
        public string user { get; set; }
        public string password { get; set; }

    }
}