using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Food_buy_back_end.Models
{
    public class CredentialsResult
    {
        public enum CredentialsStatus
        {
            UserDoesNotExist,
            Success,
            UserAlreadyExists,
            Failed
        }

        public CredentialsStatus Result { get; set; }
        public string AuthenticationToken { get; set; }

        public CredentialsResult()
        {
            AuthenticationToken = null;
            Result = CredentialsStatus.Failed;
        }
    }
}