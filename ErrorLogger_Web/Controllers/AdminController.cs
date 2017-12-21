using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using LoadersAndLogic;
using ErrorLoggerModel;
using System.Web.UI.WebControls;
using CSE686_FinalProject.Models;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CSE686_FinalProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "ADMIN")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Administration Page.";            
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult Applications(string sortOrder, string searchString)
        {
            ViewBag.Message = "Create/Edit/View all Applications.";
            ViewBag.AppIDSortParam = String.IsNullOrEmpty(sortOrder) ? "appID_desc" : "";
            ViewBag.AppNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Enable", Value = "0" });
            items.Add(new SelectListItem { Text = "Disable", Value = "1" });

            ViewBag.EnabledList = items;

            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            ICollection<Application> data = dataSource.GetAllApps();

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.appName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "appID_desc":
                    data = data.OrderByDescending(s => s.appId).ToList();
                    break;
                case "Name":
                    data = data.OrderBy(s => s.appName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(s => s.appName).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.appId).ToList();
                    break;
            }

            ModelState.Clear();

            return View(data);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Applications(string sortOrder, string searchString, Application app, string EnabledList)
        {
            ViewBag.Message = "Create/Edit/View all Applications.";
            ViewBag.AppIDSortParam = String.IsNullOrEmpty(sortOrder) ? "appID_desc" : "";
            ViewBag.AppNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Enable", Value = "0" });
            items.Add(new SelectListItem { Text = "Disable", Value = "1" });

            ViewBag.EnabledList = items;

            if (app.appId != 0)
            {
                try
                {
                    using (ErrorModel db = new ErrorModel())
                    {
                        Application appToSave = db.Applications.FirstOrDefault(m => m.appId == app.appId);
                        if (EnabledList == "1")
                        {
                            appToSave.appStatus = "disabled";
                        }
                        else
                        {
                            appToSave.appStatus = "enabled";
                        }
                        db.SaveChanges();
                    }
                }

                catch (Exception ex)
                {
                    string s = ex.InnerException.Message;
                }
            }

            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            ICollection<Application> data = dataSource.GetAllApps();

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.appName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "appID_desc":
                    data = data.OrderByDescending(s => s.appId).ToList();
                    break;
                case "Name":
                    data = data.OrderBy(s => s.appName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(s => s.appName).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.appId).ToList();
                    break;
            }

            ModelState.Clear();

            return View(data);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult ViewApp(int id)
        {
            ViewBag.Message = "Application Details.";

            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            Application data = dataSource.GetAppById(id);

            return View(data);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult DeleteApp(int id)
        {
            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            bool result = dataSource.DeleteApp(id);

            return RedirectToAction("Applications");
            //return View();
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult EditApp(int id)
        {
            using (ErrorModel db = new ErrorModel())
            {
                ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
                Application data = dataSource.GetAppById(id);

                var users = db.Users.Select(u => new
                {
                    userID = u.userID,
                    firstName = u.firstName + " " + u.lastName
                }).ToList();

                ViewBag.Users = new MultiSelectList(users, "userID", "firstName");

                return View(data);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult EditApp(Application modifiedApp, int[] userID)
        {
            int[] userList = userID;
            using (ErrorModel db = new ErrorModel())
            {
                var users = db.Users.Select(u => new
                {
                    userID = u.userID,
                    firstName = u.firstName
                }).ToList();

                ViewBag.Users = new MultiSelectList(users, "userID", "firstName");

                ICollection<User> users1 = new List<User>();

                foreach (int id in userList)
                {
                    User user = db.Users.SingleOrDefault(e => e.userID == id);
                    users1.Add(user);
                }

                modifiedApp.Users = users1;

                if (ModelState.IsValid)
                {
                    ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
                    dataSource.EditAppUsers(modifiedApp, userID);

                    return RedirectToAction("ViewApp", new { id = modifiedApp.appId });
                }

                return View(modifiedApp);

            }
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult AddApp()
        {
            ErrorModel db = new ErrorModel();
            var appTypes = db.DebugLevels.ToList();

            List<SelectListItem> items = new List<SelectListItem>();
            //int i = 0;
            foreach (DebugLevel debug in appTypes )
            {
                items.Add(new SelectListItem { Text = debug.debugDescription, Value = debug.debugID.ToString() });
            }

            ViewBag.DebugList = items;

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult AddApp(Application newApp, string DebugList)
        {
            ErrorModel db = new ErrorModel();
            var appTypes = db.DebugLevels.ToList();

            List<SelectListItem> items = new List<SelectListItem>();
            //int i = 0;
            foreach (DebugLevel debug in appTypes)
            {
                items.Add(new SelectListItem { Text = debug.debugDescription, Value = debug.debugID.ToString() });
            }

            ViewBag.DebugList = items;

            int debugID = int.Parse(DebugList);

            newApp.debugLevel = db.DebugLevels.FirstOrDefault(x => x.debugID == debugID);
            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            dataSource.AddApplication(newApp);

            return RedirectToAction("Applications");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public ActionResult ErrorLogs(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Message = "View all ErrorLogs.";
            ViewBag.LogIDSortParam = String.IsNullOrEmpty(sortOrder) ? "logID_desc" : "";
            ViewBag.LogNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.LogTypeSortParam = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.LogTimeSortParam = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LogAppSortParam = sortOrder == "App" ? "app_desc" : "App";

            using (ErrorModel db = new ErrorModel())
            {
                ICollection<ErrorLog> data = populateLogs();

                var app = new { appID = 0, appName = "All Applications" };

                var apps = db.Applications.Select(u => new
                {
                    appID = u.appId,
                    appName = u.appName
                }).ToList();

                apps.Insert(0, app);

                ViewBag.Apps = new SelectList(apps, "appID", "appName");

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

                return View(data.ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult ErrorLogs(int appID, string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Message = "View all ErrorLogs.";
            ViewBag.LogIDSortParam = String.IsNullOrEmpty(sortOrder) ? "logID_desc" : "";
            ViewBag.LogNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.LogTypeSortParam = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.LogTimeSortParam = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.LogAppSortParam = sortOrder == "App" ? "app_desc" : "App";

            int selectedID = appID;

            using (ErrorModel db = new ErrorModel())
            {
                ICollection<ErrorLog> data = populateLogs();

                var app = new { appID = 0, appName = "All Applications" };

                var apps = db.Applications.Select(u => new
                {
                    appID = u.appId,
                    appName = u.appName
                }).ToList();

                apps.Insert(0, app);

                ViewBag.Apps = new SelectList(apps, "appID", "appName");

                if (selectedID != 0)
                {
                    data = data.Where(x => x.Application.appId == selectedID).ToList();
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

                return View(data.ToPagedList(pageNumber, pageSize));
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public ActionResult GraphView()
        {
            using (ErrorModel db = new ErrorModel())
            {
                ICollection<Application> allApps = db.Applications.ToList();

                var apps = new List<string>();
                var logsPerApp = new List<int>();
                var colors = new List<string>();
                int tCount = 0;

                var months = new List<string>();
                var logsPerMonth = new List<int>();
                var colors2 = new List<string>();
                string eColor = "rgba(76, 175, 80, 0.6)";
                string dColor = "rgba(244, 67, 54, 0.6)";

                foreach (Application ap in allApps)
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

                foreach (ErrorLog er in db.ErrorLogs)
                {
                    if (!months.Contains("\"" + er.timeStamp.ToString("MMMM") + "\""))
                        months.Add("\"" + er.timeStamp.ToString("MMMM") + "\"");
                }

                foreach (string m in months)
                {
                    int count = 0;
                    foreach (ErrorLog er in db.ErrorLogs)
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

        private static ICollection<ErrorLog> populateLogs()
        {
            ErrorLogsDataHandler dataSource = new ErrorLogsDataHandler();
            ICollection<ErrorLog> data = dataSource.GetAllErrorLogs();

            return data;
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult Users(string sortOrder, string searchString)
        {
            ViewBag.Message = "View/Edit all Users.";
            ViewBag.UserFNameSortParam = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.UserLNameSortParam = sortOrder == "LName" ? "lname_desc" : "LName";
            ViewBag.UserMailSortParam = sortOrder == "Mail" ? "mail_desc" : "Mail";

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Active", Value = "0" });
            items.Add(new SelectListItem { Text = "Inactive", Value = "1" });

            ViewBag.ActiveList = items;

            UsersDataHandler dataSource = new UsersDataHandler();
            ICollection<User> data = dataSource.GetAllUsers();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.firstName.Contains(searchString) || s.lastName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "fname_desc":
                    data = data.OrderByDescending(s => s.firstName).ToList();
                    break;
                case "LName":
                    data = data.OrderBy(s => s.lastName).ToList();
                    break;
                case "lname_desc":
                    data = data.OrderByDescending(s => s.lastName).ToList();
                    break;
                case "Mail":
                    data = data.OrderBy(s => s.emailID).ToList();
                    break;
                case "mail_desc":
                    data = data.OrderByDescending(s => s.emailID).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.firstName).ToList();
                    break;
            }

            ModelState.Clear();

            return View(data);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public ActionResult Users(string sortOrder, string searchString, User user, string ActiveList)
        {
            ViewBag.Message = "View/Edit all Users.";
            ViewBag.UserFNameSortParam = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.UserLNameSortParam = sortOrder == "LName" ? "lname_desc" : "LName";
            ViewBag.UserMailSortParam = sortOrder == "Mail" ? "mail_desc" : "Mail";

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Active", Value = "0" });
            items.Add(new SelectListItem { Text = "Inactive", Value = "1" });

            ViewBag.ActiveList = items;
            
            ErrorModel context = new ErrorModel();
            User thisUser = context.Users.FirstOrDefault(x => x.emailID == user.emailID);

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var usertoSave = db.Users.ToList().FirstOrDefault(m => m.Email == thisUser.emailID);
                    if (ActiveList == "1")
                    {
                        usertoSave.LockoutEndDateUtc = DateTime.MaxValue;
                        thisUser.activeStatus = "inactive";
                        thisUser.userType = thisUser.userType;
                    }
                    else
                    {
                        usertoSave.LockoutEndDateUtc = null;
                        thisUser.activeStatus = "active";
                        thisUser.userType = thisUser.userType;
                    }
                    db.SaveChanges();
                }
                context.SaveChanges();
            }

            catch (Exception ex)
            {
                string s = ex.InnerException.Message;
            }

            UsersDataHandler dataSource = new UsersDataHandler();
            ICollection<User> data = dataSource.GetAllUsers();

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.firstName.Contains(searchString) || s.lastName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "fname_desc":
                    data = data.OrderByDescending(s => s.firstName).ToList();
                    break;
                case "LName":
                    data = data.OrderBy(s => s.lastName).ToList();
                    break;
                case "lname_desc":
                    data = data.OrderByDescending(s => s.lastName).ToList();
                    break;
                case "Mail":
                    data = data.OrderBy(s => s.emailID).ToList();
                    break;
                case "mail_desc":
                    data = data.OrderByDescending(s => s.emailID).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.firstName).ToList();
                    break;
            }

            ModelState.Clear();

            return View(data);
        }
    }
}
