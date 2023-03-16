using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Batch
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public DateTime BatchStartDate { get; set; }
        public string OnlineTimeSlots { get; set; }
        public string BatchTime { get; set; } = "";
        public string BatchCode { get; set; }//susmita 2022-09-21
        public DateTime OriginalBatchStartDate { get; set; } //susmita 2022-10-27

        public Boolean AdmissionAfterStartDate { get;set; }//susmita 2022-10-27
    }
}
