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
    public partial class frmCreateUser : Form
    {
        public frmCreateUser()
        {
            InitializeComponent();
        }

        User UserBeingEdited;

        public frmCreateUser(User ExistingUserToEdit)
        {
            InitializeComponent();

            UserBeingEdited = ExistingUserToEdit;

            txtFirstname.Text = ExistingUserToEdit.Firstname;
            txtSurname.Text = ExistingUserToEdit.Surname;
            txtLoginID.Text = ExistingUserToEdit.LoginID;
            txtPassword.BackColor = Color.WhiteSmoke;
            txtPassword2.BackColor = Color.WhiteSmoke;
            btnOK.Text = "Submit Changes";
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.BackColor == Color.WhiteSmoke)
            {
                txtPassword.BackColor = SystemColors.Control;
                txtPassword2.BackColor = SystemColors.Control;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UserBeingEdited.Firstname = txtFirstname.Text;
            UserBeingEdited.Surname = txtSurname.Text;
            UserBeingEdited.LoginID = txtLoginID.Text;
            UserBeingEdited.PasswordString = txtPassword.Text;

            User.UpdateSavedUser(UserBeingEdited);
        }
    }
}
