using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Food_buy_back_end.Models
{
    public class AppUser
    {
        private const string USER_ID_COLUMN = "Id";
        private const string USER_EMAIL_COLUMN = "Email";
        private const string USER_PASSWORD_COLUMN = "Password";
        private const string USER_ACCOUNT_TYPE_COLUMN = "AccountType";
        
        public enum AccountType
        {
            Admin = 1,
            Driver,
            System
        }

        public long? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountType Type { get; set; }

        public static AppUser ExtractObject(DataRow dataRow)
        {
            var appUser = new AppUser();
            appUser.Id = dataRow[USER_ID_COLUMN] == DBNull.Value ? null : (long?)Convert.ToInt64(dataRow[USER_ID_COLUMN]);
            appUser.Email = Convert.ToString(dataRow[USER_EMAIL_COLUMN]);
            appUser.Password = Convert.ToString(dataRow[USER_PASSWORD_COLUMN]);
            appUser.Type = (AppUser.AccountType)Convert.ToInt32(dataRow[USER_ACCOUNT_TYPE_COLUMN]);
            return appUser;
        }
    }
}