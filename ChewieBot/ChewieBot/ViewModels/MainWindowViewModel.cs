using ChewieBot.AppStart;
using ChewieBot.Commands;
using ChewieBot.Constants;
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
            get { return this.title; }
            set { this.MutateVerbose(ref title, value, RaisePropertyChanged()); }
        }

        private string connectButton;
        public string ConnectButton
        {
            get { return this.connectButton; }
            set { this.MutateVerbose(ref connectButton, value, RaisePropertyChanged()); }
        }

        private string connectStatus;
        public string ConnectStatus
        {
            get { return this.connectStatus; }
            set { this.MutateVerbose(ref connectStatus, value, RaisePropertyChanged()); }
        }

        private string connectColour;
        public string ConnectColour
        {
            get { return this.connectColour; }
            set { this.MutateVerbose(ref connectColour, value, RaisePropertyChanged()); }
        }

        public bool Connected { get; set; }


        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }


        public MenuLink[] MenuItems { get; set; }

        public MainWindowViewModel()
        {
            // First page is the song queue, so set title accordingly.
            this.Title = AppConstants.Views.SongQueue;
            this.Connected = false;
            this.ConnectButton = AppConstants.ConnectButton.Connect;
            this.ConnectStatus = AppConstants.ConnectStatus.NotConnected;
            this.ConnectColour = AppConstants.ConnectStatus.NotConnectedColourHex;
            MenuItems = new MenuLink[]
                {
                    new MenuLink(AppConstants.Views.SongQueue, new SongQueue(UnityConfig.Resolve<ISongQueueService>())),
                    new MenuLink(AppConstants.Views.Quotes, new Quotes(UnityConfig.Resolve<IQuoteService>())),
                    new MenuLink(AppConstants.Views.CommandList, new CommandList(UnityConfig.Resolve<ICommandRepository>()))
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
