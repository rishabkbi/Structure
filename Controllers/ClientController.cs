using DomainModel.EntityModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TaskManagementOsvin.Models;
using TaskManagementOsvin.Security;

namespace TaskManagementOsvin.Controllers
{    
    //[CustomAuthorize(roles: "HR,Admin,Team Leader,Project Manager")]
    public class ClientController : Controller
    {
        string BaseURL = ConfigurationManager.AppSettings["BaseURL"];
        [CustomAuthorize]
        [HttpGet]
        public ActionResult Clients()
        {
            ViewBag.Class = "display-hide";
            return View();
        }
        [HttpGet]
        public ActionResult _Clients(bool Archived = false, string Type = "true")
        {            
            var client = new HttpClient();
            List<ClientDomainModel> listClients = new List<ClientDomainModel>();
            client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
            var Clientresult = client.GetAsync(BaseURL + "/api/Client/GetAllClients?Archived="+ (Archived == true ? "Archive" : "NonArchive")).Result;
            if (Clientresult.StatusCode == HttpStatusCode.OK)
            {
                var contents = Clientresult.Content.ReadAsStringAsync().Result;
                var response = new JavaScriptSerializer().Deserialize<List<ClientDomainModel>>(contents);
                listClients = response;
                if (Type == "true")
                {
                    listClients = listClients.Where(s => s.IsActive == true).ToList();
                }
                else if (Type == "false")
                {
                    listClients = listClients.Where(s => s.IsActive == false).ToList();
                }
                else
                {
                    listClients = listClients.ToList();
                }
            }
            return PartialView(listClients);
        }
        [HttpGet]
        public ActionResult ArchiveClient(long ClientId,bool Archived = false)
        {
            ResponseDomainModel objRes = new ResponseDomainModel();
            var client = new HttpClient();
            client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
            var result = client.GetAsync(BaseURL + "/api/Client/UpdateClientArchive?ClientId=" + ClientId).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var contents = result.Content.ReadAsStringAsync().Result;
                var Response = new JavaScriptSerializer().Deserialize<ResponseDomainModel>(contents);
                objRes = Response;
            }
            return RedirectToAction("_Clients", new {Archived=Archived });
        }

        //[CustomAuthorize]
        [HttpGet]
        public ActionResult AddUpdateClient(int ClientId = 0)
        {
            ViewBag.Class = "display-hide";
            ClientModel _clients = new ClientModel();
            if (ClientId > 0)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
                var result = client.GetAsync(BaseURL + "/api/Client/GetClientById?ClientId=" + ClientId).Result;
                _clients.listSalesPortal = GetSalesPortals();
                _clients.listCountries = GetCountries();
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var contents = result.Content.ReadAsStringAsync().Result;
                    var Response = new JavaScriptSerializer().Deserialize<ClientModel>(contents);
                    _clients = Response; 
                }
            }
            _clients.listSalesPortal = GetSalesPortals();
            _clients.listCountries = GetCountries();
            return View(_clients);
        }

        [HttpPost]
        public ActionResult AddUpdateClient(ClientModel model)
        {
            if (model != null)
            {
                //if (model.ClientId == 0)
                model.Archived = "NonArchive";
                model.CreatedBy = UserManager.user.UserId;
                var serialized = new JavaScriptSerializer().Serialize(model);
                var client = new HttpClient();
                var content = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
                var result = client.PostAsync(BaseURL + "/api/Client/AddUpdateClient", content).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var contents = result.Content.ReadAsStringAsync().Result;
                    var Response = new JavaScriptSerializer().Deserialize<ResponseDomainModel>(contents);
                }
            }
            return RedirectToAction("Clients"); //RedirectToAction("_Clients", new { Archived = model.Archived=="NonArchive"?false:true });
        }

        public ActionResult ClientDetails(int ClientId)
        {
            ClientModel _clients = new ClientModel();
            if (ClientId > 0)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
                var result = client.GetAsync(BaseURL + "/api/Client/GetClientById?ClientId=" + ClientId).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var contents = result.Content.ReadAsStringAsync().Result;
                    var Response = new JavaScriptSerializer().Deserialize<ClientModel>(contents);
                    _clients = Response; 
                }
            }
            return View(_clients);
        }

        [HttpGet]
        public ActionResult ActivateDeactivateClient(long ClientId, bool IsActive, bool Archived = false)
        {
            if (ClientId > 0)
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
                var result = client.GetAsync(BaseURL + "/api/Client/ActivateDeactivateClient?ClientId=" + ClientId + "&IsActive=" + IsActive).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var contents = result.Content.ReadAsStringAsync().Result;
                    var Response = new JavaScriptSerializer().Deserialize<ResponseModel>(contents);
                }
            }
            return RedirectToAction("_Clients", Archived);
        }
        #region
        public List<CountryDomainModel> GetCountries()
        {
            List<CountryDomainModel> listCountries = new List<CountryDomainModel>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
            var result = client.GetAsync(BaseURL + "/api/Management/GetAllCountries").Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var contents = result.Content.ReadAsStringAsync().Result;
                var Response = new JavaScriptSerializer().Deserialize<CountryDomainModel>(contents);
                listCountries = Response.listCountries;
            }
            return listCountries;
        }
        public List<SalesPortalDomainModel> GetSalesPortals()
        {
            List<SalesPortalDomainModel> listSalesPortals = new List<SalesPortalDomainModel>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(HttpContext.Request.Url.AbsoluteUri);
            var result = client.GetAsync(BaseURL + "/api/SalesPortal/GetAllSalesPortals").Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var contents = result.Content.ReadAsStringAsync().Result;
                var Response = new JavaScriptSerializer().Deserialize<SalesPortalDomainModel>(contents);
                listSalesPortals = Response.listSalesPortals;
            }
            return listSalesPortals;
        }
        #endregion
    }
}