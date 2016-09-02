using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSniper.LiveFeed
{
    public partial class LiveFeed : Form
    {
        public LiveFeed()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.frm.Snipe("pidgey", "1.2", "3.4");
        }
    }
}
