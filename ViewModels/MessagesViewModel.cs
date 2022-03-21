﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class MessagesViewModel
    {
        private string _userUrl;
        public string userUrl
        {
            get { return _userUrl; }
            set
            {
                _userUrl = value;
            }
        }

        private string _sender;
        public string sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
            }
        }

        private string _time;
        public string time
        {
            get { return _time; }
            set
            {
                _time = value;
            }
        }

        private string _messageContent;
        public string messageContent
        {
            get { return _messageContent; }
            set
            {
                _messageContent = value;
            }
        }

        public MessagesViewModel(string userUrl, string sender, string time, string messageContent)
        {
            this.userUrl = userUrl;
            this.sender = sender;
            this.time = time;
            this.messageContent = messageContent;
        }
    }
}