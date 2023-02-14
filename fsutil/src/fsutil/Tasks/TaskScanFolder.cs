using System;
using System.Collections.Generic;
using System.IO;

namespace fsutil.Tasks
{
    internal class TaskScanFolder : ITask
    {
        string apiKey;
        string scanFolderPath;
        Dictionary<string, string> config;


        //****************************************************************************************************************************************************
        ///                                         Task Description & Initialization
        //****************************************************************************************************************************************************

        public string Name => "scanfolder";

        string ITask.HelpDescription => "Scans a specify folder for up to 100 files and pushes them to the Filescan folder.";

        string ITask.ExpectedSyntax => "scanfolder --apikey %APIKEY% --input %FolderToScan%";

        public void Initialize(Dictionary<string, string> config, Dictionary<string, string> parameters)
        {
            this.apiKey = TaskUtil.GetTaskValue(parameters,"apikey");
            this.scanFolderPath = TaskUtil.GetTaskValue(parameters,"input");
            this.config = config;
        }

        //****************************************************************************************************************************************************
        ///                                         Task Operations
        //****************************************************************************************************************************************************

        private Dictionary<string,string> GetScanParameters(string fileNamePath, bool isFirst)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add("apikey", apiKey);
            result.Add("input", fileNamePath);
            result.Add("isfirst", isFirst.ToString());

            return result;
        }

        //
        // Note all the code for this routine is found in TaskScanFile
        //
        // This is just using a inter-Task call to use that code
        //
        private void ScanFile(string fileNamePath, bool isFirst)
        {
            Dictionary<string, string> parameters = GetScanParameters(fileNamePath, isFirst);
            ITask scanTaskInstance = TaskUtil.GetTaskInstance("scanfile", config, parameters);
            scanTaskInstance.Run();
        }


        public void Run()
        {

            if(!Directory.Exists(scanFolderPath))
            {
                throw new Exception("Directory specified does not exist.  Check format input value and try again.");
            }

            int count = 0;
            string[] directoryFiles = Directory.GetFiles(scanFolderPath);

            foreach (string file in directoryFiles)
            {
                if (count < 100)
                {
                    bool isFirst = count == 0 ? true : false;
                    ScanFile(file,isFirst);
                }
                else
                {
                    Console.WriteLine("scanfolder is restricted to only uploading 100 files");
                }
                count++;
            }
        }

    }
}
