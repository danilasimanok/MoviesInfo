using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatabaseLoader;

namespace MoviesInfo.Data
{
    public class Repository
    {
        private ApplicationContext context;

        public Repository() {
            this.context = new ApplicationContext();
        }

        public ICollection<Movie> GetMovies(string title)
        {
            
            ICollection<Movie> result = this.context.movies.Where(movie => movie.title.Equals(title)).ToList<Movie>();
            foreach (Movie movie in result)
                movie.restore();
            return result;
        }

        // плохо, но быстро (нет, не очень-то, на самом деле)
        public void fillSimilar(Movie movie){
            HashSet<string> similarSet = new HashSet<string>();
            foreach (string tag in movie.getTags())
                similarSet.UnionWith(this.GetTag(tag).getTitles());
            IList similar = this.getRandom(new List<string>(similarSet), Movie.similarMoviesCount);
            for (int i = 0; i < similar.Count; ++i)
                movie.similar[i] = (string)similar[i];
        }

        public ICollection<string> GetSimilar(IEnumerable<Movie> movies) {
            HashSet<string> similarSet = new HashSet<string>();
            foreach (Movie movie in movies) {
                this.fillSimilar(movie);
                similarSet.UnionWith(movie.similar);
            }
            similarSet.Remove(null);
            IList similar = this.getRandom(new List<string>(similarSet), Movie.similarMoviesCount);
            string[] result = new string[similar.Count];
            for (int i = 0; i < similar.Count; ++i)
                result[i] = (string)similar[i];
            return result;
        }

        public Person GetPerson(string name) {
            Person result = this.context.persons.Where(person => person.name.Equals(name)).FirstOrDefault();
            if (result != null)
                result.restore();
            return result;
        }

        public Tag GetTag(string tag) {
            Tag result = this.context.tags.Where(tagT => tagT.tag.Equals(tag)).FirstOrDefault();
            if (result != null)
                result.restore();
            return result;
        }

        private IList getRandom(IList ts, int count) {
            Object[] result;
            if (ts.Count > count)
            {
                result = new Object[count];
                int[] ints = new int[count];
                Random random = new Random();
                for (int i = 0; i < count; ++i)
                {
                    int rand;
                    do
                        rand = random.Next(ts.Count);
                    while (ints.Contains(rand));
                    ints[i] = rand;
                    result[i] = ts[rand];
                }
            }
            else
            {
                result = new Object[ts.Count];
                for (int i = 0; i < ts.Count; ++i)
                    result[i] = ts[i];
            }
            return result;
        } 
    }
}