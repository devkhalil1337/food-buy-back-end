
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


        public long CreateBusinessUnit(BusinessInfo businessInfo)
        {
            //GetBookings(10);
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_AddNewBusinessUnit";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "BusinessName", businessInfo.BusinessName},
            };

            try
            {
                var id = dataAccessProvider.ExecuteStoredProcedureWithReturnObject(storedProcedureName, parameters);
                return id == null ? -1 : Convert.ToInt64(id);
            }
            catch (Exception ex)
            {
                /*                LogEntry logEntry = new LogEntry()
                                {
                                    Severity = System.Diagnostics.TraceEventType.Error,
                                    Title = string.Format("Creating New Booking for DriverCode: {0} with Agency Id: {1}", booking.DriverCode, booking.AgencyId),
                                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                                };
                                Logger.Write(logEntry);*/
            }


            return -1;
        }

        public bool UpdateBusinessUnit(BusinessInfo businessInfo)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_UpdateBusinesUnits";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                 { "BusinessId", businessInfo.BusinessId},
                 { "BusinessName", businessInfo.BusinessName },
             };

            try
            {
                var result = dataAccessProvider.ExecuteNonQueryStoredProcedure(storedProcedureName, parameters);
                return result;
            }
            catch (Exception ex)
            {
                /*LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Updating Booking with Id: {0} for DriverCode: {1}", booking.Id, booking.DriverCode),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);*/
            }


            return false;
        }



        public IList<BusinessInfo> GetBusinessUnitById(long BusinessId)
        {
            List<BusinessInfo> GetBusinessUnits = new List<BusinessInfo>();

            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "SP_GetBusinessUnitById";

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
                /*LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Getting Booking details for Id : {0}", AgencyId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }
            */
            }
            return GetBusinessUnits;

        }
        
           public bool DeleteBusinessUnit(long BusinessId)
           {
               IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
               var storedProcedureName = "SP_DeleteBusinesUnits";

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
                   /*LogEntry logEntry = new LogEntry()
                   {
                       Severity = System.Diagnostics.TraceEventType.Error,
                       Title = string.Format("Deleting Booking with Id: {0} for DriverCode: {1}", booking.Id, booking.DriverCode),
                       Message = ex.Message + Environment.NewLine + ex.StackTrace
                   };
                   Logger.Write(logEntry);*/
               }

               return false;
           }
    }
}