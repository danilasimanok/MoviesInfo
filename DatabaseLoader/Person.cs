using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLoader
{
    public class Person: IStorable
    {
        public int Id { get; set; }

        public String name { get; set; }

        HashSet<String> playedIn, 
            directed;
        public String playedInSerialized { get; set; }
        public String directedSerialized { get; set; }

        public Person()
        {
            this.playedIn = new HashSet<String>();
            this.directed = new HashSet<String>();
        }

        public void addPlayedTitle(String title) => this.playedIn.Add(title);

        public HashSet<String> getPlayedInTitles() => new HashSet<string>(this.playedIn);

        public void removePlayedInTitle(String title) => this.playedIn.Remove(title);

        public void addDirectedTitle(String title) => this.directed.Add(title);

        public HashSet<String> getDirectedTitles() => new HashSet<string>(this.directed);

        public void removeDirectedTitle(String title) => this.directed.Remove(title);

        public void save() {
            this.playedInSerialized = IStorable.serializer.serialize<HashSet<String>>(this.playedIn);
            this.directedSerialized = IStorable.serializer.serialize<HashSet<String>>(this.directed);
        }

        public void restore() {
            this.playedIn = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.playedInSerialized);
            this.directed = (HashSet<String>)IStorable.serializer.deserialize<HashSet<String>>(this.directedSerialized);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.AppendLine("NAME --" + this.name);
            builder.AppendLine("PLAYED IN:");
            foreach (String title in this.playedIn)
                builder.AppendLine("\t" + title);
            builder.AppendLine("DIRECTED:");
            foreach (String title in this.directed)
                builder.AppendLine("\t" + title);
            builder.AppendLine("----------");
            return builder.ToString();
        }
    }
}
