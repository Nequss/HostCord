using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.ViewModels
{
    public class LogsViewModel
    {
        Bot _bot;

        public LogsViewModel(ref Bot bot)
        {
            _bot = bot;
        }
    }
}
