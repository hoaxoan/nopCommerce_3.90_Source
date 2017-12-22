using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.Devices
{
    [JsonObject(Title = "device")]
    public class DeviceDto
    {
        [JsonProperty("cordova")]
        public string Cordova { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}