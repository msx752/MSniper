using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSniper.Enums;

namespace MSniper.LiveFeed
{
    public partial class LiveFeed : Form
    {
        public LiveFeed()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void LiveFeed_Load(object sender, EventArgs e)
        {

            List<PokemonId> lst = Enum.GetValues(typeof(PokemonId)).Cast<PokemonId>().ToList();
            foreach (var item in lst)
                cmbPokemons.Items.Add(item);

            for (int i = 0; i <= 100; i++)
                cmbIv.Items.Add(i);

            //for (int i = 0; i < 100; i++)
            //{
            //    ListViewItem item1 = new ListViewItem("Snorlax");
            //    item1.SubItems.Add("100");
            //    listPokemons.Items.Add(item1);
            //}
            listPokemons.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int IVPercent = -1;
            bool success1 = int.TryParse(cmbIv.Text, out IVPercent);
            //if (!success1)
            //{ MessageBox.Show("Wrong IV%"); return; }
            if (!(IVPercent >= 0 && IVPercent <= 100))
            { MessageBox.Show("IV% min0 max100"); return; }
        }
    }
}
