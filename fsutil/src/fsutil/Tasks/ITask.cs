using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsutil.Tasks
{
    internal interface ITask
    {
        string Name { get; }

        string HelpDescription { get; }
         
        string ExpectedSyntax { get; }

        void Initialize(Dictionary<string, string> config, Dictionary<string, string> parameters);
        
        void Run();

    }
}
