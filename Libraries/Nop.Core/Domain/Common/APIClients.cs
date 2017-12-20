using System;
using Nop.Core.Domain.Directory;

namespace Nop.Core.Domain.Common
{
    public partial class APIClients : BaseEntity, ICloneable
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the clientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ClientSecret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the CallbackUrl
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the AuthenticationCode
        /// </summary>
        public string AuthenticationCode { get; set; }

        /// <summary>
        /// Gets or sets the ServerUrl
        /// </summary>
        public string ServerUrl { get; set; }

        public object Clone()
        {
            var client = new APIClients
            {
                Name = this.Name,
                ClientId = this.ClientId,
                ClientSecret = this.ClientSecret,
                CallbackUrl = this.CallbackUrl,
                IsActive = this.IsActive,
                AuthenticationCode = this.AuthenticationCode,
                ServerUrl = this.ServerUrl
            };
            return client;
        }
    }
}
