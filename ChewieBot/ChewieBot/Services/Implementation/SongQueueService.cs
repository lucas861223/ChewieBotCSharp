using ChewieBot.Database.Model;
using ChewieBot.Enum;
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

        public SongQueueService(IYoutubeService youtubeService)
        {
            this.SongList = new List<Song>();
            this.youtubeService = youtubeService;
        }

        private string FormatUrl(string url, SongSourceType sourceType)
        {
            switch (sourceType)
            {
                case SongSourceType.Youtube:
                    {
                        return url.Replace("watch?v=", "embed/");
                    }
                default:
                    {
                        return url;
                    }
            }
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
            if (url.Contains("youtube"))
            {
                return SongSourceType.Youtube;
            }

            return SongSourceType.Youtube;
        }

        public Song AddNewSong(string url, User user, SongRequestType requestType)
        {
            var sourceType = this.GetSourceType(url);
            var song = new Song { Id = nextSongId, Url = this.FormatUrl(url, sourceType), RequestedBy = user, RequestType = requestType, SourceType = sourceType };
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
                return nextSong;
            }

            return null;
        }

        public Song GetNextSong(SongRequestType requestType)
        {
            var nextSong = SongList.FirstOrDefault(x => x.RequestType == requestType);
            currentSongId = nextSong.Id;
            return nextSong;
        }

        public void RemoveSong(Song song)
        {
            this.SongList.Remove(song);
        }

        public void RemoveSong(int id)
        {
            var song = this.SongList.FirstOrDefault(x => x.Id == id);
            if (song != null)
            {
                this.SongList.Remove(song);
            }
        }

        public void RemoveSongsForUser(string username)
        {
            this.SongList.RemoveAll(x => x.RequestedBy.Username == username);
        }
    }
}
