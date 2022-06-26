
using Food_buy_back_end.Models;
using DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Food_buy_back_end.Providers
{
    public class CredentialsProvider
    {
        public enum UserStatus
        {
            DoesNotExist,
            ExistsWithPassword,
            ExistsButWrongPasswordSpecified
        }

        string _ConnectionString;

        public CredentialsProvider()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["foodBuyConnectionString"].ConnectionString;
        }

        public long CanUserLogin(string email, string password)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_CanUserSignIn";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "email", email },
                { "password", password }
            };

            try
            {
                var userId = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);

                return userId == null ? -1 : Convert.ToInt64(userId);

            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = "User Login",
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }

            return -1;
        }

        public bool DoesUserExist(string email)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_DoesUserExist";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "email", email }
            };

            try
            {
                var userId = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);

                return userId != null;

            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = "Check User Exists",
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return false;
        }

        public long CreateNewUser(AppUser user)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_SignUpNewUser";

            var joinDate = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "email", user.Email },
                { "password", user.Password },
                { "accountType", user.Type },
            };

            try
            {
                var userId = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);

                return userId == null ? -1 : Convert.ToInt64(userId);

            }
            catch (Exception ex)
            {
                /*LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = "Agency User SignUp",
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);*/
            }


            return -1;
        }

        public AppUser GetUser(string email)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_GetUserByEmail";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "email", email }
            };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;

                DataRow dataRow = dataSet.Tables[0].Rows[0];

                var appUser = AppUser.ExtractObject(dataRow);
                return appUser;
            }
            catch (Exception ex)
            {
                /*LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Get User with email: {0}", email),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);*/
            }


            return null;
        }

        public AppUser GetUser(long userId)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_GetUserById";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "userId", userId }
            };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;

                DataRow dataRow = dataSet.Tables[0].Rows[0];

                var appUser = AppUser.ExtractObject(dataRow);
                return appUser;
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Get User with id: {0}", userId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return null;
        }

        public AppUser GetDriver(string driverCode)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_GetUserByDriverCode";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "driverCode", driverCode }
            };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;

                DataRow dataRow = dataSet.Tables[0].Rows[0];

                var appUser = AppUser.ExtractObject(dataRow);
                return appUser;
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Get User with DriverCode: {0}", driverCode),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return null;
        }

        public string RemoveDashesFromCode(string codeWithDashes)
        {
            var result = new StringBuilder();

            foreach (char c in codeWithDashes)
            {
                if (c != '-')
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public string GenerateNewDriverCode()
        {
            //Generate new code
            string newCode = GetRandomCode(16);

            //Check if code is unique
            while (!IsDriverCodeUnique(newCode))
            {
                //Generate new code
                newCode = GetRandomCode(16);
            }

            return newCode;

        }

        public string GenerateNewAgencyCode()
        {
            //Generate new code
            string newCode = GetRandomCode(16);

            //Check if code is unique
            while (!IsAgencyCodeUnique(newCode))
            {
                //Generate new code
                newCode = GetRandomCode(16);
            }

            return newCode;

        }

        private string GetRandomCode(int length)
        {
            StringBuilder randomValue = new StringBuilder(GenerateRandomCode());

            while (randomValue.Length < length)
            {
                randomValue.Append(GenerateRandomCode());
            }

            string result = randomValue.ToString().Substring(0, length);

            return result;

        }

        private string GenerateRandomCode()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        public bool IsDriverCodeUnique(string driverCode)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_DoesDriverCodeExist";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "driverCode", driverCode }
            };

            try
            {
                var refId = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);

                return refId == null;

            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = "Checking if new Driver Code is unique",
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return false;
        }

        public bool IsAgencyCodeUnique(string code)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_DoesAgencyCodeExist";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "code", code }
            };

            try
            {
                var refId = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);

                return refId == null;

            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = "Checking if new Agency Code is unique",
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return false;
        }

        public bool RequestPasswordResetCode(string email)
        {
            try
            {
                var emailServerName = ConfigurationManager.AppSettings["EmailServerName"];
                var emailServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailServerPort"]);
                var emailAddress = ConfigurationManager.AppSettings["ProfileMyDriverFromEmailAddress"];
                var emailAddressPassword = ConfigurationManager.AppSettings["ProfileMyDriverFromEmailAddressPassword"];

                DateTime today = DateTime.UtcNow;
                DateTime tomorrow = today.AddDays(1);
                string code = string.Format("{0}£{1}£{2}", email, today.ToString("yyyyMMdd"), tomorrow.ToString("yyyyMMdd"));

                string encryptedCode = CipherProvider.EncryptResetCode(code);

                var client = new SmtpClient(emailServerName)
                {
                    Credentials = new NetworkCredential(emailAddress, emailAddressPassword),
                    EnableSsl = true
                };
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                StringBuilder body = new StringBuilder();
                body.Append("Hello,");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                body.Append("You have requested to reset your ProfileMyDriver® password. In order to reset your password, you will need to use the following code on the ProfileMyDriver® website.");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                body.AppendFormat("ProfileMyDriver® Reset Code: {0}", encryptedCode);

                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                body.Append("Yours sincerely,");
                body.Append(Environment.NewLine);
                body.Append(Environment.NewLine);

                body.Append("ProfileMyDriver® Admin Team");

                client.Send(emailAddress, email, "Password Reset Code", body.ToString());

                return true;
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Sending password reset code for User with Email: {0}", email),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);

                return false;
            }
        }

        public int CheckUserExists(string email, string password)
        {
            return (int)UserExists(email, password);
        }

        private UserStatus UserExists(string email, string password)
        {
            var user = GetUser(email);

            var returnValue = UserStatus.DoesNotExist;

            if (user != null)
            {
                if (user.Password == password)
                {
                    returnValue = UserStatus.ExistsWithPassword;
                }
                else
                {
                    returnValue = UserStatus.ExistsButWrongPasswordSpecified;
                }
            }

            return returnValue;
        }

        public bool UpdatePassword(string email, string newPassword, string resetCode)
        {
            try
            {
                string decryptedCode = CipherProvider.DecryptResetCode(resetCode);

                string[] parts = decryptedCode.Split('?');

                int fromDate = Convert.ToInt32(parts[1]);
                int toDate = Convert.ToInt32(parts[2]);

                int today = Convert.ToInt32(DateTime.UtcNow.ToString("yyyyMMdd"));

                if (today >= fromDate && today <= toDate)
                {
                    var userExists = UserExists(parts[0], "");

                    if (userExists == UserStatus.ExistsButWrongPasswordSpecified)
                    {
                        IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
                        var storedProcedureName = "sp_UpdateUserPassword";

                        Dictionary<string, object> parameters = new Dictionary<string, object> {
                            { "email", parts[0] },
                            { "newPassword", newPassword }
                        };

                        try
                        {
                            var result = dataAccessProvider.ExecuteNonQueryStoredProcedure(storedProcedureName, parameters);

                            return result;

                        }
                        catch (Exception ex)
                        {
                            LogEntry logEntry = new LogEntry()
                            {
                                Severity = System.Diagnostics.TraceEventType.Error,
                                Title = string.Format("Change Password for User: {0}", email),
                                Message = ex.Message + Environment.NewLine + ex.StackTrace
                            };
                            Logger.Write(logEntry);
                        }

                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
    }
}