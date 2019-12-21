using DatabaseLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoviesInfo.Data
{
    public class Connection
    {
        public async Task<String> executeRequestAsync(String request)
        {
            WebRequest webRequest = WebRequest.Create("https://www.imdb.com/title/" + request);
            WebResponse webResponse = await webRequest.GetResponseAsync();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            String response = reader.ReadToEnd();
            reader.Close();
            return response;
        }

        public ICollection<DisplayableMovie> toDisplayable(ICollection<Movie> movies) {
            List<DisplayableMovie> result = new List<DisplayableMovie>();
            foreach (Movie movie in movies) {
                result.Add(new DisplayableMovie(movie));
            }
            return result;
        }
    }
}
