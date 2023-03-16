using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StudentPayOut
    {
        public string CustomerID { get; set; }
        public List<StudentPayOutFeeSchedule> FeeSchedules { get; set; } = new();
    }
}
