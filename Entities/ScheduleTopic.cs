using System.Collections.Generic;

namespace Entities
{
    public class ScheduleTopic
    {
        public int TopicID { get; set; }
        public string TopicName { get; set; }
        public List<ScheduleTopicBreakup> TopicBreakupList { get; set; } = new();
    }
}