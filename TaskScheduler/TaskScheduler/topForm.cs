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
            if (listViewTask.SelectedItems.Count == 0) return;

            // 編集対象は選択されている最初のタスク
            ListViewItem itemx = listViewTask.SelectedItems[0];

            // 選択されているタスクをID検索
            var matchedTask = taskService.GetTaskById(int.Parse(itemx.Text));       // for explain: port to service class
            if (matchedTask == null) return;

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
            addEditTaskForm.maxId = taskService.GetMaxId();     // for explain: Porting to service class
            addEditTaskForm.FormClosed += AddTaskClosed;
            addEditTaskForm.ShowDialog();
        }


        // Addフォーム終了時イベント
        private void AddTaskClosed(object sender, EventArgs e)
        {
            // ✕ボタンでEditFormが終了される可能性がある
            if (addEditTaskForm.targetTask == null) return;

            // TaskServiceのタスク一覧更新
            taskService.AddTask(addEditTaskForm.targetTask);

            // 追加内容をリストに反映
            InitializeListView();
        }

        // Task削除ボタンクリックイベント
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // If no selected, do nothing.
            if (listViewTask.SelectedItems.Count == 0) return;

            // create target ids for remove
            SelectedListViewItemCollection itemx = listViewTask.SelectedItems;
            List<int> targetIds = new List<int>();
            foreach(ListViewItem item in itemx)
            {
                targetIds.Add(int.Parse(item.Text));
            }

            // get remove tasks
            var targetTasks = taskService.GetTasksByIds(targetIds);
            if (targetTasks == null || targetTasks.Count == 0) return;
            taskService.RemoveTasks(targetTasks);
            InitializeListView();
        }

        // Inversion incompleteness or completeness
        private void Inversion_Click(object sender, EventArgs e)
        {
            if (listViewTask.SelectedItems.Count == 0) return;
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
