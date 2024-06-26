﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TrackerRepository
{
    [ModelBinder(BinderType = typeof(NewtonsoftModelBinder))]
    public class GPSEntity
    {
        [Required]
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [Required]
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        public bool Equals(GPSEntity obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Latitude.Equals(obj.Latitude) && Longitude.Equals(obj.Longitude);
        }
    }
}
