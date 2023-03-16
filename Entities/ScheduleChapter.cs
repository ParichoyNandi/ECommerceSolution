using System.Collections.Generic;

namespace Entities
{
    public class ScheduleChapter
    {
        public int ChapterID { get; set; }
        public string ChapterName { get; set; }
        public List<ScheduleTopic> TopicList { get; set; } = new();
    }
}