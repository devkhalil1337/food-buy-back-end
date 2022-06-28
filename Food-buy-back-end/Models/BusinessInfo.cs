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
        public const string BUSINESS_LOCALIZATION_COLUMN = "Localization";
        public const string BUSINESS_LOGO_COLUMN = "BusinessLogo";
        public const string BUSINESS_NAME_COLUMN = "BusinessName";
        public const string BUSINESS_CONTACT_COLUMN = "BusinessContact";
        public const string BUSINESS_EMAIL_COLUMN = "BusinessEmail";
        public const string BUSINESS_ADDRESS_COLUMN = "BusinessAddress";
        public const string BUSINESS_POSTCODE_COLUMN = "BusinessPostcode";
        public const string BUSINESS_CITY_COLUMN = "BusinessCity";
        public const string BUSINESS_COUNTRY_COLUMN = "BusinessCountry";
        public const string BUSINESS_DETAILS_COLUMN = "BusinessDetails";
        public const string BUSINESS_LATITUDE_COLUMN = "BusinessLatitude";
        public const string BUSINESS_LONGITUDE_COLUMN = "BusinessLongitude";
        public const string BUSINESS_CURRENCY_COLUMN = "BusinessCurrency";
        public const string BUSINESS_WEBSITE_URL_COLUMN = "BusinessWebsiteUrl";
        public const string BUSINESS_TIME_CLOSE_COLUMN = "BusinessTempClose";
        public const string BUSINESS_TILL_DATE_COLUMN = "ClosetillDate";
        public const string BUSINESS_EXPIRY_DATE_COLUMN = "BusinessExpiryDate";
        public const string BUSINESS_CREATION_DATE_COLUMN = "CreationDate";
        public const string BUSINESS_UPDATE_DATE_COLUMN = "UpdateDate";
        public const string BUSINESS_DELETED_COLUMN = "Deleted";
        public const string BUSINESS_ACTIVE_COLUMN = "Active";

        public int BusinessId { get; set; }
        public string Localization { get; set; }
        public string BusinessLogo { get; set; }
        public string BusinessName { get; set; }
        public string BusinessContact { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessPostcode { get; set; }
        public string BusinessCity { get; set; }
        public string BusinessCountry { get; set; }
        public string BusinessDetails { get; set; }
        public string BusinessLatitude { get; set; }
        public string BusinessLongitude { get; set; }
        public string BusinessCurrency { get; set; }
        public string BusinessWebsiteUrl { get; set; }
        public bool BusinessTempClose { get; set; }
       
        public DateTime? TempCloseDate { get; set; }
        public string ClosetillDate
        {
            get
            {
                return TempCloseDate.HasValue ? TempCloseDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null;
            }

            set
            {
                if (value != null)
                {
                    TempCloseDate = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }

        public DateTime? ExpiryDate { get; set; }
        public string BusinessExpiryDate
        {
            get { return ExpiryDate.HasValue ? ExpiryDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null;}

            set
            {
                if (value != null)
                {
                    ExpiryDate = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }

        public DateTime? CreateDate { get; set; }
        public string CreationDate
        {
            get {
                return CreateDate.HasValue ? CreateDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null;
            }

            set
            {
                if (value != null)
                {
                    CreateDate = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }

        public DateTime? ModifyDate { get; set; }
        public string UpdateDate
        {
            get { return ModifyDate.HasValue ? ModifyDate.Value.ToString("yyyy-MM-dd hh:mm:ss") : null; }

            set
            {
                if (value != null)
                {
                    ModifyDate = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }

        public bool Deleted { get; set; }
        public bool Active { get; set; }
        

        public static BusinessInfo ExtractObject(DataRow dataRow)
        {
            var newObject = new BusinessInfo();
            newObject.BusinessId = Convert.ToInt32(dataRow[BUSINESS_ID_COLUMN]);
            newObject.Localization = Convert.ToString(dataRow[BUSINESS_LOCALIZATION_COLUMN]);
            newObject.BusinessLogo = Convert.ToString(dataRow[BUSINESS_LOGO_COLUMN]);
            newObject.BusinessName = Convert.ToString(dataRow[BUSINESS_NAME_COLUMN]);
            newObject.BusinessContact = Convert.ToString(dataRow[BUSINESS_CONTACT_COLUMN]);
            newObject.BusinessEmail = Convert.ToString(dataRow[BUSINESS_EMAIL_COLUMN]);
            newObject.BusinessAddress = Convert.ToString(dataRow[BUSINESS_ADDRESS_COLUMN]);
            newObject.BusinessPostcode = Convert.ToString(dataRow[BUSINESS_POSTCODE_COLUMN]);
            newObject.BusinessCity = Convert.ToString(dataRow[BUSINESS_CITY_COLUMN]);
            newObject.BusinessCountry = Convert.ToString(dataRow[BUSINESS_COUNTRY_COLUMN]);
            newObject.BusinessDetails = Convert.ToString(dataRow[BUSINESS_DETAILS_COLUMN]);
            newObject.BusinessLatitude = Convert.ToString(dataRow[BUSINESS_LATITUDE_COLUMN]);
            newObject.BusinessLongitude = Convert.ToString(dataRow[BUSINESS_LONGITUDE_COLUMN]);
            newObject.BusinessCurrency = Convert.ToString(dataRow[BUSINESS_CURRENCY_COLUMN]);
            newObject.BusinessWebsiteUrl = Convert.ToString(dataRow[BUSINESS_WEBSITE_URL_COLUMN]);
            newObject.BusinessTempClose = Convert.ToBoolean(dataRow[BUSINESS_TIME_CLOSE_COLUMN]);
            newObject.TempCloseDate = dataRow[BUSINESS_TILL_DATE_COLUMN] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dataRow[BUSINESS_TILL_DATE_COLUMN]);
            newObject.ExpiryDate = dataRow[BUSINESS_EXPIRY_DATE_COLUMN] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dataRow[BUSINESS_TILL_DATE_COLUMN]);
            newObject.CreateDate = dataRow[BUSINESS_CREATION_DATE_COLUMN] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dataRow[BUSINESS_TILL_DATE_COLUMN]);
            newObject.ModifyDate = dataRow[BUSINESS_UPDATE_DATE_COLUMN] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dataRow[BUSINESS_TILL_DATE_COLUMN]);
            newObject.Deleted = Convert.ToBoolean(dataRow[BUSINESS_DELETED_COLUMN]);
            newObject.Active = Convert.ToBoolean(dataRow[BUSINESS_ACTIVE_COLUMN]);
            
            return newObject;
        }
    }
}