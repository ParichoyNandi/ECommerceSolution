using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetFeeComponentDto
    {
        public int BrandID { get; set; }
        public int FeeComponentID { get; set; }
        public string FeeComponentName { get; set; }
    }
}
