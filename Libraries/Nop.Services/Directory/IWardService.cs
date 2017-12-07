using System.Collections.Generic;
using Nop.Core.Domain.Directory;

namespace Nop.Services.Directory
{
    /// <summary>
    /// Ward service interface
    /// </summary>
    public partial interface IWardService
    {
        /// <summary>
        /// Deletes a ward
        /// </summary>
        /// <param name="ward">The ward</param>
        void DeleteWard(Ward ward);

        /// <summary>
        /// Gets a ward
        /// </summary>
        /// <param name="wardId">The ward identifier</param>
        /// <returns>ward</returns>
        Ward GetWardById(int wardId);

        /// <summary>
        /// Gets a ward 
        /// </summary>
        /// <param name="abbreviation">The ward abbreviation</param>
        /// <returns>ward</returns>
        Ward GetWardByAbbreviation(string abbreviation);

        /// <summary>
        /// Gets a ward collection by country identifier
        /// </summary>
        /// <param name="districtId">District identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Wards</returns>
        IList<Ward> GetWardsByDistrictId(int districtId, int languageId = 0, bool showHidden = false);

        /// <summary>
        /// Gets all wards
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Wards</returns>
        IList<Ward> GetWards(bool showHidden = false);

        /// <summary>
        /// Inserts a ward
        /// </summary>
        /// <param name="ward">ward</param>
        void InsertWard(Ward ward);

        /// <summary>
        /// Updates a ward
        /// </summary>
        /// <param name="ward">ward</param>
        void UpdateWard(Ward ward);
    }
}
