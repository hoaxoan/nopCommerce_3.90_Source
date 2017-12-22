using System;
using Nop.Core.Domain.Directory;

namespace Nop.Core.Domain.Common
{
    public partial class Device : BaseEntity, ICloneable
    {
        /// <summary>
        /// Gets or sets cordova
        /// </summary>
        public string Cordova { get; set; }

        /// <summary>
        /// Gets or sets model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets platform
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets uuid
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// Gets or sets version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets manufacturer
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets serial
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Gets or sets token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        public object Clone()
        {
            var device = new Device
            {
                Cordova = this.Cordova,
                Model = this.Model,
                Platform = this.Platform,
                UUID = this.UUID,
                Version = this.Version,
                Manufacturer = this.Manufacturer,
                Serial = this.Serial,
                Token = this.Token
            };
            return device;
        }
    }
}
