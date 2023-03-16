using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreateDiscountsViewModel : ResponseModel
    {

        public List<int> ChoosenDiscountBrandList { get; set; } = new();

        public String responsestring { get; set; }

        public DiscountScheme DiscountschemeList { get; set; } = new();

        public List<CreateDiscountBrandDetails> DiscountBrandLists { get; set; } = new();

        public List<FeeComponent> FeescomponentList { get; set; } = new();
        public List<Brand> BrandList { get; set; } = new();
       

        
    }
}
