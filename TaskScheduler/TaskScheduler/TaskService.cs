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

        // Taskファイル読み込み
        public void ReadTaskFile(string dataSourcePath)
        {
            this.dataSourcePath = dataSourcePath;
            StreamReader sr = new StreamReader(dataSourcePath);
            string str = sr.ReadToEnd();
            sr.Close();
            deserializedTasks = JsonConvert.DeserializeObject<Tasks>(str, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }

        // Taskの更新
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

        // 現在保持しているタスクのIDが最も大きい値を計算
        public int GetMaxId()
        {
            var tasks = deserializedTasks.tasks;
            List<int> Ids = new List<int>();
            foreach (Task task in tasks)
            {
                Ids.Add(task.id);
            }
            Ids.Reverse();
            return Ids[0];
        }

        // 1つのIDによって１つのタスクを取得
        public Task GetTaskById(int id)
        {
            var tasks = deserializedTasks.tasks;
            Task matchedTask = null;
            foreach (Task task in tasks)
            {
                if (id == task.id)
                {
                    matchedTask = task;
                    break;
                }
            }
            return matchedTask;
        }

        // 複数のIDによって複数のタスクを取得
        public List<Task> GetTasksByIds(List<int> ids)
        {
            var tasks = deserializedTasks.tasks;
            List<Task> targetTasks = new List<Task>();      // whichi is for delete.

            // if current task contains selected task's id, update taegetTasks.
            foreach (int id in ids)
            {
                foreach (Task task in tasks)
                {
                    if (id == task.id)
                    {
                        targetTasks.Add(task);
                    }
                }
            }
            return targetTasks;
        }


        // Taskの追加
        public void AddTask(Task addedTask)
        {
            deserializedTasks.tasks.Add(addedTask);
        }

        // Taskの削除
        public void RemoveTasks(List<Task> targetTasks)
        {
            foreach (Task targetTask in targetTasks)
            {
                deserializedTasks.tasks.Remove(targetTask);
            }
        }

        // Taskの完了/未完了切り替え
        public void InversionTasks(List<int> ids)
        {
            foreach(var id in ids)
            {
                foreach (Task task in deserializedTasks.tasks)
                {
                    if (id == task.id)
                    {
                        task.isDone = !task.isDone;
                    }
                }
            }
        }

        public void SaveTask()
        {
            string json = JsonConvert.SerializeObject(deserializedTasks);
            StreamWriter writer = new StreamWriter(dataSourcePath,false);
            writer.Write(json);
            writer.Close();
        }
    }
}

