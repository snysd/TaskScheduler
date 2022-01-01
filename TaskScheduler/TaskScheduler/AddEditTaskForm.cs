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

        public AddEditTaskForm()
        {
            InitializeComponent();
        }

        private void AddEditTaskForm_Load(object sender, EventArgs e)
        {
            taskName.Text = targetTask.taskName;
            taskDescription.Text = targetTask.discription;
            dueDatePicker.Value = targetTask.dueDate;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            targetTask.taskName = taskName.Text;
            targetTask.discription = taskDescription.Text;
            targetTask.dueDate = dueDatePicker.Value;
            this.Close();
        }
    }
}
