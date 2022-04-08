using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Discord.WebSocket;
using HostCord.Commands;
using System.Windows.Controls;
using HostCord.View;
using System.Windows;
using System.Windows.Threading;
using System.Dynamic;
using System.Threading;
using Discord;
using HostCord.Utils;
using HostCord.Models;


namespace HostCord.ViewModels
{
    public class TextBoxViewModel : INotifyPropertyChanged
    {
        Bot bot;
        public bool isDM = false;

        private string _text;
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        private ulong _activeChannelId;
        public ulong activeChannelId
        {
            get { return _activeChannelId; }
            set
            {
                _activeChannelId = value;

                if (value != 0)
                    text = "";

               OnPropertyChanged();
            }
        }

        public TextBoxViewModel(ref Bot bot, string text, ulong activeChannelId)
        {
            this.bot = bot;
            this.text = text;
            this.activeChannelId = activeChannelId;

            TextBoxSendCommand = new RelayCommand(TextBoxSend);
        }

        public ICommand TextBoxSendCommand { get; set; }

        private void TextBoxSend(object obj)
        {
            if (activeChannelId == 0)
            {
                text = "Not connected!";
                return;
            }

            if (isDM)
                bot.client.GetDMChannelAsync(activeChannelId).Result.SendMessageAsync((string)obj);
            else
                (bot.client.GetChannel(activeChannelId) as SocketTextChannel).SendMessageAsync((string)obj);

            text = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
