using DomainModel.EntityModel;
using Providers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Providers.Providers.SP.Repositories;
using Providers.Helper;
using TaskManagementOsvin.Security;

namespace TaskManagementOsvin.Controllers
{
    public class ClientApiController : ApiController
    {
        readonly IClient ClientRepository = new ClientRepository();
        // GET api/<controller>
        [Route("~/api/Client/GetClients")]
        public HttpResponseMessage GetClients()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                ClientDomainModel objRes = new ClientDomainModel();
                objRes.listClients = ClientRepository.GetClients();
                if (objRes.listClients != null)
                {
                    objRes.isSuccess = true;
                    objRes.response = "Success";
                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, objRes);
                }
                else
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, objRes);
                } 
                return httpResponse;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [Route("~/api/Client/GetAllClients")]
        public HttpResponseMessage GetAllClients(string Archived)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var clients = ClientRepository.GetAllClients(Archived);
                if (clients == null)
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, "Error Occurred");
                }
                else
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, clients);
                }
                return httpResponse;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpGet]
        [Route("~/api/Client/UpdateClientArchive")]
        public HttpResponseMessage UpdateClientArchive(long ClientId)
        {
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var objRes = ClientRepository.UpdateClientArchive(ClientId);
                if (objRes != null && objRes.isSuccess)
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, objRes);
                }
                else
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, objRes);
                }
                return httpResponse;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
        [HttpPost]
        [Route("~/api/Client/AddUpdateClient")]
        public HttpResponseMessage AddUpdateClient(ClientDomainModel model)
        {
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                ResponseDomainModel res = new ResponseDomainModel();
                if (model != null)
                {
                    res = ClientRepository.AddUpdateClient(model);
                    if (res != null && res.isSuccess)
                    {
                        httpResponse = Request.CreateResponse(HttpStatusCode.OK, res);
                    }
                    else
                    {
                        httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, res);
                    }
                }            
                return httpResponse;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("~/api/Client/GetClientById")]
        public HttpResponseMessage GetClientById(long ClientId)
        {
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                ClientDomainModel objRes = new ClientDomainModel();
                objRes = ClientRepository.GetClientById(ClientId);
                if (objRes != null)
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, objRes);
                }
                else
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, objRes);
                }
                return httpResponse;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("~/api/Client/ActivateDeactivateClient")]
        public HttpResponseMessage ActivateDeactivateClient(long ClientId, bool IsActive)
        {
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                var objRes = ClientRepository.ActivateDeactivateClient(ClientId, IsActive);
                if (objRes != null && objRes.isSuccess)
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.OK, objRes);
                }
                else
                {
                    httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, objRes);
                }
                return httpResponse;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "An error occurred, please try again or contact the administrator.",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }

        //[Route("~/api/Client/GetAllActiveClients")]
        //public HttpResponseMessage GetAllActiveClients()
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    try
        //    {
        //        ClientDomainModel objRes = new ClientDomainModel();
        //        objRes.listClients = ClientRepository.GetAllActiveClients();
        //        if (objRes.listClients != null)
        //        {
        //            objRes.isSuccess = true;
        //            objRes.response = "Success";
        //            httpResponse = Request.CreateResponse(HttpStatusCode.OK, objRes);
        //        }
        //        else
        //        {
        //            httpResponse = Request.CreateResponse(HttpStatusCode.Unauthorized, objRes);
        //        }
        //        return httpResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new StringContent("An error occurred, please try again or contact the administrator."),
        //            ReasonPhrase = "An error occurred, please try again or contact the administrator.",
        //            StatusCode = HttpStatusCode.InternalServerError
        //        });
        //    }
        //}
    }
}