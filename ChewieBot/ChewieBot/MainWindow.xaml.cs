using ChewieBot.AppStart;
using ChewieBot.Scripting;
using ChewieBot.Services;
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
        private ITwitchService twitchService = UnityConfig.Resolve<ITwitchService>();
        private ICommandService commandService = UnityConfig.Resolve<ICommandService>();
        private IUserService userService = UnityConfig.Resolve<IUserService>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeSetup();
            //InitializeTwitchClient();
            TestScripting();
        }

        private void InitializeSetup()
        {
            UnityConfig.Setup();
        }

        private void TestScripting()
        {
            this.userService.SetUser(new Database.Model.User { Username = "magentafall", Points = 50 });
            this.userService.SetUser(new Database.Model.User { Username = "chewiemelodies", Points = 50 });
            this.userService.SetUser(new Database.Model.User { Username = "erredece", Points = 50 });
            this.userService.SetUser(new Database.Model.User { Username = "cozmium", Points = 50 });

            var response = this.commandService.ExecuteCommand("raffle", "magentafall", new List<string>() { "start", "0", "5000" });
            Console.WriteLine(response.ToString());

            this.commandService.ExecuteCommand("raffle", "magentafall", new List<string>() { "join" });
            this.commandService.ExecuteCommand("raffle", "chewiemelodies", new List<string>() { "join" });
            this.commandService.ExecuteCommand("raffle", "cozmium", new List<string>() { "join" });
            this.commandService.ExecuteCommand("raffle", "erredece", new List<string>() { "join" });
        }

        private void InitializeTwitchClient()
        {
            if (twitchService != null)
            {
                twitchService.Initialize();
                twitchService.Connect();
            }
        }
    }
}
