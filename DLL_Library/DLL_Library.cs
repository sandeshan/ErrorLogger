using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;
using RestInteraction;

namespace DLL_Library
{
    public class DLL_Library
    {
        public DLL_Library()
        {

        }

        public void generateLog()
        {
            Random rnd = new Random();

            int[] appIDs = { 1, 2, 3, 1003, 1005 };

            for (int i = 0; i < 5; i++)
            {
                Application app = new Application()
                {
                    appId = appIDs[rnd.Next(5)] //1005 //rnd.Next(1, 4)
                };

                logType lt = new logType()
                {
                    typeID = rnd.Next(1, 6)
                };

                ErrorLog newLog = new ErrorLog();
                newLog.fileName = "logfile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                newLog.Application = app;
                newLog.logType = lt;

                string result = RestInteraction.RestInteraction.AddErrorLog(newLog);

                Console.WriteLine(result);
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "CSE 686 - DLL Library for Error Logger";
            Console.WriteLine("\n  //--------------------------------------------------------------------//");
            Console.WriteLine("  // Error Logger - DLL Library                                         //");
            Console.WriteLine("  // CSE 686 - Internet Programming : Final Project                     //");
            Console.WriteLine("  // Name: Sandesh Ashok Naik                                           //");
            Console.WriteLine("  // SUID: 394450563                                                    //");
            Console.WriteLine("  //--------------------------------------------------------------------//");

            Console.WriteLine("\nDLL Library Starting up..\n");

            DLL_Library lib = new DLL_Library();
            lib.generateLog();

            Console.WriteLine("Press key to exit..");
            Console.ReadLine();
        }
    }
}
