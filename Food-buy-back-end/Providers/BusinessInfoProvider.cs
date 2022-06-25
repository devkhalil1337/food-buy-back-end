
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
            Dictionary<string, object> parameters = new Dictionary<string, object> {};

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

       /* public bool UpdateBooking(BusinessInfo bsinessInfo)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_UpdateBooking";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "bookingId", booking.Id},
                { "agencyId", booking.AgencyId },
                { "driverCode", booking.DriverCode },
                { "startDateTime", booking.StartDateTime },
                { "endDateTime", booking.EndDateTime },
                { "actualStartDateTime", booking.ActualStartDateTime },
                { "actualEndDateTime", booking.ActualEndDateTime },
                { "fromLatitude", booking.FromLatitude },
                { "fromLongitude", booking.FromLongitude },
                { "toLatitude", booking.ToLatitude },
                { "toLongitude", booking.ToLongitude },
                { "fromAddressId", booking.FromAddressId },
                { "toAddressId", booking.ToAddressId },
                { "customerId", booking.CustomerId },
                { "customerName", booking.CustomerName },
                { "statusId", booking.StatusId },
                { "statusChangedDateTime", statusChangedDateTime },
                { "statusComments", booking.StatusComments },
                { "syncStatus", booking.SyncStatus },
                { "deleteRow", booking.DeleteRow },
                {"IsActive",booking.isActive}
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
                    Title = string.Format("Updating Booking with Id: {0} for DriverCode: {1}", booking.Id, booking.DriverCode),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return false;
        }

        public long CreateBooking(Booking booking)
        {
            //GetBookings(10);
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_AddNewBooking";

            var statusChangedDateTime = DateTime.UtcNow;

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "bookingId", booking.Id},
                { "agencyId", booking.AgencyId },
                { "driverCode", booking.DriverCode },
                { "startDateTime", booking.StartDateTime },
                { "endDateTime", booking.EndDateTime },
                { "actualStartDateTime", booking.ActualStartDateTime },
                { "actualEndDateTime", booking.ActualEndDateTime },
                { "fromLatitude", booking.FromLatitude },
                { "fromLongitude", booking.FromLongitude },
                { "toLatitude", booking.ToLatitude },
                { "toLongitude", booking.ToLongitude },
                { "fromAddressId", booking.FromAddressId },
                { "toAddressId", booking.ToAddressId },
                { "customerId", booking.CustomerId },
                { "customerName", booking.CustomerName },
                { "statusId", booking.StatusId },
                { "statusChangedDateTime", statusChangedDateTime },
                { "statusComments", booking.StatusComments },
                { "syncStatus", booking.SyncStatus },
                {"IsActive",booking.isActive }
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
                    Title = string.Format("Creating New Booking for DriverCode: {0} with Agency Id: {1}", booking.DriverCode, booking.AgencyId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }


            return -1;
        }

        public Booking GetBookingDetails(long bookingId)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_GetBookingDetails";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "bookingId", bookingId }
            };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;

                var booking = Booking.ExtractObject(dataSet.Tables[0].Rows[0]);
                return booking;
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Getting Booking details for Id : {0}", bookingId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }

            return null;
        }

        public IList<Booking> GetBookings(long AgencyId)
        {
            List<Booking> GetBookings = new List<Booking>();

            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_GetAgencyBooking";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "AgencyId", AgencyId }
            };

            try
            {
                var dataSet = dataAccessProvider.ExecuteStoredProcedure(storedProcedureName, parameters);

                if (dataSet.Tables.Count < 1 || dataSet.Tables[0].Rows.Count < 1 || dataSet.Tables[0].Rows.Count < 1)
                    return null;
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    var booking = Booking.ExtractObject(dataRow);
                    GetBookings.Add(booking);
                }
            }
            catch (Exception ex)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Severity = System.Diagnostics.TraceEventType.Error,
                    Title = string.Format("Getting Booking details for Id : {0}", AgencyId),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }

            return GetBookings;
        }

        public bool DeleteBooking(Booking booking)
        {
            IDatabaseAccessProvider dataAccessProvider = new SqlDataAccess(_ConnectionString);
            var storedProcedureName = "sp_DeleteBooking";

            Dictionary<string, object> parameters = new Dictionary<string, object> {
                { "bookingId", booking.Id}
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
                    Title = string.Format("Deleting Booking with Id: {0} for DriverCode: {1}", booking.Id, booking.DriverCode),
                    Message = ex.Message + Environment.NewLine + ex.StackTrace
                };
                Logger.Write(logEntry);
            }

            return false;
        }*/
    }
}