using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.ViewModels
{
    public class OAuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string AuthUrl { get; set; }
        private string AuthClientId { get; set; }
        private string AuthRedirectUrl { get; set; }
        private string AuthScope { get; set; }

        public OAuthViewModel(string authUrl, string authClientId, string authRedirectUrl, string authScope)
        {
            this.AuthUrl = authUrl;
            this.AuthClientId = authClientId;
            this.AuthRedirectUrl = authRedirectUrl;
            this.AuthScope = authScope;
        }

        public string GetAuthUrl()
        {
            return $"{AuthUrl}?client_id={AuthClientId}&redirect_uri={AuthRedirectUrl}&response_type=code&scope={AuthScope}";
        }
    }
}
