using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothLocker
{
    public partial class DisplayPasswords : Form
    {
        public DisplayPasswords()
        {
            InitializeComponent();
        }

        public string PassInfo 
        {
            set { textBox1.Text = value; }
        }
    }
}
