using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Stock_Manager
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            if (User.LoadAllUsers().Count == 0)
            {
                frmCreateUser form = new frmCreateUser();
                if (form.ShowDialog() == DialogResult.Cancel)
                {
                    //Close this - it's the end of the world!
                    throw new MissingFieldException("No users found to login as. Please restart this program and create a new user.");
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
