using DatabaseLoader;
using System;

namespace MoviesInfo.Data
{
    public class DisplayableMovie: Movie
    {
        public String imgUrl { get; set; }
        public String description { get; set; }

        public DisplayableMovie(Movie movie) {
            this.title = movie.title;
            this.imdbId = movie.imdbId;
            this.ratings = movie.ratings;
            foreach (String actor in movie.getActors())
                this.addActor(actor);
            foreach (String director in movie.getDirectors())
                this.addDirector(director);
            foreach (String tag in movie.getTags())
                this.addTag(tag);
            this.similar = movie.similar;
        }
    }
}
