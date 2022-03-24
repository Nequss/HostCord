using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HostCord.ViewModels;

namespace HostCord.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home(ref Bot bot)
        {
            InitializeComponent();
            DataContext = new HomeViewModel(ref bot);
        }

        private bool AutoScroll = true;

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {  
                if (chatViewer.VerticalOffset == chatViewer.ScrollableHeight)
                {  
                    AutoScroll = true;
                }
                else
                { 
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            { 
                chatViewer.ScrollToVerticalOffset(chatViewer.ExtentHeight);
            }
        }
    }
}
