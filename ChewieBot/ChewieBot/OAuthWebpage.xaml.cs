using ChewieBot.Config;
using ChewieBot.Constants;
using ChewieBot.Database.Model;
using ChewieBot.Database.Repository;
using ChewieBot.ViewModels;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for OAuthWebpage.xaml
    /// </summary>
    public partial class OAuthWebpage : MetroWindow
    {
        private OAuthViewModel viewModel;
        private IKeyValueData configData;
        public OAuthWebpage(IKeyValueData configData)
        {
            this.configData = configData;

            this.viewModel = new OAuthViewModel(AppConfig.StreamlabsAuthUrl, AppConfig.StreamlabsClientId, AppConfig.StreamlabsRedirectUri, AppConfig.StreamlabsScope);

            InitializeComponent();

            if (OAuthPopout.IsBrowserInitialized)
            {
                OAuthPopout.Dispatcher.Invoke(() =>
                {
                    OAuthPopout.Load(this.viewModel.GetAuthUrl());
                });
            }
            else
            {
                //Browser is not initliazed we'll wait then load
                DependencyPropertyChangedEventHandler handler = null;
                handler = (sender, args) =>
                {
                    OAuthPopout.IsBrowserInitializedChanged -= handler;

                    //If browser is intialized then it's safe to load
                    if (OAuthPopout.IsBrowserInitialized)
                    {
                        OAuthPopout.Load(this.viewModel.GetAuthUrl());
                        OAuthPopout.TitleChanged += OAuthPopout_TitleChanged;
                    }
                };

                OAuthPopout.IsBrowserInitializedChanged += handler;
            }
        }

        private async void OAuthPopout_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.ToString().Contains("Uh oh"))
            {
                this.Hide();

                // Check url for code property
                if (OAuthPopout.Address.Contains("code"))
                {
                    var code = OAuthPopout.Address.Substring(OAuthPopout.Address.IndexOf("code=") + 5);

                    using (var client = new HttpClient())
                    {
                        var requestContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "authorization_code"),
                            new KeyValuePair<string, string>("client_id", AppConfig.StreamlabsClientId),
                            new KeyValuePair<string, string>("client_secret", AppConfig.StreamlabsClientSecret),
                            new KeyValuePair<string, string>("redirect_uri", AppConfig.StreamlabsRedirectUri),
                            new KeyValuePair<string, string>("code", code)
                        });

                        var response = await client.PostAsync(AppConfig.StreamlabsTokenUrl, requestContent);
                        var content = response.Content;

                        using (var reader = new StreamReader(await content.ReadAsStreamAsync()))
                        {
                            // Write the output.
                            var responseText = await reader.ReadToEndAsync();
                            dynamic json = JsonConvert.DeserializeObject(responseText);
                            this.configData.Set(AppConstants.ConfigKeys.StreamlabsToken, json["access_token"].ToString());
                            verified = true;
                        }

                    }
                }
                this.Close();
            }
        }
    }
}
