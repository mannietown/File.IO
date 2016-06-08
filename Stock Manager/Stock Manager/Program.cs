using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO Convert to server based operation
namespace Stock_Manager
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
                    //TODO Open a menu for selecting which form to open if the user has more access than just the checkout. 

                    Application.Run(new frmCheckout(form.SelectedStock)); //Run the program

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
