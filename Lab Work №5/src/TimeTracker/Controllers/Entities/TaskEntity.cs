using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Tracker.TelegramBot.Controllers.Entities
{
    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class TaskEntity
    {
        [Required]
        [JsonProperty("ChatId")]
        public long ChatId { get; set; }

        [Required]
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }

        [JsonProperty("TaskDescription")]
        public string? TaskDescription { get; set; }

        [Required]
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }

        [JsonProperty("ProjectDescription")]
        public string ProjectDescription { get; set; }

        [JsonProperty("StartTime")]
        public DateTime? StartTime { get; set; }

        [JsonProperty("StartLatitude")]
        public double? StartLatitude { get; set; }

        [JsonProperty("StartLongitude")]
        public double? StartLongitude { get; set; }

        [JsonProperty("EndTime")]
        public DateTime? EndTime { get; set; }

        [JsonProperty("EndLatitude")]
        public double? EndLatitude { get; set; }

        [JsonProperty("EndLongitude")]
        public double? EndLongitude { get; set; }


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
