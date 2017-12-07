using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Nop.Core.Domain.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Common;

namespace Nop.Web.Validators.Common
{
    public partial class AddressValidator : BaseNopValidator<AddressModel>
    {
        public AddressValidator(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IDistrictService districtService,
            IWardService wardService,
            AddressSettings addressSettings)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Address.Fields.FirstName.Required"));
            //RuleFor(x => x.LastName)
            //    .NotEmpty()
            //    .WithMessage(localizationService.GetResource("Address.Fields.LastName.Required"));
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Address.Fields.Email.Required"));
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(localizationService.GetResource("Common.WrongEmail"));
            if (addressSettings.CountryEnabled)
            {
                RuleFor(x => x.CountryId)
                    .NotNull()
                    .WithMessage(localizationService.GetResource("Address.Fields.Country.Required"));
                RuleFor(x => x.CountryId)
                    .NotEqual(0)
                    .WithMessage(localizationService.GetResource("Address.Fields.Country.Required"));
            }
            if (addressSettings.CountryEnabled && addressSettings.StateProvinceEnabled)
            {
                Custom(x =>
                {
                    //does selected country has states?
                    var countryId = x.CountryId.HasValue ? x.CountryId.Value : 0;
                    var hasStates = stateProvinceService.GetStateProvincesByCountryId(countryId).Any();

                    if (hasStates)
                    {
                        //if yes, then ensure that state is selected
                        if (!x.StateProvinceId.HasValue || x.StateProvinceId.Value == 0)
                        {
                            return new ValidationFailure("StateProvinceId", localizationService.GetResource("Address.Fields.StateProvince.Required"));
                        }
                    }
                    return null;
                });
            }
            if (addressSettings.StateProvinceEnabled && addressSettings.StateProvinceEnabled)
            {
                Custom(x =>
                {
                    //does selected state has districts?
                    var stateProvinceId = x.StateProvinceId.HasValue ? x.StateProvinceId.Value : 0;
                    var hasDistricts = districtService.GetDistrictsByStateProvinceId(stateProvinceId).Any();

                    if (hasDistricts)
                    {
                        //if yes, then ensure that district is selected
                        if (!x.DistrictId.HasValue || x.DistrictId.Value == 0)
                        {
                            return new ValidationFailure("DistrictId", localizationService.GetResource("Address.Fields.District.Required"));
                        }
                    }
                    return null;
                });
            }
            if (addressSettings.StateProvinceEnabled && addressSettings.StateProvinceEnabled)
            {
                Custom(x =>
                {
                    //does selected district has wards?
                    var districtId = x.DistrictId.HasValue ? x.DistrictId.Value : 0;
                    var hasWards = wardService.GetWardsByDistrictId(districtId).Any();

                    if (hasWards)
                    {
                        //if yes, then ensure that ward is selected
                        if (!x.WardId.HasValue || x.WardId.Value == 0)
                        {
                            return new ValidationFailure("WardId", localizationService.GetResource("Address.Fields.Ward.Required"));
                        }
                    }
                    return null;
                });
            }
            if (addressSettings.CompanyRequired && addressSettings.CompanyEnabled)
            {
                RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Company.Required"));
            }
            if (addressSettings.StreetAddressRequired && addressSettings.StreetAddressEnabled)
            {
                RuleFor(x => x.Address1).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.StreetAddress.Required"));
            }
            if (addressSettings.StreetAddress2Required && addressSettings.StreetAddress2Enabled)
            {
                RuleFor(x => x.Address2).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.StreetAddress2.Required"));
            }
            if (addressSettings.ZipPostalCodeRequired && addressSettings.ZipPostalCodeEnabled)
            {
                RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.ZipPostalCode.Required"));
            }
            if (addressSettings.CityRequired && addressSettings.CityEnabled)
            {
                RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.City.Required"));
            }
            if (addressSettings.PhoneRequired && addressSettings.PhoneEnabled)
            {
                RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Phone.Required"));
            }
            if (addressSettings.FaxRequired && addressSettings.FaxEnabled)
            {
                RuleFor(x => x.FaxNumber).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Fax.Required"));
            }
        }
    }
}