using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Tracker.MongoDB.DBEntities;

namespace Tracker.TelegramBot.Controllers.Entities
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
        public string ProjectDescription { get; internal set; }

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

            return this.TaskName.Equals(obj.TaskName) &&
                this.TaskDescription.Equals(obj.TaskDescription) &&
                this.ProjectName.Equals(obj.Project.Name) &&

                (this.ProjectDescription.Equals(obj.Project.Description) || (this.ProjectDescription is null && obj.Project.Description is null)) &&
                
                (this.StartTime.Equals(obj.StartTime) || (this.StartTime is null && obj.StartTime is null)) &&
                (this.EndTime.Equals(obj.EndTime) || (this.EndTime is null && obj.EndTime is null)) &&
                
                ((this.StartLatitude is null && obj.StartPosition is null) ||
                (obj.StartPosition != null && this.StartLatitude.Equals(obj.StartPosition.Latitude))) &&

                ((this.StartLongitude is null && obj.StartPosition is null) ||
                (obj.StartPosition != null && this.StartLongitude.Equals(obj.StartPosition.Longitude))) &&

                ((this.EndLatitude is null && obj.EndPosition is null) ||
                (obj.EndPosition != null && this.EndLatitude.Equals(obj.EndPosition.Latitude))) &&

                ((this.EndLongitude is null && obj.EndPosition is null) ||
                (obj.EndPosition != null && this.EndLongitude.Equals(obj.EndPosition.Longitude)));
        }

        public bool EqualsNotStartedDBTask(DBTask obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.TaskName.Equals(obj.TaskName) &&
                this.TaskDescription.Equals(obj.TaskDescription) &&
                this.ProjectName.Equals(obj.Project.Name) &&

                (this.ProjectDescription.Equals(obj.Project.Description) || (this.ProjectDescription is null && obj.Project.Description is null)) &&

                obj.StartTime == null && obj.StartPosition == null;
        }

        public bool EqualsNotEndedDBTask(DBTask obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.TaskName.Equals(obj.TaskName) &&
                this.TaskDescription.Equals(obj.TaskDescription) &&
                this.ProjectName.Equals(obj.Project.Name) &&

                (this.ProjectDescription.Equals(obj.Project.Description) || (this.ProjectDescription is null && obj.Project.Description is null)) &&

                this.StartTime.Equals(obj.StartTime) &&
                (obj.StartPosition != null && 
                this.StartLatitude.Equals(obj.StartPosition.Latitude) && 
                this.StartLongitude.Equals(obj.StartPosition.Longitude)) &&

                obj.EndTime == null && obj.EndPosition == null;

        }
    }
}
