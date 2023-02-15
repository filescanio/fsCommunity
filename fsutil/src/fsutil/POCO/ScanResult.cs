using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ProcessFile
{
    internal class ScanResult
    {
        public string id;
        public string filename;
        public string sha256;
        public string state;
        public string verdict;
        public DateTime date;
        public string flow_id;
        public string detailsURL;
        public string fileType;
        

        private string GetExcelDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetCSVString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(filename);
            result.Append(",");

            result.Append(verdict);
            result.Append(",");

            result.Append(fileType);
            result.Append(",");

            result.Append(GetExcelDate(date));
            result.Append(",");

            result.Append(detailsURL);
            result.Append(",");

            result.Append(sha256);
            result.Append(",");

            result.Append(id);
            result.Append(",");

            result.Append(flow_id);

            return result.ToString();
        }
    }



}
