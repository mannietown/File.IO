﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Program_Logs;

//TODO Convert to server based operation
namespace Shopping_UI
{
    static class Program
    {
        public static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Stock Manager";
        public static readonly string SharedAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Stock Manager";
        public static bool CloseAll = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                frmLogin form = new frmLogin();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new frmHome()); //Run the program

                    ActivityLog.LogActivity("User " + UserManagement.User.CurrentUser + " logged off");
                }

            }
            catch (Exception ex)
            {
                Error_Log.LogError(ex, Error_Log.ShowError.ShowFull);
            }
            finally
            {
                CloseAll = true;
            }
        }
    }
}
