using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DatabaseLoader
{
    public class JsonSerializer
    {
        private Dictionary<Type, DataContractJsonSerializer> serializers;
        private MemoryStream memoryStream;
        private StreamReader reader;
        private long position;
        private static int CAPACITY = Int32.MaxValue / 10000;

        public JsonSerializer(IEnumerable<Type> types)
        {
            this.serializers = new Dictionary<Type, DataContractJsonSerializer>();
            this.memoryStream = new MemoryStream(JsonSerializer.CAPACITY * 3 / 2);
            this.reader = new StreamReader(this.memoryStream);
            this.position = 0;
            foreach (Type t in types)
                serializers.Add(t, new DataContractJsonSerializer(t));
        }

        public void addType(Type t)
        {
            this.serializers.Add(t, new DataContractJsonSerializer(t));
        }

        public void removeType(Type t)
        {
            this.serializers.Remove(t);
        }

        public String serialize<T>(T obj)
        {
            if (!this.serializers.ContainsKey(typeof(T)))
                throw new KeyNotFoundException();
            this.position = this.memoryStream.Position;
            this.serializers[typeof(T)].WriteObject(this.memoryStream, obj);
            this.memoryStream.Position = this.position;
            String result = this.reader.ReadToEnd();
            if (this.position > JsonSerializer.CAPACITY)
            {
                this.memoryStream.Close();
                this.reader.Close();
                this.memoryStream = new MemoryStream(JsonSerializer.CAPACITY * 3 / 2);
                this.reader = new StreamReader(this.memoryStream);
            }
            return result;
        }

        public void Close()
        {
            this.memoryStream.Close();
            this.reader.Close();
        }

        public Object deserialize<T>(String serialized)
        {
            if (!this.serializers.ContainsKey(typeof(T)))
                throw new KeyNotFoundException();
            byte[] byteArray = Encoding.UTF8.GetBytes(serialized);
            return this.serializers[typeof(T)].ReadObject(new MemoryStream(byteArray));
        }
    }
}