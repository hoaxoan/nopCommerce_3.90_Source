using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Validators.Checkout
{
    public partial class CheckoutValidator : BaseNopValidator<OnePageCheckoutModel>
    {
        public CheckoutValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.FullName.Required"));
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.PhoneNumber.Required"));
            //RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Address.Fields.Email2.Required"));
        }
    }
}