﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.DataStructures;
using Nop.Data;
using Nop.Plugin.Api.DTOs.Orders;
using System.Data;
using Nop.Plugin.Api.Domain;
using Nop.Plugin.Api.Data;

namespace Nop.Plugin.Api.Services
{
    public class OrderApiService : IOrderApiService
    {
        private readonly IDbContext _dbContext;
        private readonly ApiObjectContext _objectContext;
        private readonly IDataProvider _dataProvider;
        private readonly IRepository<Order> _orderRepository;

        public OrderApiService(IDbContext dbContext,
                IDataProvider dataProvider,
                ApiObjectContext objectContext,
                IRepository<Order> orderRepository)
        {
            _dbContext = dbContext;
            _dataProvider = dataProvider;
            _objectContext = objectContext;
            _orderRepository = orderRepository;
        }

        public IList<Order> GetOrdersByCustomerId(int customerId)
        {
            var query = from order in _orderRepository.TableNoTracking
                        where order.CustomerId == customerId && !order.Deleted
                        orderby order.Id
                        select order;

            return new ApiList<Order>(query, 0, Configurations.MaxLimit);
        }

        public IList<Order> GetOrders(IList<int> ids = null, DateTime? createdAtMin = null, DateTime? createdAtMax = null,
           int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId,
           IList<int> orderStatus = null, IList<int> paymentStatus = null, ShippingStatus? shippingStatus = null, int? status = null, DateTime? shippedDateUtc = null, int? customerId = null,
           int? storeId = null, int? statusId = null, int? paymentId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = GetOrdersQuery2(createdAtMin, createdAtMax, orderStatus, paymentStatus, shippingStatus, status, shippedDateUtc, ids, customerId, storeId,
                statusId, paymentId, fromDate, toDate);

            if (sinceId > 0)
            {
                query = query.Where(order => order.Id > sinceId);
            }

            return new ApiList<Order>(query, page - 1, limit);
        }

        public Order GetOrderById(int orderId)
        {
            if (orderId <= 0)
                return null;

            return _orderRepository.Table.FirstOrDefault(order => order.Id == orderId && !order.Deleted);
        }

        public int GetOrdersCount(DateTime? createdAtMin = null, DateTime? createdAtMax = null, int? status = null,
                                 PaymentStatus? paymentStatus = null, ShippingStatus? shippingStatus = null,
                                 int? customerId = null, int? storeId = null)
        {
            var query = GetOrdersQuery(createdAtMin, createdAtMax, status, paymentStatus, shippingStatus, customerId: customerId, storeId: storeId);

            return query.Count();
        }

        private IQueryable<Order> GetOrdersQuery(DateTime? createdAtMin = null, DateTime? createdAtMax = null, int? status = null,
            PaymentStatus? paymentStatus = null, ShippingStatus? shippingStatus = null, IList<int> ids = null,
            int? customerId = null, int? storeId = null)
        {
            var query = _orderRepository.TableNoTracking;

            if (customerId != null)
            {
                query = query.Where(order => order.CustomerId == customerId);
            }

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(c => ids.Contains(c.Id));
            }

            if (status != null)
            {
                query = query.Where(order => order.OrderStatusId == (int)status);
            }

            if (paymentStatus != null)
            {
                query = query.Where(order => order.PaymentStatusId == (int)paymentStatus);
            }

            if (shippingStatus != null)
            {
                query = query.Where(order => order.ShippingStatusId == (int)shippingStatus);
            }

            query = query.Where(order => !order.Deleted);

            if (createdAtMin != null)
            {
                query = query.Where(order => order.CreatedOnUtc > createdAtMin.Value.ToUniversalTime());
            }

            if (createdAtMax != null)
            {
                query = query.Where(order => order.CreatedOnUtc < createdAtMax.Value.ToUniversalTime());
            }

            if (storeId != null)
            {
                query = query.Where(order => order.StoreId == storeId);
            }

            query = query.OrderBy(order => order.Id);

            return query;
        }

        private IQueryable<Order> GetOrdersQuery2(DateTime? createdAtMin = null, DateTime? createdAtMax = null, IList<int> orderStatus = null,
            IList<int> paymentStatus = null, ShippingStatus? shippingStatus = null, int? status = null, DateTime? shippedDateUtc = null, IList<int> ids = null,
            int? customerId = null, int? storeId = null, int? statusId = null, int? paymentId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _orderRepository.TableNoTracking;

            if (customerId != null)
            {
                query = query.Where(order => order.CustomerId == customerId);
            }

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(c => ids.Contains(c.Id));
            }

