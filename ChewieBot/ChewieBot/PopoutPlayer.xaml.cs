using ChewieBot.Models;
using ChewieBot.Services;
using ChewieBot.ViewModels;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace ChewieBot
{
    /// <summary>
    /// Interaction logic for PopoutPlayer.xaml
    /// </summary>
    public partial class PopoutPlayer : MetroWindow
    {
        private SongQueueViewModel viewModel;

        public PopoutPlayer(SongQueueViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = viewModel;

            InitializeComponent();

            PopoutYoutubeEmbed.IsBrowserInitializedChanged += (o, e) =>
            {
                if (PopoutYoutubeEmbed.IsBrowserInitialized && this.viewModel.CurrentSong != null)
                {
                    PopoutYoutubeEmbed.Dispatcher.Invoke(() =>
                    {
                        PopoutYoutubeEmbed.Load(this.viewModel.CurrentSong.Url);
                    });
                }
            };           
        }

        public void ChangeSong(Song song)
        {
            PopoutYoutubeEmbed.Dispatcher.Invoke(() =>
            {
                PopoutYoutubeEmbed.Load(song.Url);
            });
        }
    }
}
