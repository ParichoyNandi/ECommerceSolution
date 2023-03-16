using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IProductManagement
    {
        void SaveProductMappings(int ProductID, string ExamCategoryIDList, List<ProductCenterMap> centerMaps, string CreatedBy);
        void SaveProductExamCategoryMappings(int ProductID, string ExamCategoryIDList, string CreatedBy);
        void SaveProductCenterFeePlanMappings(int ProductID, List<ProductCenterMap> centerMaps, string CreatedBy);
    }
}
