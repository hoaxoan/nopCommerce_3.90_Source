namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a category manufacturer mapping
    /// </summary>
    public partial class CategoryManufacturer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer identifier
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the category is featured
        /// </summary>
        public bool IsFeaturedCategory { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer
        /// </summary>
        public virtual Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public virtual Category Category { get; set; }
    }

}
