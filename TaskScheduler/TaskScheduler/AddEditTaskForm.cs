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
    public partial class AddEditTaskForm : Form
    {
        public Task targetTask;
        public bool AddForm = true;
        public int maxId;
        public AddEditTaskForm()
        {
            InitializeComponent();
        }
        private void AddEditTaskForm_Load(object sender, EventArgs e)
        {
            if (AddForm == false)
            {
                taskName.Text = targetTask.taskName;
                taskDescription.Text = targetTask.discription;
                dueDatePicker.Value = targetTask.dueDate;
                //初期値設定
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(AddForm == true)
            {
                targetTask = new Task();
                targetTask.isDone = false;
                targetTask.id = maxId + 1;
            }
            targetTask.taskName = taskName.Text;
            targetTask.discription = taskDescription.Text;
            targetTask.dueDate = dueDatePicker.Value;
            this.Close();
        }
    }
}
