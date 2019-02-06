using Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.EntityModel;
using Providers.Helper;

namespace Providers.Providers.SP.Repositories
{
    public class ClientRepository : IClient, IDisposable
    {
        SqlHelper objHelper = new SqlHelper();
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public List<ClientDomainModel> GetClients()
        {
            try
            {
                var clients = objHelper.Query<ClientDomainModel>("Get_ClientNameList", null).ToList();
                return clients;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return null;
            }
        }
        public List<ClientDomainModel> GetAllClients(string Archived)
        {
            try
            {
                //Get_ClientListNew
                var clients = objHelper.Query<ClientDomainModel>("GetAllClients", new { clienttype=Archived }).ToList();
                return clients;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                return null;
            }
        }
        public ResponseDomainModel UpdateClientArchive(long ClientId)
        {
            ResponseDomainModel objRes = new ResponseDomainModel();
            try
            {
                var res = objHelper.Query<string>("UpdateClientArchive", new { clientid = ClientId });
                string Archived = res.First();
                if (Archived == "Insert")
                {
                    objRes.isSuccess = true;
                    objRes.response = "Client has Archived.";
                }
                else if (Archived == "Update")
                {
                    objRes.isSuccess = true;
                    objRes.response = "Client has removed from Archive.";
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
            }
            return objRes;
        }
        public ResponseDomainModel AddUpdateClient(ClientDomainModel model)
        {
            ResponseDomainModel objRes = new ResponseDomainModel();
            try
            {
                int ClientId = objHelper.ExecuteScalar("AddUpdateClient", new
                {
                    ClientId = model.ClientId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Skype = model.Skype,
                    Whatsapp = model.Whatsapp,
                    PhoneNumber = model.PhoneNumber,
                    SourceId = model.SourceId,
                    BussinessName = model.BussinessName,
                    Website = model.Website,
                    Location = model.Location,
                    City = model.City,
                    State = model.State,
                    CountryId = model.CountryId,
                    ZipCode = model.ZipCode,
                    GSTNumber = model.GSTNumber,
                    PanNumber = model.PanNumber,
                    PaypalId = model.PaypalId,
                    Remarks = model.Remarks,
                    LinkedInUrl = model.LinkedInUrl,
                    FacebookUrl = model.FacebookUrl,
                    TwitterUrl = model.TwitterUrl,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    Archived=model.Archived,
                    CreatedBy=model.CreatedBy
                });
                if (model.ClientId > 0)
                {
                    objRes.isSuccess = true;
                    objRes.response = "Client updated successfully.";
                }
                else if (ClientId > 0)
                {
                    objRes.isSuccess = true;
                    objRes.response = "Client added successfully.";
                }
                else
                {
                    objRes.isSuccess = false;
                    objRes.response = "Something went wrong, please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                objRes.isSuccess = false;
                objRes.response = ex.Message;
            }
            return objRes;
        }
        public ClientDomainModel GetClientById(long ClientId)
        {
            ClientDomainModel _details = new ClientDomainModel();
            try
            {
                _details = objHelper.Query<ClientDomainModel>("GetClientById", new { ClientId = ClientId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
            }
            return _details;
        }
        public ResponseDomainModel ActivateDeactivateClient(long ClientId, bool IsActive)
        {
            ResponseDomainModel objRes = new ResponseDomainModel();
            try
            {
                var res = objHelper.Execute("ActivateDeactivateClient", new { ClientId = ClientId, IsActive = IsActive });
                if (res > 0)
                {
                    objRes.isSuccess = true;
                    objRes.response = "sucsess";
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
            }
            return objRes;
        }
    }
}
