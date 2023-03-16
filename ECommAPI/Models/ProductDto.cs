using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public CategoryDto CategoryDetails { get; set; } = new CategoryDto();
        public int CourseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int ProductLangaugeID { get; set; } //susmita
        public string ProductLanguageName { get; set; }//susmita
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string ProductImage { get; set; }
        public bool IsPublished { get; set; }
        public List<ProductCenterMapDto> CenterAvailability { get; set; } = new List<ProductCenterMapDto>();
        public List<GetProductConfigMapDto> ProductConfigList { get; set; } = new List<GetProductConfigMapDto>();
        public GetSummaryFormatDto SummaryDetails { get; set; } = new();
        public List<GetExamCategoryDto> ExamCategoryList { get; set; } = new();
    }
}
