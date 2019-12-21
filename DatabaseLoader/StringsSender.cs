using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DatabaseLoader
{
    public class StringsSender
    {
        private StreamReader reader;

        private LinkedList<IStringsRecipient> recipients;

        public StringsSender(String filename)
        {
            this.reader = new StreamReader(filename);
            this.recipients = new LinkedList<IStringsRecipient>();
        }

        public void addRecipient(IStringsRecipient recipient)
        {
            this.recipients.AddLast(recipient);
        }

        public bool removeRecipient(IStringsRecipient recepient)
        {
            return this.recipients.Remove(recepient);
        }

        public void read()
        {
            this.reader.ReadLine();
            String str = this.reader.ReadLine();
            while (str != null)
            {
                foreach (IStringsRecipient recipient in this.recipients)
                    recipient.receiveString(str);
                str = this.reader.ReadLine();
            }
            this.reader.Close();
        }
    }
}
