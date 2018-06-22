using ChewieBot.AppStart;
using ChewieBot.Models;
using ChewieBot.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { this.MutateVerbose(ref title, value, RaisePropertyChanged()); }
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }


        public MenuLink[] MenuItems { get; set; }

        public MainWindowViewModel()
        {
            this.Title = "Song Queue";
            MenuItems = new MenuLink[]
                {
                    new MenuLink("Song Queue", new SongQueue(UnityConfig.Resolve<ISongQueueService>()))
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
