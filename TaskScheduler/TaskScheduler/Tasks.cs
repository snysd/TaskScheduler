using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskScheduler
{
    public class Tasks
    {
        public List<Task> tasks;
    }
    public class Task
    {
        public int id;
        public string taskName;
        public string discription;
        public DateTime dueDate;
        public bool isDone;
    }
}
