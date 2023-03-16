using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public int CourseID { get; set; }
        public int ProductID { get; set; }
        public string TotalExams { get; set; }
        public string ExamsCovered { get; set; } = "";
        public string ScheduleName { get; set; }
        public int TotalSubjects
        {
            get
            {
                int s = 0;
                if (SMList != null && SMList.Count > 0)
                {
                    foreach(var sm in SMList)
                    {
                        s = s + sm.TotalSubjects;
                    }

                    return s;
                }

                return 0;
            }
        }
        //public int TotalChapters
        //{
        //    get
        //    {
        //        int s = 0;
        //        if (SMList != null && SMList.Count > 0)
        //        {
        //            foreach (var sm in SMList)
        //            {
        //                s = s + sm.TotalChapters;
        //            }

        //            return s;
        //        }

        //        return 0;
        //    }
        //}
        public int TotalTopics
        {
            get
            {
                int s = 0;
                if (SMList != null && SMList.Count > 0)
                {
                    foreach (var sm in SMList)
                    {
                        s = s + sm.TotalTopics;
                    }

                    return s;
                }

                return 0;
            }
        }
        public List<ScheduleSM> SMList { get; set; } = new();
    }
}
