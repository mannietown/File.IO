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
        public static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StockManager";
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
                    Application.Run(new frmCheckout(form.SelectedStock));
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
