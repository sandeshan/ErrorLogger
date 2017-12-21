using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoadersAndLogic;
using ErrorLoggerModel;
using Microsoft.AspNet.Identity;

namespace CSE686_FinalProject.Controllers
{
    public class ApplicationsController : Controller
    {
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.Message = "View your Applications";
            ViewBag.AppIDSortParam = String.IsNullOrEmpty(sortOrder) ? "appID_desc" : "";
            ViewBag.AppNameSortParam = sortOrder == "Name" ? "name_desc" : "Name";

            string user = User.Identity.GetUserName();

            UsersDataHandler userSource = new UsersDataHandler();
            List<int> appIDs = userSource.GetUserApps(user);

            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            ICollection<Application> data = dataSource.GetAppsForUser(appIDs);

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
        
        [Authorize]
        public ActionResult ViewApp(int id)
        {
            ViewBag.Message = "Application Details.";

            ApplicationsDataHandler dataSource = new ApplicationsDataHandler();
            Application data = dataSource.GetAppById(id);

            return View(data);
        }
    }
}