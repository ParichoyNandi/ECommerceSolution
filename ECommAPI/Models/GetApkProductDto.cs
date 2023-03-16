using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetApkProductDto
    {
        public int ProductID { get; set; }
        public string Banner { get; set; } = "";
        public string CourseName { get; set; }
        public int ProductLangaugeID { get; set; }//susmita
        public string ProductLangaugeName { get; set; }//susmita
        public string StudyMetarialLanguage { get; set; }//susmita
        public string Duration { get; set; }
        public string LiveClasses { get; set; }
        public string StudyMaterial { get; set; }
        public string Videos { get; set; }
        public string TotalExams { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
