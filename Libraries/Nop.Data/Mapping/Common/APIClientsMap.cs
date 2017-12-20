using Nop.Core.Domain.Common;

namespace Nop.Data.Mapping.Catalog
{
    public partial class APIClientsMap : NopEntityTypeConfiguration<APIClients>
    {
        public APIClientsMap()
        {
            this.ToTable("API_Clients");
            this.HasKey(pam => pam.Id);

            this.Property(pav => pav.ClientId).IsRequired();
            this.Property(pav => pav.ClientSecret).IsRequired();
            this.Property(pav => pav.CallbackUrl).IsRequired();
        }
    }
}