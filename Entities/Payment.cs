using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Payment
    {
        
        public string CustomerID { get; set; }
        public List<PlanPayment> PlanPaymentDetails { get; set; } = new();
        //public string Address { get; set; }
        //public int CityID { get; set; }
        //public int StateID { get; set; }
        //public int CountryID { get; set; }
        //public string Pincode { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionMode { get; set; }
        public string TransactionStatus { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidTax { get; set; }
    }
}
