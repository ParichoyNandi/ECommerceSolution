using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetApkPlanDto
    {
        public int PlanID { get; set; }
        public string Banner { get; set; } = "";
        public string CourseName { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string LiveClasses { get; set; }
        public string StudyMaterial { get; set; }
        public String StudyMetarialLanguage { set; get; } // added by susmita
        public string Videos { get; set; }
        public string TotalExams { get; set; }
        public string RedirectLink { get; set; }
        public List<int> CategoryIDList { get; set; } = new();

        
    }
}
