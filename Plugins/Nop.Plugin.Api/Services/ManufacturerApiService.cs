using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.DataStructures;
using Nop.Services.Stores;

namespace Nop.Plugin.Api.Services
{
    public class ManufacturerApiService : IManufacturerApiService
    {
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<ProductManufacturer> _productManufacturerMappingRepository;

        public ManufacturerApiService(IRepository<Manufacturer> manufacturerRepository,
            IRepository<ProductManufacturer> productManufacturerMappingRepository,
            IStoreMappingService storeMappingService)
        {
            _manufacturerRepository = manufacturerRepository;
            _productManufacturerMappingRepository = productManufacturerMappingRepository;
            _storeMappingService = storeMappingService;
        }

        public IList<Manufacturer> GetManufacturers(IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
            int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId, 
            int? productId = null,
            bool? publishedStatus = null)
        {
            var query = GetManufacturersQuery(createdAtMin, createdAtMax, updatedAtMin, updatedAtMax, publishedStatus, productId, ids);


            if (sinceId > 0)
            {
                query = query.Where(c => c.Id > sinceId);
            }

            return new ApiList<Manufacturer>(query, page - 1, limit);
        }

        public Manufacturer GetManufacturerById(int id)
        {
            if (id <= 0)
                return null;

            Manufacturer manufacturer = _manufacturerRepository.Table.FirstOrDefault(cat => cat.Id == id && !cat.Deleted);

            return manufacturer;
        }

        public int GetManufacturersCount(DateTime? createdAtMin = null, DateTime? createdAtMax = null,
           DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
           bool? publishedStatus = null, int? productId = null)
        {
            var query = GetManufacturersQuery(createdAtMin, createdAtMax, updatedAtMin, updatedAtMax,
                                           publishedStatus, productId);

            return query.ToList().Count(c => _storeMappingService.Authorize(c));
        }

        private IQueryable<Manufacturer> GetManufacturersQuery(
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
            bool? publishedStatus = null, int? productId = null, IList<int> ids = null)
        {
            var query = _manufacturerRepository.TableNoTracking;

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(c => ids.Contains(c.Id));
            }

            if (publishedStatus != null)
            {
                query = query.Where(c => c.Published == publishedStatus.Value);
            }

            query = query.Where(c => !c.Deleted);

            if (createdAtMin != null)
            {
                query = query.Where(c => c.CreatedOnUtc > createdAtMin.Value);
            }

            if (createdAtMax != null)
            {

                query = query.Where(c => c.CreatedOnUtc < createdAtMax.Value);
            }

            if (updatedAtMin != null)
            {
                query = query.Where(c => c.UpdatedOnUtc > updatedAtMin.Value);
            }

            if (updatedAtMax != null)
            {
                query = query.Where(c => c.UpdatedOnUtc < updatedAtMax.Value);
            }

            //only distinct categories (group by ID)
            query = from c in query
                    group c by c.Id
                        into cGroup
                    orderby cGroup.Key
                    select cGroup.FirstOrDefault();

            if (productId != null)
            {
                var manufacturerMappingsForProduct = from productManufacturerMapping in _productManufacturerMappingRepository.TableNoTracking
                                                 where productManufacturerMapping.ProductId == productId
                                                 select productManufacturerMapping;

                query = from manufacturer in query
                        join productManufacturerMapping in manufacturerMappingsForProduct on manufacturer.Id equals productManufacturerMapping.ManufacturerId
                        select manufacturer;
            }

            query = query.OrderBy(category => category.Id);

            return query;
        }
    }
}