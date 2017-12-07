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
    /// Ward service
    /// </summary>
    public partial class WardService : IWardService
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
        private const string WARDS_ALL_KEY = "Nop.ward.all-{0}-{1}-{2}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string WARDS_PATTERN_KEY = "Nop.ward.";

        #endregion

        #region Fields

        private readonly IRepository<Ward> _wardRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="wardRepository">Ward repository</param>
        /// <param name="eventPublisher">Event published</param>
        public WardService(ICacheManager cacheManager,
            IRepository<Ward> wardRepository,
            IEventPublisher eventPublisher)
        {
            _cacheManager = cacheManager;
            _wardRepository = wardRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Deletes a ward
        /// </summary>
        /// <param name="ward">The ward</param>
        public virtual void DeleteWard(Ward ward)
        {
            if (ward == null)
                throw new ArgumentNullException("ward");
            
            _wardRepository.Delete(ward);

            _cacheManager.RemoveByPattern(WARDS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(ward);
        }

        /// <summary>
        /// Gets a ward
        /// </summary>
        /// <param name="wardId">The ward identifier</param>
        /// <returns>ward</returns>
        public virtual Ward GetWardById(int wardId)
        {
            if (wardId == 0)
                return null;

            return _wardRepository.GetById(wardId);
        }

        /// <summary>
        /// Gets a ward 
        /// </summary>
        /// <param name="abbreviation">The ward abbreviation</param>
        /// <returns>ward</returns>
        public virtual Ward GetWardByAbbreviation(string abbreviation)
        {
            var query = from sp in _wardRepository.Table
                        where sp.Abbreviation == abbreviation
                        select sp;
            var ward = query.FirstOrDefault();
            return ward;
        }

        /// <summary>
        /// Gets a ward collection by country identifier
        /// </summary>
        /// <param name="districtId">District identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Wards</returns>
        public virtual IList<Ward> GetWardsByDistrictId(int districtId, int languageId = 0, bool showHidden = false)
        {
            string key = string.Format(WARDS_ALL_KEY, districtId, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from sp in _wardRepository.Table
                            orderby sp.DisplayOrder, sp.Name
                            where sp.DistrictId == districtId &&
                            (showHidden || sp.Published)
                            select sp;
                var wards = query.ToList();

                if (languageId > 0)
                {
                    //we should sort states by localized names when they have the same display order
                    wards = wards
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(c => c.GetLocalized(x => x.Name, languageId))
                        .ToList();
                }
                return wards;
            });
        }

        /// <summary>
        /// Gets all wards
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Wards</returns>
        public virtual IList<Ward> GetWards(bool showHidden = false)
        {
            var query = from sp in _wardRepository.Table
                        orderby sp.DistrictId, sp.DisplayOrder, sp.Name
                where showHidden || sp.Published
                select sp;
            var wards = query.ToList();
            return wards;
        }

        /// <summary>
        /// Inserts a ward
        /// </summary>
        /// <param name="ward">ward</param>
        public virtual void InsertWard(Ward ward)
        {
            if (ward == null)
                throw new ArgumentNullException("ward");

            _wardRepository.Insert(ward);

            _cacheManager.RemoveByPattern(WARDS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(ward);
        }

        /// <summary>
        /// Updates a ward
        /// </summary>
        /// <param name="ward">ward</param>
        public virtual void UpdateWard(Ward ward)
        {
            if (ward == null)
                throw new ArgumentNullException("ward");

            _wardRepository.Update(ward);

            _cacheManager.RemoveByPattern(WARDS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(ward);
        }

        #endregion
    }
}
