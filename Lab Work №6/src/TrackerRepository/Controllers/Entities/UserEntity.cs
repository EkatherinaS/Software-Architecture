using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TrackerRepository;

[ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
public class UserEntity
{
    [Required]
    [JsonProperty("IdChat")]
    public long IdChat { get; set; }
    [JsonProperty("FIO")]
    public string? FIO { get; set; }
    [JsonProperty("Username")]
    public string? Username { get; set; }
    [JsonProperty("Nickname")]
    public string? Nickname { get; set; }
    [JsonProperty("Status")]
    public string? Status { get; set; }
    [JsonProperty("IsAdmin")]
    public bool IsAdmin { get; set; } = false;
    [JsonProperty("Company")]
    public CompanyEntity Company { get; set; }
    [JsonProperty("CurrentTask")]
    public TaskEntity? CurrentTask { get; set; }
}
