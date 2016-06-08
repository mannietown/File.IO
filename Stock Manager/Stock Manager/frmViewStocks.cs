using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserManagement;

namespace Stock_Manager
{
    public partial class frmViewStocks : Form
    {
        private Stock ActiveStock;
        public frmViewStocks(Stock StocksToView)
        {
            InitializeComponent();
            ActiveStock = StocksToView;
        }

        private void frmViewStocks_Load(object sender, EventArgs e)
        {
            dgvStocks.DataSource = (User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.ItemStocks, ActiveStock, Permissions.PermissionLevel.ReadOnly)) ? stockItemBindingSource : null;

            dgvStocks.ReadOnly = !User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.ItemStocks, ActiveStock, Permissions.PermissionLevel.ReadAndEdit);

            dgvStocks.AllowUserToDeleteRows = User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.ItemStocks, ActiveStock, Permissions.PermissionLevel.FullAccess);

            dgvStocks.AllowUserToAddRows = User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.ItemStocks, ActiveStock, Permissions.PermissionLevel.FullAccess);

            btnOrderStock.Enabled = User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.IncomingOrders, ActiveStock, Permissions.PermissionLevel.ReadAndEdit);

            btnViewScheduledOrders.Enabled = User.CurrentUser.HasAccess(
                Permissions.SiteAccess.AreaOfAccess.AccessArea.IncomingOrders, ActiveStock, Permissions.PermissionLevel.ReadOnly) ||
                User.CurrentUser.HasAccess(
                    Permissions.SiteAccess.AreaOfAccess.AccessArea.OutgoingOrders, ActiveStock, Permissions.PermissionLevel.ReadOnly);

            ActivityLog.LogActivity("User checked stocks");
            stockItemBindingSource.DataSource = ActiveStock;
        }

        private void dgvStocks_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the stock entry for " + e.Row.Cells["BarcodeValue"].FormattedValue.ToString() + "?",
                "Delete Stock Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void dgvStocks_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //TODO Save changes
            ActiveStock.Save();
        }

        private void dgvStocks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //TODO Save changes
            ActiveStock.Save();
        }

        private void dgvStocks_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            frmNewStockEntry form = new frmNewStockEntry();
            if (form.ShowDialog() == DialogResult.OK)
            {
                DataGridViewCellCollection dgvc = dgvStocks.Rows[dgvStocks.RowCount - 1].Cells;
                dgvc["BarcodeValue"].Value = form.BarcodeValue;
                dgvc["ItemDescription"].Value = form.ItemDesciption;
                dgvc["NumberInStock"].Value = form.NumberInStock;
                dgvc["ItemValue"].Value = form.ItemValue;

                //Save
                ActiveStock.Save();
            }

            else stockItemBindingSource.RemoveAt(stockItemBindingSource.Count - 1); //Remove the new row
        }
    }
}
