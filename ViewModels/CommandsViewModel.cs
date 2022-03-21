using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class CommandsViewModel
    {
        private string _commandName;
        public string commandName
        {
            get { return _commandName; }
            set
            {
                _commandName = value;
            }
        }
 
        private string _commandSummary;
        public string commandSummary
        {
            get { return _commandSummary; }
            set
            {
                _commandSummary = value;
            }
        }

        public CommandsViewModel(string commandName, string commandSummary)
        {
            this.commandName = commandName;
            this.commandSummary = commandSummary;
        }
    }
}
