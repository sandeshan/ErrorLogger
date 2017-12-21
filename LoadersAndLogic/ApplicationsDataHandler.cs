using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;

namespace LoadersAndLogic
{
    public class ApplicationsDataHandler
    {
        private static List<Application> appData;

        public ApplicationsDataHandler()
        {

            using (ErrorModel db = new ErrorModel())
            {
                db.Database.Initialize(false);
                db.Applications.Count();
            }

            using (ErrorModel context = new ErrorModel())
            {
                //appData = context.Applications.ToList();
                appData = context.Applications.Include(m => m.debugLevel).ToList();
                appData = context.Applications.Include(m => m.Users).ToList();
                appData = context.Applications.Include(m => m.ErrorLogs).ToList();
            }
        }

        public ICollection<Application> GetAllApps()
        {
            return appData;
        }

        public List<Application> GetAppsForUser(List<int> appIDs)
        {
            List<Application> apps = new List<Application>();

            foreach (int id in appIDs)
                apps.AddRange(appData.Where(x => x.appId == id));

            return apps;
        }

        public Application GetAppById(int id)
        {
            Application app = null;

            if (appData.Any(x => x.appId == id))
            {
                app = appData.Single(x => x.appId == id);
            }

            return app;
        }

        public bool DeleteApp(int id)
        {
            bool result = false;
            Application app = null;

            ErrorModel db = new ErrorModel();
            app = db.Applications.Single(x => x.appId == id);

            foreach (ErrorLog er in db.ErrorLogs)
            {
                if (er.Application.appId == app.appId)
                    db.ErrorLogs.Remove(er);
            }

            if (app != null)
            {
                try
                {
                    db.Applications.Remove(app);
                    db.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }                
            }

            return result;
        }

        public bool EditAppUsers(Application modifiedApp, int[] userID)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {
                var entry = context.Applications.SingleOrDefault(e => e.appId == modifiedApp.appId);
                if (entry != null)
                {
                    entry.Users.Clear();
                    foreach (int id in userID)
                    {
                        User user = context.Users.SingleOrDefault(e => e.userID == id);
                        entry.Users.Add(user);                    
                    }

                    entry.Users.Add(context.Users.SingleOrDefault(e => e.userType.userTypeID == 1));

                    context.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

        public bool AddApplication(Application appToSave)
        {
            bool result = false;

            using (ErrorModel context = new ErrorModel())
            {
                Application newApp = new Application()
                {
                    appName = appToSave.appName,
                    appType = appToSave.appType,
                    appStatus = "enabled"
                };

                newApp.debugLevel = context.DebugLevels.First(x => x.debugDescription == appToSave.debugLevel.debugDescription);
                newApp.Users = context.Users.Where(x => x.userType.userTypeID == 1).ToList();

                context.Applications.Add(newApp);
                context.SaveChanges();

                result = true;
            }

            return result;
        }

    }
}
