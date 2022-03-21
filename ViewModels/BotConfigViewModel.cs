using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HostCord.ViewModels;
using HostCord.Commands;
using System.Windows.Input;
using System.Diagnostics;

namespace HostCord.ViewModels
{
    public class BotConfigViewModel
    {
        Bot _bot;

        private ObservableCollection<ModulesViewModel> _modulesViewModels = new ObservableCollection<ModulesViewModel>();
        public ObservableCollection<ModulesViewModel> modulesViewModels
        {
            get { return _modulesViewModels; }
        }

        private ObservableCollection<CommandsViewModel> _commandsViewModels = new ObservableCollection<CommandsViewModel>();
        public ObservableCollection<CommandsViewModel> commandsViewModels
        {
            get { return _commandsViewModels; }
        }

        public ICommand GenerateCommandsCommand { get; set; }
        public BotConfigViewModel(ref Bot bot)
        {
            GenerateCommandsCommand = new RelayCommand(GenerateCommands);

            _bot = bot;

            foreach (var module in _bot.GetModules())
                Application.Current.Dispatcher.BeginInvoke(()
                    => modulesViewModels.Add(new ModulesViewModel(module.Name + "", true)));
        }

        private void GenerateCommands(object obj)
        {
            commandsViewModels.Clear();

            foreach (var module in _bot.GetModules())
                if (module.Name == (string)obj)
                {
                    foreach (var command in module.Commands)
                        Application.Current.Dispatcher.BeginInvoke(()
                            => commandsViewModels.Add(new CommandsViewModel(command.Name, command.Summary)));
                    return;
                }
        }
    }
}
