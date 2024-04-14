namespace TrackerRepository
{
    public class DBTask : DBEntity
    {
        public TaskEntity toTaskEntity()
        {
            TaskEntity taskEntity = new TaskEntity();
            taskEntity.ChatId = ChatId;
            taskEntity.TaskName = TaskName;
            taskEntity.TaskDescription = TaskDescription;
            taskEntity.StartTime = StartTime;
            taskEntity.EndTime = EndTime;

            taskEntity.StartLatitude = StartPosition?.Latitude;
            taskEntity.StartLongitude = StartPosition?.Longitude;

            taskEntity.EndLatitude = EndPosition?.Latitude;
            taskEntity.EndLongitude = EndPosition?.Longitude;

            taskEntity.ProjectName = Project.Name;
            taskEntity.ProjectDescription = Project.Description;

            return taskEntity;
        }


        public long ChatId { get; set; }
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; }

        public DBProject Project { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DBGPS? StartPosition { get; set; }
        public DBGPS? EndPosition { get; set; }

        public bool Equals(DBTask obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (TaskName == obj.TaskName) &&
                (TaskDescription != null && obj.TaskDescription != null && TaskDescription == obj.TaskDescription || 
                (TaskDescription == null && obj.TaskDescription == null)) &&

                (Project?.Name == obj.Project?.Name) &&

                ((Project?.Description != null && Project?.Description == obj.Project?.Description) || 
                (Project?.Description is null && obj.Project?.Description is null)) &&

                ((StartTime == obj.StartTime) || (StartTime is null && obj.StartTime is null)) &&
                ((EndTime == obj.EndTime) || (EndTime is null && obj.EndTime is null)) &&

                ((StartPosition is null && obj.StartPosition is null) ||
                (obj.StartPosition != null && StartPosition != null && StartPosition?.Latitude  == obj.StartPosition?.Latitude)) &&

                ((StartPosition is null && obj.StartPosition is null) ||
                (obj.StartPosition != null && StartPosition != null && StartPosition?.Longitude == obj.StartPosition?.Longitude)) &&

                ((EndPosition is null && obj.EndPosition is null) ||
                (obj.EndPosition != null && EndPosition != null && EndPosition?.Latitude == obj.EndPosition?.Latitude)) &&

                ((EndPosition is null && obj.EndPosition is null) ||
                (obj.EndPosition != null && EndPosition != null && EndPosition?.Longitude == obj.EndPosition?.Longitude));
        }
    }
}
