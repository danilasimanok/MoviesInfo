using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public class Movie: IStorable
    {
        public const int similarMoviesCount = 10;
        public int Id { get; set; }
        public String imdbId { get; set; }

        public String title { get; set; }

        private HashSet<String> actors,
            directors;
        public String actorsSerialized { get; set; }
        public String directorsSerialized {get; set;}

        private HashSet<String> tags;
        public String tagsSerialized { get; set; }

        public double ratings { get; set; }

        public String[] similar;
        //public String similarSerialized;

        public Movie()
        {
            this.actors = new HashSet<String>();
            this.directors = new HashSet<string>();
            this.tags = new HashSet<String>();
            this.similar = new String[Movie.similarMoviesCount];
        }

        public void addActor(String name) => this.actors.Add(name);

        public HashSet<String> getActors() => new HashSet<String>(this.actors);

        public void removeActor(String name) => this.actors.Remove(name);

        public void addDirector(String name) => this.directors.Add(name);

        public HashSet<String> getDirectors() => new HashSet<String>(this.directors);

        public void removeDirector(String name) => this.directors.Remove(name);

        public void addTag(String tag) => this.tags.Add(tag);

        public HashSet<String> getTags() => new HashSet<String>(this.tags);

        public void removeTag(String tag) => this.tags.Remove(tag);

        public static double estimate(Movie m1, Movie m2)
        {
            int sum = m1.actors.Count >= m2.actors.Count ? m1.actors.Count : m2.actors.Count;
            sum += m1.tags.Count >= m2.tags.Count ? m1.tags.Count : m2.tags.Count;
            if (sum == 0)
                return 1;
            double similarities = 0;
            foreach (String actor in m1.actors)
                similarities += m2.actors.Contains(actor) ? 1 : 0;
            foreach (String tag in m1.tags)
                similarities += m2.tags.Contains(tag) ? 1 : 0;
            return similarities / sum;
        }

        public void restore()
        {
            this.actors = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.actorsSerialized);
            this.directors = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.directorsSerialized);
            this.tags = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.tagsSerialized);
            //this.similar = (String[])IStorable.serializer.deserialize<Movie[]>(this.similarSerialized);
        }

        public void save() {
            this.actorsSerialized = IStorable.serializer.serialize<HashSet<String>>(this.actors);
            this.directorsSerialized = IStorable.serializer.serialize<HashSet<String>>(this.directors);
            this.tagsSerialized = IStorable.serializer.serialize<HashSet<String>>(this.tags);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.AppendLine("TITLE -- " + this.title);
            builder.AppendLine("IMDB ID -- " + this.imdbId);
            builder.AppendLine("RATINGS -- " + this.ratings);
            builder.AppendLine("ACTORS:");
            foreach (String actor in this.actors)
                builder.AppendLine("\t" + actor);
            builder.AppendLine("DIRECTORS:");
            foreach (String director in this.directors)
                builder.AppendLine("\t" + director);
            builder.AppendLine("TAGS:");
            foreach (String tag in this.tags)
                builder.AppendLine("\t" + tag);
            if (this.similar[0] != null)
                foreach (String similar in this.similar)
                    builder.AppendLine("\t" + similar);
            builder.AppendLine("----------");
            return builder.ToString();
        }
    }
}
