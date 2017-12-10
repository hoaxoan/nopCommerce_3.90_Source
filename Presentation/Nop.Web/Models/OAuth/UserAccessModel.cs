namespace Nop.Web.Models.OAuth
{
    public class UserAccessModel
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ServerUrl { get; set; }

        public string RedirectUrl { get; set; }
    }
}