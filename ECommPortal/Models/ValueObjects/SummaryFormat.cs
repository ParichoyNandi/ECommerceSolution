using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models.ValueObjects
{
    public class SummaryFormat
    {
        public string DownloadLink { get; set; }
        public List<SummaryItem> ItemList { get; set; } = new();
    }
}
