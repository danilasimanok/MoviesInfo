using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public static class CollectionPrinter<T>
    {
        public static String print(ICollection<T> collection)
        {
            if (collection == null)
                return null;
            StringBuilder builder = new StringBuilder(10240);
            foreach (T t in collection)
                builder.AppendLine(t.ToString());
            return builder.ToString();
        }
    }
}