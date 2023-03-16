using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.ValueObjectsService
{
    public class CentreFeePlanMapServiceProperty
    {
        public int feePlanID { get; set; }
        public int centerID { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
    }
}
