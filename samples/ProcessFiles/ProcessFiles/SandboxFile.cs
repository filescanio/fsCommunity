using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ProcessFile
{
    public class SandboxFile
    {
        static HttpClient client = new HttpClient();
        private string serverURL;
        private string apikey;
        private string sourceDir;

        public string ServerURL { get => serverURL; set => serverURL = value; }
        public string ApiKey { get => apikey; set => apikey = value; }
        public string SourceDir { get => sourceDir; set => sourceDir = value; }

        public void intitialize()
        {
            string configJSON = File.ReadAllText("serverConfig.json");

            JObject o = JObject.Parse(configJSON);

            ServerURL = (string)o["serverUrl"];
            SourceDir = (string)o["sourceDir"];

        }


        public string ScanDir(string sourcePath)
        {
            StringBuilder endResult = new StringBuilder();
            if (String.IsNullOrEmpty(sourcePath))
            {
                sourcePath = sourceDir;
            }

            if(Directory.Exists(sourcePath) && Directory.GetFiles(sourcePath).Length > 0)
            {
                endResult.AppendLine("[");


                int count = 0;
                string[] directoryFiles = Directory.GetFiles(sourcePath);
                
                foreach(string file in directoryFiles)
                {
                    FileInfo fi = new FileInfo(file);

                    // Max File size is 20 MB
                    long MaxFileSize = 1024 * 1024 * 20;
                    if(fi.Length > MaxFileSize)
                    {
                        Console.WriteLine("File is greater than 20 MB: " + file + "   Size: " + fi.Length / (1024 * 1024) + " MB");
                        continue;
                    }
                 
                    if (count < 100)
                    {
                        string result = ScanFile(file);
                        if (result != null)
                        {
                            if (count > 0)
                            {
                                endResult.AppendLine(",");
                            }

                            endResult.Append(result);
                            count++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("This is restricted to only uploading 100 files");
                    }

 
                }
                endResult.AppendLine("");
                endResult.AppendLine("]");

            }
            else
            {
                Console.WriteLine("Directory does not exist or there are no files in the directory.  Please check the path and try again");
            }

            return endResult.ToString();
        }




        public string ScanFile(string filePath)
        {
            string requestUrl = ServerURL + "/scan/file";
            string result = null;

            //ComponentResposne reference = null;
            using (var httpClient = new HttpClient())
            {
                string fileName = Path.GetFileName(filePath);
                requestUrl = requestUrl + "?description=" + fileName;

                httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");


                MultipartFormDataContent form = new MultipartFormDataContent();
                ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                form.Add(new StringContent(fileName), "description");
                form.Add(fileContent, "file", fileName);


                //add file part
                var response = httpClient.PostAsync(requestUrl, form).Result;
                string responseText = response.StatusCode + ":" + response.ReasonPhrase;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Uploaded file: " + fileName);
                    result = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    System.Console.WriteLine("Failed to upload file: " + response.StatusCode + ":" + response.ReasonPhrase);
                }

                // return URI of the created resource.
                return result;
            }
        }

    }
}
