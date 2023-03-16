using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public Category CategoryDetails { get; set; } = new Category();
        public int CourseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public int StatusID { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ProductImage { get; set; }
        public int ProductLangaugeID { get; set; } //susmita
        public string ProductLanguageName { get; set; }//susmita
        public List<ProductCenterMap> CenterAvailability { get; set; } = new List<ProductCenterMap>();
        public List<ProductConfig> ProductConfigList { get; set; } = new List<ProductConfig>();
        public List<ExamCategory> ExamCategoryList { get; set; } = new();
    }
}
