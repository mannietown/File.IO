using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class Program
    {
        static void Main(string[] args)
        {

            
            
            
        }
    }

    static class DataIO
    {
        static readonly int Number;
        private static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Teaching\\Project 1";
        private static readonly string DatabaseFileLocation = AppDataFolder + "\\Database.dat";

        public static List<string> LoadDataFromFile()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            if (!File.Exists(DatabaseFileLocation))
                File.Create(DatabaseFileLocation);

            List<string> ReturnValue = new List<string>();
            using (StreamReader reader = new StreamReader(DatabaseFileLocation))
            {
                string Dataline = reader.ReadLine();

                while (Dataline != null)
                {
                    ReturnValue.Add(Dataline);
                    Dataline = reader.ReadLine();
                }
            }

            return ReturnValue;

        }


        public static void WriteDataToFile(List<string> list)
        {
            if (Directory.Exists(AppDataFolder))
            {
                using(StreamWriter sw = new StreamWriter(DatabaseFileLocation))
                {
                    foreach(string data in LoadDataFromFile())
                    {
                        sw.WriteLine(data);
                    }
                }
            }
        }
    }
}
