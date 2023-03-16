using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreateProductsViewModel : ResponseModel
    {
        public List<Brand> BrandList { get; set; } = new();
        public List<Course> CourseList { get; set; } = new();
        public List<Category> CategoryList { get; set; } = new();
        public Product ProductDetails { get; set; } = new();
        public IFormFile ProductImageIFile { get; set; }
    }
}
