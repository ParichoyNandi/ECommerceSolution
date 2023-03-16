using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class IssueCategory
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public string DesignatedEmailID { get; set; }
    }
}
