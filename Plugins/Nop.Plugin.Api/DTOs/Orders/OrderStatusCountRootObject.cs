using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Customers;

namespace Nop.Plugin.Api.DTOs.Orders
{
    public class OrderStatusCountRootObject : ISerializableObject
    {
        public OrderStatusCountRootObject()
        {
            OrderStatusCount = new List<OrderStatusCountDto>();
        }

        [JsonProperty("orders_status_counts")]
        public IList<OrderStatusCountDto> OrderStatusCount { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "orders_status_counts";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (OrderStatusCountDto);
        }
    }
}