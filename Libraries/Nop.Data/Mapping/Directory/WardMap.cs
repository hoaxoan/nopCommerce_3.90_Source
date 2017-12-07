using Nop.Core.Domain.Directory;

namespace Nop.Data.Mapping.Directory
{
    public partial class WardMap : NopEntityTypeConfiguration<Ward>
    {
        public WardMap()
        {
            this.ToTable(" Ward");
            this.HasKey(sp => sp.Id);
            this.Property(sp => sp.Name).IsRequired().HasMaxLength(100);
            this.Property(sp => sp.Abbreviation).HasMaxLength(100);


            this.HasRequired(sp => sp.District)
                .WithMany(c => c.Wards)
                .HasForeignKey(sp => sp.DistrictId);
        }
    }
}