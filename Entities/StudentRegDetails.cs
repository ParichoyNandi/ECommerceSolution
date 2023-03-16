using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StudentRegDetails
    {
        public string CustomerID { get; set; }
        public int RegID { get; set; }
        public List<int> EnquiryNos { get; set; } = new List<int>();
        public List<string> StudentIDs { get; set; }= new List<string>();
    }
}
