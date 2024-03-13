using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Tracker.TelegramBot.Controllers.Entities
{
    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class GetInfoEntity
    {
        [JsonProperty("FIO")]
        public string? FIO { get; internal set; }
        [Required]
        [JsonProperty("TaskName")]
        public string TaskName { get; internal set; }
        [Required]
        [JsonProperty("ProjectName")]
        public string ProjectName { get; internal set; }
        [JsonProperty("StartTime")]
        public DateTime? StartTime { get; internal set; }
    }
}