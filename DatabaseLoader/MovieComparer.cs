using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    class MovieComparer : IComparer<Movie>
    {
        private Movie reference;

        public MovieComparer(Movie reference)
        {
            this.reference = reference;
        }

        public int Compare(Movie x, Movie y)
        {
            double xGrade = Movie.estimate(this.reference, x) / 2 + x.ratings / 20,
                yGrade = Movie.estimate(this.reference, y) / 2 + y.ratings / 20;
            return Math.Sign(xGrade - yGrade);
        }
    }
}
