using Nop.Core.Domain.Directory;

namespace Nop.Data.Mapping.Directory
{
    public partial class DistrictMap : NopEntityTypeConfiguration<District>
    {
        public DistrictMap()
        {
            this.ToTable("District");
            this.HasKey(sp => sp.Id);
            this.Property(sp => sp.Name).IsRequired().HasMaxLength(100);
            this.Property(sp => sp.Abbreviation).HasMaxLength(100);

            this.HasRequired(sp => sp.StateProvince)
                .WithMany(c => c.Districts)
                .HasForeignKey(sp => sp.StateProvinceId);
        }
    }
}