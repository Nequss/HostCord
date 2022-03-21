using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class ChannelsViewModel
    {
        private ulong _guildId;
        public ulong guildId
        {
            get { return _guildId; }
            set
            {
                _guildId = value;
            }
        }

        private string _guildName;
        public string guildName
        {
            get { return _guildName; }
            set
            {
                _guildName = value;
            }
        }

        private ulong _channelId;
        public ulong channelId
        {
            get { return _channelId; }
            set
            {
                _channelId = value;
            }
        }

        private string _channelName;
        public string channelName
        {
            get { return _channelName; }
            set
            {
                _channelName = value;
            }
        }

        public ChannelsViewModel(ulong guildId, string guildName, ulong channelId, string channelName)
        {
            this.guildId = guildId;
            this.guildName = guildName;
            this.channelId = channelId;
            this.channelName = channelName;
        }
    }
}
