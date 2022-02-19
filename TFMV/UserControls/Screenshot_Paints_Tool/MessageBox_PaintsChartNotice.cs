using System;
using System.Windows.Forms;

namespace TFMV.UserControls.Screenshot_Paints_Tool
{
    public partial class MessageBox_PaintsChartNotice : Form
    {

        public bool do_not_show_dialog_option = false;
        public MessageBox_PaintsChartNotice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
