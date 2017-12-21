using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorLoggerModel
{
    public class DBHandler
    {
        //private static List<ErrorModel> errorLogsData;
        public static void CreateDB()
        {
            Console.WriteLine("~~~~ Creating the DB ~~~~");
            Console.WriteLine();

            using (ErrorModel db = new ErrorModel())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                // Seeding runs the first time you try to use the DB, so we make it seed here..
                // It only runs IF the initializer condition is met, regardless of the True/False above
                db.ErrorLogs.Count();
            }
        }

        public static void DeleteDB()
        {
            Console.WriteLine("~~~~ Deleting the DB ~~~~");
            Console.WriteLine();

            using (ErrorModel db = new ErrorModel())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
            }
        }

        public static string SPACE = "   ";

        /// <summary>
        /// Gets the data grouped by Courses
        /// </summary>
        public static void PullOutDataByApplication()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by Application): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorModel context = new ErrorModel())
            {
                foreach (Application application in context.Applications.ToList())
                {
                    Console.WriteLine("\tApplication ID: {0}, Application Name: {1}, \n\tApplication Type: {2}, Debug Level: {3}",
                    application.appId, application.appName, application.appType, application.debugLevel.debugDescription);

                    Console.WriteLine("\t Related Error Logs: ");

                    foreach (ErrorLog log in application.ErrorLogs)
                    {
                        Console.WriteLine("\t Log Id: {0}, Name: {1}",
                            log.logID, log.fileName);
                        Console.WriteLine("\n");
                    }

                    Console.WriteLine("\t Related Users: ");

                    foreach (User user in application.Users)
                    {
                        Console.WriteLine("\t First Name: {0}, Last Name: {1}",
                            user.firstName, user.lastName);
                        Console.WriteLine("\n");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the data grouped by Applications
        /// </summary>
        public static void PullOutDataByErrorLog()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by error-log file): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorModel context = new ErrorModel())
            {
                foreach (ErrorLog errorlog in context.ErrorLogs.ToList())
                {
                    Console.WriteLine("\tErrorLog Id: {0}", errorlog.logID);
                    Console.WriteLine("\t  FileName: {0}", errorlog.fileName);
                    Console.WriteLine("\t  Log Type: {0}", errorlog.logType.typeName);
                    Console.WriteLine("\t  TimeStamp: {0}", errorlog.timeStamp);
                    Console.WriteLine("\t  source_application: {0}", errorlog.Application.appName);
                    Console.WriteLine("\n");
                }
            }
        }

        public static void PullOutDataByUsers()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by error-log file): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorModel context = new ErrorModel())
            {
                foreach (User user in context.Users.ToList())
                {
                    Console.WriteLine("\tUser Id: {0}", user.userID);
                    Console.WriteLine("\t  First Name: {0}", user.firstName);
                    Console.WriteLine("\t  Last Name: {0}", user.lastName);
                    Console.WriteLine("\t  E-Mail ID: {0}", user.emailID);
                    Console.WriteLine("\t  User Type: {0}", user.userType);
                    Console.WriteLine("\n");

                    Console.WriteLine("\t Application Access: ");

                    foreach (Application app in user.Applications)
                    {
                        Console.WriteLine("\t App ID: {0}, App Name: {1}",
                            app.appId, app.appName);
                        Console.WriteLine("\n");
                    }
                }
            }
        }
        /// <summary>
        /// Inserts a dummy student & repulls the data
        /// </summary>
        public static void InsertApplication()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Inserting a demo application ~~~~");

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorModel context = new ErrorModel())
            {
                Application newApp = new Application()
                {
                    appName = "test-app",
                    appType = "test"
                };

                //newApp.ErrorLogs = new List<ErrorLog>();
                //newApp.ErrorLogs.Add(context.ErrorLogs.First(x => x.logID == 1));

                context.Applications.Add(newApp);
                context.SaveChanges();
            }

            PullOutDataByApplication();
        }

        /// <summary>
        /// Deletes the dummy student & repulls the data
        /// </summary>
        public static void DeleteApplication()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Deleting the demo application ~~~~");

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorModel context = new ErrorModel())
            {
                Application app = context.Applications.First(x => x.appName == "test-app");

                context.Applications.Remove(app);
                context.SaveChanges();
            }

            PullOutDataByApplication();
        }
    }
}