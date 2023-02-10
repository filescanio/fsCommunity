using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace fsutil.Tasks
{
    internal class TaskCreateInvite : ITask
    {
        private string secret;
        private string serverURL;


        //****************************************************************************************************************************************************
        ///                                         Task Description & Initialization
        //****************************************************************************************************************************************************


        public string Name => "createinvite";

        public string HelpDescription => "Create an invite for the server that can be shared with customers";

        public string ExpectedSyntax => "results --secret %secret%";


        //****************************************************************************************************************************************************
        ///                                         Task Operations
        //****************************************************************************************************************************************************

        public void Initialize(string serverURL, Dictionary<string, string> parameters)
        {
            this.secret = TaskUtil.GetTaskValue(parameters, "secret");
            //this.serverURL = serverURL;
            this.serverURL = "https://51.195.89.2:22022";
        }

        public void Run()
        {

            string requestUrl = serverURL + "/invite";
            string result = "";

            //
            // NOTE this is Dangerous code and should only be used
            // when you know the site
            //
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };


            //ComponentResposne reference = null;
            using (var httpClient = new HttpClient(handler))
            {
                //
                // Setup the header with the API key and specify the json
                //
                httpClient.DefaultRequestHeaders.Add("secret", secret);

                //
                // POST the file to the filescan server
                //
                var response = httpClient.GetAsync(requestUrl).Result;

                string responseText = response.StatusCode + ":" + response.ReasonPhrase;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);

                    if(File.Exists("invitelink"))
                    {
                        File.Delete("invitelink");
                    }

                    File.WriteAllText("invitelink", result);
                    Console.WriteLine("Link has also been written to file invitelink");
                }
                else
                {
                    throw new Exception("Failed to generate an invite link. " + response.StatusCode + ":" + response.ReasonPhrase);
                }

            }
        }
    }
}
