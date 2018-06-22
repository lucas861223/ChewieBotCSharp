using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ChewieBot.Models;
using Newtonsoft.Json;

namespace ChewieBot.Services.Implementation
{
    public class YoutubeService : IYoutubeService
    {
        public Song GetSongDetails(Song song)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/videos");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var key = song.Url.Split('/').Last().Split('?')[1].Substring(2);
                var urlParams = $"?part=contentDetails,snippet,status&id={key}&key={ConfigurationManager.AppSettings["Youtube-API-Key"]}";
                var response = client.GetAsync(urlParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                    song.Embeddable = (bool)json.items[0].status.embeddable;
                    song.Title = json.items[0].snippet.title;
                    song.Duration = XmlConvert.ToTimeSpan((string)json.items[0].contentDetails.duration);
                }
                return song;
            }
        }
    }
}
