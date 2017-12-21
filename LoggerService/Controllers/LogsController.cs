using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LoadersAndLogic;
using System.Threading.Tasks;
using ErrorLoggerModel;

namespace LoggerService.Controllers
{
    public class LogsController : ApiController
    {
        //public string AddLog([FromBody]ErrorLog newLogFile)
        [HttpPost]
        public string AddLog(ErrorLog newLogFile)
        {
            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            ErrorModel context = new ErrorModel();
            Application app = context.Applications.FirstOrDefault(x => x.appId == newLogFile.Application.appId);

            if (dataSource.AddErrorLog(newLogFile))
            {
                return String.Format("Log file added:\n\tApplication: " + app.appName 
                    + "\n\tApp Status: " + app.appStatus + "\n\tErrorLog FileName: " + newLogFile.fileName);
            }
            else
            {
                return String.Format("Failed to add Log File for:\n\tApplication: " + app.appName
                    + "\n\tApp Status: " + app.appStatus);
            }                
        }

        [HttpGet]
        public String ViewLog(int id)
        {
            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            ErrorLog data = dataSource.GetLogByFileName(id);

            String log = data.fileName + "," + data.logType.typeName;

            return log;

            //return data;
        }

        // GET: api/Logs
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Logs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Logs
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Logs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Logs/5
        public void Delete(int id)
        {
        }
    }
}
