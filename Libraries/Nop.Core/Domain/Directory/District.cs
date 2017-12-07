
using Nop.Core.Domain.Localization;
using System.Collections.Generic;

namespace Nop.Core.Domain.Directory
{
    /// <summary>
    /// Represents a district
    /// </summary>
    public partial class District : BaseEntity, ILocalizedEntity
    {
        private ICollection<Ward> _wards;
    
        /// <summary>
        /// Gets or sets the state/province identifier
        /// </summary>
        public int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the state/province
        /// </summary>
        public virtual StateProvince StateProvince { get; set; }

        /// <summary>
        /// Gets or sets the wards
        /// </summary>
        public virtual ICollection<Ward> Wards
        {
            get { return _wards ?? (_wards = new List<Ward>()); }
            protected set { _wards = value; }
        }
    }

}
