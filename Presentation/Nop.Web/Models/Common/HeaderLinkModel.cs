using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Common
{
    public partial class HeaderLinkModel : BaseNopModel
    {
        public string CompanyAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string Email { get; set; }
        public SocialModel SocialModel { get; set; }
    }
}