using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Food_buy_back_end.Models
{
    public class BusinessInfo
    {
        public const string BUSINESS_ID_COLUMN = "BusinessId";
        public const string BUSINESS_NAME_COLUMN = "BusinessName";

        public Int64 BusinessId { get; set; }
        public string BusinessName { get; set; }

        public static BusinessInfo ExtractObject(DataRow dataRow)
        {
            var newObject = new BusinessInfo();
            newObject.BusinessId = Convert.ToInt64(dataRow[BUSINESS_ID_COLUMN]);
            newObject.BusinessName = dataRow[BUSINESS_NAME_COLUMN].ToString();
            return newObject;
        }
    }
}