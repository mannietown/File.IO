using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManager;
using UserManagement;
using UserManagement.PermissionManagement;

namespace Shopping_UI
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();

            if (User.CurrentUser.HasAccess(AreaOfAccess.AdminSettings, Stock.ActiveStocks, PermissionLevel.ReadOnly))
            {
                btnAdminPanel.Enabled = true;
            }
            if (User.CurrentUser.HasAccess(AreaOfAccess.ImmediateOrders, Stock.ActiveStocks, PermissionLevel.ReadOnly))
            {
                btnCheckout.Enabled = true;
            }
            if (User.CurrentUser.HasAccess(AreaOfAccess.ItemStocks, Stock.ActiveStocks, PermissionLevel.ReadOnly))
            {
                btnStockManager.Enabled = true;
            }

            RefreshWelcomeMessage();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            bool AdminPanelEnabled = btnAdminPanel.Enabled, CheckoutEnabled = btnCheckout.Enabled, StockManagerEnabled = btnStockManager.Enabled;

            if (AdminPanelEnabled && !(CheckoutEnabled || StockManagerEnabled))
            {
                btnAdminPanel.PerformClick();
                Close();
            }
            else if (CheckoutEnabled && !(AdminPanelEnabled || StockManagerEnabled))
            {
                btnCheckout.PerformClick();
                Close();
            }
            else if (StockManagerEnabled && !(AdminPanelEnabled || CheckoutEnabled))
            {
                btnStockManager.PerformClick();
                Close();
            }
            else Opacity = 100;
        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            using (frmLogin form = new frmLogin())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshWelcomeMessage();
                }
            }
        }

        void RefreshWelcomeMessage()
        {
            TimeSpan Now = DateTime.Now.TimeOfDay;
            string TimeGreeting = null;

            TimeSpan Noon = new TimeSpan(12, 0, 0);
            TimeSpan MorningStart = new TimeSpan(4, 30, 0);
            TimeSpan EveningStart = new TimeSpan(17, 0, 0);
            TimeSpan NightStart = new TimeSpan(22, 0, 0);

            if (Now < Noon && Now >= MorningStart)
            {
                TimeGreeting = "morning";
            }
            else if (Now >= Noon && Now < EveningStart)
            {
                TimeGreeting = "afternoon";
            }
            else if (Now >= EveningStart && Now < NightStart)
            {
                TimeGreeting = "evening";
            }
            else
            {
                TimeGreeting = "night";
            }
            
            lblWelcomeMessage.Text = "Good " + TimeGreeting + ", " + User.CurrentUser.Firstname;
        }

        private void tmrDateTime_Tick(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Now;
            lblTime.Text = "Time: " + Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern) +
                Environment.NewLine + "Date: " + Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        private void frmHome_Enter(object sender, EventArgs e)
        {
            RefreshWelcomeMessage();
        }
    }
}
