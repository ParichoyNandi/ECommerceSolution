using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateBatchNotificationDto
    {
        [Required]
        public string CustomerID { get; set; }

        [Required]
        public int PlanID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CenterID { get; set; }

        [Required]
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
    }
}
