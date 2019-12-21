using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesInfo.Data
{
    public class CollectionPrinter
    {
        public static string Print<T>(ICollection<T> ts) {
            if(ts == null)
                return "No data.";
            StringBuilder builder = new StringBuilder(10240);
            foreach (T t in ts)
                builder.Append(t.ToString() + ", ");
            return builder.ToString();
        }
    }
}
