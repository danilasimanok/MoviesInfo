using DatabaseLoader;
using System;
using System.Collections.Concurrent;
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
        private static Regex IMG_REGEX = new Regex("\"image\": \".*\","),
            DESCRIPTION_REGEX = new Regex("\"description\": \".*\",");

        private static int IMG_BEG = 10,
            DESCRIPTION_BEG = 16;

        public async Task<String> executeRequestAsync(String request)
        {
            WebRequest webRequest = WebRequest.Create("https://www.imdb.com/title/" + request);
            WebResponse webResponse = await webRequest.GetResponseAsync();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            String response = reader.ReadToEnd();
            reader.Close();
            return response;
        }

        public String executeRequest(String request) { 
            WebRequest webRequest = WebRequest.Create("https://www.imdb.com/title/" + request);
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            String response = reader.ReadToEnd();
            reader.Close();
            return response;
        }

        private static String getSubstring(String str, Regex regex, int beg)
        {
            String result = regex.Match(str).ToString();
            return result.Substring(beg, result.Length - beg - 2);
        }

        public ICollection<DisplayableMovie> toDisplayable(ICollection<Movie> movies) {
            ConcurrentBag<DisplayableMovie> result = new ConcurrentBag<DisplayableMovie>();
            Parallel.ForEach(
                movies,
                (movie) => {
                    String imgUrl, description, response;
                    response = imgUrl = description = null;

                    bool webErrorOccured, imgErrorOccured, descriptionErrorOccured;
                    webErrorOccured = imgErrorOccured = descriptionErrorOccured = false;

                    try
                    {
                        response = this.executeRequest(movie.imdbId);
                    }
                    catch (Exception)
                    {
                        webErrorOccured = true;
                    }

                    try
                    {
                        imgUrl = Connection.getSubstring(response, Connection.IMG_REGEX, Connection.IMG_BEG);
                    }
                    catch (Exception)
                    {
                        imgErrorOccured = true;
                    }

                    try
                    {
                        description = Connection.getSubstring(response, Connection.DESCRIPTION_REGEX, Connection.DESCRIPTION_BEG);
                    }
                    catch (Exception)
                    {
                        descriptionErrorOccured = true;
                    }

                    DisplayableMovie displayableMovie = new DisplayableMovie(movie);
                    if (!webErrorOccured)
                    {
                        if (!imgErrorOccured)
                            displayableMovie.imgUrl = imgUrl;
                        if (!descriptionErrorOccured)
                            displayableMovie.description = description;
                    }
                    result.Add(displayableMovie);
                }
            );
            return result.ToArray();
        }
    }
}

/*List<DisplayableMovie> result = new List<DisplayableMovie>();
            Parallel.ForEach(movies,
                async (movie) =>
                {
                    String imgUrl, description, response;
                    response = imgUrl = description = null;

                    bool webErrorOccured, imgErrorOccured, descriptionErrorOccured;
                    webErrorOccured = imgErrorOccured = descriptionErrorOccured = false;
                    
                    try
                    {
                        response = await this.executeRequestAsync(movie.imdbId);
                    }
                    catch (Exception)
                    {
                        webErrorOccured = true;
                    }

                    try
                    {
                        imgUrl = Connection.getSubstring(response, Connection.IMG_REGEX, Connection.IMG_BEG);
                    }
                    catch (Exception)
                    {
                        imgErrorOccured = true;
                    }

                    try
                    {
                        description = Connection.getSubstring(response, Connection.DESCRIPTION_REGEX, Connection.DESCRIPTION_BEG);
                    }
                    catch (Exception)
                    {
                        descriptionErrorOccured = true;
                    }

                    DisplayableMovie displayableMovie = new DisplayableMovie(movie);
                    if (!webErrorOccured && !imgErrorOccured)
                        displayableMovie.imgUrl = imgUrl;
                    if (!webErrorOccured && !descriptionErrorOccured)
                        displayableMovie.description = description;
                    result.Add(displayableMovie);
                }
            );
            return result;*/
