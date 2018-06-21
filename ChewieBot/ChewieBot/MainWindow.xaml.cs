using ChewieBot.AppStart;
using ChewieBot.Scripting;
using ChewieBot.Services;
using ChewieBot.Twitch;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private IPythonEngine scriptEngine = UnityConfig.Resolve<IPythonEngine>();
        private ISongQueueService songService = UnityConfig.Resolve<ISongQueueService>();
        private IYoutubeService youtubeService = UnityConfig.Resolve<IYoutubeService>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeSetup();
            InitializeTwitchClient();
            //TestScripting();
            //TestPython();
            TestEmbeds();
        }

        private void InitializeSetup()
        {
            UnityConfig.Setup();
            SongList.ItemsSource = this.songService.SongList;
        }
        
        private void TestEmbeds()
        {
            //this.songService.AddNewSong("https://www.youtube.com/watch?v=POiTHZO2yso", userService.GetUser("magentafall"), Enum.SongRequestType.Donation);
            //this.songService.AddNewSong("https://www.youtube.com/watch?v=7Iweue-OcMo", userService.GetUser("magentafall"), Enum.SongRequestType.Donation);
            /*SongList.ItemsSource = this.songService.GetSongList();

            var nextSong = this.songService.GetNextSong();
            //YoutubeEmbed.Address = nextSong.Url;

            var timer = new Timer();
            timer.Interval = 8000;
            timer.Elapsed += (o, e) =>
            {
                nextSong = this.songService.GetNextSong();
                //YoutubeEmbed.Load(nextSong.Url);
                timer.Stop();
            };
            timer.Start();*/
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TestPython()
        {
            this.commandService.ExecuteCommand("addpoints", "magentatall", new List<string>() { "magentafall", "50" });
            this.commandService.ExecuteCommand("raffle", "magentafall", new List<string>() { "Raffle", "5" });
            this.commandService.ExecuteCommand("joinraffle", "magentafall");
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
                twitchService.Connect();
            }
        }
    }
}
