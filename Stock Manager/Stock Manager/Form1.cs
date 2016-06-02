using System;
using System.Collections.Generic;
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
        List<ScheduledStockChange> ScheduledStockChanges = new List<ScheduledStockChange>();

        public Stock(string FileLocation, bool CreateNew)
        {

        }

        public void PerformScheduledStockChanges()
        {
            for (int Index = 0; Index < ScheduledStockChanges.Count; Index++)
            {
                if (ScheduledStockChanges[Index].ItemHasArrived)
                {
                    //Perform stock change
                    NumberInStock += ScheduledStockChanges[Index].NumberToAddRemove;
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

        public long NumberToAddRemove { get { return numbertoaddremove; } set { numbertoaddremove = value; } }
        private long numbertoaddremove;

        public bool ItemHasArrived { get { return ItemSentOrReceived; } }
        private bool ItemSentOrReceived = false;

        /// <summary>
        /// Create a scheduled stock change (an order for a specific date/time)
        /// </summary>
        /// <param name="TransactionDateTime">When the order will arrive/ be sent out</param>
        /// <param name="NumberToAddOrRemove">The number of items that will be added to stock (positive number) or the number of items to remove (negative number)</param>
        public ScheduledStockChange(DateTime TransactionDateTime, long NumberToAddRemove)
        {
            OrderETA = TransactionDateTime;
            this.NumberToAddRemove = NumberToAddRemove;
        }

        public void ItemArrived()
        {
            ItemSentOrReceived = true;
        }

        public List<string> ItemsBeingChanged {
            get
            {
                List<string> ReturnValue = new List<string>();
                foreach(StringAndInt si in StocksToChange)
                {
                    if (!ReturnValue.Contains(si.s))
                        ReturnValue.Add(si.s);
                }

                return ReturnValue;
            } }

        public long TotalExportCount
        {
            get
            {
                long ReturnValue = 0;
                foreach(StringAndInt si in StocksToChange)
                {
                    
                }
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
