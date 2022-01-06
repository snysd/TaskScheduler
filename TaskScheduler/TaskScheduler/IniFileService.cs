using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace TaskScheduler
{
    class IniFileService
    {
        public string iniFilePath;
        TaskService taskService = new TaskService();
        public Tasks ReadIniFile()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);
            string dataSourcePath = data["DataSource"]["DataSourcePath"];

            taskService.dataSourcePath = dataSourcePath;
            var deserializedTasks = taskService.ReadTaskFile();
            return deserializedTasks;
        }

        public void SaveTasks(Tasks deserializedTasks)
        {
            taskService.SaveTask(deserializedTasks);
        }
        
    }
}