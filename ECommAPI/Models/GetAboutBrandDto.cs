using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetAboutBrandDto
    {
        public int ID { get; set; }
        public int BrandID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BannerImage { get; set; }
        public string VideoLink { get; set; }
        public string ToBeDisplayedIn { get; set; }
    }
}
