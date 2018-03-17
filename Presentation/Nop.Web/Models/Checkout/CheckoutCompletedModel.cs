using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Order;

namespace Nop.Web.Models.Checkout
{
    public partial class CheckoutCompletedModel : BaseNopModel
    {
        public CheckoutCompletedModel()
        {
            OrderDetails = new OrderDetailsModel();
        }

        public int OrderId { get; set; }
        public string CustomOrderNumber { get; set; }
        public bool OnePageCheckoutEnabled { get; set; }

        public string CompanyAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }

        public OrderDetailsModel OrderDetails { get; set; }
    }
}