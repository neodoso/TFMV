using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TFMV.UserControls
{
    public partial class VPKSelect : Form
    {
       List<string> mdls = new List<string>();
       public Boolean closed = false;

        public VPKSelect(List<string> models)
        {
            InitializeComponent();

            mdls = models ;
          
            for (int i = 0; i < models.Count; i++)
            {
               listBox_vpks.Items.Add(Path.GetFileName(models[i]));  
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (listBox_vpks.SelectedItems.Count == 0) { MessageBox.Show("Select a vpk."); return; }

            Main.vpk_tmp_path = mdls[listBox_vpks.SelectedIndex];
            closed = true;
           this.Close();
        }

        private void VPKSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
        }
    }
}
