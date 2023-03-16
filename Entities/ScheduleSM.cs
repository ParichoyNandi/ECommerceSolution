using System.Collections.Generic;

namespace Entities
{
    public class ScheduleSM
    {
        public int SMID { get; set; }
        public string SMName { get; set; }
        public int TotalSubjects
        {
            get
            {
                if (SubjectList != null && SubjectList.Count > 0)
                    return SubjectList.Count;

                return 0;
            }
        }
        //public int TotalChapters
        //{
        //    get
        //    {
        //        if (SubjectList != null && SubjectList.Count > 0)
        //        {
        //            int c = 0;

        //            foreach (var s in SubjectList)
        //            {
        //                c = c + s.TotalChapter;
        //            }

        //            return c;
        //        }

        //        return 0;
        //    }
        //}
        public int TotalTopics
        {
            get
            {
                if (SubjectList != null && SubjectList.Count > 0)
                {
                    int t = 0;

                    foreach (var s in SubjectList)
                    {
                        t = t + s.TotalTopic;
                    }

                    return t;
                }

                return 0;
            }
        }
        public List<ScheduleSubject> SubjectList { get; set; } = new();
    }
}