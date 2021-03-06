﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Program_Logs;

namespace Shopping_UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            if (UserManagement.User.LoadAllUsers().Count == 0)
            {
                frmCreateUser form = new frmCreateUser();
                if (form.ShowDialog() == DialogResult.Cancel)
                {
                    //Close this - no users and the current user doesn't want to make one
                    throw new MissingFieldException("No users found to login as. Please restart this program and create a new user.");
                }
            }

            if (Properties.Settings.Default.Username != null)
                txtUsername.Text = Properties.Settings.Default.Username;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //TODO Get available sites for user


            try
            {
                if (UserManagement.User.Login(txtUsername.Text, txtPassword.Text))
                {
                    try
                    {
                        Properties.Settings.Default.Username = txtUsername.Text;
                        Properties.Settings.Default.Save();
                    }
                    catch (IOException ex) //Couldn't save default username due to a read/write error
                    {
                        Error_Log.LogError(ex, Error_Log.ShowError.ShowNone);
                    }

                    ActivityLog.LogActivity("User logged in");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else MessageBox.Show("Incorrect username or password. Please check your credentials and try again",
                    "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (IOException ex)
            {
                if (Error_Log.LogError(ex, Error_Log.ShowError.ShowUserFriendlyMessage, MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                {
                    ActivityLog.LogActivity("User attempted but failed due to an error to log in as user: " + txtUsername.Text);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }
    }
}
