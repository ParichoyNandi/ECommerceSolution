using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class ProductPublishDeactiveViewModel : ResponseModel
    {
        public int ChoosenBrandId { get; set; }
        public List<Brand> BrandList { get; set; } = new();
        public List<Product> ProductLists { get; set; } = new();
        public List<ChoosenProductForActiveDeactive> UpdatedChoosenProductLists { get; set; } = new();


    }
}
