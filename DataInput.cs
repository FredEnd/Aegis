using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aegis
{
    public partial class DataInput : Form
    {
        public DataInput()
        {
            InitializeComponent();

        }

        private void Enter_Click(object sender, EventArgs e)
        {
            var input = EnterBox.Text;


        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
