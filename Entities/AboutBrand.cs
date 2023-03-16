using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AboutBrand
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
