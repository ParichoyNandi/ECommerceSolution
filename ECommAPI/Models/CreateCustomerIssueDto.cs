using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateCustomerIssueDto
    {
        [Required]
        public int IssueCategoryID { get; set; }

        [Required]
        public string Issue { get; set; }
        public string Name { get; set; } = "";
        public string StudentID { get; set; } = "";

        [Required]
        public string CustomerID { get; set; } = "";
        public string ContactNo { get; set; } = "";
        public string EmailID { get; set; } = "";
    }
}
