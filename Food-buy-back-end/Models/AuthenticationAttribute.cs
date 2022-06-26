using Food_buy_back_end.Providers;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Food_buy_back_end.Models
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            bool noRedirect;


            if (!IsUserTokenValid(filterContext.HttpContext, out noRedirect))
            {
                string redirectFrom = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;

                if (noRedirect)
                {
                    redirectFrom = string.Empty;
                }

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Index",
                    redirectedFrom = redirectFrom
                }));
            }
        }



        public bool IsUserTokenValid(HttpContextBase context, out bool noRedirect)
        {
            if (!context.Items.Contains(MvcApplication.AUTHENTICATION_COOKIE))
            {
                noRedirect = false;
                return false;
            }

            UserToken token = (UserToken)context.Items[MvcApplication.AUTHENTICATION_COOKIE];


            if (token.ExpiryDate.CompareTo(DateTime.UtcNow) < 1)
            {
                noRedirect = true;
                return false;
            }

            //Renew Token.
            int sessionLength = Convert.ToInt32(ConfigurationManager.AppSettings["LoginSessionLength"]);
            var newToken = token.GetRenewedToken(sessionLength);
            CreateAuthenticationCookie(newToken, context);
            noRedirect = false;

            return true;
        }

        protected void CreateAuthenticationCookie(UserToken token, HttpContextBase context)
        {
            string encryptedCode = CipherProvider.EncryptPassword(token.GetToken());

            var authCookie = context.Response.Cookies.AllKeys.FirstOrDefault(k => k == MvcApplication.AUTHENTICATION_COOKIE);

            if (string.IsNullOrWhiteSpace(authCookie))
            {
                context.Response.Cookies.Add(new System.Web.HttpCookie(MvcApplication.AUTHENTICATION_COOKIE, encryptedCode));
            }
            else
            {
                context.Response.Cookies.Set(new System.Web.HttpCookie(MvcApplication.AUTHENTICATION_COOKIE, encryptedCode));
            }
        }
    }
}