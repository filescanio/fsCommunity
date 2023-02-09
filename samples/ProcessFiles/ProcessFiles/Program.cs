using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

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
            System.Console.WriteLine("There needs to be a file name specified in order to run this program");
            System.Console.WriteLine("Example: ProcessFile %filename%");
            System.Console.WriteLine("");
            System.Console.WriteLine("Note: The serverConfig.json will also need to be created to login to the MetaDefender Core");
        }

        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                PrintUsage();
            }

            string sourcePath = args[0];

            SandboxFile mdFile = new SandboxFile();
            mdFile.intitialize();
            string resultJson = mdFile.ScanDir(sourcePath);

            if(resultJson != null)
            {
                System.Console.WriteLine(resultJson);
                File.WriteAllText("UploadResult", resultJson);
            }

        }
    }
}
