using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSE686_FinalProject.Models;
using System.Web.Mvc;
using LoadersAndLogic;
using ErrorLoggerModel;
using Microsoft.AspNet.Identity;
using PagedList;

namespace CSE686_FinalProject.Controllers
{
    public class ErrorLogsController : Controller
    {
        /// <summary>
        /// Error Logs Home page. lists all available Error Logs
        /// </summary>
        /// <returns>Error Logs Home page</returns>
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string appString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LogIDSortParam = String.IsNullOrEmpty(sortOrder) ? "logID_desc" : "";
            ViewBag.LogNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.LogTypeSortParam = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.LogTimeSortParam = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LogAppSortParam = sortOrder == "App" ? "app_desc" : "App";

            if (appString != null)
                page = 1;
            else
                appString = currentFilter;

            ViewBag.CurrentFilter = appString;

            string user = User.Identity.GetUserName();

            UsersDataHandler userSource = new UsersDataHandler();
            List<int> appIDs = userSource.GetUserApps(user);

            using (ErrorModel db = new ErrorModel())
            {
                List<Application> apps = new List<Application>();

                foreach (int id in appIDs)
                    apps.AddRange(db.Applications.Where(x => x.appId == id));

                ViewBag.Apps = apps;
            }

            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            ICollection<ErrorLog> data = dataSource.GetLogsForUser(appIDs);

            if (!String.IsNullOrEmpty(appString))
            {
                data = data.Where(s => s.Application.appName.Contains(appString)).ToList();
            }

            switch (sortOrder)
            {
                case "logID_desc":
                    data = data.OrderByDescending(s => s.logID).ToList();
                    break;
                case "Name":
                    data = data.OrderBy(s => s.fileName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(s => s.fileName).ToList();
                    break;
                case "Type":
                    data = data.OrderBy(s => s.logType.typeName).ToList();
                    break;
                case "type_desc":
                    data = data.OrderByDescending(s => s.logType.typeName).ToList();
                    break;
                case "Time":
                    data = data.OrderBy(s => s.timeStamp).ToList();
                    break;
                case "time_desc":
                    data = data.OrderByDescending(s => s.timeStamp).ToList();
                    break;
                case "App":
                    data = data.OrderBy(s => s.Application.appName).ToList();
                    break;
                case "app_desc":
                    data = data.OrderByDescending(s => s.Application.appName).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.logID).ToList();
                    break;
            }

            ViewBag.TotalCount = data.Count;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ModelState.Clear();

            return View(data.ToPagedList(pageNumber, pageSize));
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult GraphView()
        {
            string user = User.Identity.GetUserName();

            UsersDataHandler userSource = new UsersDataHandler();
            List<int> appIDs = userSource.GetUserApps(user);

            using (ErrorModel db = new ErrorModel())
            {
                ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
                ICollection<ErrorLog> data = dataSource.GetLogsForUser(appIDs);

                List<Application> userApps = new List<Application>();

                foreach (int id in appIDs)
                    userApps.AddRange(db.Applications.Where(x => x.appId == id));

                ViewBag.Apps = userApps;

                var apps = new List<string>();
                var logsPerApp = new List<int>();
                var colors = new List<string>();
                int tCount = 0;

                var months = new List<string>();
                var logsPerMonth = new List<int>();
                var colors2 = new List<string>();
                string eColor = "rgba(76, 175, 80, 0.6)";
                string dColor = "rgba(244, 67, 54, 0.6)";

                foreach (Application ap in userApps)
                {
                    apps.Add("\"" + ap.appName + "\"");
                    logsPerApp.Add(ap.ErrorLogs.Count);
                    tCount += ap.ErrorLogs.Count;
                    if (ap.appStatus == "enabled")
                        colors.Add("\'" + eColor + "\'");
                    else
                        colors.Add("\'" + dColor + "\'");
                }

                ViewBag.labelList = string.Join(",", apps);
                ViewBag.dataList = string.Join(",", logsPerApp);
                ViewBag.colorList = string.Join(",", colors);
                ViewBag.totalCount = tCount;

                foreach (ErrorLog er in data)
                {
                    if (!months.Contains("\"" + er.timeStamp.ToString("MMMM") + "\""))
                        months.Add("\"" + er.timeStamp.ToString("MMMM") + "\"");
                }

                foreach (string m in months)
                {
                    int count = 0;
                    foreach (ErrorLog er in data)
                    {
                        if ("\"" + er.timeStamp.ToString("MMMM") + "\"" == m)
                            count++;
                    }
                    logsPerMonth.Add(count);
                    colors2.Add("\'" + eColor + "\'");
                }

                ViewBag.labelList2 = string.Join(",", months);
                ViewBag.dataList2 = string.Join(",", logsPerMonth);
                ViewBag.colorList2 = string.Join(",", colors2);

                return View();
            }
        }

        /// <summary>
        /// View details about selected ErrorLog
        /// </summary>
        /// <param logID="id">ErrorLog logID</param>
        /// <returns>ErrorLog Detail View</returns>
        [Authorize]
        public ActionResult ViewLog(int id)
        {
            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            ErrorLog data = dataSource.GetLogByFileName(id);

            return View(data);
        }

        #region Add

        [Authorize]
        public ActionResult AddLog()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLog(ErrorLog newLogFile)
        {
            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            dataSource.AddErrorLog(newLogFile);

            return RedirectToAction("Index");
        }

        #endregion
    }
}
