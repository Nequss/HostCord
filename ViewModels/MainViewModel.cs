﻿using System;
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

namespace HostCord.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Home homePage;
        Modules modulesPage;
        Settings settingsPage;
        Logs logsPage;
        Help helpPage;

        BotConfigViewModel botConfigViewModel;

        Bot bot = new Bot();

        public ICommand SwitchPowerCommand { get; set; }
        public ICommand SwitchHomeCommand { get; set; }
        public ICommand SwitchModulesCommand { get; set; }
        public ICommand SwitchSettingsCommand { get; set; }
        public ICommand SwitchLogsCommand { get; set; }
        public ICommand SwitchHelpCommand { get; set; }

        public MainViewModel()
        {
            botConfigViewModel = new BotConfigViewModel(ref bot);

            homePage = new Home(ref bot);
            modulesPage = new Modules(ref botConfigViewModel);
            settingsPage = new Settings(ref botConfigViewModel);
            logsPage = new Logs(ref bot);
            helpPage = new Help();

            activeFrameContent = homePage;

            SwitchPowerCommand    = new RelayCommand(SwitchPower);
            SwitchHomeCommand     = new RelayCommand(SwitchHome);
            SwitchModulesCommand  = new RelayCommand(SwitchModules);
            SwitchSettingsCommand = new RelayCommand(SwitchSettings);
            SwitchLogsCommand     = new RelayCommand(SwitchLogs);
            SwitchHelpCommand     = new RelayCommand(SwitchHelp);
        }

        private void SwitchPower(object obj)
        {
            bot.MainAsync("NDUyNTQxMzIyNjY3MjI5MTk0.WxLj8w.7fKEktD-IWOCmGraaRsvDAs88D4");
        }

        private void SwitchHome(object obj)     => activeFrameContent = homePage;
        private void SwitchModules(object obj)  => activeFrameContent = modulesPage;
        private void SwitchSettings(object obj) => activeFrameContent = settingsPage;
        private void SwitchLogs(object obj)     => activeFrameContent = logsPage;
        private void SwitchHelp(object obj)     => activeFrameContent = helpPage;

        private Page _activeFrameContent;
        public Page activeFrameContent
        {
            get { return _activeFrameContent; }
            set
            {
                _activeFrameContent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
