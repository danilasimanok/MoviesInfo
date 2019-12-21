using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public class Tag: IStorable
    {
        public int Id { get; set; }

        public String tag { get; set; }

        HashSet<String> titles;
        public String titlesSerialized;

        public Tag()
        {
            this.titles = new HashSet<String>();
        }

        public void addTitle(String title) => this.titles.Add(title);

        public HashSet<String> getTitles() => new HashSet<String>(this.titles);

        public void removeTitle(String title) => this.titles.Remove(title);

        public void save()
        {
            this.titlesSerialized = IStorable.serializer.serialize<HashSet<String>>(this.titles);
        }

        public void restore()
        {
            this.titles = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.titlesSerialized);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.AppendLine("TAG -- " + this.tag);
            builder.AppendLine("TITLES:");
            foreach (String title in this.titles)
                builder.AppendLine("\t" + title);
            builder.AppendLine("----------");
            return builder.ToString();
        }
    }
}
