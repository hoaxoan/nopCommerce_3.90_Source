using System.Collections.Generic;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class ManufacturerExtensions
    {
        /// <summary>
        /// Returns a ProductManufacturer that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>A ProductManufacturer that has the specified values; otherwise null</returns>
        public static ProductManufacturer FindProductManufacturer(this IList<ProductManufacturer> source,
            int productId, int manufacturerId)
        {
            foreach (var productManufacturer in source)
                if (productManufacturer.ProductId == productId && productManufacturer.ManufacturerId == manufacturerId)
                    return productManufacturer;

            return null;
        }

        /// <summary>
        /// Returns a CategoryManufacturer that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>A CategoryManufacturer that has the specified values; otherwise null</returns>
        public static CategoryManufacturer FindCategoryManufacturer(this IList<CategoryManufacturer> source,
            int categoryId, int manufacturerId)
        {
            foreach (var categoryManufacturer in source)
                if (categoryManufacturer.CategoryId == categoryId && categoryManufacturer.ManufacturerId == manufacturerId)
                    return categoryManufacturer;

            return null;
        }

    }
}
