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

        public void fillSimilar(Movie movie) {/* в следующих обновлениях */}

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
    }
}

/*List<Movie> result = new List<Movie>();

            Movie m1 = new Movie();
            m1.title = title;
            m1.ratings = 8;
            m1.addActor("Федя Дюдзе");
            m1.addActor("Мишель Д'Орош");
            m1.addDirector("Андрей Григорьев");
            m1.pictureUrl = title != null ? "http://gorod.tomsk.ru/uploads/11942/1240826431/869_7.jpg" : "https://cs5.pikabu.ru/post_img/2015/06/09/9/1433865319_2056920725.png";
            m1.description = "m1 description";

            Movie m2 = new Movie();
            m2.title = title;
            m2.ratings = 8;
            m2.addActor("Вячеслав Шлягович");
            m2.addActor("Мишель Зароков");
            m2.addDirector("Дмитрий Григорьев");
            m2.pictureUrl = title != null ? "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcStD-omsY28QJreiVeEakK5-NHPFPz-U7RUSu_tmFzDJPfgZ4ldwA&s" : "https://cs5.pikabu.ru/post_img/2015/06/09/9/1433865319_2056920725.png";
            m2.description = "m2 description";

            result.Add(m1);
            result.Add(m2);

            return result;*/
