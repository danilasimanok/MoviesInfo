using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DatabaseLoader
{
    struct KeyValueNumbersPair
    {
        public int keyNumber;
        public int valueNumber;

        public KeyValueNumbersPair(int keyNumber, int valueNumber)
        {
            this.keyNumber = keyNumber;
            this.valueNumber = valueNumber;
        }
    }

    abstract class DictionaryCreator<V> : IStringsRecipient
    {

        protected KeyValueNumbersPair pair;

        protected Regex regex;

        protected Dictionary<String, V> result;

        public DictionaryCreator(int keyNumber, int valueNumber, String pattern, StringsSender sender)
        {
            this.pair = new KeyValueNumbersPair(keyNumber, valueNumber);
            this.regex = pattern == null ? null : new Regex(pattern);
            this.result = new Dictionary<string, V>();
            sender.addRecipient(this);
        }

        public Dictionary<String, V> createDictionary()
        {
            return this.result;
        }

        public virtual void receiveString(string str)
        {
            if (this.regex == null || this.regex.IsMatch(str))
            {
                this.processString(str.Split("\t".ToCharArray()));
            }

        }

        protected abstract void processString(string[] splitted);
    }

    class OneToOneDictionaryCreator : DictionaryCreator<String>
    {
        public OneToOneDictionaryCreator(int keyNumber, int valueNumber, string pattern, StringsSender sender) : base(keyNumber, valueNumber, pattern, sender)
        {
        }

        protected override void processString(string[] splitted)
        {
            result[splitted[this.pair.keyNumber]] = splitted[this.pair.valueNumber];
        }
    }

    class OneToManyDictionaryCreator : DictionaryCreator<HashSet<String>>
    {
        public OneToManyDictionaryCreator(int keyNumber, int valueNumber, string pattern, StringsSender sender) : base(keyNumber, valueNumber, pattern, sender)
        {
        }

        protected override void processString(string[] splitted)
        {
            if (result.ContainsKey(splitted[this.pair.keyNumber]))
                result[splitted[this.pair.keyNumber]].Add(splitted[this.pair.valueNumber]);
            else
                result[splitted[this.pair.keyNumber]] = new HashSet<string>() { splitted[this.pair.valueNumber] };
        }
    }
}
