using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Tracker.TelegramBot.Controllers.Entities
{
    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class ProjectEntity
    {
        private TaskEntity[] _tasks;

        public TaskIterator getTaskIterator()
        {
            TaskIterator taskIterator = new TaskIterator(_tasks);
            return taskIterator;
        }

        [Required]
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }

        [JsonProperty("ProjectDescription")]
        public string? ProjectDescription { get; set; }
    }
}
