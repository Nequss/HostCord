using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        Bot _bot;

        private string _textLog;
        public string textLog
        {
            get { return _textLog; }
            set
            {
                _textLog = value;
                OnPropertyChanged();
            }
        }

        public LogsViewModel(ref Bot bot)
        {
            _bot = bot;

            _bot.client.Log += Client_Log;
            _bot.services.GetRequiredService<CommandService>().Log += CommandService_Log;
        }

        private Task CommandService_Log(LogMessage arg)
        {
            textLog = string.Join($"{DateTime.Now} {arg.Severity} {arg.Source} | ", textLog, arg.Message + "\n");
            return Task.CompletedTask;
        }

        private Task Client_Log(LogMessage arg)
        {
            textLog = string.Join($"{DateTime.Now} {arg.Severity} {arg.Source} | ", textLog, arg.Message + "\n");
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
