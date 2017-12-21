using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class ErrorLogsDBInitializer : DropCreateDatabaseIfModelChanges<ErrorModel>
    {
        protected override void Seed(ErrorModel context)
        {
            Console.WriteLine(" ### Seeding ###");

            //log types and error logs

            logType logtype1 = new logType()
            {
                typeID = 1,
                typeName = "type1"
            };

            logType logtype2 = new logType()
            {
                typeID = 2,
                typeName = "type2"
            };

            ErrorLog errorlog1 = new ErrorLog()
            {
                logID = 1,
                fileName = "logfile1.txt",
                logType = logtype1,
                timeStamp = DateTime.Now
            };

            ErrorLog errorlog2 = new ErrorLog()
            {
                logID = 2,
                fileName = "logfile2.txt",
                logType = logtype2,
                timeStamp = DateTime.Now
            };

            //debug levels and applications

            DebugLevel debugLevel1 = new DebugLevel()
            {
                debugID = 1,
                debugDescription = "INFO"
            };

            DebugLevel debugLevel2 = new DebugLevel()
            {
                debugID = 2,
                debugDescription = "WARN"
            };

            Application app1 = new Application()
            {
                appId = 1,
                appName = "web-application",
                appType = "type_1",
                debugLevel = debugLevel1,
                ErrorLogs = new List<ErrorLog>() { errorlog1}
            };
            Application app2 = new Application
            {
                appId = 2,
                appName = "db-application",
                appType = "type_2",
                debugLevel = debugLevel2,
                ErrorLogs = new List<ErrorLog>() { errorlog2 }
            };

            //user types and users:
            UserType userType1 = new UserType()
            {
                userTypeID = 1,
                userType = "ADMIN"
            };

            UserType userType2 = new UserType()
            {
                userTypeID = 2,
                userType = "USER"
            };

            User user1 = new User()
            {
                userID = 1,
                firstName = "John",
                lastName = "Doe",
                emailID = "john_doe@gmail.com",
                userType = userType1,
                lastLoginDate = DateTime.Now,
                Applications = new List<Application>() { app1, app2 },
            };

            User user2 = new User()
            {
                userID = 1,
                firstName = "Jane",
                lastName = "Doe",
                emailID = "jane_doe@gmail.com",
                userType = userType2,
                lastLoginDate = DateTime.Now,
                Applications = new List<Application>() { app2 },
            };

            context.LogTypes.Add(logtype1);
            context.LogTypes.Add(logtype2);

            context.ErrorLogs.Add(errorlog1);
            context.ErrorLogs.Add(errorlog2);

            context.DebugLevels.Add(debugLevel1);
            context.DebugLevels.Add(debugLevel2);

            context.Applications.Add(app1);
            context.Applications.Add(app2);

            context.UserTypes.Add(userType1);
            context.UserTypes.Add(userType2);

            context.Users.Add(user1);
            context.Users.Add(user2);

            // letting the base method do anything it needs to get done
            base.Seed(context);

            // Save the changes you made, when adding the data above
            context.SaveChanges();
        }
    }
}
