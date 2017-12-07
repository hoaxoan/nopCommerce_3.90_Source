namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the interface of the country model factory
    /// </summary>
    public partial interface ICountryModelFactory
    {
        /// <summary>
        /// Get states and provinces by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of states</param>
        /// <returns>List of identifiers and names of states and provinces</returns>
        dynamic GetStatesByCountryId(string countryId, bool addSelectStateItem);

        /// <summary>
        /// Get districts by state and province identifier
        /// </summary>
        /// <param name="stateProvinceId">State and province identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of districts</param>
        /// <returns>List of identifiers and names of districts</returns>
        dynamic GetDistrictsByStateProvinceId(string stateProvinceId, bool addSelectStateItem);

        /// <summary>
        /// Get wards by district identifier
        /// </summary>
        /// <param name="districtId">District identifier</param>
        /// <param name="addSelectStateItem">Whether to add "Select state" item to list of wards</param>
        /// <returns>List of identifiers and names of wards</returns>
        dynamic GetWardsByDistrictId(string districtId, bool addSelectStateItem);
    }
}
