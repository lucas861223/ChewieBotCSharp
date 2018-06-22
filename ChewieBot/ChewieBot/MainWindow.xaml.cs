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
            SongList.Dispatcher.Invoke(() =>
            {
                SongList.ItemsSource = songService.SongList;
            });

            songService.SongAddedEvent += (o, e) =>
            {
                SongList.Dispatcher.Invoke(() =>
                {
                    SongList.Items.Refresh();
                });
            };
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

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
