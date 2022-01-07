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
        string dataSourcePath;
        public Tasks deserializedTasks;

        public void ReadTaskFile(string dataSourcePath)
        {
            this.dataSourcePath = dataSourcePath;
            StreamReader sr = new StreamReader(dataSourcePath);
            string str = sr.ReadToEnd();
            sr.Close();
            deserializedTasks = JsonConvert.DeserializeObject<Tasks>(str, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }

        public void UpdateTask(Task editedTask)
        {
            var tasks = deserializedTasks.tasks;
            int i = 0;
            int matchedIndex = -1;
            foreach (Task task in tasks)
            {
                if (editedTask.id == task.id)
                {
                    matchedIndex = i;
                    break;
                }
                i++;
            }
            if (matchedIndex < 0)
            {
                return;
            }
            deserializedTasks.tasks[matchedIndex] = editedTask;

        }

        public void SaveTask(Tasks deserializedTasks)
        {
            string json = JsonConvert.SerializeObject(deserializedTasks);
            StreamWriter writer = new StreamWriter(dataSourcePath,false);
            writer.Write(json);
            writer.Close();
        }
    }
}
