using ChewieBot.AppStart;
using ChewieBot.Twitch;
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

namespace ChewieBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITwitchClient client = UnityConfig.Resolve<ITwitchClient>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeSetup();
            InitializeTwitchClient();
        }

        private void InitializeSetup()
        {
            UnityConfig.Setup();
        }

        private void InitializeTwitchClient()
        {
            if (client != null)
            {
                client.Start();
            }
        }
    }
}
