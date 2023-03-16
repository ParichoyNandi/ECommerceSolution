using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public bool IsOnline { get; set; }
        public bool IsOffline { get; set; }
    }
}
