using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TaskScheduler
{
    class TaskService
    {
        public string dataSourcePath;

        public void ReadTaskFile()
        {
            StreamReader sr = new StreamReader(dataSourcePath);
            string str = sr.ReadToEnd();
            sr.Close();
            Tasks deserializedTasks = JsonConvert.DeserializeObject<Tasks>(str,
                   new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }


    }
}
