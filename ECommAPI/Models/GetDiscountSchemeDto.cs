using System.Collections.Generic;

namespace ECommAPI.Models
{
    public class GetDiscountSchemeDto
    {
        public int DiscountSchemeID { get; set; }
        public string DiscountSchemeName { get; set; }
        public List<GetDiscountSchemeDetailDto> DiscountSchemeDetails { get; set; } = new();
    }
}