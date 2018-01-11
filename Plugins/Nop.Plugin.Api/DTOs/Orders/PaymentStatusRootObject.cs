using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Customers;

namespace Nop.Plugin.Api.DTOs.Orders
{
    public class PaymentStatusRootObject : ISerializableObject
    {
        public PaymentStatusRootObject()
        {
           PaymentStatus = new List<PaymentStatusDto>();
        }

        [JsonProperty("payments_status")]
        public IList<PaymentStatusDto> PaymentStatus { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "payments_status";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (PaymentStatusDto);
        }
    }
}