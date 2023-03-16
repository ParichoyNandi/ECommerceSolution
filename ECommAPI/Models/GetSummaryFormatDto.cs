using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetSummaryFormatDto
    {
        public string DownloadLink { get; set; }
        public List<GetSummaryItemDto> ItemList { get; set; } = new();
    }
}
