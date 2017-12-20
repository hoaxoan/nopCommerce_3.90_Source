using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.Domain;
using Nop.Plugin.Api.DTOs.Orders;

namespace Nop.Plugin.Api.MappingExtensions
{
    public static class OrderDtoMappings
    {
        public static OrderDto ToDto(this Order order)
        {
            return order.MapTo<Order, OrderDto>();
        }

        public static OrderStatusCountDto ToDto(this OrderStatusCount orderStatusCount)
        {
            return orderStatusCount.MapTo<OrderStatusCount, OrderStatusCountDto>();
        }
    }
}