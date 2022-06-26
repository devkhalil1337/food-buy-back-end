using Food_buy_back_end.Models;
using Food_buy_back_end.Providers;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Configuration;
using System.Web.Mvc;
using static Food_buy_back_end.Models.CredentialsResult;

namespace Food_buy_back_end.Controllers
{
    public class SignupController : BaseController
    {
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Create(string email, string password)
        {
            CredentialsResult credentialsResult = new CredentialsResult();

            CredentialsProvider credentialsProvider = new CredentialsProvider();

            var signUpTracingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SignUpTracingEnabled"]);

            /*if (signUpTracingEnabled)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Information,
                    Title = "Tracing user sign up",
                    Message = string.Format("User attempting to sign up with Email {0} and Password {1}", email, password),
                };
                Logger.Write(logEntry);
            }*/

            if (credentialsProvider.DoesUserExist(email))
            {
                credentialsResult.Result = CredentialsStatus.UserAlreadyExists;

                if (signUpTracingEnabled)
                {
                    LogEntry logEntry = new LogEntry()
                    {
                        Severity = System.Diagnostics.TraceEventType.Information,
                        Title = "Tracing user sign up",
                        Message = string.Format("User sign up failed with Email {0} and Password {1}, because email is already in use.", email, password),
                    };
                    Logger.Write(logEntry);
                }
            }
            else
            {
                string encryptedPassword = CipherProvider.EncryptPassword(password);


                AppUser newUser = new AppUser()
                {
                    Email = email,
                    Password = encryptedPassword,
                    Type = AppUser.AccountType.Admin
                };

                var userId = credentialsProvider.CreateNewUser(newUser);
                if (userId > 0)
                {
                    credentialsResult.Result = CredentialsStatus.Success;

                    var user = credentialsProvider.GetUser(email);


                    int sessionLength = Convert.ToInt32(ConfigurationManager.AppSettings["LoginSessionLength"]);
                    DateTime timeLimit = DateTime.UtcNow.AddMinutes(sessionLength);
                    UserToken token = new UserToken(userId,timeLimit, user.Type);

                    CreateAuthenticationCookie(token);

                    if (signUpTracingEnabled)
                    {
                        LogEntry logEntry = new LogEntry()
                        {
                            Severity = System.Diagnostics.TraceEventType.Information,
                            Title = "Tracing user sign up",
                            Message = string.Format("User signed up successfully with Email {0} and Password {1}", email, encryptedPassword),
                        };
                        Logger.Write(logEntry);
                    }
                }

            }

            return Json(credentialsResult, JsonRequestBehavior.AllowGet);
        }
    }
}