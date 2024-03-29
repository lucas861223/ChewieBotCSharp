﻿using ChewieBot.Database.Model;
using ChewieBot.Enums;
using ChewieBot.Events;
using ChewieBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Services
{
    public interface ISongQueueService
    {
        Song GetNextSong();
        Song GetNextSong(SongRequestType requestType);
        Song AddNewSong(string url, User user, SongRequestType requestType);
        void RemoveSong(Song song);
        void RemoveSong(int id);
        void RemoveSongsForUser(string username);
        List<Song> SongList { get; set; }
        event EventHandler<SongAddedEventArgs> SongAddedEvent;
        event EventHandler<SongRemovedEventArgs> SongRemovedEvent;
        event EventHandler<SongChangedEventArgs> SongChangedEvent;
    }
}
