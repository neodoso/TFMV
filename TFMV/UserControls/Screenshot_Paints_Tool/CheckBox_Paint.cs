using System;
using System.Drawing;
using System.Windows.Forms;

namespace TFMV.UserControls.Screenshot_Paints_Tool
{
    public partial class CheckBox_Paint : UserControl
    {
        public byte paint_id { get; set; }

        public CheckBox_Paint()
        {
            InitializeComponent();
        }

        public void set_params(Color _color, string _label, byte _paint_id)
        {
            label_paint.Text = _label;
            this.BackColor = _color;
            this.paint_id = _paint_id;
        }

        private void on_hover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void exit_hover(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void check_box(object sender, EventArgs e)
        {
            if (cb_paint.Checked)
            {
                cb_paint.Checked = false;

            }
            else
            {
                cb_paint.Checked = true;
            }
        }

        private void CheckBox_Paint_Click(object sender, EventArgs e)
        {
            cb_paint.Checked = true;
        }

    }
}
