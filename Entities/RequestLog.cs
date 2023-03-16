using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RequestLog
    {
        public int RequestID { get; set; }
        public string InvokedRoute { get; set; }
        public string InvokedMethod { get; set; }
        public string UniqueAttributeName { get; set; }
        public string UniqueAttributeValue { get; set; }
        public string RequestParameters { get; set; }
        public string RequestResult { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime LogDate { get; set; }
    }
}
