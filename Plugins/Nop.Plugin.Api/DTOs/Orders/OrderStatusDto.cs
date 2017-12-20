using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Nop.Plugin.Api.Validators;

namespace Nop.Plugin.Api.DTOs.Orders
{
    [JsonObject(Title = "order_status")]
    public class OrderStatusDto
    {
        public OrderStatusDto(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        /// <summary>
        /// Gets or sets a value indicating the id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
