using fsutil.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ProcessFile
{
    class Program
    {
        private static string getPrettyJson(string json)
        {
            JObject o = JObject.Parse(json);
            string result = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            return result;
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine("");
            System.Console.WriteLine("");
            System.Console.WriteLine("To use the fslist utility pass in a command followed by parameters");
            System.Console.WriteLine("Ex: fslist %Task% %parameters%");

            System.Console.WriteLine("");
            TaskUtil.PrintTaskHelpDescripton();
        }


        public static string getServerURL()
        {
            string result = "";

            string configJSON = File.ReadAllText("serverConfig.json");

            JObject o = JObject.Parse(configJSON);
            result = (string)o["serverUrl"];
            return result;
        }




        static void Main(string[] args)
        {
            try
            {
                if(args.Length == 0)
                {
                    PrintUsage();
                    return;
                }


                string taskName = TaskUtil.GetTaskName(args);

                if(taskName == "help")
                {
                    PrintUsage();
                    return;
                }



                Dictionary<string, string> parameters = TaskUtil.GetInputParamters(args);
                string serverURL = getServerURL();

                if (taskName != null)
                {
                    ITask task = TaskUtil.GetTaskInstance(taskName, serverURL, parameters);

                    if (task != null)
                    {
                        //
                        //
                        // This is the code that will run the task
                        //
                        // To see samples of each task look under the Tasks folder and then the Run method for that task
                        //
                        task.Run();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Task entered.  Please check syntax and try again.");
                        Console.WriteLine("Run \"fsutil help\" for more details");
                    }
                }
                else
                {
                    PrintUsage();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine(e.Message);
            }

        }
        }
}
