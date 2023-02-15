using Newtonsoft.Json.Linq;
using ProcessFile;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.IO;

namespace fsutil.Tasks
{
    internal class TaskGetResults : ITask
    {
        private string apiKey;
        private string serverURL;
        private string csvFile;

        //****************************************************************************************************************************************************
        ///                                         Task Description & Initialization
        //****************************************************************************************************************************************************


        public string Name => "results";

        public string HelpDescription => "Get the last 200 results that have been posted to the given account.  Write them to a csv file";

        public string ExpectedSyntax => "results --apikey %APIKEY% --output %csvFile%";


        //****************************************************************************************************************************************************
        ///                                         Task Operations
        //****************************************************************************************************************************************************

        public void Initialize(Dictionary<string, string> config, Dictionary<string, string> parameters)
        {
            this.apiKey = TaskUtil.GetTaskValue(parameters, "apikey");
            this.csvFile = TaskUtil.GetTaskValue(parameters, "output");
            this.serverURL = TaskUtil.GetTaskValue(config, "serverUrl");
        }

        private void PrintHeader()
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-50}", "Issue", "Verdict", "File");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
        }


        private void PrintResult(ScanResult scanResult)
        {
            bool hasIssue = false;
            string newFileName = scanResult.filename;

            if (newFileName.Length > 47)
            {
                newFileName = newFileName.Substring(0, 47) + "...";
            }

            if(scanResult.verdict.Contains("malicious"))
            {
                hasIssue = true;
            }


            if (hasIssue)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-50}", "*", scanResult.verdict, newFileName);
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-50}", "", scanResult.verdict, newFileName);
            }
        }



        private string DumpResultsToCSV(List<ScanResult> scanResultList)
        {
            StringBuilder csvFileString = new StringBuilder();

            //csvFileString.AppendLine("Name,Verdict,State,Date,Detail URL,Sha256,Id,Flow ID");
            csvFileString.AppendLine("Name,Verdict,File Type,Date,Detail URL,Sha256,Id,Flow ID");

            foreach (ScanResult current in scanResultList)
            {
                csvFileString.AppendLine(current.GetCSVString());
            }

            return csvFileString.ToString();
        }

        private DateTime GetDateTimeFromFS(string dateString)
        {
            DateTime result = DateTime.Now;





            return result;
        }



        private bool ParseResults(string jsonResult, List<ScanResult> resultList)
        {
            bool result = false;
            JObject parsedJSON = JObject.Parse(jsonResult);

            JArray itemsArray = (JArray)parsedJSON["items"];

            foreach (JObject currentItem in itemsArray)
            {
                result = true;
                ScanResult newResult = new ScanResult();
                newResult.filename = (string)currentItem["file"]["name"];
                newResult.sha256 = (string)currentItem["file"]["sha256"];
                newResult.fileType = (string)currentItem["file"]["short_type"];
                newResult.id = (string)currentItem["id"];
                newResult.state = (string)currentItem["state"];
                newResult.verdict = (string)currentItem["verdict"];
                newResult.flow_id = (string)currentItem["scan_init"]["id"];
                newResult.date = (DateTime)currentItem["date"];

                //
                // Create the URL
                //
                newResult.detailsURL = serverURL + "/uploads/" + newResult.flow_id + "/reports/" + newResult.id + "/overview";

                PrintResult(newResult);
                resultList.Add(newResult);
            }

            return result;
        }




        public string GetUploadResults(int pageNumber)
        {
            string requestUrl = serverURL + "/api/users/me/uploads?page=" + pageNumber + "&page_size=20";
            string result = "";

            //ComponentResposne reference = null;
            using (var httpClient = new HttpClient())
            {
                //
                // Setup the header with the API key and specify the json
                //
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");


                //
                // POST the file to the filescan server
                //
                var response = httpClient.GetAsync(requestUrl).Result;

                string responseText = response.StatusCode + ":" + response.ReasonPhrase;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception("Failed to get results on page: " + pageNumber + "  " + response.StatusCode + ":" + response.ReasonPhrase);
                }

            }

            // return URI of the created resource.
            return result;
        }



        public void Run()
        {

            bool stillResults = true;
            int count = 1;
            List<ScanResult> result = new List<ScanResult>();

            while (stillResults)
            {

                //
                // This method will make the call to the Filescan server
                //
                string currentResponseJSON = GetUploadResults(count);
                if (currentResponseJSON != null)
                {
                    stillResults = ParseResults(currentResponseJSON, result);
                }
                else
                {
                    stillResults = false;
                }

                if (count >= 10)
                {
                    stillResults = false;
                }

                count++;
            }


            string fileResults = DumpResultsToCSV(result);
            File.WriteAllText(csvFile, fileResults);
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Retrieved total of: " + result.Count);

            int maliciousCount = 0;
            foreach(ScanResult current in result)
            {
                if(current.verdict.Contains("malicious"))
                {
                    maliciousCount++;
                }
            }

            Console.WriteLine("Found malicious file count: " + maliciousCount);
            Console.WriteLine("File results have been written to: " + csvFile);
        }
    }
}
