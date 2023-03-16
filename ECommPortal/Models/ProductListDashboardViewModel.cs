using Entities;
using System;
using System.Collections.Generic;

namespace ECommPortal.Models
{
    public class ProductListDashboardViewModel
    {
        public String publication { get; set; }
        public List<Brand> BrandList { get; set; }= new ();
        public List<Product> ProductLists { get; set; }=new ();
    }
}
