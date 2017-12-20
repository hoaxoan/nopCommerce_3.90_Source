

using Nop.Core.Domain.Common;
using System.Collections.Generic;

namespace Nop.Services.Common
{
    /// <summary>
    /// APIClient service interface
    /// </summary>
    public partial interface IAPIClientsService
    {
        bool ValidateClient(string clientId, string clientSecret, string authenticationCode);
        APIClients GetClient(string clientId);
        bool ValidateClientById(string clientId);
        IList<APIClients> GetAllClients();
        void DeleteClient(APIClients client);
        APIClients GetClientById(int id);
        APIClients GetClientByClientId(string clientId);
        APIClients GetClientByName(string name);

        void InsertClient(APIClients client);
        void UpdateClient(APIClients client);
    }
}