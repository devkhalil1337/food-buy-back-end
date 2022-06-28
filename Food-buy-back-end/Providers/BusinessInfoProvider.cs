
using Food_buy_back_end.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace Food_buy_back_end.Providers
{
    public class BusinessInfoProvider
    {


        string _ConnectionString;

        public BusinessInfoProvider()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["foodBuyConnectionString"].ConnectionString;
        }

        public IList<BusinessInfo> GetAllBusinesUnits()
        {
            List<BusinessInfo> businessUnits = new List<BusinessInfo>();

            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_GetAllBusinesUnits";
            Dictionary<string, object> parameters = new Dictionary<string, object> { };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;

                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    var businessInfo = new BusinessInfo();
                    businessInfo.BusinessId = Convert.ToInt64(dataRow[BusinessInfo.BUSINESS_ID_COLUMN]);
                    businessInfo.BusinessName = dataRow[BusinessInfo.BUSINESS_NAME_COLUMN].ToString().Replace(" ", ""); ;
                    businessUnits.Add(businessInfo);
                }
            }
            catch (Exception ex)
            {
                /* LogEntry logEntry = new LogEntry()
                 {
                     Severity = System.Diagnostics.TraceEventType.Error,
                     Message = ex.Message + Environment.NewLine + ex.StackTrace
                 };
                 Logger.Write(logEntry);*/
            }

            return businessUnits;
        }



        public long AddNewBusinessUnit (BusinessInfo businessInfo)
        {
            
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_AddNewBusiness";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
            

                { "Localization", businessInfo.Localization},
                { "BusinessName", businessInfo.BusinessName},
                { "BusinessContact", businessInfo.BusinessContact },
                { "BusinessEmail", businessInfo.BusinessEmail },
                { "BusinessAddress", businessInfo.BusinessAddress },
                { "BusinessCity", businessInfo.BusinessCity },
                { "BusinessCountry", businessInfo.BusinessCountry },
                { "BusinessDetails", businessInfo.BusinessDetails },
                { "BusinessLatitude", businessInfo.BusinessLatitude },
                { "BusinessLongitude", businessInfo.BusinessLongitude },
                { "BusinessCurrency", businessInfo.BusinessCurrency },
                { "BusinessWebsiteUrl", businessInfo.BusinessWebsiteUrl },
                { "BusinessTempClose", businessInfo.BusinessTempClose },
                { "ClosetillDate", businessInfo.ClosetillDate },
                { "BusinessExpiryDate", businessInfo.BusinessExpiryDate },
                { "CreationDate", businessInfo.CreationDate },
                { "UpdateDate", statusChangedDateTime },
                { "Deleted", businessInfo.Deleted },
                { "Active", businessInfo.Active },
            };

            try
            {
                var id = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);
                return id == null ? -1 : Convert.ToInt64(id);
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Creating New business Info for a customer ", businessInfo.BusinessName),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return -1;
        }
        public bool UpdateBusinessUnit(BusinessInfo businessInfo)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_UpdateBusinesInfo";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "BusinessId", businessInfo.BusinessId},
                { "Localization", businessInfo.Localization},
                { "BusinessName", businessInfo.BusinessName},
                { "BusinessContact", businessInfo.BusinessContact },
                { "BusinessEmail", businessInfo.BusinessEmail },
                { "BusinessAddress", businessInfo.BusinessAddress },
                { "BusinessCity", businessInfo.BusinessCity },
                { "BusinessCountry", businessInfo.BusinessCountry },
                { "BusinessDetails", businessInfo.BusinessDetails },
                { "BusinessLatitude", businessInfo.BusinessLatitude },
                { "BusinessLongitude", businessInfo.BusinessLongitude },
                { "BusinessCurrency", businessInfo.BusinessCurrency },
                { "BusinessWebsiteUrl", businessInfo.BusinessWebsiteUrl },
                { "BusinessTempClose", businessInfo.BusinessTempClose },
                { "ClosetillDate", businessInfo.ClosetillDate },
                { "BusinessExpiryDate", businessInfo.BusinessExpiryDate },
                { "CreationDate", businessInfo.CreationDate },
                { "UpdateDate", statusChangedDateTime },
                { "Deleted", businessInfo.Deleted },
                { "Active", businessInfo.Active },
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
                    Title = string.Format("Updating business Info with Id: {0} for Business Name: {1}", businessInfo.BusinessId, businessInfo.BusinessName),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return false;
        }



        public IList<BusinessInfo> GetBusinessUnitById(int BusinessId)
        {
            List<BusinessInfo> GetBusinessUnits = new List<BusinessInfo>();

            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_GetBusinessInfo";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                   { "BusinessId", BusinessId }
               };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    var busessInfo = BusinessInfo.ExtractObject(dataRow);
                    GetBusinessUnits.Add(busessInfo);
                }
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Getting business details for Id : {0}", BusinessId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }
            
            
            return GetBusinessUnits;

        }
        
           public bool DeleteBusinessUnit(long BusinessId)
           {
               IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
               var storedProcedureName = "SP_DeleteBusinessInfo";

               Dictionary<string, object> parameters = new Dictionary<string, object> {
                   { "BusinessId", BusinessId}
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
                       Title = string.Format("Deleting Business with Id: {0}", BusinessId),
                       Message = ex.Message + Environment.NewLine + ex.StackTrace
                   };
                   Logger.Write(logEntry);
               }

               return false;
           }
    }
}