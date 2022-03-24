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
using System.Reflection;
using Discord.WebSocket;
using Discord;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;

namespace HostCord.ViewModels
{
    public class BotConfigViewModel : INotifyPropertyChanged
    {
        Bot _bot;

        private string _token;
        public string token
        {
            get { return _token; }
            set
            {
                _token = value;
                _bot.token = value;
                OnPropertyChanged();
            }
        }

        private string _prefix;
        public string prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                _bot.commandHandler.SetPrefix(value);
                OnPropertyChanged();
            }
        }

        private string _filterWords;
        public string filterWords
        {
            get { return _filterWords; }
            set
            {
                _filterWords = value;
                OnPropertyChanged();
            }
        }

        private int _servers;
        public int servers
        {
            get { return _servers; }
            set
            {
                _servers = value;
                OnPropertyChanged();
            }
        }

        private int _textChannels;
        public int textChannels
        {
            get { return _textChannels; }
            set
            {
                _textChannels = value;
                OnPropertyChanged();
            }
        }

        private int _messages;
        public int messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        private int _users;
        public int users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private int _commands;
        public int commands
        {
            get { return _commands; }
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _cpu;
        public string cpu
        {
            get { return _cpu; }
            set
            {
                _cpu = value;
                OnPropertyChanged();
            }
        }

        private string _ram;
        public string ram
        {
            get { return _ram; }
            set
            {
                _ram = value;
                OnPropertyChanged();
            }
        }

        private string _bytesSent;
        public string bytesSent
        {
            get { return _bytesSent; }
            set
            {
                _bytesSent = value;
                OnPropertyChanged();
            }
        }

        private string _bytesReceived;
        public string bytesReceived
        {
            get { return _bytesReceived; }
            set
            {
                _bytesReceived = value;
                OnPropertyChanged();
            }
        }

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
            _bot = bot;

            token = "";
            prefix = "+";
            users = 0;
            servers = 0;
            textChannels = 0;
            status = "Disconnected";

            cpu = "0 %";
            ram = "0 MB";
            bytesSent = "0 B";
            bytesReceived = "0 B";

            GenerateCommandsCommand = new RelayCommand(GenerateCommands);

            foreach (var module in _bot.GetModules())
                Application.Current.Dispatcher.BeginInvoke(()
                    => modulesViewModels.Add(new ModulesViewModel(module.Name)));

            _bot.client.MessageReceived += Client_MessageReceived;
            _bot.client.Disconnected    += Client_Disconnected;
            _bot.client.Ready           += Client_Ready;
            _bot.client.JoinedGuild     += Client_JoinedGuild;
            _bot.client.LeftGuild       += Client_LeftGuild;
            _bot.client.UserJoined      += Client_UserJoined;
            _bot.client.UserLeft        += Client_UserLeft;
            _bot.services.GetRequiredService<CommandService>().CommandExecuted += BotConfigViewModel_CommandExecuted;
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
        private Task Client_Ready()
        {
            servers = _bot.client.Guilds.Count;

            foreach (var guild in _bot.client.Guilds)
            {
                users += guild.Users.Count;
                textChannels += guild.TextChannels.Count;
            }

            status = "Connected";
            return Task.CompletedTask;
        }

        private Task Client_UserLeft(SocketGuild arg1, SocketUser arg2)
        {
            users--;
            return Task.CompletedTask;
        }

        private Task Client_UserJoined(SocketGuildUser arg)
        {
            users++;
            return Task.CompletedTask;
        }

        private Task Client_LeftGuild(SocketGuild arg)
        {
            servers--;
            return Task.CompletedTask;
        }

        private Task Client_JoinedGuild(SocketGuild arg)
        {
            servers++;
            return Task.CompletedTask;
        }

        private Task Client_Disconnected(Exception arg)
        {
            status = "Disconnected";
            return Task.CompletedTask;
        }

        private Task Client_MessageReceived(SocketMessage arg)
        {
            messages++;
            return Task.CompletedTask;
        }

        private Task BotConfigViewModel_CommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3)
        {
            commands++;
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
