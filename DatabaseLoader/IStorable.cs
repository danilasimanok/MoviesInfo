using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public interface IStorable
    {
        public static JsonSerializer serializer;
        public void save();
        public void restore();
    }
}
