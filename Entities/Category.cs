using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public int BrandID { get; set; }
        public int StatusID { get; set; }
        public bool IsOnline { get; set; }
        public bool IsOffline { get; set; }
    }
}
