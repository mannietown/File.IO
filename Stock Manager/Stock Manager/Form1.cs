using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Stock_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    class Stock
    {
        public List<ScheduledStockChange> ScheduledStockChanges;
        public List<StockItem> AllStock;
        private string FileLocation;

        public Stock(string FileLocation, bool CreateNew)
        {
            this.FileLocation = FileLocation;
            if (CreateNew)
            {
                ScheduledStockChanges = new List<ScheduledStockChange>();
                AllStock = new List<StockItem>();
            }
             else
                Load(FileLocation);
        }

        public void PerformScheduledStockChanges()
        {
            for (int Index = 0; Index < ScheduledStockChanges.Count; Index++)
            {
                if (ScheduledStockChanges[Index].ItemHasArrived)
                {
                    //Perform stock change
                    foreach (StockItem si in AllStock)
                    {
                        for (int Index2 = 0; Index2 < Index; Index2++)
                        {
                            if (si.ItemID == ScheduledStockChanges[Index].ItemsBeingChanged[Index2])
                                si.NumberInStock += ScheduledStockChanges[Index].NumberToAddRemove;
                        }
                    }

                    ScheduledStockChanges.RemoveAt(Index);
                    Index--;
                }
            }
        }

        public List<ScheduledStockChange> GetOverdueOrders()
        {
            List<ScheduledStockChange> ReturnValue = new List<ScheduledStockChange>();

            DateTime Current = DateTime.Now;
            foreach (ScheduledStockChange ssc in ScheduledStockChanges)
            {
                if (ssc.OrderETA > Current)
                {
                    ReturnValue.Add(ssc);
                }
            }

            return ReturnValue;
        }

        public void Save()
        {
            Backup(FileLocation);
        }

        public void Backup(string FileLocation)
        {
            string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));

            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            using (FileStream fs = new FileStream(FileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

                bf.Serialize(fs, AllStock);
                bf.Serialize(fs, ScheduledStockChanges);
            }
        }
        
        public void Load()
        {
            Load(FileLocation);
        }

        public void Load(string FileLocation)
        {
            using (FileStream fs = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                AllStock = (List<StockItem>)bf.Deserialize(fs);
                ScheduledStockChanges = (List<ScheduledStockChange>)bf.Deserialize(fs);
            }
        }
    }

    [Serializable]
    class StockItem
    {
        public long NumberInStock { get { return numberinstock; } set { numberinstock = value; } }
        private long numberinstock = 0;

        public string ItemID { get { return itemid; } set { itemid = value; } }
        private string itemid = "";

        public double ItemValue { get { return itemvalue; } set { itemvalue = Math.Round(value, 2); } }
        private double itemvalue = 0;

        public StockItem(string ItemID, long NumberInStock, double ItemValue)
        {
            this.ItemID = ItemID;
            this.NumberInStock = NumberInStock;
            this.ItemValue = ItemValue;
        }
    }

    [Serializable]
    class ScheduledStockChange
    {
        public DateTime OrderETA { get { return ordereta; } set { ordereta = value; } }
        private DateTime ordereta;

        List<StringAndInt> StocksToChange = new List<StringAndInt>();

        public bool ItemHasArrived { get { return ItemSentOrReceived; } }
        private bool ItemSentOrReceived = false;

        /// <summary>
        /// Create a scheduled stock change (an order for a specific date/time)
        /// </summary>
        /// <param name="TransactionDateTime">When the order will arrive/ be sent out</param>
        /// <param name="NumberToAddOrRemove">The number of items that will be added to stock (positive number) or the number of items to remove (negative number)</param>
        public ScheduledStockChange(DateTime TransactionDateTime, long NumberToAddRemove, List<string> StocksToChange)
        {
            OrderETA = TransactionDateTime;


        }

        public void ItemArrived()
        {
            ItemSentOrReceived = true;
        }

        public List<string> ItemsBeingChanged
        {
            get
            {
                List<string> ReturnValue = new List<string>();
                foreach (StringAndInt si in StocksToChange)
                {
                    if (!ReturnValue.Contains(si.s))
                        ReturnValue.Add(si.s);
                }

                return ReturnValue;
            }
        }

        public long TotalExportCount
        {
            get
            {
                long ReturnValue = 0;
                foreach (StringAndInt si in StocksToChange)
                {
                    ReturnValue += si.i;
                }
                return ReturnValue;
            }
        }
    }

    [Serializable]
    class StringAndInt
    {
        public int i { get; set; }
        public string s { get; set; }

        public StringAndInt(string s, int i)
        {
            this.i = i;
            this.s = s;
        }
    }
}
