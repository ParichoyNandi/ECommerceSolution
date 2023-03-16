using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class ProductsServiceProperties
    {
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productShortDesc { get; set; }
        public int courseID { get; set; }
        public int brandID { get; set; }
        public int categoryID { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public string createdBy { get; set; } = "rice-group-admin";
        public string productImage { get; set; }
    }
}
