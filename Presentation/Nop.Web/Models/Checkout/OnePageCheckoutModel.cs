using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.ShoppingCart;
using Nop.Web.Validators.Checkout;
using System.Web.Mvc;

namespace Nop.Web.Models.Checkout
{
    [Validator(typeof(CheckoutValidator))]
    public partial class OnePageCheckoutModel : BaseNopModel
    {
        public OnePageCheckoutModel()
        {
            ShoppingCart = new ShoppingCartModel();
            OrderTotals = new OrderTotalsModel();
        }

        public ShoppingCartModel ShoppingCart { get; set; }
        public OrderTotalsModel OrderTotals { get; set; }
       
        public bool ShippingRequired { get; set; }
        public bool DisableBillingAddressCheckoutStep { get; set; }
        public bool PaymentInfoRequired { get; set; }

        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}