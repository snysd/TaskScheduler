using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskScheduler
{
    public partial class topForm : Form
    {
        public string IniPath = @"..\..\Settings\Ini.ini";
        public Tasks deserializedTasks;
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
            AddEditTaskForm addEditTaskForm = new AddEditTaskForm();
            addEditTaskForm.ShowDialog();

        }
    }
}
