using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace fsutil.Tasks
{
    internal class TaskUtil
    {

        public static ITask GetTaskInstance(string taskName, string serverURL, Dictionary<string,string> parameters)
        {
            ITask result = null;

            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(ITask))))
            {
                ITask currentTask= Activator.CreateInstance(mytype) as ITask;
                
                //
                // Now initialize the task
                //
                if(currentTask.Name == taskName)
                {
                    currentTask.Initialize(serverURL, parameters);
                    result = currentTask;
                    break;
                }

                //do stuff
            }

            return result;
        }



        public static string PrintTaskHelpDescripton()
        {
            StringBuilder result = new StringBuilder();

            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(ITask))))
            {
                ITask currentTask = Activator.CreateInstance(mytype) as ITask;

                Console.WriteLine("{0,-15} {1,40}", currentTask.Name, currentTask.HelpDescription);
                Console.WriteLine("{0,-15} {1,40}", "", "Ex: " + currentTask.ExpectedSyntax);
                Console.WriteLine("");


                result.AppendLine(currentTask.HelpDescription);
                result.AppendLine();
            }

            return result.ToString();
        }



        public static string GetTaskValue(Dictionary<string, string> parameters, string taskName, string defaultValue)
        {
            string result = null;


            if (parameters.ContainsKey(taskName))
            {
                result = parameters[taskName];
            }
            else
            {
                if (String.IsNullOrEmpty(defaultValue))
                {
                    throw new Exception("Missing parameter: " + taskName);
                }

                result = defaultValue;
            }

            return result;
        }

        public static string GetTaskValue(Dictionary<string, string> parameters, string taskName)
        {
            return GetTaskValue(parameters, taskName, null);
        }


        internal static Dictionary<string, string> GetInputParamters(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (args.Length >= 2)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].StartsWith("--"))
                    {
                        string value = "";

                        if (!args[i + 1].StartsWith("--"))
                        {
                            value = args[i + 1];
                        }

                        result.Add(args[i].Substring(2), value);
                        i++;
                    }
                }
            }

            return result;
        }

        internal static string GetTaskName(string[] args)
        {
            string result = "";

            if (args.Length >= 1)
            {
                result = args[0];
            }
            else
            {
                throw new Exception("Task not found.  Please recheck and try again");
            }


            return result;
        }



    }
}
