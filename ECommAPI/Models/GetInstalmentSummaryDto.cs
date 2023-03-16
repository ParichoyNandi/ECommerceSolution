using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetInstalmentSummaryDto
    {
        public int InstalmentNo { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public DateTime InstalmentDate { get; set; }
    }
}
