using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InstalmentSummary
    {
        public int InstalmentNo { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public DateTime InstalmentDate { get; set; }
    }
}
