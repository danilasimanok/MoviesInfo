using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Movie> movies
        {
            get;
            set;
        }
        public DbSet<Person> persons
        {
            get;
            set;
        }
        public DbSet<Tag> tags
        {
            get;
            set;
        }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=moviesinfodb;Trusted_Connection=True;");
        }
    }
}
