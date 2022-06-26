using Food_buy_back_end.Models;
namespace Food_buy_back_end.Providers
{
    public class CipherProvider
    {
        //public const string PasswordKey = "1453567891121947";
        public const string PasswordKey = "1234567891123489";
        public const string PasswordResetKey = "1133557788002244";

        public static string EncryptPassword(string password)
        {
            return EncryptedString.EncryptString(password, PasswordKey);
        }

        public static string DecryptPassword(string password)
        {
            return EncryptedString.DecryptString(password, PasswordKey);
        }

        public static string EncryptResetCode(string password)
        {
            return EncryptedString.EncryptString(password, PasswordResetKey);
        }

        public static string DecryptResetCode(string password)
        {
            return EncryptedString.DecryptString(password, PasswordResetKey);
        }
    }
}