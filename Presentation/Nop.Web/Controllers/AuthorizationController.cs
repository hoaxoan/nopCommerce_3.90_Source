using Newtonsoft.Json;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Models.Customer;
using Nop.Web.Models.OAuth;
using Nop.Web.OAuths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerService _customerService;
        private readonly IAPIClientsService _clientsService;
        private readonly ICustomerModelFactory _customerModelFactory;
        #endregion

        #region Ctor
        public AuthorizationController(IAuthenticationService authenticationService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService,
            IAPIClientsService clientsService,
            ICustomerModelFactory customerModelFactory)
        {
            this._authenticationService = authenticationService;
            this._customerRegistrationService = customerRegistrationService;
            this._customerService = customerService;
            this._clientsService = clientsService;
            this._customerModelFactory = customerModelFactory;
        }
        #endregion
        [HttpPost]
        [AllowAnonymous]
        public ActionResult OAuth(string email, string password)
        {  
            APIClients client = _clientsService.GetClientByName(email);
            var nopAuthorizationManager = new AuthorizationManager(client.ClientId, client.ClientSecret, client.ServerUrl);

            var redirectUrl = Url.RouteUrl("GetAccessToken", null, Request.Url.Scheme);

            string state = string.Format("{0}:{1}", email, password);
            string authUrl = nopAuthorizationManager.BuildAuthUrl(redirectUrl, new string[] { }, state);

            return Redirect(authUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAccessToken(string code, string state)
        {
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                var model = new AccessModel();

                try
                {
                    string[] states = state.Split(':');
                    if(states.Length <= 1)
                    {
                        return BadRequest();
                    }

                    string email = states[0];
                    string password = states[1];
                    var loginResult =
                           _customerRegistrationService.ValidateCustomer(email, password);
                    if (CustomerLoginResults.Successful != loginResult)
                    {
                        return BadRequest();
                    }

                    var customer = _customerService.GetCustomerByEmail(email);

                    APIClients client = _clientsService.GetClientByName(email);

                    var redirectUrl = Url.RouteUrl("GetAccessToken", null, Request.Url.Scheme);

                    var authParameters = new AuthParameters()
                    {
                        ClientId = client.ClientId,
                        ClientSecret = client.ClientSecret,
                        ServerUrl = client.ServerUrl,
                        RedirectUrl = redirectUrl,
                        GrantType = "authorization_code",
                        Code = code
                    };

                    var nopAuthorizationManager = new AuthorizationManager(authParameters.ClientId, authParameters.ClientSecret, authParameters.ServerUrl);

                    string responseJson = nopAuthorizationManager.GetAuthorizationData(authParameters);

                    var customerModel = new CustomerInfoModel();
                    customerModel = _customerModelFactory.PrepareCustomerInfoModel(customerModel, customer, false);

                    AuthorizationModel authorizationModel = JsonConvert.DeserializeObject<AuthorizationModel>(responseJson);

                    model.AuthorizationModel = authorizationModel;
                    model.UserAccessModel = new UserAccessModel()
                    {
                        ClientId = client.ClientId,
                        ClientSecret = client.ClientSecret,
                        ServerUrl = client.ServerUrl,
                        RedirectUrl = redirectUrl
                    };
                    model.CustomerModel = customerModel;

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult RefreshAccessToken(string refreshToken, string clientId, string serverUrl)
        {
            string json = string.Empty;

            if (ModelState.IsValid &&
                !string.IsNullOrEmpty(refreshToken) &&
                !string.IsNullOrEmpty(clientId) &&
                !string.IsNullOrEmpty(serverUrl))
            {
                var model = new AccessModel();

                try
                {
                    var authParameters = new AuthParameters()
                    {
                        ClientId = clientId,
                        ServerUrl = serverUrl,
                        RefreshToken = refreshToken,
                        GrantType = "refresh_token"
                    };

                    var nopAuthorizationManager = new AuthorizationManager(authParameters.ClientId,
                        authParameters.ClientSecret, authParameters.ServerUrl);

                    string responseJson = nopAuthorizationManager.RefreshAuthorizationData(authParameters);

                    AuthorizationModel authorizationModel =
                        JsonConvert.DeserializeObject<AuthorizationModel>(responseJson);

                    model.AuthorizationModel = authorizationModel;
                    model.UserAccessModel = new UserAccessModel()
                    {
                        ClientId = clientId,
                        ServerUrl = serverUrl
                    };

                    // Here we use the temp data because this method is called via ajax and here we can't hold a session.
                    // This is needed for the GetCustomers method in the CustomersController.
                    TempData["accessToken"] = authorizationModel.AccessToken;
                    TempData["serverUrl"] = serverUrl;
                }
                catch (Exception ex)
                {
                    json = string.Format("error: '{0}'", ex.Message);

                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                json = JsonConvert.SerializeObject(model.AuthorizationModel);
            }
            else
            {
                json = "error: 'something went wrong'";
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private JsonResult BadRequest(string message = "Bad Request")
        {
            return Json(message);
        }
    }
}