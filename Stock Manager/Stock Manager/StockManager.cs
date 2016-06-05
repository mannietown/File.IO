using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Manager
{
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
                if (User.CurrentUser.HasAccess(AreaOfAccess.AccessArea.ItemStocks, AreaOfAccess.PermissionLevel.FullAccess))
                {
                    ScheduledStockChanges = new List<ScheduledStockChange>();
                    AllStock = new List<StockItem>();
                }
                else throw User.CurrentUser.PermissionDeniedMessage(AreaOfAccess.AccessArea.ItemStocks, "Create new stock database", AreaOfAccess.PermissionLevel.FullAccess);
            }
            else
                Load(FileLocation);
        }

        /// <summary>
        /// Checks which orders have arrived, and adjusts the stock counts accordingly
        /// </summary>
        public void RefreshStockCounts()
        {
            for (int Index = 0; Index < ScheduledStockChanges.Count; Index++)
            {
                if (ScheduledStockChanges[Index].ItemSentOrReceived)
                {
                    //Perform stock change
                    foreach (StockItem si in AllStock)
                    {
                        for (int Index2 = 0; Index2 < Index; Index2++)
                        {
                            if (si.ItemID == ScheduledStockChanges[Index].ItemsBeingChanged[Index2])
                                si.NumberInStock += ScheduledStockChanges[Index].StocksToChange[Index2].i;
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

        public double TotalStockValue
        {
            get
            {
                double ReturnValue = 0;

                foreach (StockItem si in AllStock)
                    ReturnValue += si.ItemValue * si.NumberInStock;

                return ReturnValue;
            }
        }

        /// <summary>
        /// Gets the predicted value based on ScheduledStockChanges that the stock will have at a set date/time
        /// </summary>
        /// <param name="ValueAt">The date/time to predict the value for</param>
        /// <returns></returns>
        public double PredictedValue(DateTime ValueAt)
        {
            double CurrentStockValue = TotalStockValue;

            //Simulate performing ScheduledStockChanges
            foreach (ScheduledStockChange ssc in ScheduledStockChanges)
            {
                if (ssc.OrderETA <= ValueAt)
                {
                    foreach (StringAndInt si in ssc.StocksToChange)
                    {
                        bool FoundMatch = false;
                        foreach (StockItem Item in AllStock)
                        {
                            if (Item.ItemID == si.s)
                            {
                                CurrentStockValue += (Item.ItemValue * si.i);

                                FoundMatch = true;
                                break;
                            }
                        }

                        if (!FoundMatch)
                            throw new KeyNotFoundException("ItemID " + si.ToString() + " not found.");
                    }
                }
            }

            return CurrentStockValue;
        }
    }

    [Serializable]
    class StockItem
    {
        public long NumberInStock { get { return numberinstock; } set { numberinstock = value; } }
        private long numberinstock = 0;

        public string ItemID { get { return itemid; } set { itemid = value; } }
        private string itemid = "";

        public string ItemDescription { get { return itemdescription; } set { itemdescription = value; } }
        private string itemdescription = "";

        public double ItemValue { get { return itemvalue; } set { itemvalue = Math.Round(value, 2); } }
        private double itemvalue = 0;

        public StockItem(string ItemID, string ItemDescription, long NumberInStock, double ItemValue)
        {
            itemid = ItemID;
            itemdescription = ItemDescription;
            numberinstock = NumberInStock;
            itemvalue = ItemValue;
        }
    }

    [Serializable]
    class ScheduledStockChange
    {
        public DateTime OrderETA { get { return ordereta; } set { ordereta = value; } }
        private DateTime ordereta;

        List<StringAndInt> stockstochange = new List<StringAndInt>();
        public List<StringAndInt> StocksToChange { get { return stockstochange; } }

        public bool ItemSentOrReceived { get { return itemsentorrecieved; } }
        private bool itemsentorrecieved = false;

        public int this[string ItemID]
        {
            get
            {
                foreach (StringAndInt si in StocksToChange)
                {
                    if (ItemID == si.s)
                        return si.i;
                }

                throw new KeyNotFoundException("ItemID " + ItemID + " not found");
            }
            set
            {
                foreach (StringAndInt si in StocksToChange)
                {
                    if (ItemID == si.s)
                    {
                        si.i = value;
                        return;
                    }
                }
                throw new KeyNotFoundException("ItemID " + ItemID + " not found");
            }
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
                    int ValueToAdd = si.i;
                    if (ValueToAdd < 0)
                        ValueToAdd = 0 - ValueToAdd;

                    ReturnValue += ValueToAdd;
                }
                return ReturnValue;
            }
        }

        public ScheduledStockChange(List<StringAndInt> StocksToChange, bool AddToStocks)
        {
            OrderETA = DateTime.Now;
            itemsentorrecieved = true;
            stockstochange = StocksToChange;
        }

        /// <summary>
        /// Create a scheduled stock change (an order for a specific date/time)
        /// </summary>
        /// <param name="TransactionDateTime">When the order will arrive/ be sent out</param>
        /// <param name="StocksToChange">The items that will be added to stock or removed</param>
        /// <param name="AddToStocks">Whether the item is being added to or removed from the stock (delivery to us or a customer)</param>
        public ScheduledStockChange(DateTime TransactionDateTime, List<StringAndInt> StocksToChange, bool AddToStocks)
        {
            OrderETA = TransactionDateTime;

            stockstochange = StocksToChange;
        }

        public void ItemArrived()
        {
            itemsentorrecieved = true;
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
