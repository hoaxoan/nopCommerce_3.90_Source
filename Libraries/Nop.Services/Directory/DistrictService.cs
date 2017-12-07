using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Services.Events;
using Nop.Services.Localization;

namespace Nop.Services.Directory
{
    /// <summary>
    /// District service
    /// </summary>
    public partial class DistrictService : IDistrictService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        private const string DISTRICTS_ALL_KEY = "Nop.district.all-{0}-{1}-{2}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string DISTRICTS_PATTERN_KEY = "Nop.district.";

        #endregion

        #region Fields

        private readonly IRepository<District> _districtRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="districtRepository">District repository</param>
        /// <param name="eventPublisher">Event published</param>
        public DistrictService(ICacheManager cacheManager,
            IRepository<District> districtRepository,
            IEventPublisher eventPublisher)
        {
            _cacheManager = cacheManager;
            _districtRepository = districtRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Deletes a district
        /// </summary>
        /// <param name="district">The district</param>
        public virtual void DeleteDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");
            
            _districtRepository.Delete(district);

            _cacheManager.RemoveByPattern(DISTRICTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(district);
        }

        /// <summary>
        /// Gets a district
        /// </summary>
        /// <param name="districtId">The district identifier</param>
        /// <returns>District/returns>
        public virtual District GetDistrictById(int districtId)
        {
            if (districtId == 0)
                return null;

            return _districtRepository.GetById(districtId);
        }

        /// <summary>
        /// Gets a district
        /// </summary>
        /// <param name="abbreviation">The district abbreviation</param>
        /// <returns>District</returns>
        public virtual District GetDistrictByAbbreviation(string abbreviation)
        {
            var query = from sp in _districtRepository.Table
                        where sp.Abbreviation == abbreviation
                        select sp;
            var district = query.FirstOrDefault();
            return district;
        }

        /// <summary>
        /// Gets a district collection by country identifier
        /// </summary>
        /// <param name="stateProvinceId">State/Province identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Districts</returns>
        public virtual IList<District> GetDistrictsByStateProvinceId(int stateProvinceId, int languageId = 0, bool showHidden = false)
        {
            string key = string.Format(DISTRICTS_ALL_KEY, stateProvinceId, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from sp in _districtRepository.Table
                            orderby sp.DisplayOrder, sp.Name
                            where sp.StateProvinceId == stateProvinceId &&
                            (showHidden || sp.Published)
                            select sp;
                var districts = query.ToList();

                if (languageId > 0)
                {
                    //we should sort states by localized names when they have the same display order
                    districts = districts
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(c => c.GetLocalized(x => x.Name, languageId))
                        .ToList();
                }
                return districts;
            });
        }

        /// <summary>
        /// Gets all district
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>District</returns>
        public virtual IList<District> GetDistricts(bool showHidden = false)
        {
            var query = from sp in _districtRepository.Table
                        orderby sp.StateProvinceId, sp.DisplayOrder, sp.Name
                where showHidden || sp.Published
                select sp;
            var districts = query.ToList();
            return districts;
        }

        /// <summary>
        /// Inserts a district
        /// </summary>
        /// <param name="district">District</param>
        public virtual void InsertDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");

            _districtRepository.Insert(district);

            _cacheManager.RemoveByPattern(DISTRICTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(district);
        }

        /// <summary>
        /// Updates a district
        /// </summary>
        /// <param name="district">District</param>
        public virtual void UpdateDistrict(District district)
        {
            if (district == null)
                throw new ArgumentNullException("district");

            _districtRepository.Update(district);

            _cacheManager.RemoveByPattern(DISTRICTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(district);
        }

        #endregion
    }
}
