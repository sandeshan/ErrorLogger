using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;
using LoadersAndLogic;
using System.Net.Http.Formatting;
using System.Web.UI;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RestInteraction
{
    public static class RestInteraction
    {
        //static HttpClient client = new HttpClient();

        #region Constants for the REST Service

        private static int SERVICE_PORT = 41412; //40919;
        private static string SERVICE_URL = "http://localhost:{0}/";
        private static string ADD_LOG = "ErrorLogs/AddLog"; //"api/Logs/AddLog"; 
        private static string VIEW_LOG = "ErrorLogs/ViewLog?name={0}"; //"api/Logs/ViewLog?filename={0}";

        #endregion

        public static string AddErrorLog(ErrorLog newLogFile)
        {
            HttpClient client = new HttpClient();
            string result = string.Empty;

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@"http://localhost/CSE686_LoggerService/" + "api/Logs/AddLog"),
                Method = HttpMethod.Post,
            };

            request.Content = new ObjectContent<ErrorLog>(newLogFile, new JsonMediaTypeFormatter());

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response =  client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> task = response.Content.ReadAsAsync<string>();  // returns immediately
                string temp = task.Result;  // blocks until task completes

                Console.WriteLine("HTTP Message OK!");
                result = temp;
            }
            else
            {
                result = string.Format("ERROR: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                HttpError error = response.Content.ReadAsAsync<HttpError>().Result;
            }

            return result;
        }

        public static String ViewErrorLog(String filename)
        {
            string result = string.Empty;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
            HttpResponseMessage response = client.GetAsync(String.Format(VIEW_LOG, filename)).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<String> task = response.Content.ReadAsStringAsync();  // returns immediately
                string temp = task.Result;  // blocks until task completes

                result = "Error Log Added" + temp.ToString();
            }
            else
            {
                result = string.Format("ERROR: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }
    }
}
