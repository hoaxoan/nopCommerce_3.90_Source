using Nop.Web.Models.Customer;

namespace Nop.Web.Models.OAuth
{
    public class AccessModel
    {
        public AuthorizationModel AuthorizationModel { get; set; }
        public UserAccessModel UserAccessModel { get; set; }
        public CustomerInfoModel CustomerModel { get; set; }
    }
}