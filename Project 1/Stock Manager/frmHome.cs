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
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();

        }
    }

    class DataClass
    {
        string objectdescription;
        public string ObjectDescription { get { return objectdescription; } set { } }
    }

    static class DataIO
    {
        public static void Load()
        {

        }

        public static void Save()
        {

        }
    }
}
