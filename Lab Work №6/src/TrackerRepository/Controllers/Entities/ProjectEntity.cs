using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TrackerRepository
{
    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class ProjectEntity
    {
        [Required]
        [JsonProperty("ProjectName")]
        public string ProjectName { get; internal set; }

        [JsonProperty("ProjectDescription")]
        public string? ProjectDescription { get; internal set; }

        public bool EqualsDBProject(DBProject obj)
        {
            if (obj == null)
            {
                return false;
            }
            return this.ProjectName.Equals(obj.Name) &&
                ((this.ProjectDescription is null && obj.Description is null) ||
                (this.ProjectDescription != null && this.ProjectDescription.Equals(obj.Description)));
        }
    }
}
