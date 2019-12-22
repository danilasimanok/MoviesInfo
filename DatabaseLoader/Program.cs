using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLoader
{
    class Program
    {
        private static void printPesponse(Object response)
        {
            if (response == null)
                Console.WriteLine("No data.");
            else
                Console.WriteLine(response);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Reloading DB.");
            IStorable.serializer = new JsonSerializer(new Type[1] { typeof(HashSet<String>)});
            /*using (ApplicationContext applicationContext = new ApplicationContext()) {
                var query = applicationContext.tags.Where(tagT => tagT.tag.Equals("1930s"));
                Tag t = query.FirstOrDefault();
                if (t != null)
                    t.restore();
                Console.WriteLine(t);
            }
            Console.ReadKey();
            return;*/
            using (ApplicationContext applicationContext = new ApplicationContext())
            {
                applicationContext.Database.EnsureDeleted();
            }
            using (ApplicationContext applicationContext = new ApplicationContext())
            {
                Console.WriteLine("Loading dicts...");

                Repository repository = new Repository(applicationContext);

                Console.WriteLine("Done.");

                String comand = null, request = null;
                Object response = null;
                do
                {
                    Console.WriteLine("Input comand (film, person, tag, exit):");
                    comand = Console.ReadLine();

                    if (!comand.Equals("exit"))
                    {
                        Console.WriteLine("Input request:");
                        request = Console.ReadLine();
                    }

                    if (comand.Equals("film"))
                    {
                        response = CollectionPrinter<Movie>.print(repository.selectMoviesByTitle(request));
                        Program.printPesponse(response);
                    }
                    else if (comand.Equals("person"))
                    {
                        response = repository.selectPersonByName(request);
                        Program.printPesponse(response);
                    }
                    else if (comand.Equals("tag"))
                    {
                        response = repository.selectTag(request);
                        Program.printPesponse(response);
                    }
                } while (!comand.Equals("exit"));

                Console.WriteLine("Creating new DB. Please, don't shut down the program.");
                repository.save();
                Console.WriteLine("Done.");
            }
        }
    }
}
