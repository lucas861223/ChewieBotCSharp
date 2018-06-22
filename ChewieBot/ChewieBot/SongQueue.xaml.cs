using ChewieBot.Models;
using ChewieBot.Services;
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
    /// Interaction logic for SongQueue.xaml
    /// </summary>
    public partial class SongQueue : UserControl
    {
        private ISongQueueService songService;
        private Song currentSong;
        private Func<ISongQueueService> resolve;

        public SongQueue(ISongQueueService songService)
        {
            this.songService = songService;
            InitializeComponent();
            this.Init();
        }

        private void Init()
        {
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

            SongList.SelectionChanged += (SelectionChangedEventHandler)((o, e) =>
            {
                if (e.AddedItems.Count == 1)
                {
                    var song = e.AddedItems[0] as Song;
                    if (currentSong != song)
                    {
                        currentSong = song;
                        YoutubeEmbed.Dispatcher.Invoke(() =>
                        {
                            YoutubeEmbed.Load(song.Url);
                        });
                    }
                }
            });
        }
    }
}
