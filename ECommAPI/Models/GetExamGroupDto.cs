using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetExamGroupDto
    {
        public int ExamGroupID { get; set; }
        public string ExamGroupDesc { get; set; }
        public string ExamGroupDetail { get; set; }
    }
}
