using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class ModulesViewModel : INotifyPropertyChanged
    {
        private string _moduleName;
        public string moduleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
            }
        }

        private bool _selected;
        public bool selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Trace.WriteLine(selected);
                OnPropertyChanged();
            }
        }

        public ModulesViewModel(string moduleName, bool selected)
        {
            this.moduleName = moduleName;
            this.selected = selected;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
