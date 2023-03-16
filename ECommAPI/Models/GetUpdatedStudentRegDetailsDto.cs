using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetUpdatedStudentRegDetailsDto
    {
        public string CustomerID { get; set; }
        public int RegID { get; set; }
        public List<int> EnquiryNos { get; set; } = new List<int>();
        public List<string> StudentIDs { get; set; } = new List<string>();
    }
}
