using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Customers;

namespace Nop.Plugin.Api.DTOs.Orders
{
    public class OrderStatusRootObject : ISerializableObject
    {
        public OrderStatusRootObject()
        {
            OrderStatus = new List<OrderStatusDto>();
        }

        [JsonProperty("orders_status")]
        public IList<OrderStatusDto> OrderStatus { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "orders_status";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (OrderStatusDto);
        }
    }
}