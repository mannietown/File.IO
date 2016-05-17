using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sender
{
    class Sender
    {
        //All characters stored in an array rather than a file. Note that it doesn't contain numbers. Just didn't bother adding them - you're welcome to
        readonly char[] AlphaNumericChars = new char[] { '!', '\"', '£', '$', '%', '^', '&', '*', '(', ')', '\'', '-', '_', '=',
            '`', '¬', '|', '\\', ',', '<', '.', '>', '/', '?', ';', ':', '@', '~', '#', ']', '[', '}', '{', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
            'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public SortedList AlphaKeyValue = new SortedList();

        //Consider changing the Random to a parameter for the GetKey method, and create the Random object when using DataToSortedList method as it is the only place it is used. 
        Random rand = new Random(); 

        //Ensures that there are no 2 keys with the same values
        public int GetKey(int CountLeft)
        {
            int randnum = rand.Next(0, CountLeft);
            while (AlphaKeyValue.ContainsValue(randnum))
            {
                randnum = rand.Next(0, CountLeft);
            }

            return randnum;
        }

        public void DataToSortedList()
        {
            AlphaKeyValue.Clear();

            for (int Index = 0; Index < AlphaNumericChars.Length; Index++)
            {
                AlphaKeyValue.Add(AlphaNumericChars[Index], GetKey(AlphaNumericChars.Length));
            }
        }

        /// <summary>
        /// Converts character to the Code and adds to the arraylist
        /// </summary>
        /// <param name="Message"></param>
        public string EncryptMessage(string Message)
        {
            string EncryptedMessage = "";

            foreach (char character in Message)
            {
                EncryptedMessage += AlphaKeyValue[character].ToString() + ',';
            }

            return EncryptedMessage;
        }

        //Converts Code in the arraylist back to char
        public string DecryptMessage(string EncryptedMessage)
        {
            string DecryptedMessage = "";

            foreach (string ID in EncryptedMessage.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                DecryptedMessage += AlphaKeyValue.GetKey(AlphaKeyValue.IndexOfValue(int.Parse(ID)));
            }

            return DecryptedMessage;
        }

        //Now what i want to do is store the current key/value pairs in a file so that the reciever can then copy them to a SortedList.
        //But i can't do this because i don't know how to write tables to files and retrieve the data as a table.

        //Problem is mate, if you store the key and the character together it's super easy to break your algorithm and work out what the original text was.

        public void PrintEncrypted(string FileLocation, string Message)
        {
            //Consider adding an If Directory.Exists(folder) thing

            using (StreamWriter writer = new StreamWriter(FileLocation))
            {
                writer.Write(Message);
            }
        }

        public void PrintKeyCode(string FileLocation)
        {
            //Consider adding an If Directory.Exists(folder) thing

            using (FileStream fs = new FileStream(FileLocation, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, AlphaKeyValue); //Turns list into raw data

                    byte[] BytesToWrite = ms.ToArray();
                    fs.Write(BytesToWrite, 0, BytesToWrite.Length);
                }
            }
        }

        public string LoadEncrypted(string EncryptedMessageLocation, string KeyCodeLocation)
        {
            if (!File.Exists(EncryptedMessageLocation) || !File.Exists(KeyCodeLocation))
            {
                throw new FileNotFoundException("Files missing for LoadEncrypted. Check file paths:" +
                    Environment.NewLine + EncryptedMessageLocation + Environment.NewLine + KeyCodeLocation);
            }

            //Load KeyCode
            using (FileStream fs = new FileStream(KeyCodeLocation, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                AlphaKeyValue = (SortedList)bf.Deserialize(fs); //Turns raw data into list
            }

            //Load encrypted message
            using (StreamReader reader = new StreamReader(EncryptedMessageLocation))
            {
                return DecryptMessage(reader.ReadToEnd());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sender send = new Sender();
            send.DataToSortedList();
            Console.WriteLine("Please enter the message you'd like to send:");
            string MessageToSend = Console.ReadLine();

            string Encrypted = send.EncryptMessage(MessageToSend); //Encrypted
            Console.WriteLine(Encrypted);
            Console.WriteLine(send.DecryptMessage(Encrypted));

            Console.ReadLine();
        }

    }
}
