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
using System.Windows.Threading;
using System.Diagnostics;
using System.Dynamic;
using System.Collections.ObjectModel;
using Discord.WebSocket;
using System.Threading;
using Discord;

namespace HostCord.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        Bot _bot;
        DateTime startDate;
        DispatcherTimer dispatcherTimer;

        public ObservableCollection<ServersViewModel> serversViewModels
        {
            get { return _serversViewModels; }
        }

        private ObservableCollection<ServersViewModel> _serversViewModels = new ObservableCollection<ServersViewModel>();

        public ObservableCollection<ChannelsViewModel> channelsViewModels
        {
            get { return _channelsViewModels; }
        }

        private ObservableCollection<ChannelsViewModel> _channelsViewModels = new ObservableCollection<ChannelsViewModel>();

        public ObservableCollection<MessagesViewModel> messagesViewModels
        {
            get { return _messagesViewModels; }
        }

        private ObservableCollection<MessagesViewModel> _messagesViewModels = new ObservableCollection<MessagesViewModel>();

        public ObservableCollection<TextBoxViewModel> textBoxViewModels
        {
            get { return _textBoxViewModels; }
        }

        private ObservableCollection<TextBoxViewModel> _textBoxViewModels = new ObservableCollection<TextBoxViewModel>();

        private string _botImage;
        public string botImage
        {
            get { return _botImage; }
            set
            {
                _botImage = value;
                OnPropertyChanged();
            }
        }

        private string _botName;
        public string botName
        {
            get { return _botName; }
            set
            {
                _botName = value;
                OnPropertyChanged();
            }
        }

        private string _botLatency;
        public string botLatency
        {
            get { return _botLatency; }
            set
            {
                _botLatency = value;
                OnPropertyChanged();
            }
        }

        private string _botServers;
        public string botServers
        {
            get { return _botServers; }
            set
            {
                _botServers = value;
                OnPropertyChanged();
            }
        }

        private string _botUptime;
        public string botUptime
        {
            get { return _botUptime; }
            set
            {
                _botUptime = value;
                OnPropertyChanged();
            }
        }

        private string _cpuUsage;
        public string cpuUsage
        {
            get { return _cpuUsage; }
            set
            {
                _cpuUsage = value;
                OnPropertyChanged();
            }
        }

        private string _ramUsage;
        public string ramUsage
        {
            get { return _ramUsage; }
            set
            {
                _ramUsage = value;
                OnPropertyChanged();
            }
        }

        private SocketTextChannel _activeChannel;
        public SocketTextChannel activeChannel
        {
            get { return _activeChannel; }
            set
            {
                _activeChannel = value;
                activeChannelName = $"# {activeChannel.Name} | {activeChannel.Topic}";
            }
        }


        private string _activeChannelName;
        public string activeChannelName
        {
            get { return _activeChannelName; }
            set
            {
                _activeChannelName = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetActiveChannelCommand { get; set; }

        private void SetActiveChannel(object obj)
        {
            messagesViewModels.Clear();
            activeChannel = (SocketTextChannel)_bot.client.GetChannel((ulong)obj);

            if (textBoxViewModels.Any())
                textBoxViewModels[0].activeChannelId = activeChannel.Id;

            SetMessages();
        }

        private async Task SetMessages()
        {
            var messages = activeChannel.GetMessagesAsync(50).Flatten().Reverse();

            await foreach (var message in messages)
            {
                await Application.Current.Dispatcher.BeginInvoke(()
                    => messagesViewModels.Add(new MessagesViewModel(
                        message.Author.GetAvatarUrl(ImageFormat.Jpeg, 128),
                        message.Author.Username,
                        message.Timestamp.ToString(),
                        message.Content)));
            }
        }

        public HomeViewModel(ref Bot bot)
        {
            _bot = bot;
            _bot.client.Connected += Client_Connected;
            _bot.client.LatencyUpdated += Client_LatencyUpdated;
            _bot.client.Ready += Client_Ready;
            _bot.client.MessageReceived += Client_MessageReceived;

            botImage = @"~\..\Images\logo.png";
            botName = "Bot#0000";
            botLatency = "Latency: 0";
            botServers = "Servers: 0";
            botUptime = "Uptime: 0";
            cpuUsage = "CPU Usage: 0";
            ramUsage = "RAM Usage: 0";

            dispatcherTimer = new DispatcherTimer();
        }

        private Task Client_MessageReceived(SocketMessage message)
        {
            if (message.Channel.Id != activeChannel.Id)
                return Task.CompletedTask;

            Application.Current.Dispatcher.BeginInvoke(()
                => messagesViewModels.Add(new MessagesViewModel(
                    message.Author.GetAvatarUrl(),
                    message.Author.Username,
                    message.Timestamp.ToString(),
                    message.Content)));

            return Task.CompletedTask;
        }

        private Task Client_Ready()
        {
            GenerateChannelsCommand = new RelayCommand(GenerateChannels);

            foreach (SocketGuild guild in _bot.client.Guilds)
                Application.Current.Dispatcher.BeginInvoke(() 
                    => serversViewModels.Add(new ServersViewModel(guild.IconUrl, guild.Name)));

            return Task.CompletedTask;
        }

        public ICommand GenerateChannelsCommand { get; set; }

        private void GenerateChannels(object obj)
        {
            SetActiveChannelCommand = new RelayCommand(SetActiveChannel);

            _channelsViewModels.Clear();

            foreach (SocketGuild guild in _bot.client.Guilds)
            {
                if (guild.Name == obj as string)
                {
                    foreach (SocketTextChannel channel in guild.TextChannels)
                        Application.Current.Dispatcher.BeginInvoke(()
                            => channelsViewModels.Add(new ChannelsViewModel(guild.Id, guild.Name, channel.Id, "# " + channel.Name)));

                    if (!textBoxViewModels.Any())
                        Application.Current.Dispatcher.BeginInvoke(()
                            => textBoxViewModels.Add(new TextBoxViewModel(ref _bot, "", activeChannel.Id)));


                    SetActiveChannel(guild.DefaultChannel.Id);

                    return;
                }
            }
        }

        private Task Client_LatencyUpdated(int arg1, int arg2)
        {
            botLatency = "Latency: " + _bot.client.Latency;

            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            startDate = DateTime.Now;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            botImage = _bot.client.CurrentUser.GetAvatarUrl(Discord.ImageFormat.Png, 256);
            botName = $"{_bot.client.CurrentUser.Username}#{_bot.client.CurrentUser.Discriminator}";
            botServers = "Servers: " + _bot.client.Guilds.Count;

            return Task.CompletedTask;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            botUptime = "Uptime: " + DateTime.Now.Subtract(startDate).ToString(@"hh\:mm\:ss");

            string[] usage = GetUsage();

            cpuUsage = "CPU: " + usage[0] + "%";
            ramUsage = "RAM:" + usage[1] + "MB";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] GetUsage()
        {
            var process = Process.GetCurrentProcess();

            var name = string.Empty;

            foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
            {
                if (instance.StartsWith(process.ProcessName))
                {
                    using (var processId = new PerformanceCounter("Process", "ID Process", instance, true))
                    {
                        if (process.Id == (int)processId.RawValue)
                        {
                            name = instance;
                            break;
                        }
                    }
                }
            }

            var cpu = new PerformanceCounter("Process", "% Processor Time", name, true);
            var ram = new PerformanceCounter("Process", "Private Bytes", name, true);

            cpu.NextValue();
            ram.NextValue();

            //https://stackoverflow.com/questions/12262645/performance-counter-inaccurate-in-c-sharp

            string[] usage = new string[] { 
                Math.Round(cpu.NextValue() / Environment.ProcessorCount, 2).ToString(),
                Math.Round(ram.NextValue() / 1024 / 1024, 2).ToString()
            };

            return usage;
        }
    }
}