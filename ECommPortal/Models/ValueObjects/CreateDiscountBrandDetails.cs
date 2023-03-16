using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models.ValueObjects
{
    public class CreateDiscountBrandDetails
    {
        public int BrandID { get; set; }

        public List<DiscountSchemeDetail> DiscountSchemeLumpsumList { get; set; } = new();
        public List<DiscountSchemeDetail> DiscountSchemeInstallmentList { get; set; } = new();
    }
}
