using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace TaskScheduler
{
    public partial class topForm : Form
    {
        public string IniPath = @"..\..\Settings\Ini.ini";
        public Tasks deserializedTasks;
        AddEditTaskForm addEditTaskForm;
        public topForm()
        {
            var iniFileService = new IniFileService();
            iniFileService.iniFilePath = IniPath;
            deserializedTasks = iniFileService.ReadIniFile();
            InitializeComponent();
        }
        private void topForm_Load(object sender, EventArgs e)
        {
            InitializeListView();
        }
        private void InitializeListView()
        {
            listViewTask.FullRowSelect = true;
            listViewTask.GridLines = true;
            //listViewTask.Sorting = SortOrder.Ascending;
            listViewTask.View = View.Details;

            listViewTask.Columns.Clear();
            var columnTaskId = new ColumnHeader();
            var columnTask = new ColumnHeader();
            var columnDescription = new ColumnHeader();
            var columnDate = new ColumnHeader();
            columnTaskId.Text = "タスクID";
            columnTaskId.Width = 100;
            columnTask.Text = "タスク";
            columnTask.Width = 60;
            columnDescription.Text = "説明";
            columnDescription.Width = 140;
            columnDate.Text = "期限";
            columnDate.Width = 140;
            ColumnHeader[] colHeaderRegValue =
            { columnTaskId, columnTask,columnDescription,columnDate };
            listViewTask.Columns.AddRange(colHeaderRegValue);

            var tasks = deserializedTasks.tasks;
            listViewTask.Items.Clear();
            foreach (Task task in tasks)
            {
                string[] item = { task.id.ToString(), task.taskName, task.discription, task.dueDate.ToString("yyyy/MM/dd") };
                listViewTask.Items.Add(new ListViewItem(item));
            }
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewTask.SelectedItems.Count == 0)
            {
                return;
            }
            ListViewItem itemx = listViewTask.SelectedItems[0];
            var tasks = deserializedTasks.tasks;

            Task matchedTask = null;
            foreach (Task task in tasks)
            {
                if(itemx.Text == task.id.ToString())
                {
                    matchedTask = task;
                    break;
                }
            }
            if(matchedTask == null)
            {
                return;
            }
            addEditTaskForm = new AddEditTaskForm();
            addEditTaskForm.targetTask = matchedTask;
            addEditTaskForm.FormClosed += EditTaskClosed;
            addEditTaskForm.AddForm = false;
            addEditTaskForm.ShowDialog();
        }
        private void EditTaskClosed(object sender, EventArgs e)
        {
            if (addEditTaskForm.targetTask == null)
            {
                return;
            }
            Task editedTask = addEditTaskForm.targetTask;
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
            InitializeListView();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            addEditTaskForm = new AddEditTaskForm();
            addEditTaskForm.AddForm = true;
            addEditTaskForm.maxId = GetMaxId();
            addEditTaskForm.FormClosed += AddTaskClosed;
            addEditTaskForm.ShowDialog();
        }
        private int GetMaxId()
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
        private void AddTaskClosed(object sender, EventArgs e)
        {
            if (addEditTaskForm.targetTask == null)
            {
                return;
            }
            Task addedTask = addEditTaskForm.targetTask;
            deserializedTasks.tasks.Add(addedTask);
            InitializeListView();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewTask.SelectedItems.Count == 0)
            {
                return;
            }
            SelectedListViewItemCollection itemx = listViewTask.SelectedItems;
            var tasks = deserializedTasks.tasks;
            List<Task> targetTasks = new List<Task>();
            foreach (ListViewItem item in itemx)
            {
                foreach(Task task in tasks)
                {
                    if(item.Text == task.id.ToString())
                    {
                        targetTasks.Add(task);
                    }
                }
            }
            foreach(Task targetTask in targetTasks)
            {
                deserializedTasks.tasks.Remove(targetTask);
            }
            InitializeListView();
        }
    }
}
