using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TFMV.UserControls.Screenshot_Paints_Tool;


namespace TFMV.UserControls
{
    public partial class PickPaints_List : Form
    {

        #region paints list (with shorter names)

        List<string> paints_list = new List<string> 
        { 
        "230 230 230:Extra. Abundance of Tinge",
        "216 190 216:Color No. 216-190-216",
        "197 175 145:Peculiarly Drab Tincture",
        "126 126 126:Aged Moustache Grey",
        "20 20 20:A Distinctive Lack of Hue",
        "105 77 58:Radigan Conagher Brown",
        "124 108 87:Ye Olde Rustic Color",
        "165 117 69:Muskelmannbraun",
        "231 181 59:Australium Gold",
        "240 230 140:Gent's Business Pants", //The Color of a 
        "233 150 122:Dark Salmon Injustice",
        "207 115 54:Mann Co. Orange",
        "255 105 180:Pink as Hell",
        "125 64 113:Commitment to Purple", //A Deep Commitment to Purple
        "81 56 74:Noble Hatter's Violet",
        "47 79 79:A Color Similar to Slate",
        "50 205 50:Bitter Taste of Lime", //The Bitter Taste of Defeat and Lime
        "114 158 66:Indubitably Green",
        "128 128 0:Drably Olive",
        "66 79 59:Zephaniah's Greed",
        "188 221 179:A Mann's Mint",
        "45 45 36:After Eight",
        "168 154 140:Waterlogged Lab Coat (RED)",
        "131 159 163:Waterlogged Lab Coat (BLU)",
        "59 31 35:Balaclavas Are Forever (RED)",
        "24 35 61:Balaclavas Are Forever (BLU)",
        "184 56 59:Team Spirit (RED)",
        "88 133 162:Team Spirit (BLU)",
        "72 56 56:Operator's Overalls (RED)",
        "56 66 72:Operator's Overalls (BLU)",
        "128 48 32:Value of Teamwork (RED)",
        "37 109 141:Value of Teamwork (BLU)",
        "101 71 64:An Air of Debonair (RED)",
        "40 57 77:An Air of Debonair (BLU)",
        "195 108 45:Cream Spirit (RED)",
        "184 128 53:Cream Spirit (BLU)"
        };

        #endregion


        public List<byte> paints_selection = new List<byte>();

        private List<CheckBox_Paint> CheckBox_Paint_List = new List<CheckBox_Paint>();

        private byte max_items_per_collumn = 10;

        private int pos_x = 0;
        private int pos_y = 0;

        private int padding_x = 5;
        private int padding_y = 5;

        Control panel = new Control();

        public PickPaints_List()
        {
            InitializeComponent();
        }

        public void populate_paints_panel(List<byte> _paints_selection)
        {
            paints_selection = _paints_selection;

            panel = (Control)this.Controls["panel_paints"];

            //Items.Clear();
            for (int i = 0; i < paints_list.Count; i++)
            {
                string[] paint = paints_list[i].Split(':');
                string[] color = paint[0].Split(' ');

                add_paint_checkbox(Color.FromArgb(Convert.ToInt16(color[0]), Convert.ToInt16(color[1]), Convert.ToInt16(color[2])), paint[1], Convert.ToByte(i), is_paint_checked(Convert.ToByte(i)));
            }

            #region add RED and BLU pickers

            CheckBox_Paint cb = new CheckBox_Paint();
            cb.set_params(Color.FromArgb(189, 59, 59), "RED (Default team color)", 50);
            cb.Location = new Point(0, (cb.Size.Height * 11));
            cb.cb_paint.Checked = is_paint_checked(50);
            panel.Controls.Add(cb);

            CheckBox_Paint_List.Add(cb);

            cb = new CheckBox_Paint();
            cb.set_params(Color.FromArgb(91, 122, 140), "BLU (Default team color)", 60);
            cb.Location = new Point(cb.Size.Width + padding_x, (cb.Size.Height * 11));
            cb.cb_paint.Checked = is_paint_checked(60);
            panel.Controls.Add(cb);

            CheckBox_Paint_List.Add(cb);
            #endregion
        }

        // checks selected_paints list and if id exists returns true
        private bool is_paint_checked(byte id)
        {
            foreach (var item in paints_selection)
            {
                if (item == id) { return true; }
            }

            return false;
        }

        private void add_paint_checkbox(Color _color, string _name, byte _paint_id, bool _checked)
        {
            CheckBox_Paint cb = new CheckBox_Paint();
            cb.set_params(_color, _name, _paint_id);
            cb.Location = new Point(pos_x, pos_y);
            cb.cb_paint.Checked = _checked;
            panel.Controls.Add(cb);

            #region text color

            // change brightness of text depending on if paint color is bright or dark color
            int b = (_color.R + _color.G + _color.B) / 3;
            int t = 0;

            if (b > 125) { t = 0; }
            if (b < 125) { t = 245; }

            Color c = Color.FromArgb(t, t, t);
            cb.label_paint.ForeColor = c;
            #endregion
            CheckBox_Paint_List.Add(cb);

            // offset for next ones
            pos_y += cb.Size.Height + padding_y;

            // make new collumn every 10 items
            if (pos_y >= cb.Size.Height * max_items_per_collumn) { pos_x += cb.Size.Width + padding_x; pos_y = 0; }
        }


        private List<byte> get_selected_paints()
        {
            List<byte> selected_paints_list = new List<byte>();
            //foreach (var mp in panel.Controls.OfType<CheckBox_Paint>())
            foreach (var item in CheckBox_Paint_List)
            {
                if (item.cb_paint.Checked)
                {
                    selected_paints_list.Add(item.paint_id);
                }
            }

            return selected_paints_list;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            this.paints_selection = get_selected_paints();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_uncheck_all_Click(object sender, EventArgs e)
        {
            foreach (var item in CheckBox_Paint_List)
            {
                item.cb_paint.Checked = false;
            }
        }

        private void cb_check_all_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_check_all.Checked)
            {
                cb_check_all.Text = "unkcheck all";
            }
            else
            {
                cb_check_all.Text = "check all";
            }

            foreach (var item in CheckBox_Paint_List)
            {
                item.cb_paint.Checked = cb_check_all.Checked;
            }
        }
    }
}
