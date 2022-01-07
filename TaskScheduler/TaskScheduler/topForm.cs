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

        AddEditTaskForm addEditTaskForm;
        TaskService taskService = new TaskService();

        public topForm()
        {
            // Iniファイルの読み込み
            IniFileService iniFileService = new IniFileService(IniPath);

            // Taskファイルの読み込み
            taskService.ReadTaskFile(iniFileService.iniData["DataSource"]["DataSourcePath"]);
            InitializeComponent();
        }

        // Top画面のロードイベント
        private void topForm_Load(object sender, EventArgs e)
        {
            // タスクリストの作成
            InitializeListView();
        }

        // ListViewの作成
        private void InitializeListView()
        {
            listViewTask.FullRowSelect = true;
            listViewTask.GridLines = true;
            //listViewTask.Sorting = SortOrder.Ascending;
            listViewTask.View = View.Details;

            listViewTask.Columns.Clear();
            string[] headers = new string[]{ "タスクID", "タスク", "説明", "期限" };

            // タスクリストのヘッダ作成
            foreach(var header in headers)
            {
                ColumnHeader columnHeader = new ColumnHeader();
                columnHeader.Text = header;
                listViewTask.Columns.Add(columnHeader);
            }

            // TaskServiceよりTask一覧取得
            var tasks = taskService.deserializedTasks.tasks;

            // タスク一覧作成
            listViewTask.Items.Clear();
            foreach (Task task in tasks)
            {
                string[] item = { task.id.ToString(), task.taskName, task.discription, task.dueDate.ToString("yyyy/MM/dd") };
                ListViewItem targetItem =  new ListViewItem(item);
                if (task.isDone == true)
                {
                    targetItem.BackColor = Color.DarkGray;
                }
                listViewTask.Items.Add(targetItem);
            }
        }

        // タスク編集時
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // 選択しているタスクがなかったら何もしない
            if (listViewTask.SelectedItems.Count == 0)
            {
                return;
            }

            // 編集対象は選択されている最初のタスク
            ListViewItem itemx = listViewTask.SelectedItems[0];

            // 選択されているタスクをID検索
            var tasks = taskService.deserializedTasks.tasks;
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

            // AddEditForm表示
            addEditTaskForm = new AddEditTaskForm();
            addEditTaskForm.targetTask = matchedTask;
            addEditTaskForm.FormClosed += EditTaskClosed;
            addEditTaskForm.AddForm = false;
            addEditTaskForm.ShowDialog();
        }

        // EditForm終了イベント
        private void EditTaskClosed(object sender, EventArgs e)
        {
            // ✕ボタンでEditFormが終了される可能性がある
            if (addEditTaskForm.targetTask == null) return;

            // TaskServiceのタスク一覧を編集対象タスクで更新
            Task editedTask = addEditTaskForm.targetTask;
            taskService.UpdateTask(editedTask);

            // タスク一覧へ変更内容を反映
            InitializeListView();
        }

        // Addボタンクリックイベント
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

        private void Inversion_Click(object sender, EventArgs e)
        {
            if (listViewTask.SelectedItems.Count == 0)
            {
                return;
            }
            SelectedListViewItemCollection itemx = listViewTask.SelectedItems;
            foreach (ListViewItem item in itemx)
            {
                foreach (Task task in deserializedTasks.tasks)
                {
                    if (item.Text == task.id.ToString())
                    {
                        task.isDone = !task.isDone;
                    }
                }
            }
            InitializeListView();
        }

        private void topForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            iniFileService.SaveTasks(deserializedTasks);
        }
    }
}
