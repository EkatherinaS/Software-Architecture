using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TrackerRepository
{

    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class TaskEntity
    {
        [Required]
        [JsonProperty("ChatId")]
        public long ChatId { get; internal set; }

        [Required]
        [JsonProperty("TaskName")]
        public string TaskName { get; internal set; }

        [JsonProperty("TaskDescription")]
        public string? TaskDescription { get; internal set; }

        [Required]
        [JsonProperty("ProjectName")]
        public string ProjectName { get; internal set; }

        [JsonProperty("ProjectDescription")]
        public string? ProjectDescription { get; internal set; }

        [JsonProperty("StartTime")]
        public DateTime? StartTime { get; internal set; }

        [JsonProperty("StartLatitude")]
        public double? StartLatitude { get; internal set; }

        [JsonProperty("StartLongitude")]
        public double? StartLongitude { get; internal set; }

        [JsonProperty("EndTime")]
        public DateTime? EndTime { get; internal set; }

        [JsonProperty("EndLatitude")]
        public double? EndLatitude { get; internal set; }

        [JsonProperty("EndLongitude")]
        public double? EndLongitude { get; internal set; }


        public bool EqualsDBTask(DBTask obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (TaskName == obj.TaskName) &&
                (TaskDescription == obj.TaskDescription) &&

                (ProjectName == obj.Project?.Name) &&

                ((ProjectDescription == obj.Project?.Description) ||
                (ProjectDescription is null && obj.Project?.Description is null));
        }

        public TaskEntity getDeepCopy()
        {
            TaskEntity entity = new TaskEntity();
            entity.ChatId = ChatId;
            entity.TaskName = TaskName;
            entity.TaskDescription = TaskDescription;
            entity.ProjectName = ProjectName;
            entity.ProjectDescription = ProjectDescription;
            entity.StartTime = StartTime;
            entity.StartLatitude = StartLatitude;
            entity.StartLongitude = StartLongitude;
            entity.EndTime = EndTime;
            entity.EndLatitude = EndLatitude;
            entity.EndLongitude = EndLongitude;
            return entity;
        }
    }
}