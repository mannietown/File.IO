using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Manager
{
    public partial class frmNewStockEntry : Form
    {
        public string ItemID { get { return txtUID.Text; } }
        public string ItemDesciption { get { return txtDescription.Text; } }
        public int NumberInStock { get { return (int)numStock.Value; } }
        public double ItemValue { get { return (double)numValue.Value; } }

        public frmNewStockEntry()
        {
            InitializeComponent();
        }

        private void txtUID_MouseHover(object sender, EventArgs e)
        {
            ttpGeneric.Show("E.g. Orange Juice 500ml", this, txtUID.Location);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //TODO Validate, then save entry
            string InvalidReason = null;
            if (txtUID.Text == "")
            {
                InvalidReason = "the UID cannot for the object cannot be blank";
                goto Invalid;
            }

            if (/*Stock.Contains(AnyItemWithTheSameUID)*/true)
            {
                InvalidReason = "the UID is already in use with the following stock item: " + Environment.NewLine + 
                    ""/*StockItem.ToString()*/;
            }

            Invalid:;
            if (InvalidReason != null)
            {
                MessageBox.Show("We cannot process this entry because " + InvalidReason);
            }

        }

        private void txtDescription_MouseHover(object sender, EventArgs e)
        {
            ttpGeneric.Show("E.g. Orange Juice Carton from Sunny D - 500ml", this, txtUID.Location);
        }
    }
}
