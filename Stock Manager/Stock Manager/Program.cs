using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Manager
{
    static class Program
    {
        public static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StockManager";
        public static User ActiveUser = new User("SysAdmin", "Sytem", "SysAdmin", null, ;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin form = new frmLogin();

            if (form.ShowDialog() == DialogResult.OK)
                Application.Run(new frmHome());
        }
    }
}
