using Newtonsoft.Json;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication;
using Nop.Services.Customers;
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
        #endregion

        #region Ctor
        public AuthorizationController(IAuthenticationService authenticationService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService)
        {
            this._authenticationService = authenticationService;
            this._customerRegistrationService = customerRegistrationService;
            this._customerService = customerService;
        }
        #endregion
        [HttpPost]
        [AllowAnonymous]
        public ActionResult OAuth(string email, string password)
        {
            //string email, string password
            //var loginResult =
            //       _customerRegistrationService.ValidateCustomer(email, password);
            //if (CustomerLoginResults.Successful == loginResult)
            //{
            //    var customer = _customerService.GetCustomerByEmail(email);

            //    //sign in new customer
            //    _authenticationService.SignIn(customer, true);
            //}

            string clientId = "c46cb1f2-a0e9-419a-bd0e-c1c0eb057cd8";
            string clientSecret = "8af3824b-5b35-47d9-9aa4-fbf2156ef130";
            string serverUrl = "http://localhost:15536";
            var nopAuthorizationManager = new AuthorizationManager(clientId, clientSecret, serverUrl);

            var redirectUrl = Url.RouteUrl("GetAccessToken", null, Request.Url.Scheme);

            // For demo purposes this data is kept into the current Session, but in production environment you should keep it in your database
            //Session["clientId"] = clientId;
            //Session["clientSecret"] = clientSecret;
            //Session["serverUrl"] = serverUrl;
            //Session["redirectUrl"] = redirectUrl;

            //// This should not be saved anywhere.
            var state = Guid.NewGuid();
            //Session["state"] = state;

            string authUrl = nopAuthorizationManager.BuildAuthUrl(redirectUrl, new string[] { }, state.ToString());

            return Redirect(authUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAccessToken(string code, string state)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var model = new AccessModel();

                try
                {
                    // TODO: Here you should get the authorization user data from the database instead from the current Session.
                    //string clientId = Session["clientId"].ToString();
                    //string clientSecret = Session["clientSecret"].ToString();
                    //string serverUrl = Session["serverUrl"].ToString();
                    //string redirectUrl = Session["redirectUrl"].ToString();

                    string clientId = "c46cb1f2-a0e9-419a-bd0e-c1c0eb057cd8";
                    string clientSecret = "8af3824b-5b35-47d9-9aa4-fbf2156ef130";
                    string serverUrl = "http://localhost:15536";
                    var redirectUrl = Url.RouteUrl("GetAccessToken", null, Request.Url.Scheme);

                    var authParameters = new AuthParameters()
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        ServerUrl = serverUrl,
                        RedirectUrl = redirectUrl,
                        GrantType = "authorization_code",
                        Code = code
                    };

                    var nopAuthorizationManager = new AuthorizationManager(authParameters.ClientId, authParameters.ClientSecret, authParameters.ServerUrl);

                    string responseJson = nopAuthorizationManager.GetAuthorizationData(authParameters);

                    AuthorizationModel authorizationModel = JsonConvert.DeserializeObject<AuthorizationModel>(responseJson);

                    model.AuthorizationModel = authorizationModel;
                    model.UserAccessModel = new UserAccessModel()
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        ServerUrl = serverUrl,
                        RedirectUrl = redirectUrl
                    };

                    // TODO: Here you can save your access and refresh tokens in the database. For illustration purposes we will save them in the Session and show them in the view.
                    //Session["accessToken"] = authorizationModel.AccessToken;
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