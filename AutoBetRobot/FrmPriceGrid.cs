using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoBetRobot
{
    public partial class FrmPriceGrid : Form
    {
        public FrmPriceGrid()
        {
            InitializeComponent();
        }

        private void FrmPriceGrid_Load(object sender, EventArgs e)
        {
            Timer tm = new Timer();
            tm.Interval = 1;

        }
    }
}
