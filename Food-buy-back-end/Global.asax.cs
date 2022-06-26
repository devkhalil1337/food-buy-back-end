using Food_buy_back_end.Models;
using Food_buy_back_end.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Food_buy_back_end
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string AUTHENTICATION_COOKIE = "ProfileMyDriverCookie";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[AUTHENTICATION_COOKIE];
            if (authCookie != null)
            {

                string decryptedCode = CipherProvider.DecryptPassword(authCookie.Value);
                var token = new UserToken(decryptedCode);
                Context.Items.Add(AUTHENTICATION_COOKIE, token);
            }
        }
    }
}
