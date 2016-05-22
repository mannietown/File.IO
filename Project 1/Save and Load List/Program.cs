using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Save_and_Load_List
{
    class Program
    {
        static string FileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Test.txt";
        static void Main(string[] args)
        {
            List<string> Values;
            if (File.Exists(FileLocation))
                Values = (List<string>)LoadObjectFromFile(FileLocation);
            else
                Values = new List<string>();

            double db = 185;
            float ft = 156;
            
            string ValueToAdd = null;

            while (ValueToAdd == null || ValueToAdd != "") //Exit when the user does not enter a value
            {
                if (ValueToAdd != null)
                    Values.Add(ValueToAdd);

                Console.Write("Please enter a value to add to the list: ");
                ValueToAdd = Console.ReadLine();
                Console.WriteLine(DisplayList(Values));
            }

            SaveObjectToFile(FileLocation, Values);
            Console.ReadLine();
        }

        static string DisplayList(List<string> ListToDisplay)
        {
            string ReturnValue = "";
            foreach (string Item in ListToDisplay)
            {
                ReturnValue += Item.ToString() + Environment.NewLine;
            }

            return ReturnValue;
        }

        static object LoadObjectFromFile(string Filelocation)
        {
            using (FileStream fs = new FileStream(Filelocation, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(fs); //Turns raw data into list
            }
        }

        static void SaveObjectToFile(string FileLocation, object SaveThis)
        {
            using (FileStream fs = new FileStream(FileLocation, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                bf.Serialize(fs, SaveThis);
            }
        }
    }
}
