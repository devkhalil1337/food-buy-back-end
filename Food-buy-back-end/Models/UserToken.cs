using System;
using System.Text;
using static Food_buy_back_end.Models.AppUser;

namespace Food_buy_back_end.Models
{
    public class UserToken
    {
        private enum TokenItems
        {
            UserId,
            Expiry,
            AccountType
        }

        public long UserId { get; }
        public DateTime ExpiryDate { get; }
        public AccountType UserAccountType { get; }

        public UserToken(string token)
        {
            var items = token.Split(',');

            UserId = Convert.ToInt64(items[(int)TokenItems.UserId]);
            var date = Convert.ToString(items[(int)TokenItems.Expiry]);
            ExpiryDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            UserAccountType = (AccountType)Convert.ToInt32(items[(int)TokenItems.AccountType]);
        }

        public UserToken(long userId, DateTime expiry, AccountType userAccountType)
        {
            UserId = userId;
            ExpiryDate = expiry;
            UserAccountType = userAccountType;
        }

        public string GetToken()
        {
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(UserId);
            tokenBuilder.Append(",");
            tokenBuilder.Append(ExpiryDate.ToString("yyyy-MM-dd HH:mm"));
            tokenBuilder.Append(",");
            tokenBuilder.Append((int)UserAccountType);

            return tokenBuilder.ToString();
        }

        public UserToken GetRenewedToken(int sessionLength)
        {
            DateTime timeLimit = DateTime.UtcNow.AddMinutes(sessionLength);
            var newToken = new UserToken(UserId,timeLimit, UserAccountType);
            return newToken;
        }
    }
}