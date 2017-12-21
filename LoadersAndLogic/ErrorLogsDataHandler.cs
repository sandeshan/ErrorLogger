using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;

namespace LoadersAndLogic
{
    public class ErrorLogsDataHandler
    {
        private static List<ErrorLog> errorLogsData;

        public ErrorLogsDataHandler()
        {

            using (ErrorModel db = new ErrorModel())
            {
                db.Database.Initialize(false);                
                db.ErrorLogs.Count();
                
            }

            using (ErrorModel context = new ErrorModel())
            {
                errorLogsData = context.ErrorLogs.Include(m => m.Application).ToList();
                errorLogsData = context.ErrorLogs.Include(m => m.logType).ToList();
            }
        }

        public ICollection<ErrorLog> GetAllErrorLogs()
        {
            return errorLogsData;
        }

        public ErrorLog GetLogByFileName(int id)
        {
            ErrorLog errorLog = null;

            if (errorLogsData.Any(x => x.logID == id))
            {
                errorLog = errorLogsData.Single(x => x.logID == id);
            }

            return errorLog;
        }

        public List<ErrorLog> GetLogsForUser(List<int> appIDs)
        {
            List<ErrorLog> errorLogs = new List<ErrorLog>();

            foreach (int id in appIDs)
                errorLogs.AddRange(errorLogsData.Where(x => x.Application.appId == id));

            return errorLogs;
        }

        public bool EditErrorLogType(ErrorLog logFileToEdit)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {

                var entry = context.ErrorLogs.SingleOrDefault(e => e.logID == logFileToEdit.logID);
                if (entry != null)
                {
                    entry.logType = context.LogTypes.First(x => x.typeName == logFileToEdit.logType.typeName); //logFileToEdit.logType;
                    context.SaveChanges();

                    result = true;
                }                
            }

            return result;
        }

        public bool AddErrorLog(ErrorLog logFileToSave)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {
                Application app = context.Applications.FirstOrDefault(x => x.appId == logFileToSave.Application.appId);

                if (app.appStatus == "enabled")
                {
                    ErrorLog newLog = new ErrorLog()
                    {
                        fileName = logFileToSave.fileName,
                        timeStamp = DateTime.Now
                    };

                    newLog.Application = context.Applications.First(x => x.appId == logFileToSave.Application.appId);
                    newLog.logType = context.LogTypes.First(x => x.typeID == logFileToSave.logType.typeID);

                    context.ErrorLogs.Add(newLog);
                    context.SaveChanges();

                    result = true;
                }

                string s = app.appStatus;
            }

            return result;
        }

    }
}
