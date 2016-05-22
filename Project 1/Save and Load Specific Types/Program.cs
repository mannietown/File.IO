using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Save_and_Load_Specific_Types
{
    class Program
    {
        static void Main(string[] args)
        {
            string FileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Test.txt";
            Save(FileLocation, 
                new string[] { "item 1", "item 2", "item 3", "item 4" }, 
                new double[] { 1, 2, 3, 4 });

            string[] IDs;
            double[] Numbers;
            Load(FileLocation, out IDs, out Numbers);

            for (int Index = 0; Index < IDs.Length; Index++)
                Console.WriteLine(IDs[Index] + ": " + Numbers[Index].ToString());

            Console.ReadLine();
        }

        static void Save(string FileLocation, string[] DoubleIDs, double[] Numbers)
        {
            if (DoubleIDs.Length != Numbers.Length)
                throw new ArgumentException("DoubleIDs length doesn't match ItemsToSave length");

            using (FileStream fs = new FileStream(FileLocation, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(DoubleIDs.Length);

                    for (int Index = 0; Index < DoubleIDs.Length; Index++)
                    {
                        bw.Write(DoubleIDs[Index]);
                        bw.Write(Numbers[Index]);
                    }
                }
            }
        }

        static void Load(string FileLocation, out string[] DoubleIDs, out double[] Numbers)
        {
            List<string> IDs = new List<string>();
            List<double> NumbersList = new List<double>();

            using (FileStream fs = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int Length = br.ReadInt32();
                    
                    for (int Index = 0; Index < Length; Index++)
                    {
                        IDs.Add(br.ReadString());
                        NumbersList.Add(br.ReadDouble());
                    }
                }
            }

            DoubleIDs = IDs.ToArray();
            Numbers = NumbersList.ToArray();
        }
    }
}