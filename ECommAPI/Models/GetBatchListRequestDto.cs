using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetBatchListRequestDto
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CenterID { get; set; }
        public DateTime AfterDate { get; set; }
    }
}
