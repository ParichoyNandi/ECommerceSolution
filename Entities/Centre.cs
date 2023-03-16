using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Centre
    {
        public int CenterID { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string EmailID { get; set; }
        public string ContactNo { get; set; }
        public string GSTINNo { get; set; } = "19AACCR4567N1ZP";
        public string CIN { get; set; } = "NA";
        public string PAN { get; set; } = "NA";
        public string PlaceofSupply 
        {
            get
            {
                return State;
            }
        }
        public string StateCode { get; set; } = "19";
    }
}
