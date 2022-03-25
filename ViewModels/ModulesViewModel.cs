using HostCord.BotModules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class ModulesViewModel
    {
        private string _moduleName;
        public string moduleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                moduleDisplayName = Char.ToUpper(value[0]).ToString() + value.Substring(1) + "\nModule";
            }
        }

        private string _moduleDisplayName;
        public string moduleDisplayName
        {
            get { return _moduleDisplayName; }
            set
            {
                _moduleDisplayName = value;
            }
        }

        public ModulesViewModel(string moduleName)
        {
            this.moduleName = moduleName;
        }
    }
}