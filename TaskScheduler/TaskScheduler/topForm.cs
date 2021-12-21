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
        public topForm()
        {
            var iniFileService = new IniFileService();
            iniFileService.iniFilePath = IniPath;
            iniFileService.ReadIniFile();
            InitializeComponent();
        }

    }
}
