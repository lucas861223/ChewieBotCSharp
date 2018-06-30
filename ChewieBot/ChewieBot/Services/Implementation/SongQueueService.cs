using ChewieBot.Database.Model;
using ChewieBot.Enums;
using ChewieBot.Events;
using ChewieBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services.Implementation
{
    public class SongQueueService : ISongQueueService
    {
        public List<Song> SongList { get; set; }
        private int nextSongId = 0;
        private int currentSongId = -1;
        private IYoutubeService youtubeService;

        public event EventHandler<SongAddedEventArgs> SongAddedEvent;
        public event EventHandler<SongChangedEventArgs> SongChangedEvent;
        public event EventHandler<SongRemovedEventArgs> SongRemovedEvent;

        public SongQueueService(IYoutubeService youtubeService)
        {
            this.SongList = new List<Song>();
            this.youtubeService = youtubeService;
        }

        private Song GetSongDetails(Song song)
        {
            switch (song.SourceType)
            {
                case SongSourceType.Youtube:
                    {
                        return youtubeService.GetSongDetails(song);
                    }
                default:
                    {
                        return song;
                    }
            }
        }

        private SongSourceType GetSourceType(string url)
        {
            if (url.Contains("youtu"))
            {
                return SongSourceType.Youtube;
            }

            return SongSourceType.Youtube;
        }

        public Song AddNewSong(string url, User user, SongRequestType requestType)
        {
            var sourceType = this.GetSourceType(url);
            if (sourceType == SongSourceType.Invalid)
            {
                return null;
            }

            var song = new Song { Id = nextSongId, Url = url, RequestedBy = user, RequestType = requestType, SourceType = sourceType };
            song = this.GetSongDetails(song);
            this.SongList.Add(song);
            nextSongId++;
            this.SongAddedEvent?.Invoke(this, new SongAddedEventArgs { Song = song, SongList = this.SongList });
            return song;
        }

        public Song GetNextSong()
        {
            if (SongList.Count >= currentSongId && SongList.Count > 0)
            {
                var nextSong = SongList[++currentSongId];
                SongChangedEvent?.Invoke(this, new SongChangedEventArgs { Song = nextSong });
                return nextSong;
            }

            return null;
        }

        public Song GetNextSong(SongRequestType requestType)
        {
            var nextSong = SongList.FirstOrDefault(x => x.RequestType == requestType);
            if (nextSong != null)
            {
                currentSongId = nextSong.Id;
                SongChangedEvent?.Invoke(this, new SongChangedEventArgs { Song = nextSong });
                return nextSong;
            }
            return null;
        }

        public void RemoveSong(Song song)
        {
            this.SongList.Remove(song);
            this.SongRemovedEvent?.Invoke(this, new SongRemovedEventArgs { Song = song, SongList = this.SongList });
        }

        public void RemoveSong(int id)
        {
            var song = this.SongList.FirstOrDefault(x => x.Id == id);
            if (song != null)
            {
                this.SongList.Remove(song);
                this.SongRemovedEvent?.Invoke(this, new SongRemovedEventArgs { Song = song, SongList = this.SongList });
            }
        }

        public void RemoveSongsForUser(string username)
        {
            this.SongList.RemoveAll(x => x.RequestedBy.Username == username);
        }
    }
}
