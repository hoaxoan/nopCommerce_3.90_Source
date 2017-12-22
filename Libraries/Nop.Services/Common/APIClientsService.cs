using System;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Services.Directory;
using Nop.Services.Events;
using System.Collections.Generic;

namespace Nop.Services.Common
{
    /// <summary>
    /// APIClient service
    /// </summary>
    public partial class APIClientsService : IAPIClientsService
    {
        #region Fields

        private readonly IRepository<APIClients> _clientRepository;

        #endregion

        #region Ctor

        public APIClientsService(IRepository<APIClients> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        #endregion


        #region Methods

        public bool ValidateClient(string clientId, string clientSecret, string authenticationCode)
        {
            return _clientRepository.Table.Any(client => client.ClientId == clientId &&
                                                         client.ClientSecret == clientSecret &&
                                                         client.AuthenticationCode == authenticationCode);
        }

        public APIClients GetClient(string clientId)
        {
            return _clientRepository.Table.FirstOrDefault(client => client.ClientId == clientId);
        }

        public bool ValidateClientById(string clientId)
        {
            return _clientRepository.Table.Any(client => client.ClientId == clientId);
        }

        public IList<APIClients> GetAllClients()
        {
            return _clientRepository.Table.ToList();
        }

        public APIClients GetClientById(int id)
        {
            return _clientRepository.GetById(id);
        }

        public APIClients GetClientByClientId(string clientId)
        {
            return _clientRepository.Table.FirstOrDefault(client => client.ClientId == clientId);
        }

        public APIClients GetClientByName(string name)
        {
            return _clientRepository.Table.FirstOrDefault(client => client.IsActive && client.Name == name);
        }

        public void InsertClient(APIClients client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            _clientRepository.Insert(client);
        }

        public void UpdateClient(APIClients client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            _clientRepository.Update(client);
        }

        public void DeleteClient(APIClients client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _clientRepository.Delete(client);
        }

        #endregion
    }
}