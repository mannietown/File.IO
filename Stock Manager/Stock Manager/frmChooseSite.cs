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
    public partial class frmChooseSite : Form
    {
        public frmChooseSite()
        {
            InitializeComponent();
            foreach (User.SiteAccess SiteID in User.CurrentUser.SitesTheyCanAccess)
            {
                if ((byte)SiteID.pl >= (byte)User.AreaOfAccess.PermissionLevel.ReadOnly) //At least read permissions
                    lsbDeselected.Items.Add(SiteID);
            }
        }

        enum SwitchMode : byte
        {
            MovingRight = 0,
            MovingLeft
        }

        SwitchMode CurrentSwitchMode
        {
            get
            {
                switch (btnSwitchSides.Text)
                {
                    case ">":
                        return SwitchMode.MovingRight;
                    case "<":
                        return SwitchMode.MovingLeft;
                    default:
                        throw new InvalidEnumArgumentException("Button text \"" + btnSwitchSides.Text + "\" not recognised. Text must be \">\" or \"<\".");
                }
            }
            set
            {
                if (value == SwitchMode.MovingLeft)
                {
                    btnSwitchSides.Text = "<";
                }
                else if (value == SwitchMode.MovingRight)
                {
                    btnSwitchSides.Text = ">";
                }
                else throw new InvalidEnumArgumentException("Enum \"SwitchMode\" is out of range. Value is currently: " + (byte)value + ".");
            }
        }

        private void lsbDeselected_Enter(object sender, EventArgs e)
        {
            CurrentSwitchMode = SwitchMode.MovingRight;
        }

        private void lsbSelected_Enter(object sender, EventArgs e)
        {
            CurrentSwitchMode = SwitchMode.MovingLeft;
        }

        private void btnSwitchSides_Click(object sender, EventArgs e)
        {
            if (CurrentSwitchMode == SwitchMode.MovingLeft)
            {
                ListBox.SelectedObjectCollection soc = lsbDeselected.SelectedItems;
                foreach (object o in soc)
                {
                    lsbSelected.Items.Add(o);
                    lsbDeselected.Items.Remove(o);
                }
            }
            else if (CurrentSwitchMode == SwitchMode.MovingRight)
            {
                ListBox.SelectedObjectCollection soc = lsbSelected.SelectedItems;
                foreach (object o in soc)
                {
                    lsbDeselected.Items.Add(o);
                    lsbSelected.Items.Remove(o);
                }
            }
        }
    }
}
