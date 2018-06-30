using ChewieBot.Models;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class SongQueueViewModel : INotifyPropertyChanged
    {
        private ISongQueueService songService;

        public SongQueueViewModel(ISongQueueService songService)
        {
            this.songService = songService;
            this.songList = new List<Song>();
        }

        private Song currentSong;
        private List<Song> songList;

        public Song CurrentSong
        {
            get { return currentSong; }
            set { this.MutateVerbose(ref currentSong, value, RaisePropertyChanged()); }
        }

        public List<Song> SongList
        {
            get { return songList; }
            set { this.MutateVerbose(ref songList, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        public void RemoveSong(Song song)
        {
            this.songService.RemoveSong(song.Id);
        }
    }
}
