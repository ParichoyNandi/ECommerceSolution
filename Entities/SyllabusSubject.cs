using System.Collections.Generic;

namespace Entities
{
    public class SyllabusSubject
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int TotalChapter 
        {
            get
            {
                if (ChapterList != null && ChapterList.Count > 0)
                    return ChapterList.Count;

                return 0;
            }
        }
        public int TotalTopic
        {
            get
            {
                if (ChapterList != null && ChapterList.Count > 0)
                {
                    int t = 0;
                    foreach(var c in ChapterList)
                    {
                        if (c.TopicList != null && c.TopicList.Count > 0)
                            t = t + c.TopicList.Count;
                    }

                    return t;
                }

                return 0;
            }
        }
        public List<SyllabusChapter> ChapterList { get; set; } = new();
    }
}