using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetFAQDto
    {
        public int FAQID { get; set; }
        public string Name { get; set; }
        public List<GetFAQDetailDto> FAQDetails { get; set; } = new();
    }
}
