using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                bf.Serialize(fs, AlphaKeyValue); //Turns list into raw data
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

namespace BigData
{
    public class BigList<T> : IEnumerable where T : IComparable
    {
        protected List<List<T>> AllItems;
        protected bool issorted = false;

        /// <summary>
        /// Whether the data is sorted
        /// </summary>
        public bool IsSorted
        {
            get { return issorted; }
            set
            {
                if (value)
                    Sort();
                else
                    issorted = false;
            }
        }

        public T this[long index]
        {
            get
            {
                int AllItemsIndex = 0;
                long LengthSoFar = 0;
                while (AllItems[AllItemsIndex].Count + LengthSoFar < index)
                {
                    LengthSoFar += AllItems[AllItemsIndex].Count;
                    AllItemsIndex++;
                }

                return AllItems[AllItemsIndex][(int)(index - LengthSoFar)];
            }
            set
            {
                int AllItemsIndex = 0;
                long LengthSoFar = 0;
                while (AllItems.ElementAt(AllItemsIndex).Count() + LengthSoFar < index)
                {
                    LengthSoFar += AllItems.ElementAt(AllItemsIndex).Count();
                    AllItemsIndex++;
                }

                AllItems[AllItemsIndex][(int)(index - LengthSoFar)] = value;
            }
        }

        /// <summary>
        /// Initialize a new empty BigList. IsSorted is set to false
        /// </summary>
        public BigList()
        {
            AllItems = new List<List<T>>();
        }

        /// <summary>
        /// Initializes a new empty BigList, and sets the IsSorted property.
        /// </summary>
        /// <param name="Sort"></param>
        public BigList(bool Sort)
        {
            AllItems = new List<List<T>>();
            IsSorted = Sort;
        }

        /// <summary>
        /// Initialize a new BigList with an existing tiered list
        /// </summary>
        /// <param name="input"></param>
        /// <param name="AlreadySorted"></param>
        public BigList(List<List<T>> input, bool AlreadySorted)
        {
            AllItems = input.Cast<List<T>>().ToList();
            issorted = AlreadySorted;
        }

        /// <summary>
        /// Initialize a new BigList with an existing normal list
        /// </summary>
        /// <param name="input"></param>
        /// <param name="AlreadySorted"></param>
        public BigList(List<T> input, bool AlreadySorted)
        {
            AllItems = new List<List<T>>();
            AllItems.Add(input);
            issorted = AlreadySorted;
        }

