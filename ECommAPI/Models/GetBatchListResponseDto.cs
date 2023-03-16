using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetBatchListResponseDto
    {
        public int ProductID { get; set; }
        public int CenterID { get; set; }
        public DateTime AfterDate { get; set; }
        public List<GetBatchDto> BatchList { get; set; } = new();
    }
}
