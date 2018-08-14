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
                //var key = song.Url.Split('/').Last().Split('?')[1].Substring(2);
                var key = this.GetYoutubeSongId(song.Url);
                if (key != null)
                {
                    song.Url = $"{this.FormatYoutubeUrl(key)}&autoplay=0";  // Disable autoplay
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
                else
                {
                    return null;
                }
            }
        }

        private string FormatYoutubeUrl(string id)
        {
            return $"https://www.youtube.com/tv#/watch?v={id}";
        }

        private string GetYoutubeSongId(string url)
        {
            var startIndex = url.IndexOf("v=");
            var endIndex = url.IndexOf("&");
            if (startIndex == -1)
            {
                return this.GetShortYoutubeSongId(url);
            }
            else
            {
                // adding offset for v=
                startIndex += 2;
            }

            if (endIndex == -1)
            {
                return url.Substring(startIndex);
            }
            else
            {
                return url.Substring(startIndex, endIndex - startIndex);
            }
        }

        private string GetShortYoutubeSongId(string url)
        {
            var startIndex = url.IndexOf("youtu.be/");
            var offset = "youtu.be/".Length;
            if (startIndex != -1)
            {
                startIndex += offset;
                var endIndex = url.IndexOf("?");
                if (endIndex == -1)
                {
                    return url.Substring(startIndex);
                }
                else
                {
                    return url.Substring(startIndex, endIndex - startIndex);
                }
            }

            return null;
        }
    }
}
