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
using Discord;
using HostCord.Utils;
using HostCord.Models;

namespace HostCord.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        Bot _bot;
        DateTime startDate;
        DispatcherTimer dispatcherTimer;
        PerformanceMonitor performanceMonitor = PerformanceMonitor.getInstance();

        private List<IDMChannel> openDMChannels;

        private ObservableCollection<ServersViewModel> _serversViewModels;
        public ObservableCollection<ServersViewModel> serversViewModels
        {
            get { return _serversViewModels; }
            set { _serversViewModels = value; }
        }

        private ObservableCollection<ChannelsViewModel> _channelsViewModels;
        public ObservableCollection<ChannelsViewModel> channelsViewModels
        {
            get { return _channelsViewModels; }
            set { _channelsViewModels = value; }
        }

        private ObservableCollection<MessagesViewModel> _messagesViewModels;
        public ObservableCollection<MessagesViewModel> messagesViewModels
        {
            get { return _messagesViewModels; }
            set { _messagesViewModels = value; }
        }

        private ObservableCollection<TextBoxViewModel> _textBoxViewModels;
        public ObservableCollection<TextBoxViewModel> textBoxViewModels
        {
            get { return _textBoxViewModels; }
            set { _textBoxViewModels = value; }
        }

        private string _mailImage;
        public string mailImage
        {
            get { return _mailImage; }
            set
            {
                _mailImage = value;
                OnPropertyChanged();
            }
        }

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
                activeChannelName = $"# {value.Name} | {value.Topic}";
            }
        }

        private IDMChannel _activeDMChannel;
        public IDMChannel activeDMChannel
        {
            get { return _activeDMChannel; }
            set
            {
                _activeDMChannel = value;
                //activeChannelName = $"@ {value.Recipient.Username} | {value.Recipient.Status}";
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

        private int _dms;
        public int dms
        {
            get { return _dms; }
            set
            {
                _dms = value;

                if (value == 0)
                {
                    dmCanvas = Visibility.Hidden;
                }
                else if (value > 99)
                {
                    dmNotificationsText = value + "+";
                }
                else if (value > 9)
                {
                    dmNotificationsText = value + "";
                    dmCanvasLeft = 22.5f;
                }
                else
                {
                    dmNotificationsText = value + "";
                    dmCanvasLeft = 26;

                    dmCanvas = Visibility.Visible;
                }
            }
        }

        private string _dmNotificationsText;
        public string dmNotificationsText
        {
            get { return _dmNotificationsText; }
            set
            {
                _dmNotificationsText = value;
                OnPropertyChanged();
            }
        }

        private float _dmCanvasLeft;
        public float dmCanvasLeft
        {
            get { return _dmCanvasLeft; }
            set
            {
                _dmCanvasLeft = value;
                OnPropertyChanged();
            }
        }

        private Visibility _dmCanvas;
        public Visibility dmCanvas
        {
            get { return _dmCanvas; }
            set
            {
                _dmCanvas = value;
                OnPropertyChanged();
            }
        }

        public ICommand SetActiveChannelCommand { get; set; }
        public ICommand GenerateChannelsCommand { get; set; }
        public ICommand GenerateDMsCommand { get; set; }

        public HomeViewModel(ref Bot bot)
        {
            _bot = bot;
            _bot.client.Connected += Client_Connected;
            _bot.client.LatencyUpdated += Client_LatencyUpdated;
            _bot.client.Ready += Client_Ready;
            _bot.client.MessageReceived += Client_MessageReceived;

            mailImage  = @"/HostCord;component/Images/mail.png";
            botImage   = @"/HostCord;component/Images/logo.png";
            botName    = "HostCord#0000";
            botLatency = "Latency: 0";
            botServers = "Servers: 0";
            botUptime  = "Uptime: 0";
            activeChannelName = "# Default channel name | Channel description";
            dms = 0;
            dmCanvas = Visibility.Hidden;

            GenerateDMsCommand = new RelayCommand(GenerateDMs);

            openDMChannels = new List<IDMChannel>();

            serversViewModels = new ObservableCollection<ServersViewModel>()
            { 
                new ServersViewModel(0, botImage, "Default Server"),
            };

            channelsViewModels = new ObservableCollection<ChannelsViewModel>()
            {
                new ChannelsViewModel(0, "", 0, "# Default text channel", false),

            };

            messagesViewModels = new ObservableCollection<MessagesViewModel>()
            {
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "Welcome to the HostCord!", ""),
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "This is the Home Page\nYou can read messages from different servers and chat as a bot here", ""),
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "If you are completely new to discord bots, please visit Help Page", ""),
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "You will find there all needed information", ""),
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "In order to simply start your bot you only need the token", ""),
                new MessagesViewModel(botImage, "HostCord", DateTime.Now.ToString(@"hh\:mm"), "Copy and paste the bot's token to the Settings Page", ""),
            };

            textBoxViewModels = new ObservableCollection<TextBoxViewModel>()
            {
                new TextBoxViewModel(ref _bot, "", 0)
            };

            startDate = DateTime.Now;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void GenerateDMs(object obj)
        {
            dms = 0;

            Application.Current.Dispatcher.BeginInvoke(()
                => channelsViewModels.Clear());

            if (openDMChannels.Count() > 0)
            {
                foreach (var openDMChannel in openDMChannels)
                {
                    Application.Current.Dispatcher.BeginInvoke(()
                        => channelsViewModels.Add(new ChannelsViewModel(0, "", openDMChannel.Id, openDMChannel.Recipient.Username, true)));
                }

                SetActiveChannel(openDMChannels.First().Id);
                SetMessagesDM(openDMChannels.First());
            }
        }

        private async Task SetMessagesDM(IDMChannel openDMChannel)
        {
            await Application.Current.Dispatcher.BeginInvoke(()
                => messagesViewModels.Clear());
            
            var messages = openDMChannel.GetMessagesAsync(100).Flatten().Reverse();
    
            await foreach (var message in messages)
            {
                await Application.Current.Dispatcher.BeginInvoke(()
                    => messagesViewModels.Add(new MessagesViewModel(
                        message.Author.GetAvatarUrl(ImageFormat.Jpeg, 128),
                        message.Author.Username,
                        message.Timestamp.LocalDateTime.ToString(),
                        message.Content,
                        GetImageUrl(message))));
            }
        }

        private void GenerateChannels(object obj)
        {

            Application.Current.Dispatcher.BeginInvoke(()
                => channelsViewModels.Clear());

            SetActiveChannelCommand = new RelayCommand(SetActiveChannel);

            foreach (SocketGuild guild in _bot.client.Guilds)
            {
                if (guild.Name == obj as string)
                {
                    foreach (SocketTextChannel channel in guild.TextChannels)
                        Application.Current.Dispatcher.BeginInvoke(()
                            => channelsViewModels.Add(new ChannelsViewModel(guild.Id, guild.Name, channel.Id, "# " + channel.Name, false)));

                    SetActiveChannel(guild.DefaultChannel.Id);

                    return;
                }
            }
        }

        private void SetActiveChannel(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(()
                => messagesViewModels.Clear());

            activeDMChannel = _bot.client.GetDMChannelAsync((ulong)obj).Result;

            if (activeDMChannel != null)
            { 

                if (textBoxViewModels.Any())
                    textBoxViewModels[0].activeChannelId = activeDMChannel.Id;

                SetMessagesDM(activeDMChannel);
            }
            else 
            { 
                activeChannel = (SocketTextChannel)_bot.client.GetChannel((ulong)obj);

                if (textBoxViewModels.Any())
                    textBoxViewModels[0].activeChannelId = activeChannel.Id;

                SetMessages();
            }
        }

        private async Task SetMessages()
        {
            var messages = activeChannel.GetMessagesAsync(40).Flatten().Reverse();

            await foreach (var message in messages)
            {

                await Application.Current.Dispatcher.BeginInvoke(()
                    => messagesViewModels.Add(new MessagesViewModel(
                        message.Author.GetAvatarUrl(ImageFormat.Jpeg, 128),
                        message.Author.Username,
                        message.Timestamp.LocalDateTime.ToString(),
                        message.Content,
                        GetImageUrl(message))));
            }
        }

        private string GetImageUrl(IMessage message)
        {
            if (message.Attachments.Count != 0)
            {
                var image = message.Attachments.Where(x =>
                x.Filename.EndsWith(".jpg") || x.Filename.EndsWith(".png") ||
                x.Filename.EndsWith(".gif") || x.Filename.EndsWith(".bmp") ||
                x.Filename.EndsWith(".jpeg"));

                if (image.Any())
                    return image.First().Url;
            }

            return "";
        }

        private bool IsDM(SocketMessage msg)
        {
            return (msg.Channel.GetType() == typeof(SocketDMChannel));
        }

        private Task Client_Ready()
        {
            Application.Current.Dispatcher.BeginInvoke(() 
                => serversViewModels.Clear());

            GenerateChannelsCommand = new RelayCommand(GenerateChannels);


            foreach (SocketGuild guild in _bot.client.Guilds)
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    serversViewModels.Add(new ServersViewModel(guild.Id, guild.IconUrl, guild.Name));
                });


            GenerateChannels(_bot.client.Guilds.First().Name);

            return Task.CompletedTask;
        }

        private Task Client_MessageReceived(SocketMessage message)
        {
            if (IsDM(message))
            {
                dms++;

                if (activeDMChannel != null)
                {
                    if (message.Channel.Id != activeChannel.Id)
                        return Task.CompletedTask;

                    Application.Current.Dispatcher.BeginInvoke(()
                        => messagesViewModels.Add(new MessagesViewModel(
                            message.Author.GetAvatarUrl(ImageFormat.Jpeg, 128),
                            message.Author.Username,
                            message.Timestamp.LocalDateTime.ToString(),
                            message.Content,
                            GetImageUrl(message))));

                    return Task.CompletedTask;
                }
                else
                {
                    openDMChannels.Add((IDMChannel)message.Channel);
                    return Task.CompletedTask;
                }
            }
            else
            {
                if (activeChannel != null)
                {
                    if (message.Channel.Id != activeChannel.Id)
                        return Task.CompletedTask;

                    Application.Current.Dispatcher.BeginInvoke(()
                        => messagesViewModels.Add(new MessagesViewModel(
                            message.Author.GetAvatarUrl(),
                            message.Author.Username,
                            message.Timestamp.LocalDateTime.ToString(),
                            message.Content,
                            GetImageUrl(message))));
                }

            }
            return Task.CompletedTask;
        }

        private Task Client_LatencyUpdated(int arg1, int arg2)
        {
            botLatency = "Latency: " + _bot.client.Latency;

            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            botImage = _bot.client.CurrentUser.GetAvatarUrl(Discord.ImageFormat.Png, 256);
            botName = $"{_bot.client.CurrentUser.Username}#{_bot.client.CurrentUser.Discriminator}";
            botServers = "Servers: " + _bot.client.Guilds.Count;

            return Task.CompletedTask;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            botUptime = "Uptime: " + DateTime.Now.Subtract(startDate).ToString(@"hh\:mm\:ss");

            cpuUsage = $" CPU: {performanceMonitor.cpuUsage} %";
            ramUsage = $"RAM: {performanceMonitor.ramUsage} MB";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}