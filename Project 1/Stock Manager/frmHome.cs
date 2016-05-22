using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Stock_Manager
{
    public partial class frmHome : Form
    {
        List<DataEntry> MainDatabase;
        public frmHome()
        {
            InitializeComponent();
            try
            {
                MainDatabase = DataIO.Load();
            }
            catch (FileNotFoundException)
            { MainDatabase = new List<DataEntry>(); }

            bsDatabase.DataSource = MainDatabase;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MainDatabase.Add(new DataEntry(txtDescription.Text, (double)numPrice.Value, (int)numStock.Value));
            
            bsDatabase.DataSource = null;
            bsDatabase.DataSource = MainDatabase;
        }
    }

    public class DataEntry
    {
        string objectdescription;
        public string ObjectDescription { get { return objectdescription; } set { objectdescription = value; } }
        DateTime laststockadded;
        public DateTime LastStockAdded { get { return laststockadded; } }
        private int currentstock = 0;
        public int CurrentStock
        {
            get { return currentstock; }
            set
            {
                if (value > currentstock)
                {
                    laststockadded = DateTime.Now;
                    currentstock = value;
                }
                else
                    currentstock = value;
            }
        }

        private double price = 0;
        public double Price { get { return price; } set { price = Math.Round(value, 2); } }

        public DataEntry()
        {
            objectdescription = "";
            currentstock = 0;
        }

        public DataEntry(string ObjectDescription, double Price, int CurrentStock)
        {
            objectdescription = ObjectDescription;
            this.Price = Price;
            laststockadded = DateTime.Now;
            this.CurrentStock = CurrentStock;
        }

        public double ApplyDiscount(int PercentageOff)
        {
            Price = Price * ((100 - PercentageOff) / 100);
            return Price;
        }
    }

    static class DataIO
    {
        private static string FileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Stock Manager\\Database.dat";

        public static List<DataEntry> Load()
        {
            if (!File.Exists(FileLocation))
                throw new FileNotFoundException("Database file not found: " + Environment.NewLine + FileLocation);

            using (FileStream reader = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                return (List<DataEntry>)bf.Deserialize(reader);
            }
        }

        public static void Save(List<DataEntry> EntriesToSave)
        {
            string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            using (FileStream writer = new FileStream(FileLocation, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                bf.Serialize(writer, EntriesToSave);
            }
        }
    }
}
