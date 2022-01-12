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
        public IniData iniData;

        public IniFileService(string iniFilePath)
        {
            ReadIniFile(iniFilePath);   
        }

        public void ReadIniFile(string iniFilePath)
        {
            var parser = new FileIniDataParser();
            iniData = parser.ReadFile(iniFilePath);
        }
        
    }
}