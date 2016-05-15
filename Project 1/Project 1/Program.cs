﻿using System;
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
            Console.WriteLine("Loading...");
            List<string> AllData = DataIO.LoadDataFromFile();

            foreach (string Entry in AllData)
            {
                Console.WriteLine(Entry);
            }

            while (true)
            {
                Console.Write("Please enter a task: (? for help) ");
                ConsoleKeyInfo ck = Console.ReadKey();
                Console.WriteLine();

                switch (ck.Key)
                {
                    case ConsoleKey.A:
                        Console.Write("Record to add: ");
                        string RecordToAdd = Console.ReadLine();
                        DataIO.AddRecordToFile(RecordToAdd);
                        break;
                    case ConsoleKey.D:

                        break;
                    case ConsoleKey.S:

                        break;
                    default:
                        DisplayHelp();
                        break;
                }
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("A: Add a new record");
            Console.WriteLine("D: Delete a record if it exists");
            Console.WriteLine("S: Sort all records");
            Console.WriteLine("?: Display this menu");
        }
    }

    static class DataIO
    {
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
        
        public static void WriteDataToFile(List<string> DataToWrite)
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            using (StreamWriter sw = new StreamWriter(DatabaseFileLocation))
            {
                foreach (string data in DataToWrite)
                {
                    sw.WriteLine(data);
                }
            }
        }

        public static void AddRecordToFile(params string[] RecordsToAdd)
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            using (StreamWriter sw = new StreamWriter(DatabaseFileLocation, true /*Adds to the end of the file*/))
            {
                foreach(string Record in RecordsToAdd)
                {
                    sw.WriteLine(Record);
                }
            }
        }

        public static void DeleteRecord(params string[] RecordsToRemove)
        {

        }
    }
}
