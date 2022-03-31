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
        private bool _isDM;
        public bool isDM
        {
            get { return _isDM; }
            set
            {
                _isDM = value;
            }
        }

        public ChannelsViewModel(ulong guildId, string guildName, ulong channelId, string channelName, bool isDM)
        {
            this.guildId = guildId;
            this.guildName = guildName;
            this.channelId = channelId;
            this.channelName = channelName;
            this.isDM = isDM;
        }
    }
}
