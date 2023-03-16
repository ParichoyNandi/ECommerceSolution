using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class AccountsViewModel
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        //public User UserDetails { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
