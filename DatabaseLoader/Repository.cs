using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLoader
{
    internal class PersonIdToTitleIdCreator : OneToManyDictionaryCreator
    {
        Dictionary<String, String> idToTitle;

        public PersonIdToTitleIdCreator(int keyNumber, int valueNumber, string pattern, StringsSender sender, Dictionary<String, String> idToTitle) : base(keyNumber, valueNumber, pattern, sender)
        {
            this.idToTitle = idToTitle;
        }

        public override void receiveString(string str)
        {
            String id = str.Substring(0, 9);
            if (!this.idToTitle.ContainsKey(id))
                return;
            base.receiveString(str);
        }
    }
    class Repository
    {
        private static String actorsDirectorsCodesIMDB = @"data_source\ActorsDirectorsCodes_IMDB.tsv",
            actorsDirectorsNamesIMDB = @"data_source\ActorsDirectorsNames_IMDB.tsv",
            linksIMDB_MovieLens = @"data_source\links_IMDB_MovieLens.tsv",
            movieCodesIMDB = @"data_source\MovieCodes_IMDB.tsv",
            ratingsIMDB = @"data_source\Ratings_IMDB.tsv",
            tagCodesMovieLens = @"data_source\TagCodes_MovieLens.tsv",
            tagScoresMovieLens = @"data_source\TagScores_MovieLens.tsv";

        protected Dictionary<String, String> nameToIdIMDB,
            idToNameIMDB;

        protected Dictionary<String, String> titleIdToRatingIMDB;

        protected Dictionary<String, String> iMDBToMovieLens,
            movieLensToIMDB;

        protected Dictionary<String, String> tagToIdMovieLens,
            idToTagMovieLens;

        protected Dictionary<String, String> idToTitleIMDB;
        protected Dictionary<String, HashSet<String>> titleToIdIMDB;

        protected Dictionary<String, HashSet<String>> titleIdToDirectorIdIMDB,
            directorIdToTitleIdIMDB;

        protected Dictionary<String, HashSet<String>> titleIdToActorIdIMDB,
            actorIdToTitleIdIMDB;

        protected Dictionary<String, HashSet<String>> tagIdToTitleIdMovieLens,
            titleIdToTagIdMovieLens;

        private ApplicationContext context;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            StringsSender movieCodesIMDBSender = new StringsSender(Repository.movieCodesIMDB),
                actorsDirectorsNamesIMDBSender = new StringsSender(Repository.actorsDirectorsNamesIMDB),
                ratingsIMDBSender = new StringsSender(Repository.ratingsIMDB),
                linksIMDB_MovieLensSender = new StringsSender(Repository.linksIMDB_MovieLens),
                tagCodesMovieLensSender = new StringsSender(Repository.tagCodesMovieLens),
                actorsDirectorsCodesIMDBSender = new StringsSender(Repository.actorsDirectorsCodesIMDB),
                tagScoresMovieLensSender = new StringsSender(Repository.tagScoresMovieLens);

            OneToOneDictionaryCreator nameToIdCreator = new OneToOneDictionaryCreator(1, 0, null, actorsDirectorsNamesIMDBSender),
                idToNameCreator = new OneToOneDictionaryCreator(0, 1, null, actorsDirectorsNamesIMDBSender);

            OneToOneDictionaryCreator titleIdToRatingCreator = new OneToOneDictionaryCreator(0, 1, null, ratingsIMDBSender);

            OneToOneDictionaryCreator iMDBToMovieLensCreator = new OneToOneDictionaryCreator(1, 0, null, linksIMDB_MovieLensSender),
                movieLensToIMDBCreator = new OneToOneDictionaryCreator(0, 1, null, linksIMDB_MovieLensSender);

            OneToOneDictionaryCreator tagToIdCreator = new OneToOneDictionaryCreator(1, 0, null, tagCodesMovieLensSender),
                idToTagCreator = new OneToOneDictionaryCreator(0, 1, null, tagCodesMovieLensSender);

            OneToManyDictionaryCreator titleToIdCreator = new OneToManyDictionaryCreator(2, 0, @"\t(RU|US)\t", movieCodesIMDBSender);
            OneToOneDictionaryCreator idToTitleCreator = new OneToOneDictionaryCreator(0, 2, @"\t(RU|US)\t", movieCodesIMDBSender);

            OneToManyDictionaryCreator tagIdToTitleIdCreator = new OneToManyDictionaryCreator(1, 0, @"\t(1\.0|0\.[5-9]+.*)", tagScoresMovieLensSender),
                titleIdToTagIdCreator = new OneToManyDictionaryCreator(0, 1, @"\t(1\.0|0\.[5-9]+.*)", tagScoresMovieLensSender);

            //NB!
            Action[] actions = new Action[] {
                movieCodesIMDBSender.read,
                actorsDirectorsNamesIMDBSender.read,
                ratingsIMDBSender.read,
                linksIMDB_MovieLensSender.read,
                tagCodesMovieLensSender.read,
                tagScoresMovieLensSender.read
            };
            Parallel.Invoke(actions);


            this.nameToIdIMDB = nameToIdCreator.createDictionary();
            this.idToNameIMDB = idToNameCreator.createDictionary();

            this.titleIdToRatingIMDB = titleIdToRatingCreator.createDictionary();

            this.iMDBToMovieLens = iMDBToMovieLensCreator.createDictionary();
            this.movieLensToIMDB = movieLensToIMDBCreator.createDictionary();

            this.tagToIdMovieLens = tagToIdCreator.createDictionary();
            this.idToTagMovieLens = idToTagCreator.createDictionary();

            this.titleToIdIMDB = titleToIdCreator.createDictionary();
            this.idToTitleIMDB = idToTitleCreator.createDictionary();

            OneToManyDictionaryCreator directorIdToTitleIdCreator = new PersonIdToTitleIdCreator(2, 0, @"\tdirector\t", actorsDirectorsCodesIMDBSender, idToTitleIMDB),
                titleIdToDirectorIdCreator = new PersonIdToTitleIdCreator(0, 2, @"\tdirector\t", actorsDirectorsCodesIMDBSender, idToTitleIMDB);

            OneToManyDictionaryCreator actorIdToTitleIdCreator = new PersonIdToTitleIdCreator(2, 0, @"\tact(or|ress){1}\t", actorsDirectorsCodesIMDBSender, idToTitleIMDB),
                titleIdToActorIdCreator = new PersonIdToTitleIdCreator(0, 2, @"\tact(or|ress){1}\t", actorsDirectorsCodesIMDBSender, idToTitleIMDB);

            //NBB!
            actorsDirectorsCodesIMDBSender.read();

            this.directorIdToTitleIdIMDB = directorIdToTitleIdCreator.createDictionary();
            this.titleIdToDirectorIdIMDB = titleIdToDirectorIdCreator.createDictionary();

            this.actorIdToTitleIdIMDB = actorIdToTitleIdCreator.createDictionary();
            this.titleIdToActorIdIMDB = titleIdToActorIdCreator.createDictionary();

            this.tagIdToTitleIdMovieLens = tagIdToTitleIdCreator.createDictionary();
            this.titleIdToTagIdMovieLens = titleIdToTagIdCreator.createDictionary();
        }

        public virtual LinkedList<Movie> selectMoviesByTitle(String title)
        {
            LinkedList<Movie> result = new LinkedList<Movie>();
            if (!this.titleToIdIMDB.ContainsKey(title))
                return null;
            HashSet<String> titleIds = this.titleToIdIMDB[title];
            foreach (String titleId in titleIds)
            {
                Movie movie = new Movie();
                movie.title = title;
                movie.imdbId = titleId;
                if (this.titleIdToRatingIMDB.ContainsKey(titleId))
                    movie.ratings = Double.Parse(this.titleIdToRatingIMDB[titleId], System.Globalization.CultureInfo.InvariantCulture);
                if (this.titleIdToActorIdIMDB.ContainsKey(titleId))
                    foreach (String actorId in this.titleIdToActorIdIMDB[titleId])
                        if (this.idToNameIMDB.ContainsKey(actorId))
                            movie.addActor(this.idToNameIMDB[actorId]);
                if (this.titleIdToDirectorIdIMDB.ContainsKey(titleId))
                    foreach (String directorId in this.titleIdToDirectorIdIMDB[titleId])
                        if (this.idToNameIMDB.ContainsKey(directorId))
                            movie.addDirector(this.idToNameIMDB[directorId]);
                String movieLensId = null;
                if (this.iMDBToMovieLens.ContainsKey(titleId.Substring(2)))
                    movieLensId = this.iMDBToMovieLens[titleId.Substring(2)];
                if ((movieLensId != null) && this.titleIdToTagIdMovieLens.ContainsKey(movieLensId))
                    foreach (String tagId in this.titleIdToTagIdMovieLens[movieLensId])
                        if (this.idToTagMovieLens.ContainsKey(tagId))
                            movie.addTag(this.idToTagMovieLens[tagId]);
                result.AddLast(movie);
            }
            return result;
        }

        public virtual Person selectPersonByName(String name)
        {
            Person result = new Person();
            result.name = name;
            String personId = this.nameToIdIMDB.ContainsKey(name) ? this.nameToIdIMDB[name] : null;
            if (personId == null)
                return null;
            if (this.actorIdToTitleIdIMDB.ContainsKey(personId))
                foreach (String titleId in this.actorIdToTitleIdIMDB[personId])
                    if (this.idToTitleIMDB.ContainsKey(titleId))
                        result.addPlayedTitle(this.idToTitleIMDB[titleId]);
            if (this.directorIdToTitleIdIMDB.ContainsKey(personId))
                foreach (String titleId in this.directorIdToTitleIdIMDB[personId])
                    if (this.idToTitleIMDB.ContainsKey(titleId))
                        result.addDirectedTitle(this.idToTitleIMDB[titleId]);
            return result;
        }

        public virtual Tag selectTag(String tag)
        {
            Tag result = new Tag();
            result.tag = tag;
            String tagId = this.tagToIdMovieLens.ContainsKey(tag) ? this.tagToIdMovieLens[tag] : null;
            if (tagId == null)
                return null;
            if (this.tagIdToTitleIdMovieLens.ContainsKey(tagId))
                foreach (String movieLensId in this.tagIdToTitleIdMovieLens[tagId])
                {
                    String titleId = this.movieLensToIMDB.ContainsKey(movieLensId) ? "tt" + this.movieLensToIMDB[movieLensId] : null;
                    if (titleId == null)
                        continue;
                    if (this.idToTitleIMDB.ContainsKey(titleId))
                        result.addTitle(this.idToTitleIMDB[titleId]);
                }
            return result;
        }

        protected virtual void fillSimilars(Movie movie) {
            HashSet<Movie> similar = new HashSet<Movie>();
            HashSet<String> similarTitles = new HashSet<String>();
            foreach (String tag in movie.getTags())
                similarTitles.UnionWith(this.selectTag(tag).getTitles());
            foreach (String similarTitle in similarTitles)
                similar.UnionWith(this.selectMoviesByTitle(similarTitle));
            Movie[] sorted = similar.ToArray();
            Array.Sort(sorted, new MovieComparer(movie));
            int i = 0;
            while ((i < Movie.similarMoviesCount) && (i < sorted.Length))
                movie.similar[i] = sorted[i++].title;
        }

        public LinkedList<Movie> selectMoviesWithSimilar(String title) {
            LinkedList<Movie> result = this.selectMoviesByTitle(title);
            foreach (Movie movie in result)
                this.fillSimilars(movie);
            return result;
        }

        public void save()
        {
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
            int i = 0;
            foreach (String title in this.titleToIdIMDB.Keys)
                foreach (Movie movie in this.selectMoviesByTitle(title)) {
                    //this.fillSimilars(movie);
                    movie.save();
                    this.context.movies.Add(movie);
                }
            this.context.ChangeTracker.DetectChanges();
            this.context.SaveChanges();
            foreach (String name in this.nameToIdIMDB.Keys)
            {
                Person person = this.selectPersonByName(name);
                person.save();
                this.context.persons.Add(person);
                i = (i + 1) % 750000;
                if (i == 0)
                {
                    this.context.ChangeTracker.DetectChanges();
                    this.context.SaveChanges();
                }
            }
            this.context.ChangeTracker.DetectChanges();
            this.context.SaveChanges();
            foreach (String tag in this.tagToIdMovieLens.Keys) {
                Tag t = this.selectTag(tag);
                t.save();
                this.context.tags.Add(t);
            }   
            this.context.ChangeTracker.DetectChanges();
            this.context.SaveChanges();
        }
    }
}
