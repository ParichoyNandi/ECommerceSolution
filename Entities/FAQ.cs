using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FAQ
    {
        public int FAQID { get; set; }
        public string Name { get; set; }
        public int StatusID { get; set; }
        public List<FAQDetail> FAQDetails { get; set; } = new();
    }
}
