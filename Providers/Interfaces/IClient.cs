using DomainModel.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Providers.Repositories
{
    public interface IClient
    {
        List<ClientDomainModel> GetClients();
        List<ClientDomainModel> GetAllClients(string Archived);
        ResponseDomainModel UpdateClientArchive(long ClientId);
        ResponseDomainModel AddUpdateClient(ClientDomainModel model);
        ClientDomainModel GetClientById(long ClientId);
        ResponseDomainModel ActivateDeactivateClient(long ClientId, bool IsActive);
    }
}
