using Food_buy_back_end.Models;
using Food_buy_back_end.Providers;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Food_buy_back_end.Models.CredentialsResult;

namespace Food_buy_back_end.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CheckUserCredentials(string email, string password)
        {
            CredentialsResult credentialsResult = new CredentialsResult();

            CredentialsProvider credentialsProvider = new CredentialsProvider();

            string encryptedPassword = CipherProvider.EncryptPassword(password);

            var loginTracingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LoginTracingEnabled"]);

            /*if (loginTracingEnabled)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Information,
                    Title = "Tracing user login",
                    Message = string.Format("User attempting to login with Email {0} and Password {1}", email, encryptedPassword),
                };
                Logger.Write(logEntry);
            }*/

            var userId = credentialsProvider.CanUserLogin(email, encryptedPassword);

            if (userId > 0)
            {
                credentialsResult.Result = CredentialsStatus.Success;

                var user = credentialsProvider.GetUser(email);


                int sessionLength = Convert.ToInt32(ConfigurationManager.AppSettings["LoginSessionLength"]);
                DateTime timeLimit = DateTime.UtcNow.AddMinutes(sessionLength);
                UserToken token = new UserToken(userId,timeLimit, user.Type);

                CreateAuthenticationCookie(token);


               /* if (loginTracingEnabled)
                {
                    LogEntry logEntry = new LogEntry()
                    {
                        Severity = System.Diagnostics.TraceEventType.Information,
                        Title = "Tracing user login",
                        Message = string.Format("User logged in successfully with Email {0} and Password {1}", email, encryptedPassword),
                    };
                    Logger.Write(logEntry);
                }*/
            }

            return Json(credentialsResult, JsonRequestBehavior.AllowGet);
        }
    }
}