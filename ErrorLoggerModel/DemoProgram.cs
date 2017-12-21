using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    class DemoProgram
    {
        /// <summary>
        /// Entry point into the app
        /// </summary>
        static void Main(string[] args)
        {
            // Do cleanup first, so I don't have to manually delete them..
            //DBHandler.DeleteDB();

            #region Demonstrate Code First

            Console.WriteLine("****** Code first Database Demo ******");

            // Create the DB
            DBHandler.CreateDB();

            // Doing this twice to prove that the many-to-many relationship works both ways via navigation properties,
            // even though when we seeded it only through Courses
            DBHandler.PullOutDataByApplication();
            DBHandler.PullOutDataByErrorLog();

            //Console.ReadLine();
            //DBHandler.InsertApplication();
            //Console.ReadLine();
            //DBHandler.DeleteApplication();

            #endregion

            Console.ReadLine();
        }
    }
}
