using ChewieBot.Models;
using ChewieBot.Services;
using ChewieBot.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private SongQueueViewModel viewModel;

        public SongQueue(ISongQueueService songService)
        {
            songService.SongAddedEvent += (o, e) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.viewModel.SongList = e.SongList;
                    this.DataContext = null;
                    this.DataContext = viewModel;
                });
            };

            this.viewModel = new SongQueueViewModel(songService);
            this.DataContext = viewModel;
            InitializeComponent();            
        }

        public void SongChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var song = e.AddedItems[0] as Song;
                if (viewModel.CurrentSong != song)
                {
                    viewModel.CurrentSong = song;
                    YoutubeEmbed.Dispatcher.Invoke(() =>
                    {
                        YoutubeEmbed.Load(song.Url);
                    });
                }
            }
        }
    }
}
