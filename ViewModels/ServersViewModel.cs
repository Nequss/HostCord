using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using HostCord.View;
using System.Windows.Input;
using HostCord.Commands;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using System.Dynamic;
using System.Collections.ObjectModel;
using Discord.WebSocket;
using System.Threading;

namespace HostCord.ViewModels
{
    public class ServersViewModel
    {
        private string _url;
        public string url
        {
            get { return _url; }
            set
            {
                _url = value;
            }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public ServersViewModel(string url, string name)
        {
            this.url = url;
            this.name = name;
        }
    }
}
