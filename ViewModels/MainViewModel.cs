using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using HostCord.View;
using System.Windows.Input;
using HostCord.Commands;
using HostCord.Utils;

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

        private string _powerUrl;
        public string powerUrl
        {
            get { return _powerUrl; }
            set
            {
                _powerUrl = value;
                OnPropertyChanged();
            }
        }

        private string _homeUrl;
        public string homeUrl
        {
            get { return _homeUrl; }
            set
            {
                _homeUrl = value;
                OnPropertyChanged();
            }
        }

        private string _modulesUrl;
        public string modulesUrl
        {
            get { return _modulesUrl; }
            set
            {
                _modulesUrl = value;
                OnPropertyChanged();
            }
        }

        private string _settingsUrl;
        public string settingsUrl
        {
            get { return _settingsUrl; }
            set
            {
                _settingsUrl = value;
                OnPropertyChanged();
            }
        }

        private string _logsUrl;
        public string logsUrl
        {
            get { return _logsUrl; }
            set
            {
                _logsUrl = value;
                OnPropertyChanged();
            }
        }

        private string _helpUrl;
        public string helpUrl
        {
            get { return _helpUrl; }
            set
            {
                _helpUrl = value;
                OnPropertyChanged();
            }
        }

        private string _exitUrl;
        public string exitUrl
        {
            get { return _exitUrl; }
            set
            {
                _exitUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand SwitchPowerCommand { get; set; }
        public ICommand SwitchHomeCommand { get; set; }
        public ICommand SwitchModulesCommand { get; set; }
        public ICommand SwitchSettingsCommand { get; set; }
        public ICommand SwitchLogsCommand { get; set; }
        public ICommand SwitchHelpCommand { get; set; }

        public MainViewModel()
        {
            PerformanceMonitor.getInstance().start();

            powerUrl    = @"/HostCord;component/Images/power.png";
            homeUrl     = @"/HostCord;component/Images/home.png";
            modulesUrl  = @"/HostCord;component/Images/extension.png";
            settingsUrl = @"/HostCord;component/Images/config.png";
            logsUrl     = @"/HostCord;component/Images/logs.png";
            helpUrl     = @"/HostCord;component/Images/info.png";
            exitUrl     = @"/HostCord;component/Images/exit.png";

            botConfigViewModel = new BotConfigViewModel(ref bot);
            
            homePage     = new Home(ref bot);
            modulesPage  = new Modules(ref botConfigViewModel);
            settingsPage = new Settings(ref botConfigViewModel);
            logsPage     = new Logs(ref bot);
            helpPage     = new Help();

            activeFrameContent = homePage;

            SwitchPowerCommand    = new RelayCommand(SwitchPower);
            SwitchHomeCommand     = new RelayCommand(SwitchHome);
            SwitchModulesCommand  = new RelayCommand(SwitchModules);
            SwitchSettingsCommand = new RelayCommand(SwitchSettings);
            SwitchLogsCommand     = new RelayCommand(SwitchLogs);
            SwitchHelpCommand     = new RelayCommand(SwitchHelp);
        }

        private void SwitchPower(object obj)    => bot.MainAsync();
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
