using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using File = System.IO.File;

namespace fsutil.Tasks
{
    internal class TaskScanFile : ITask
    {
        string apiKey;
        string serverURL;
        string scanFilePath;
        string isFirst;


        //****************************************************************************************************************************************************
        ///                                         Task Description & Initialization
        //****************************************************************************************************************************************************


        public string Name => "scanfile";

        public string HelpDescription => "Scan a single file and push it to the Filescan server";

        public string ExpectedSyntax => "scanfile --apikey %APIKEY% --input %FileToScan%";


        //****************************************************************************************************************************************************
        ///                                         Task Operations
        //****************************************************************************************************************************************************


        public void Initialize(string serverURL, Dictionary<string, string> parameters)
        {
            this.apiKey = TaskUtil.GetTaskValue(parameters, "apikey");
            this.serverURL = serverURL;
            this.scanFilePath = TaskUtil.GetTaskValue(parameters, "input");
            this.isFirst = TaskUtil.GetTaskValue(parameters, "isfirst", "true");
        }

        private string GetFlowID(string jsonResult)
        {
            string result = "";
            JObject parsedJSON = JObject.Parse(jsonResult);
            result = (string)parsedJSON["flow_id"];
            return result;
        }

        private void PrintHeader()
        {
            if (isFirst == "True")
            {
                Console.WriteLine("{0,-10} {1,-50} {2,-30}", "File", "Uploaded", "Flow ID/Reason");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            }
        }


        private void PrintResult(string fileName, string result)
        {
            string flowId = GetFlowID(result);
            string newFileName = fileName;


            if (newFileName.Length > 47)
            {
                newFileName = newFileName.Substring(0, 47) + "...";
            }

            Console.WriteLine("{0,-10} {1,-50} {2,-30}", "Uploaded", newFileName, flowId);
        }

        private void PrintError(string fileName, string reason)
        {
            string newFileName = fileName;

            if(newFileName.Length > 47)
            {
                newFileName = newFileName.Substring(0, 47) + "...";
            }
            
            Console.WriteLine("{0,-10} {1,-50} {2,-30}", "Failed", newFileName, reason);
        }

        private bool ValidateFile()
        {
            bool result = true;

            if (!File.Exists(scanFilePath))
            {
                throw (new Exception("File does not exist.  Check patch and syntax."));
            }

            // Max File size is 20 MB
            long MaxFileSize = 1024 * 1024 * 20;
            FileInfo fi = new FileInfo(scanFilePath);
            if (fi.Length > MaxFileSize)
            {
                PrintError(fi.Name, "File is greater than 20 MB Size: \"" + fi.Length / (1024 * 1024) + "\" MB\"");
                result = false;
            }

            return result;
        }


        public void Run()
        {
            string requestUrl = serverURL + "/api/scan/file";

            ///
            // Check to see if file is not too large and exists
            //
            if(!ValidateFile())
            {
                return;
            }


            string fileName = Path.GetFileName(scanFilePath);
            PrintHeader();

            //
            // Using the .Net client for posting the file
            //
            using (var httpClient = new HttpClient())
            {

                //
                // Setup the header with the API key and specify the json
                //
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
  
                
                //
                // Create the Multi-part form
                //
                MultipartFormDataContent form = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(scanFilePath));
                form.Add(fileContent, "file", fileName);


                //
                // POST the file to the filescan server
                //
                var response = httpClient.PostAsync(requestUrl, form).Result;
                string responseText = response.StatusCode + ":" + response.ReasonPhrase;



                //
                // Handle the response here
                //
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    PrintResult(fileName, response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    PrintError(fileName, response.ReasonPhrase);
                }
            }
        }
    }
}
