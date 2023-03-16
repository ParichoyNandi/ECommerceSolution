using System;

namespace ECommAPI.Models
{
    public class GetBatchDto
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public string BatchDisplayName 
        {
            get
            {
                if (BatchID > 0)
                {
                    return $"{BatchStartDate.ToString("yyyy-MM-dd")} | {BatchTime}";
                }

                return "";
            }
        }
        public string BatchDisplayNameAlt
        {
            get
            {
                if (BatchID > 0)
                {
                    return $"{BatchStartDate.ToString("dd-MMMM")}, {BatchTime}";
                }

                return "";
            }
        }
        public DateTime BatchStartDate { get; set; }
        public string OnlineTimeSlots { get; set; }
        public string BatchTime { get; set; }
        public string BatchCode { get; set; } //susmita 2022-09-21

        public DateTime OriginalBatchStartDate { get; set; } //susmita 2022-10-27

        public Boolean AdmissionAfterStartDate { get; set; }//susmita 2022-10-27
    }
}