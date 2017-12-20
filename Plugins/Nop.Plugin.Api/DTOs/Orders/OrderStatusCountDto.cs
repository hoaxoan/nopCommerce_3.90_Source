using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Nop.Plugin.Api.Validators;

namespace Nop.Plugin.Api.DTOs.Orders
{
    [JsonObject(Title = "order_status_count")]
    public class OrderStatusCountDto
    {
        /// <summary>
        /// Gets or sets a value indicating the id
        /// </summary>
        [JsonProperty("order_status_id")]
        public Int32 OrderStatusId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the id
        /// </summary>
        [JsonProperty("order_status_name")]
        public String OrderStatusName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name
        /// </summary>
        [JsonProperty("order_count")]
        public Int32 OrderCount { get; set; }

    }
}
