using System.Collections.Generic;
using Nop.Core.Domain.Directory;

namespace Nop.Services.Directory
{
    /// <summary>
    /// District service interface
    /// </summary>
    public partial interface IDistrictService
    {
        /// <summary>
        /// Deletes a district
        /// </summary>
        /// <param name="district">The district</param>
        void DeleteDistrict(District district);

        /// <summary>
        /// Gets a district
        /// </summary>
        /// <param name="districtId">The district identifier</param>
        /// <returns>District/returns>
        District GetDistrictById(int districtId);

        /// <summary>
        /// Gets a district
        /// </summary>
        /// <param name="abbreviation">The district abbreviation</param>
        /// <returns>District</returns>
        District GetDistrictByAbbreviation(string abbreviation);

        /// <summary>
        /// Gets a district collection by country identifier
        /// </summary>
        /// <param name="stateProvinceId">State/Province identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Districts</returns>
        IList<District> GetDistrictsByStateProvinceId(int stateProvinceId, int languageId = 0, bool showHidden = false);

        /// <summary>
        /// Gets all district
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>District</returns>
        IList<District> GetDistricts(bool showHidden = false);

        /// <summary>
        /// Inserts a district
        /// </summary>
        /// <param name="district">District</param>
        void InsertDistrict(District district);

        /// <summary>
        /// Updates a district
        /// </summary>
        /// <param name="district">District</param>
        void UpdateDistrict(District district);
    }
}
