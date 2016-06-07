using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Manager
{
    public class Stock
    {
        public static Stock ActiveStock;
        public List<ScheduledStockChange> ScheduledStockChanges;
        public List<StockItem> AllStock;
        private string FileLocation { get { return Program.SharedAppDataFolder + "\\" + siteid; } }

        private string siteid;
        public string SiteID { get { return siteid; } }

        public Stock(string SiteID, bool CreateNew)
        {
            this.siteid = SiteID;
            if (CreateNew)
            {
                if (User.CurrentUser.HasAccess(User.AreaOfAccess.AccessArea.ItemStocks, this, User.AreaOfAccess.PermissionLevel.FullAccess))
                {
                    File.Delete(FileLocation); //Replace old data

                    ScheduledStockChanges = new List<ScheduledStockChange>();
                    AllStock = new List<StockItem>();

                    Save(); //Replace old data
                }
                else throw User.CurrentUser.PermissionDeniedMessage(User.AreaOfAccess.AccessArea.ItemStocks,
                    "Create new stock database", User.AreaOfAccess.PermissionLevel.FullAccess);
            }
            else
                Load();
        }

        public override string ToString()
        {
            return SiteID;
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
                            if (si.BarcodeValue == ScheduledStockChanges[Index].ItemsBeingChanged[Index2])
                                si.NumberInStock += ScheduledStockChanges[Index].OrderedItems[Index2].i;
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
                    foreach (StringAndInt si in ssc.OrderedItems)
                    {
                        bool FoundMatch = false;
                        foreach (StockItem Item in AllStock)
                        {
                            if (Item.BarcodeValue == si.s)
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
    public class StockItem
    {
        public long NumberInStock { get { return numberinstock; } set { numberinstock = value; } }
        private long numberinstock = 0;

        public string BarcodeValue { get { return barcodevalue; } }
        private string barcodevalue = "";

        public string ItemDescription { get { return itemdescription; } set { itemdescription = value; } }
        private string itemdescription = "";

        public double ItemValue { get { return itemvalue; } set { itemvalue = Math.Round(value, 2); } }
        private double itemvalue = 0;

        public StockItem(string ItemID, string ItemDescription, long NumberInStock, double ItemValue)
        {
            barcodevalue = ItemID;
            itemdescription = ItemDescription;
            numberinstock = NumberInStock;
            itemvalue = ItemValue;
        }
    }

    [Serializable]
    public class ScheduledStockChange : Cart
    {
        public DateTime OrderETA { get { return ordereta; } set { ordereta = value; } }
        private DateTime ordereta;

        public enum DeliveryType
        {
            HighSpeed,
            MediumHighSpeed,
            MediumSpeed,
            LowMediumSpeed,
            LowSpeed
        }

        public bool ItemSentOrReceived { get { return itemsentorrecieved; } }
        private bool itemsentorrecieved = false;

        public int this[string ItemID]
        {
            get
            {
                foreach (StringAndInt si in OrderedItems)
                {
                    if (ItemID == si.s)
                        return si.i;
                }

                throw new KeyNotFoundException("ItemID " + ItemID + " not found");
            }
            set
            {
                foreach (StringAndInt si in OrderedItems)
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
                foreach (StringAndInt si in OrderedItems)
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
                foreach (StringAndInt si in OrderedItems)
                {
                    int ValueToAdd = si.i;
                    if (ValueToAdd < 0)
                        ValueToAdd = 0 - ValueToAdd;

                    ReturnValue += ValueToAdd;
                }
                return ReturnValue;
            }
        }

        /// <summary>
        /// Immediate Scheduled stock change (useful for returns)
        /// </summary>
        /// <param name="OrderedItems">The items being changed</param>
        /// <param name="AddToStocks">Whether the items are being removed from stock (sent out) or added to stock (recieved)</param>
        public ScheduledStockChange(List<StringAndInt> OrderedItems, bool AddToStocks)
        {
            OrderETA = DateTime.Now;
            itemsentorrecieved = true;
            ordereditems = OrderedItems;
        }

        /// <summary>
        /// Create a scheduled stock change (an order for a specific date/time)
        /// </summary>
        /// <param name="TransactionDateTime">When the order will arrive/ be sent out</param>
        /// <param name="OrderedItems">The items that will be added to stock or removed</param>
        /// <param name="AddToStocks">Whether the items are being removed from stock (sent out) or added to stock (recieved)</param>
        public ScheduledStockChange(DateTime TransactionDateTime, List<StringAndInt> OrderedItems, bool AddToStocks)
        {
            OrderETA = TransactionDateTime;

            ordereditems = OrderedItems;
        }

        /// <summary>
        /// The item has arrived at us or been sent on its way
        /// </summary>
        public void ItemArrived()
        {
            itemsentorrecieved = true;
        }
    }

    [Serializable]
    public class StringAndInt
    {
        public int i { get; set; }
        public string s { get; set; }

        public StringAndInt(string s, int i)
        {
            this.i = i;
            this.s = s;
        }
    }

    [Serializable]
    public class Cart
    {
        protected List<StringAndInt> ordereditems = new List<StringAndInt>();
        public List<StringAndInt> OrderedItems { get { return ordereditems; } }

        public Cart()
        {
            ordereditems = new List<StringAndInt>();
        }

        public void ClearCart()
        {
            ordereditems = new List<StringAndInt>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BarcodeScanned"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
        public double AddItemToCart(string BarcodeScanned, int ItemCount)
        {
            StockItem si = null;

            foreach (StockItem siEnumerator in Stock.ActiveStock.AllStock)
            {
                if (siEnumerator.BarcodeValue == BarcodeScanned)
                {
                    si = siEnumerator;
                }
            }

            if (si == null)
                throw new ArgumentNullException("ItemID");

            if (si.NumberInStock < ItemCount)
            {
                if (MessageBox.Show("There seems to be not enough of " + si.BarcodeValue + " in stock to carry out the order. Proceed anyway?",
                    "Item Not In Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                { //TODO Come up with a better solution for dealing with this
                    throw new IndexOutOfRangeException("Not enough of " + si.BarcodeValue + " in stock to carry out the order");
                }
                //else proceed
            }

            ordereditems.Add(new StringAndInt(BarcodeScanned, ItemCount));
            return si.ItemValue;
        }
    }

    [Serializable]
    public class Coupon
    {
        public DateTime ExpiryDate
        {
            get { return expirydate; }
            set { expirydate = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0); }
        }

        DateTime expirydate;
        string code;
        public string CouponCode { get { return code; } }

        public string Description { get; set; }

        public Discount CouponDiscount { get; set; }

        public bool HasExpired
        {
            get
            {
                DateTime current = DateTime.Now;
                return ExpiryDate < new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, 59);
            }
        }

        public Coupon(string Description, string CouponCode, DateTime ExpiryDate, List<StockItem> ItemsAffected)
        {
            this.ExpiryDate = ExpiryDate;
            code = CouponCode;
            this.Description = Description;
        }

        public Coupon(string Description, string CouponCode, DateTime ExpiryDate, Discount DiscountForCoupon)
        {

        }

        public class Discount
        {
            public enum DiscountType : byte
            {
                PercentageOff = 0,
                BuyXGetXPercentageOff,
                BuyXGetXFree,
                SpendXGetPercentageOff,
                SpendMoreThanXLessThanY,
                BuyXPriceBecomesY,
                FreeDelivery,
                BuyXOfThisGetXOfThatFree,
                BuyXOfThisGetXOfThatPercentageOff
            }

            public enum QualifierType : byte
            {
                /// <summary>
                /// There's no qualifier! Everyone gets the coupon's effect, as long as they have the coupon code
                /// </summary>
                NoQualifier = 0,

                /// <summary>
                /// The customer must have the coupon code
                /// </summary>
                UseCode,

                /// <summary>
                /// Spend at least a certain amout in total
                /// </summary>
                SpendAtLeastXAmount,

                /// <summary>
                /// Spend up to (and including) a certain amount in total
                /// </summary>
                SpendUpToXAmount,

                /// <summary>
                /// Buy a certain product
                /// </summary>
                BuyX,

                /// <summary>
                /// Don't buy a certain product
                /// </summary>
                DontBuyX,

                /// <summary>
                /// Buy exactly a certain number of a product
                /// </summary>
                BuyXofY,

                /// <summary>
                /// Buy a multiple of a certain product (useful for buy 3, get 2 free type deals)
                /// </summary>
                BuyXLotsOfY,

                /// <summary>
                /// Buy up to (and including) a certain number of a product
                /// </summary>
                BuyUpToXofY,

                /// <summary>
                /// Opt for delivery in checkout
                /// </summary>
                GettingDelivered,

                /// <summary>
                /// Opt for a specific type of delivery in checkout
                /// </summary>
                GettingDeliveryType,

                /// <summary>
                /// Order before (and including) a certain time of day (accurate to the minute).
                /// </summary>
                OrderBeforeTime,

                /// <summary>
                /// Order after (and including) a certain time of day (accurate to the minute).
                /// </summary>
                OrderAfterTime,

                /// <summary>
                /// Order before (and including) a certain date.
                /// </summary>
                OrderBeforeDate,

                /// <summary>
                /// Order after (and including) a certain date.
                /// </summary>
                OrderAfterDate,

                /// <summary>
                /// Order before (and including) a certain date and time (accurate to the minute).
                /// </summary>
                OrderBeforeDateTime,

                /// <summary>
                /// Order after (and including) a certain date and time (accurate to the minute).
                /// </summary>
                OrderAfterDateTime,
            }

            public enum EffectType : byte
            {
                /// <summary>
                /// Get a percentage off the whole cart
                /// </summary>
                PercentageOff = 0,

                /// <summary>
                /// Get a specific amount off the total bill
                /// </summary>
                GetXOff,

                /// <summary>
                /// Get a percentage off any of a particular item present in the cart
                /// </summary>
                PercentageOffItemType,

                /// <summary>
                /// Get a specific amount off the bill for a certain item up to getting the item for free
                /// </summary>
                GetXOffY,

                /// <summary>
                /// Get a particular item type free
                /// </summary>
                GetItemTypeFree,

                /// <summary>
                /// Get a certain amount off the delivery cost
                /// </summary>
                PercentageOffDelivery,

                /// <summary>
                /// Get free delivery
                /// </summary>
                DeliveryFree
            }

            private List<QualifierType> qualifiers;
            public List<QualifierType> Qualifiers { get { return qualifiers; } }
            private List<EffectType> effects;
            public List<EffectType> Effects { get { return effects; } }

            private string CouponCode;

            public Discount(List<QualifierType> Qualifiers, List<EffectType> Effects)
            {
                qualifiers = Qualifiers;
                effects = Effects;
            }

            public Discount(List<QualifierType> Qualifiers, List<EffectType> Effects, string CouponCode)
            {
                qualifiers = Qualifiers;
                effects = Effects;
                this.CouponCode = CouponCode;
            }

            public bool DoIQualify(Cart MyShoppingCart)
            {
                if (CouponCode == null)
                {
                    return DoIQualifyNoCodeCheck(MyShoppingCart);
                }
                else return false;
            }

            public bool DoIQualify(Cart MyShoppingCart, string CouponCode)
            {
                if (CouponCode != null && CouponCode == this.CouponCode)
                {
                    return DoIQualifyNoCodeCheck(MyShoppingCart);
                }
                else return false;
            }

            private bool DoIQualifyNoCodeCheck(Cart MyShoppingCart)
            {
                bool IQualify = true;
                foreach (QualifierType qt in qualifiers)
                {

                }

                return IQualify;
            }
        }
    }

    //TODO Item Returns
}
