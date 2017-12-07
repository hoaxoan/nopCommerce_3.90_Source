using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Infrastructure.Cache;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the country model factory
    /// </summary>
    public partial class CountryModelFactory : ICountryModelFactory
    {
		#region Fields

        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

	    #endregion

		#region Constructors

        public CountryModelFactory(ICountryService countryService, 
            IStateProvinceService stateProvinceService,
            IDistrictService districtService,
            IWardService wardService,
            ILocalizationService localizationService, 
            IWorkContext workContext,
            ICacheManager cacheManager)
		{
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._districtService = districtService;
            this._wardService = wardService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Get states and provinces by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of states</param>
        /// <returns>List of identifiers and names of states and provinces</returns>
        public virtual dynamic GetStatesByCountryId(string countryId, bool addSelectStateItem)
        {
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            string cacheKey = string.Format(ModelCacheEventConsumer.STATEPROVINCES_BY_COUNTRY_MODEL_KEY, countryId, addSelectStateItem, _workContext.WorkingLanguage.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
                var states = _stateProvinceService.GetStateProvincesByCountryId(country != null ? country.Id : 0, _workContext.WorkingLanguage.Id).ToList();
                var result = (from s in states
                              select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();


                if (country == null)
                {
                    //country is not selected ("choose country" item)
                    if (addSelectStateItem)
                    {
                        result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                    }
                    else
                    {
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                }
                else
                {
                    //some country is selected
                    if (!result.Any())
                    {
                        //country does not have states
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                    else
                    {
                        //country has some states
                        if (addSelectStateItem)
                        {
                            result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                        }
                    }
                }

                return result;
            });
            return cachedModel;
        }

        /// <summary>
        /// Get districts by state and province identifier
        /// </summary>
        /// <param name="stateProvinceId">State and province identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of districts</param>
        /// <returns>List of identifiers and names of districts</returns>
        public virtual dynamic GetDistrictsByStateProvinceId(string stateProvinceId, bool addSelectStateItem)
        {
            if (String.IsNullOrEmpty(stateProvinceId))
                throw new ArgumentNullException("stateProvinceId");

            string cacheKey = string.Format(ModelCacheEventConsumer.DISTRICTS_BY_STATEPROVICE_MODEL_KEY, stateProvinceId, addSelectStateItem, _workContext.WorkingLanguage.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var state = _stateProvinceService.GetStateProvinceById(Convert.ToInt32(stateProvinceId));
                var districts = _districtService.GetDistrictsByStateProvinceId(state != null ? state.Id : 0, _workContext.WorkingLanguage.Id).ToList();
                var result = (from s in districts
                              select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();


                if (state == null)
                {
                    //district is not selected ("choose district" item)
                    if (addSelectStateItem)
                    {
                        result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                    }
                    else
                    {
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                }
                else
                {
                    //some district is selected
                    if (!result.Any())
                    {
                        //district does not have states
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                    else
                    {
                        //district has some states
                        if (addSelectStateItem)
                        {
                            result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                        }
                    }
                }

                return result;
            });
            return cachedModel;
        }

        /// <summary>
        /// Get wards by district identifier
        /// </summary>
        /// <param name="districtId">District identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of wards</param>
        /// <returns>List of identifiers and names of wards</returns>
        public virtual dynamic GetWardsByDistrictId(string districtId, bool addSelectStateItem)
        {
            if (String.IsNullOrEmpty(districtId))
                throw new ArgumentNullException("districtId");

            string cacheKey = string.Format(ModelCacheEventConsumer.WARDS_BY_DISTRICT_MODEL_KEY, districtId, addSelectStateItem, _workContext.WorkingLanguage.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var district = _districtService.GetDistrictById(Convert.ToInt32(districtId));
                var wards = _wardService.GetWardsByDistrictId(district != null ? district.Id : 0, _workContext.WorkingLanguage.Id).ToList();
                var result = (from s in wards
                              select new { id = s.Id, name = s.GetLocalized(x => x.Name) })
                              .ToList();


                if (district == null)
                {
                    //ward is not selected ("choose ward" item)
                    if (addSelectStateItem)
                    {
                        result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                    }
                    else
                    {
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                }
                else
                {
                    //some ward is selected
                    if (!result.Any())
                    {
                        //ward does not have districts
                        //result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.OtherNonUS") });
                    }
                    else
                    {
                        //ward has some districts
                        if (addSelectStateItem)
                        {
                            result.Insert(0, new { id = 0, name = _localizationService.GetResource("Address.SelectState") });
                        }
                    }
                }

                return result;
            });
            return cachedModel;
        }
        #endregion
    }
}