            if (status != null && orderStatus.Count > 0)
            {
                query = query.Where(order => orderStatus.Contains(order.OrderStatusId));
            }

            if (paymentStatus != null && paymentStatus.Count > 0)
            {
                query = query.Where(order => paymentStatus.Contains(order.PaymentStatusId));
            }

            if (shippingStatus != null)
            {
                query = query.Where(order => order.ShippingStatusId == (int)shippingStatus);
            }

            if (shippedDateUtc != null)
            {
                if (status == 1 || status == 2)
                {
                    query = query.Where(order => order.ShippedDateUtc != null 
                        && order.ShippedDateUtc.Value.Year == shippedDateUtc.Value.Year
                        && order.ShippedDateUtc.Value.Month == shippedDateUtc.Value.Month
                        && order.ShippedDateUtc.Value.Day == shippedDateUtc.Value.Day
                    );
                }
                else
                {
                    query = query.Where(order => order.ShippedDateUtc != null
                    
                    && ((order.ShippedDateUtc.Value.Year > shippedDateUtc.Value.Year)
                    || (order.ShippedDateUtc.Value.Year == shippedDateUtc.Value.Year
                    && order.ShippedDateUtc.Value.Month > shippedDateUtc.Value.Month)
                    || (order.ShippedDateUtc.Value.Year == shippedDateUtc.Value.Year
                    && order.ShippedDateUtc.Value.Month == shippedDateUtc.Value.Month
                    && order.ShippedDateUtc.Value.Day >= shippedDateUtc.Value.Day))
                    );
                }
            }

            if(statusId != null)
            {
                query = query.Where(order => order.OrderStatusId == statusId);
            }

            if (paymentId != null)
            {
                query = query.Where(order => order.PaymentStatusId == paymentId);
            }

            if (fromDate != null)
            {
                query = query.Where(order => order.ShippedDateUtc != null && order.ShippedDateUtc >= fromDate);
            }

            if (toDate != null)
            {
                query = query.Where(order => order.ShippedDateUtc != null && order.ShippedDateUtc <= toDate);
            }

            query = query.Where(order => !order.Deleted);

            if (createdAtMin != null)
            {
                query = query.Where(order => order.CreatedOnUtc > createdAtMin.Value.ToUniversalTime());
            }

            if (createdAtMax != null)
            {
                query = query.Where(order => order.CreatedOnUtc < createdAtMax.Value.ToUniversalTime());
            }

            if (storeId != null)
            {
                query = query.Where(order => order.StoreId == storeId);
            }

            query = query.OrderByDescending(order => order.CreatedOnUtc);

            return query;
        }

        public IList<OrderStatusCount> DashboardOrdersLoadAll(int markAsDeliveryDate, string deliveryDate)
        {
            IList<OrderStatusCount> orderStatusCounts = new List<OrderStatusCount>();
            //prepare parameters
            if (_dataProvider.StoredProceduredSupported)
            {
                var pMarkAsDeliveryDate = _dataProvider.GetParameter();
                pMarkAsDeliveryDate.ParameterName = "MarkAsDeliveryDate";
                pMarkAsDeliveryDate.Value = markAsDeliveryDate;
                pMarkAsDeliveryDate.DbType = DbType.Int32;

                var pDeliveryDate = _dataProvider.GetParameter();
                pDeliveryDate.ParameterName = "DeliveryDate";
                pDeliveryDate.Value = deliveryDate;
                pDeliveryDate.DbType = DbType.String;

                //invoke stored procedure
                orderStatusCounts = _objectContext.ExecuteStoredProcedureList<OrderStatusCount>(
                    "DashboardOrdersLoadAll",
                    pMarkAsDeliveryDate,
                    pDeliveryDate);


            }

            return orderStatusCounts;
        }

        public IList<OrderStatusCount> DashboardOrdersLoadStatus()
        {
            IList<OrderStatusCount> orderStatusCounts = new List<OrderStatusCount>();
            //prepare parameters
            if (_dataProvider.StoredProceduredSupported)
            {
                //invoke stored procedure
                orderStatusCounts = _objectContext.ExecuteStoredProcedureList<OrderStatusCount>(
                    "DashboardOrdersLoadStatus");
            }

            return orderStatusCounts;
        }
    }
}