        /// <summary>
        /// The number of items in the BigList
        /// </summary>
        public long Count
        {
            get
            {
                int ReturnValue = 0;
                foreach (List<T> li in AllItems)
                    ReturnValue += li.Count;
                return ReturnValue;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a new item in the appropriate position.
        /// </summary>
        /// <param name="ItemToAdd"></param>
        public void Add(T ItemToAdd)
        {
            if (AllItems.Count == 0)
            {
                AllItems.Add(new List<T>() { ItemToAdd });
                return;
            }

            if (issorted)
            {
                //Maintain order
                long Index = Find(ItemToAdd);

                if (Index > 0)
                    InsertAt(Index, ItemToAdd);
                else
                    InsertAt(~Index, ItemToAdd);
            }
            else
            {
                try
                {
                    AllItems[AllItems.Count - 1].Add(ItemToAdd);
                }
                catch (OutOfMemoryException)
                {
                    AllItems.Add(new List<T>() { ItemToAdd });
                    AllItems[AllItems.Count - 1].Add(ItemToAdd);
                }
            }
        }

        /// <summary>
        /// Adds an item assuming it belongs at the end of the array, ignoring whether the data is sorted.
        /// </summary>
        /// <param name="ItemToAdd"></param>
        public void LowLevelAdd(T ItemToAdd)
        {
            if (issorted)
            {
                issorted = false;
                Add(ItemToAdd);
                issorted = true;
            }
            else Add(ItemToAdd);
        }

        public bool Contains(T value)
        {
            return Find(value) >= 0;
        }

        /// <summary>
        /// Sorts the entirety of the data
        /// </summary>
        protected void Sort()
        {
            lock (this)
            {
                //Stack manager
                long StackCount = 0;
                BigList<long> StackLeft = new BigList<long>();
                BigList<long> StackRight = new BigList<long>();

                long left = 0;

                long right = Count - 1;

                if (right == -1) //No items in AllItems;
                {
                    issorted = true;
                    return;
                }

                QuicksortStart:;
                long i = left, j = right;
                T pivot = this[(left + right) / 2];

                while (i <= j)
                {
                    while (this[i].CompareTo(pivot) < 0)
                        i++;

                    while (this[j].CompareTo(pivot) > 0)
                        j--;

                    if (i <= j)
                    {
                        // Swap
                        T tmp = this[i];
                        this[i] = this[j];
                        this[j] = tmp;

                        i++;
                        j--;
                    }
                }

                // Recursive calls
                if (left < j)
                {
                    StackCount++;
                    StackRight.Add(right);
                    StackLeft.Add(left);

                    right = j;
                    goto QuicksortStart;
                }

                StackRelease:;
                if (i < right)
                {
                    left = i;
                    goto QuicksortStart;
                }

                if (StackCount > 0)
                {
                    StackCount--;

                    left = StackLeft[StackCount];
                    right = StackRight[StackCount];
                    StackLeft.RemoveAt(StackCount);
                    StackRight.RemoveAt(StackCount);
                    goto StackRelease;
                }

                issorted = true;
            }
        }

        /// <summary>
        /// Sorts, then removes duplicates.
        /// </summary>
        public void RemoveDuplicates()
        {
            if (!IsSorted)
                Sort();

            lock (this)
            {
                T CurrentItem = default(T);
                bool FirstItemFound = false;
                for (int Index = 0; Index < AllItems.Count; Index++)
                {
                    int SubIndex = 0;

                    if (FirstItemFound == false && AllItems[Index].Count > 0) //Not empty list
                    {
                        CurrentItem = AllItems[Index][0];
                        SubIndex++;
                        FirstItemFound = true;
                    }

                    if (FirstItemFound)
                    {
                        for (; SubIndex < AllItems[Index].Count; SubIndex++)
                        {
                            if (AllItems[Index][SubIndex].CompareTo(CurrentItem) == 0)
                            {
                                AllItems[Index].RemoveAt(SubIndex);
                                SubIndex--;
                            }
                            else
                            {
                                CurrentItem = AllItems[Index][SubIndex];
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Search for an item
        /// </summary>
        /// <param name="FindThis">The item to find</param>
        /// <returns></returns>
        public long Find(T FindThis)
        {
            if (IsSorted)
            {
                long min = 0;
                long N = Count;
                long max = N - 1;
                while (min <= max)
                {
                    long mid = (min + max) / 2;

                    int Compare = FindThis.CompareTo(this[mid]);
                    if (Compare > 0)
                        min = mid + 1;
                    else if (Compare < 0)
                        max = mid - 1;
                    else
                        return mid; //Found
                }

                return ~min; //Not found
            }
            else
            {
                long Index = 0;
                foreach (T Item in this)
                {
                    if (Item.CompareTo(FindThis) == 0)
                        return Index; //Found

                    Index++;
                }

                return -1; //Not found
            }
        }

        /// <summary>
        /// Inserts an item at a set position
        /// </summary>
        /// <param name="Index">The index at which to add the item</param>
        /// <param name="ItemToAdd"></param>
        private void InsertAt(long Index, T ItemToAdd)
        {
            if (AllItems.Count == 0)
            {
                AllItems.Add(new List<T>() { ItemToAdd });
                return;
            }

            int ParentListIndex = 0;
            long SumCountPastLists = 0;

            while (AllItems[ParentListIndex].Count + SumCountPastLists < Index)
            {
                SumCountPastLists += AllItems[ParentListIndex].Count;
                ParentListIndex++;
            }

            try
            {
                AllItems[ParentListIndex].Insert((int)(Index - SumCountPastLists), ItemToAdd);
            }
            catch (OutOfMemoryException)
            {
                T Buffer1 = ItemToAdd, Buffer2;
                for (; ParentListIndex < AllItems.Count; ParentListIndex++)
                {
                    for (int SubListIndex = (int)(Index - SumCountPastLists); SubListIndex < AllItems[ParentListIndex].Count - 1; SubListIndex++)
                    {
                        Buffer2 = AllItems[ParentListIndex][SubListIndex];
                        AllItems[ParentListIndex][SubListIndex] = Buffer1;

                        Buffer1 = AllItems[ParentListIndex][SubListIndex + 1];
                        AllItems[ParentListIndex][SubListIndex + 1] = Buffer2;
                    }
                }
            }
        }

        /// <summary>
        /// Removes an object (and all its occurences)
        /// </summary>
        /// <param name="ItemToRemove"></param>
        /// <param name="RemoveAllOccurences"></param>
        public void Remove(T ItemToRemove, bool RemoveAllOccurences)
        {
            lock (this)
            {
                long Index = Find(ItemToRemove);

                if (Index >= 0 && !RemoveAllOccurences)
                {
                    RemoveAt(Index);
                    return;
                }
                else
                    while (Index >= 0)
                    {
                        RemoveAt(Index);

                        Index = Find(ItemToRemove);
                    }
            }

        }

        /// <summary>
        /// Removes the first instance of an object
        /// </summary>
        /// <param name="ItemToRemove">The object to remove</param>
        public void Remove(T ItemToRemove)
        {
            Remove(ItemToRemove, false);
        }

        /// <summary>
        /// Removes an item at a set index
        /// </summary>
        /// <param name="Index"></param>
        public void RemoveAt(long Index)
        {
            long PastRecordsCount = 0;
            int ParentListIndex = 0;

            for (; PastRecordsCount + AllItems[ParentListIndex].Count < Index; ParentListIndex++)
                PastRecordsCount += AllItems[ParentListIndex].Count;

            AllItems[ParentListIndex].RemoveAt((int)(Index - PastRecordsCount));
        }

        /// <summary>
        /// Sets IsSorted to false, then randomly arranges all data
        /// </summary>
        public void Mix()
        {
            lock (this)
            {
                IsSorted = false;
                Random rnd = new Random();
                for (long Index = 0; Index < Count; Index++)
                {
                    long MoveTo = GetRandomLong(0, Count, rnd);

                    T Buffer = this[MoveTo];
                    this[MoveTo] = this[Index];
                    this[Index] = Buffer;
                }
            }
        }

        /// <summary>
        /// Used for Mix, getting a random location to move the current item to
        /// </summary>
        /// <param name="Minimum">Inclusive minimum</param>
        /// <param name="Maximum">Exclusive maximum</param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private static long GetRandomLong(long Minimum, long Maximum, Random rnd)
        {
            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (Maximum - Minimum)) + Minimum);
        }

        /// <summary>
        /// Saves the file in a BigList exclusive format
        /// </summary>
        /// <param name="FileLocation">Where to save the file</param>
        /// <param name="Headers">The headers for the BigList</param>
        public virtual void SaveSerial(string FileLocation)
        {
            lock (this)
            {
                if (File.Exists(FileLocation))
                    File.Delete(FileLocation);

                using (FileStream writer = new FileStream(FileLocation, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                    bf.Serialize(writer, issorted);
                    bf.Serialize(writer, AllItems);
                }
            }
        }

        /// <summary>
        /// Initializes a new BigList and loads the data from file
        /// </summary>
        /// <param name="FileLocation">The BigList data file</param>
        public virtual void LoadSerial(string FileLocation)
        {
            if (!File.Exists(FileLocation))
                throw new FileNotFoundException();

            using (FileStream reader = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                issorted = (bool)bf.Deserialize(reader);
                AllItems = (List<List<T>>)bf.Deserialize(reader);
            }
        }

        public virtual void ExportAsStrings(string Filelocation, params string[] Headers)
        {
            if (!Filelocation.Contains('\\'))
                throw new ArgumentException("Filelocation is invalid. Path is: " + Environment.NewLine + Filelocation);

            string Folder = Filelocation.Substring(0, Filelocation.LastIndexOf('\\'));
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            lock (this)
            {
                using (StreamWriter writer = new StreamWriter(Filelocation, false))
                {
                    string Header = "";
                    for (int HeaderIndex = 0; HeaderIndex < Headers.Length - 1; HeaderIndex++)
                    {
                        Header += Headers[HeaderIndex] + '\a';
                    }
                    Header += Headers[Headers.Length - 1];

                    writer.WriteLine(Header);

                    foreach (T DataItem in this)
                    {
                        writer.WriteLine(DataItem.ToString());
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (List<T> l in AllItems)
                l.Clear();

            AllItems.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            lock (AllItems)
            {
                for (int ParentListIndex = 0; ParentListIndex < AllItems.Count; ParentListIndex++)
                {
                    for (int SubListIndex = 0; SubListIndex < AllItems[ParentListIndex].Count; SubListIndex++)
                    {
                        yield return AllItems[ParentListIndex][SubListIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Convert BigList to a single dimensional array. Can cause OutOfMemoryException if BigList contains more than 2GB of data
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            lock (this)
            {
                T[] ReturnValue = new T[Count];

                long Index = 0;
                foreach (T Item in this)
                {
                    ReturnValue[Index] = Item;

                    Index++;
                }

                return ReturnValue;
            }
        }

        /// <summary>
        /// Converts BigList to a two dimensional array. Safer than a single dimensional array for large data segements, but far more difficult to maintain
        /// </summary>
        /// <returns></returns>
        public T[][] To2DArray()
        {
            lock (this)
            {
                T[][] ReturnValue = new T[AllItems.Count][];

                long Index = 0;
                foreach (List<T> ChildList in AllItems)
                {
                    ReturnValue[Index] = ChildList.ToArray();
                    Index++;
                }

                return ReturnValue;
            }
        }

        public static implicit operator T[] (BigList<T> Data)
        {
            return Data.ToArray();
        }

        public static implicit operator T[][] (BigList<T> Data)
        {
            return Data.To2DArray();
        }

        public static implicit operator List<T>(BigList<T> Data)
        {
            List<T> ReturnValue = new List<T>();
            foreach (List<T> li in Data.AllItems)
            {
                ReturnValue.AddRange(li);
            }
            return ReturnValue;
        }

        public static implicit operator List<List<T>>(BigList<T> Data)
        {
            return Data.AllItems;
        }
    }

    public class Database : BigList<DatabaseEntry>
    {
        private string filelocation;

        /// <summary>
        /// Where the database is currently stored
        /// </summary>
        public string FileLocation { get { return filelocation; } }

        private string[] headers;
        /// <summary>
        /// The title for each column
        /// </summary>
        public string[] Headers { get { return headers; } set { headers = value; } }

        private int columnsortedby;

        /// <summary>
        /// The column the database is currently sorted by (-2 means not sorted, -1 means sorted by UID)
        /// </summary>
        public int ColumnSortedBy
        {
            get
            {
                return columnsortedby;
            }
            set
            {
                if (value == -1)
                {
                    Sort();
                    columnsortedby = -1;
                }
                else if (value == -2)
                    columnsortedby = value;
                else
                    OrderBy(value);
            }
        }

        new public bool IsSorted { get { return ColumnSortedBy == -1; } set { ColumnSortedBy = -1; } }

        /// <summary>
        /// Loads a database from file
        /// </summary>
        /// <param name="FileLocation">The file location of the database</param>
        public Database(string FileLocation) : base()
        {
            filelocation = FileLocation;

            if (!File.Exists(FileLocation))
            {
                throw new FileNotFoundException("Database file missing from location:" + Environment.NewLine
                    + FileLocation);
            }
            else
                RestoreFrom(FileLocation);
        }

        /// <summary>
        /// Create a new database
        /// </summary>
        /// <param name="FileLocation">Where to store the new database</param>
        /// <param name="SortByColumn">Which column the data should be sorted by (-2 means not sorted, -1 means by UID)</param>
        /// <param name="Headers">The headers for the database</param>
        public Database(string FileLocation, int SortByColumn, params string[] Headers) : base(SortByColumn == -1)
        {
            filelocation = FileLocation;
            this.Headers = Headers;

            if (SortByColumn >= 0)
                OrderBy(SortByColumn);

            Save();
        }

        /// <summary>
        /// Create a new database from existing data
        /// </summary>
        /// <param name="FileLocation">Where to store the new database</param>
        /// <param name="SortByColumn">Which column the data should be sorted by (-2 means not sorted, -1 means by UID)</param>
        /// <param name="DataToLoad">The data to store in the new database</param>
        /// <param name="Headers">The headers for the database</param>
        public Database(string FileLocation, int SortByColumn, List<DatabaseEntry> DataToLoad, params string[] Headers) : base(DataToLoad, SortByColumn == -1)
        {
            filelocation = FileLocation;
            this.Headers = Headers;

            if (SortByColumn >= 0)
                OrderBy(SortByColumn);

            Save();
        }

        /// <summary>
        /// Load a serialized database (usually a previous backup version)
        /// </summary>
        /// <param name="FileLocation">The location of the serialized backup to restore from</param>
        public override void LoadSerial(string FileLocation)
        {
            if (!File.Exists(FileLocation))
                throw new FileNotFoundException("File missing for RestoreFrom:" + Environment.NewLine + FileLocation);

            using (FileStream fs = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
            {
                lock (this)
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    //Load whether database is sorted.
                    columnsortedby = (int)bf.Deserialize(fs);

                    //Load headers
                    headers = (string[])bf.Deserialize(fs);

                    //Load data
                    AllItems = (List<List<DatabaseEntry>>)bf.Deserialize(fs);
                }
            }
        }

        /// <summary>
        /// Restore the database from a backup
        /// </summary>
        /// <param name="FileLocation">The location of the backup to restore from</param>
        public void RestoreFrom(string FileLocation)
        {
            LoadSerial(FileLocation);
        }

        public void LoadTextDatabase(string FileLocation, bool AlreadySorted, bool AppendToCurrentData, Type[] ObjectTypes, params char[] Seperator)
        {
            LoadTextDatabase(FileLocation, AlreadySorted, AppendToCurrentData, ObjectTypes, -1, Seperator);
        }

        /// <summary>
        /// Loads a text based database that already has RecordUIDs associated with each record. 
        /// </summary>
        /// <param name="FileLocation">The location of the text-based database</param>
        /// <param name="AlreadySorted">Whether the database has already been sorted</param>
        /// <param name="AppendToCurrentData">Whether the data should be added onto the current data in this database, or overwrite the current database with this data</param>
        /// <param name="ObjectTypes">The types of objects to expect from the database (use typeof() keyword)</param>
        /// <param name="ColumnContainingUID">The column containing the UID</param>
        /// <param name="Seperator">The character(s) which seperate each column</param>
        public void LoadTextDatabase(string FileLocation, bool AlreadySorted, bool AppendToCurrentData, Type[] ObjectTypes, int ColumnContainingUID, params char[] Seperator)
        {
            if (ColumnContainingUID < -1)
                throw new IndexOutOfRangeException("ColumnContainingUID is out of range. Value is: " + ColumnContainingUID.ToString());

            if (!File.Exists(FileLocation))
                throw new FileNotFoundException("File for loading database not found. File address:" + Environment.NewLine +
                    FileLocation);

            long RecordUID = 0;

            lock (this)
            {
                if (!AppendToCurrentData)
                    Clear();
                else if (ColumnContainingUID == -1)
                {
                    foreach (DatabaseEntry de in this)
                        if (de.UID >= RecordUID)
                            RecordUID = de.UID + 1;
                }

                using (StreamReader reader = new StreamReader(FileLocation))
                {
                    string Dataline = reader.ReadLine();

                    if (ColumnContainingUID == -1)
                    {
                        headers = Dataline.Split(Seperator);
                    }
                    else
                    {
                        List<string> Headers = Dataline.Split(Seperator).ToList();
                        Headers.RemoveAt(ColumnContainingUID);
                        headers = Headers.ToArray();
                    }

                    Dataline = reader.ReadLine();
                    while (Dataline != null)
                    {
                        string[] StringFields = Dataline.Split(Seperator);

                        if (ColumnContainingUID == -1)
                        {
                            if (StringFields.Length != ObjectTypes.Length)
                                throw new IndexOutOfRangeException("Fields count is invalid for ObjectTypes count" + Environment.NewLine +
                                    "Record ID: " + RecordUID);
                        }
                        else
                        {
                            if (StringFields.Length != ObjectTypes.Length + 1)
                                throw new IndexOutOfRangeException("Fields count is invalid for ObjectTypes count" + Environment.NewLine +
                                    "Record: " + Dataline);
                        }

                        object[] Fields;

                        if (ColumnContainingUID == -1)
                            Fields = new object[StringFields.Length];
                        else
                            Fields = new object[StringFields.Length - 1];

                        bool RecordUIDFound = false;
                        for (int Index = 0; Index < StringFields.Length; Index++)
                        {
                            if (Index == ColumnContainingUID)
                            {
                                RecordUID = Convert.ToInt64(StringFields[Index]);
                                RecordUIDFound = true;
                            }
                            else
                            {
                                if (!RecordUIDFound)
                                    Fields[Index] = Convert.ChangeType(StringFields[Index], ObjectTypes[Index]);
                                else
                                    Fields[Index - 1] = Convert.ChangeType(StringFields[Index], ObjectTypes[Index - 1]);
                            }
                        }

                        if (AlreadySorted && !AppendToCurrentData)
                        {
                            LowLevelAdd(new DatabaseEntry(RecordUID, Fields));
                        }
                        else
                        {
                            Add(new DatabaseEntry(RecordUID, Fields));
                        }

                        if (ColumnContainingUID == -1)
                            RecordUID++;
                        Dataline = reader.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Saves changes.
        /// </summary>
        public void Save()
        {
            SaveSerial(filelocation);
        }

        /// <summary>
        /// Saves all data in a serialized format. Useful for loading back within this application, but is not cross-compatible.
        /// </summary>
        /// <param name="FileLocation">Where to save the serialized database</param>
        public override void SaveSerial(string FileLocation)
        {
            string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            if (File.Exists(FileLocation))
                File.Delete(FileLocation);

            lock (this)
            {
                using (FileStream fs = new FileStream(FileLocation, FileMode.CreateNew, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                    //Save whether database is sorted
                    bf.Serialize(fs, columnsortedby);

                    //Save headers
                    bf.Serialize(fs, headers);

                    //Save data
                    bf.Serialize(fs, AllItems);
                }
            }
        }

        /// <summary>
        /// Create a backup of the database. Uses the current filename with the current date and time added
        /// </summary>
        public void Backup()
        {
            string Filename = filelocation.Substring(filelocation.LastIndexOf('\\') + 1);
            string Folder = filelocation.Substring(0, filelocation.LastIndexOf('\\'));

            string Extension;
            if (Filename.Contains('.'))
            {
                int ExtensionStart = Filename.LastIndexOf('.');
                Extension = Filename.Substring(ExtensionStart);
                Filename = Filename.Substring(0, ExtensionStart);
            }
            else Extension = "";

            SaveSerial(Folder + '\\' + Filename + " - Backup at " + DateTime.Now + Extension);
        }

        /// <summary>
        /// Backup the database to another location. Saves the database in a serialized format
        /// </summary>
        /// <param name="FileLocation">Where to save the backup</param>
        public void Backup(string FileLocation)
        {
            SaveSerial(FileLocation);
        }

        /// <summary>
        /// Exports the full database as text (with headers)
        /// </summary>
        /// <param name="FileLocation">Where to export the database to</param>
        public void Export(string FileLocation)
        {
            int[] Columns;
            if (Count == 0)
                Columns = new int[0];
            else
                Columns = new int[this[0].Fields.Length];

            Export(FileLocation, Columns);
        }

        /// <summary>
        /// Exports selected columns from the database as text (with headers)
        /// </summary>
        /// <param name="FileLocation">Where to export the database</param>
        /// <param name="ColumnsToBackup">Which columns to include in the export</param>
        public void Export(string FileLocation, int[] ColumnsToBackup)
        {
            List<int> ColumnsToBackupL = ColumnsToBackup.ToList();

            lock (this)
            {
                for (int ColumnIndex = 0; ColumnIndex < ColumnsToBackupL.Count; ColumnIndex++)
                {
                    if (Count > 0 && ColumnsToBackupL[ColumnIndex] >= this[0].Fields.Length)
                        throw new IndexOutOfRangeException("Column index to backup is greater than the total number of columns");

                    for (int ColumnIndex2 = ColumnIndex + 1; ColumnIndex2 < ColumnsToBackupL.Count; ColumnIndex2++)
                    {
                        if (ColumnIndex == ColumnIndex2)
                            ColumnsToBackupL.RemoveAt(ColumnIndex2);
                    }
                }

                string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);

                using (StreamWriter stream = new StreamWriter(FileLocation))
                {
                    string Header = "";
                    for (int HeaderIndex = 0; HeaderIndex < Headers.Length - 1; HeaderIndex++)
                        Header += Headers[HeaderIndex];

                    Header += Headers[Headers.Length - 1];

                    stream.WriteLine(Header);

                    foreach (DatabaseEntry de in this)
                    {
                        stream.WriteLine(de.ToExportValue(','), ColumnsToBackupL.ToArray());
                    }
                }
            }
        }

        public void OrderBy(int ColumnID)
        {
            lock (this)
            {
                if (Count == 0)
                    return;

                //Check if the column is within the length of the DatabaseEntry fields
                if (Headers.Length < ColumnID)
                    throw new IndexOutOfRangeException("Column to OrderBy (" + ColumnID +
                        ") is greater than the number of fields (" + Headers.Length + ").");

                //Check if the column is IComparable
                if (!(this[0].Fields[ColumnID] is IComparable))
                    throw new ArgumentException("Column to compare (" + ColumnID + ": " + Headers[ColumnID] +
                        ") is not Comparable, and therefore not compatible with OrderBy");

                //Sort by the column
                //Stack manager
                long StackCount = 0;
                BigList<long> StackLeft = new BigList<long>();
                BigList<long> StackRight = new BigList<long>();

                long left = 0;

                long right = Count - 1;

                if (right == -1) //No items in AllItems;
                {
                    issorted = true;
                    return;
                }

                QuicksortStart:;
                long i = left, j = right;
                IComparable pivot = (IComparable)this[(left + right) / 2].Fields[ColumnID];

                while (i <= j)
                {
                    while (((IComparable)this[i].Fields[ColumnID]).CompareTo(pivot) < 0)
                        i++;

                    while (((IComparable)this[j].Fields[ColumnID]).CompareTo(pivot) > 0)
                        j--;

                    if (i <= j)
                    {
                        // Swap
                        DatabaseEntry tmp = this[i];
                        this[i] = this[j];
                        this[j] = tmp;

                        i++;
                        j--;
                    }
                }

                // Recursive calls
                if (left < j)
                {
                    StackCount++;
                    StackRight.Add(right);
                    StackLeft.Add(left);

                    right = j;
                    goto QuicksortStart;
                }

                StackRelease:;
                if (i < right)
                {
                    left = i;
                    goto QuicksortStart;
                }

                if (StackCount > 0)
                {
                    StackCount--;

                    left = StackLeft[StackCount];
                    right = StackRight[StackCount];
                    StackLeft.RemoveAt(StackCount);
                    StackRight.RemoveAt(StackCount);
                    goto StackRelease;
                }

                issorted = false;

                columnsortedby = ColumnID;
            }
        }
    }

    [Serializable]
    public class DatabaseEntry : IComparable, IComparer<DatabaseEntry>, IComparable<DatabaseEntry>
    {
        private long uid;
        public long UID { get { return uid; } }

        public object[] Fields;

        public object this[int Index]
        {
            get { return Fields[Index]; }
            set { Fields[Index] = value; }
        }

        public DatabaseEntry(long UID, params object[] Fields)
        {
            uid = UID;
            this.Fields = Fields;
        }

        public int CompareTo(object obj)
        {
            if (obj is DatabaseEntry)
            {
                return CompareTo((DatabaseEntry)obj);
            }
            else return UID.CompareTo(obj);
        }

        public int CompareTo(DatabaseEntry CompareToEntry)
        {
            return UID.CompareTo(CompareToEntry.UID);
        }

        /// <summary>
        /// Concatenates all items including the UID into a '\a' seperated string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ReturnValue = UID.ToString() + ',';

            for (int Index = 0; Index < Fields.Length - 1; Index++)
            {
                ReturnValue += Fields[Index].ToString() + ',';
            }
            ReturnValue += Fields[Fields.Length - 1];

            return ReturnValue;
        }

        /// <summary>
        /// Converts the item to an export value (like csv, but a custom seperator). The UID is not exported
        /// </summary>
        /// <param name="Seperator">The character to use as a seperator</param>
        /// <returns></returns>
        public string ToExportValue(char Seperator)
        {
            return ToExportValue(Seperator, false);
        }

        /// <summary>
        /// Converts the item to an export value (like csv, but a custom seperator). The UID is not exported. You can choose which fields to include in the export
        /// </summary>
        /// <param name="Seperator">The character to use as a seperator</param>
        /// <param name="IncludeUID">Whether to include the UID in the export</param>
        /// <param name="ColumnsToExport">The columns to be included in the export</param>
        /// <returns></returns>
        public string ToExportValue(char Seperator, bool IncludeUID, params int[] ColumnsToExport)
        {
            string ReturnValue = "";

            if (IncludeUID)
                ReturnValue += UID.ToString() + Seperator;

            for (int ColumnIndex = 0; ColumnIndex < ColumnsToExport.Length - 1; ColumnIndex++)
            {
                ReturnValue += Fields[ColumnsToExport[ColumnIndex]].ToString() + Seperator;
            }
            ReturnValue += Fields[Fields.Length - 1];

            return ReturnValue;
        }

        /// <summary>
        /// Converts the item to an export value (like csv, but a custom seperator). The UID is not exported. You can choose which fields to include in the export
        /// </summary>
        /// <param name="Seperator">The character to use as a seperator</param>
        /// <param name="IncludeUID">Whether to include the UID in the export</param>
        /// <returns></returns>
        public string ToExportValue(char Seperator, bool IncludeUID)
        {
            int[] AllColumns = new int[Fields.Length];
            for (int Index = 0; Index < Fields.Length; Index++)
                AllColumns[Index] = Index;

            return ToExportValue(Seperator, IncludeUID, AllColumns);
        }

        public int Compare(DatabaseEntry x, DatabaseEntry y)
        {
            return x.CompareTo(y);
        }
    }
}
