using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Tracker.TelegramBot.Controllers.Entities;
[ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
public class CompanyEntity
{
    [Required]
    [JsonProperty("CompanyName")]
    public string Name { get; set; }
}
