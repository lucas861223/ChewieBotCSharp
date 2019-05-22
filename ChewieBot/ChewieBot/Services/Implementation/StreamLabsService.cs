using CefSharp;
using CefSharp.Wpf;
using ChewieBot.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ChewieBot.Services.Implementation
{
    public class StreamLabsService : IStreamLabsService
    {
        // StreamLabs socket implementation usse Socket.IO instead of actual websockets, meaning 
        // it's really convoluted to use with .Net. We'll use the alternative of just polling their
        // donations API every 30 seconds...

        // This does mean we can't get access to other things from streamlabs - e.g. followers, bit donations, subs etc.
        // We'll need to get those from Twitch directly..

        private List<string> donations;
        private string accessToken;
        
        public StreamLabsService(IConfigService configService)
        {
            this.accessToken = configService.Get(AppConstants.ConfigKeys.StreamlabsToken);
            this.donations = new List<string>();
        }

        public void Test()
        {
            this.UpdateLatestDonations();
        }

        private async void UpdateLatestDonations()
        {
            using (var client = new HttpClient())
            {
                var donationUri = $"https://streamlabs.com/api/v1.0/donations?access_token={accessToken}&limit=50";
                if (donations.Count > 0)
                {
                    donationUri += $"&after={donations.Last()}";
                }
                var response = await client.GetAsync(donationUri);
                var content = response.Content;

                using (var reader = new StreamReader(await content.ReadAsStreamAsync()))
                {
                    // Write the output.
                    var responseText = await reader.ReadToEndAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseText);
                    donations.Add("test");
                }

            }
        }
    }
}
