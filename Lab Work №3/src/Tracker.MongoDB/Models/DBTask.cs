using System;
using Tracker.MongoDB.Models;

namespace Tracker.MongoDB.DBEntities
{
    public class DBTask : DBEntity
    {
        public long ChatId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DBGPS? StartPosition { get; set; }
        public DBGPS? EndPosition { get; set; }
    }
}
