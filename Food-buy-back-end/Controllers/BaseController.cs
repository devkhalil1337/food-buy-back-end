using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Food_buy_back_end.Providers;
using Food_buy_back_end.Models;
using System.Linq;
using System;
using System.Configuration;
using System.IO;

namespace Food_buy_back_end.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
//            Logger.SetLogWriter(new LogWriterFactory().Create(), false);
        }

        protected void CreateAuthenticationCookie(UserToken token)
        {
            string encryptedCode = CipherProvider.EncryptPassword(token.GetToken());

            var authCookie = HttpContext.Response.Cookies.AllKeys.FirstOrDefault(k => k == MvcApplication.AUTHENTICATION_COOKIE);

            if (string.IsNullOrWhiteSpace(authCookie))
            {
                HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(MvcApplication.AUTHENTICATION_COOKIE, encryptedCode));
            }
            else
            {
                HttpContext.Response.Cookies.Set(new System.Web.HttpCookie(MvcApplication.AUTHENTICATION_COOKIE, encryptedCode));
            }
        }

        protected void RemoveAuthenticationCookie()
        {

            DateTime timeLimit = DateTime.UtcNow.AddMinutes(-10);
            var token = new UserToken(-99, timeLimit, AppUser.AccountType.Driver);
            CreateAuthenticationCookie(token);
        }

        protected string GetProfileImagePath(long userId)
        {
            string htmlFilePath;

            try
            {
                var imageFolderPath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["ServerImagesPath"], userId.ToString()));

                //Check if any image files exist.
                DirectoryInfo dirInfo = new DirectoryInfo(imageFolderPath);
                if (dirInfo.Exists)
                {
                    var existingFiles = dirInfo.GetFiles();

                    if (existingFiles != null && existingFiles.Length > 0)
                    {
                        //Get the existing image file.
                        var fileName = existingFiles[0].Name;
                        htmlFilePath = string.Format(ConfigurationManager.AppSettings["ServerImagesDisplayPath"], userId, fileName);
                    }
                    else
                    {
                        //TODO: Check the database in case the files exist on dropbox.

                        //Return the default image.
                        htmlFilePath = GetDefaultProfileImagePath();
                    }
                }
                else
                {
                    //TODO: Check the database in case the files exist on dropbox.

                    //Return the default image.
                    htmlFilePath = GetDefaultProfileImagePath();
                }
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Could not find image for UserId {0}. {1}", userId, ex.Message),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);

                htmlFilePath = GetDefaultProfileImagePath();
            }

            return htmlFilePath;
        }

        protected string GetDefaultProfileImagePath()
        {
            return ConfigurationManager.AppSettings["DefaultProfilePicPath"];
        }

        protected UserToken Token
        {
            get
            {
                return (UserToken)HttpContext.Items[MvcApplication.AUTHENTICATION_COOKIE];
            }
        }

        protected long? UserID
        {
            get
            {
                if (Token != null)
                {
                    return Token.UserId;
                }
                return null;
            }
        }
    }
}