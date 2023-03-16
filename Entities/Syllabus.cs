using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Syllabus
    {
        public int CourseID { get; set; }
        public int ProductID { get; set; }
        public int SyllabusID { get; set; }
        public string SyllabusName { get; set; }
        public int TotalSubjects 
        {
            get
            {
                if (SubjectList != null && SubjectList.Count > 0)
                    return SubjectList.Count;

                return 0;
            }
        }
        public int TotalChapters 
        {
            get
            {
                if (SubjectList != null && SubjectList.Count > 0)
                {
                    int c = 0;

                    foreach(var s in SubjectList)
                    {
                        c = c + s.TotalChapter;
                    }

                    return c;
                }

                return 0;
            }
        }
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
        public List<SyllabusSubject> SubjectList { get; set; } = new();
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
