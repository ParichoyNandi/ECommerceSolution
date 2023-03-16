using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreatePlanViewModel : ResponseModel
    {
        public List<Brand> BrandLists { get; set; } = new();

        public List<int> ChoosenBrandsLists { get; set; } = new();

        public Plan choosenPlan { get; set; } = new();

        public String PlanDesc { get; set; }

        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public int ProductLanguageID { get; set; }//susmita
        public string ProductLanguageName { get; set; }//susmita
        public IFormFile PlanImageIFile { get; set; }

        public String PlanImageString { get; set; }

    }
}
