using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class CategoryManufacturerMap : NopEntityTypeConfiguration<CategoryManufacturer>
    {
        public CategoryManufacturerMap()
        {
            this.ToTable("Category_Manufacturer_Mapping");
            this.HasKey(pm => pm.Id);
            
            this.HasRequired(pm => pm.Manufacturer)
                .WithMany()
                .HasForeignKey(pm => pm.ManufacturerId);


            this.HasRequired(pm => pm.Category)
                .WithMany(p => p.CategoryManufacturers)
                .HasForeignKey(pm => pm.CategoryId);
        }
    }
}