using System.Collections.Generic;

namespace Entities
{
    public class SyllabusChapter
    {
        public int ChapterID { get; set; }
        public string ChapterName { get; set; }
        public List<SyllabusTopic> TopicList { get; set; } = new();
    }
}