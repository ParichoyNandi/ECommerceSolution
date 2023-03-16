using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class PlanServiceProperty
    {
        public string planCode { get; set; }
        public string planName { get; set; }
        public string planDesc { get; set; }
        public string brandIDList { get; set; }
        public string planImage { get; set; } 
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public string createdBy { get; set; } = "rice-group-admin";

    }
}
