
namespace SampleLogger
{
    using System;
    using DLL_Library;

    public class Logger
    {
        int i;
        /// <summary>
        /// Constructor. Put whatever initialization code in here that you need
        /// </summary>
        public Logger()
        {
            i = 0;
        }

        /// <summary>
        /// This method is called by the test harness. So inside of it you should call your logger..
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <param name="logLevel">Error Log Level</param>
        /// <param name="number">Number</param>
        /// <param name="ex">Optional Exception</param>
        public void Log(string errorMessage, int logLevel, int number, Exception ex = null)
        {
            // this is a stub to allow us to do mean things....
            Console.WriteLine("Calling DLL Library: " + i++);
            Console.WriteLine();

            //DLL_Library lib = new DLL_Library();
            //lib.generateLog();
        }
    }
}
