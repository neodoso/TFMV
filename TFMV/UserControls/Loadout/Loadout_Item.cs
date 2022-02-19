using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TFMV.UserControls.Loadout
{
    [Serializable]
    public partial class Loadout_Item : UserControl
    {
        // version number in case class structure is changed, since this item can be saved as a loadout file
        public Int32 item_id { get; set; }
        public string item_name { get; set; }
        public string item_slot { get; set; }
        public string model_path { get; set; }
        public string workshop_zip_path { get; set; }
        public Image icon { get; set; }

        public string equip_region { get; set; }
        public bool not_paintable { get; set; }

        // if skin_override_all != 255 then it overrides skin_red_override and skin_blue_override
        public byte skin_override_all { get; set; }

        public byte skin_red { get; set; } // visuals_red.skin (overrides skin #)
        public byte skin_blu { get; set; } // visuals_blu.skin (overrides skin #)

        public bool selected { get; set; }

        public bool paintability { get; set; }

        public bool _selected = false;

        public Loadout_Item()
        {
            InitializeComponent();

            skin_blu = 1;
            skin_override_all = 255;

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            label.Text = item_name;
        }

        private void btn_remove_item_Click(object sender, EventArgs e)
        {
            remove_item();
        }

        private void remove_item()
        {
            // get parent form and find the item list control
            ListView items_list_control = this.Parent.Parent.Controls.Find("list_view", true).FirstOrDefault() as ListView;

            // uncheck item in items list
            if (items_list_control != null)
            {
                int unchecked_items = 0;
                // for each item in the listview (schema items) // uncheck if item id matches
                for (int c = 0; c < items_list_control.Items.Count; c++)
                {
                    ExtdListViewItem listview_item = (ExtdListViewItem)items_list_control.Items[c];

                    bool remove_item = false;

                    if (listview_item.item_id == this.item_id) { remove_item = true; }
                    if ((listview_item.workshop_zip_path == this.workshop_zip_path) && (listview_item.workshop_zip_path != "") && (listview_item.workshop_zip_path != null)) { remove_item = true; }

                    if (remove_item)
                    {
                        listview_item.Checked = false;
                        unchecked_items++;
                    }

                    // item styles have the same item_id, so we only exit the search after having disabled a few
                    if (unchecked_items > 5) { break; }

                }
            }


            this.Dispose();
        }

        private void action_DoubleClick(object sender, EventArgs e)
        {
            remove_item();
        }


        private void select_Click(object sender, EventArgs e)
        {
            if (_selected)
            {
                this.deselect();
            }
            else
            {
                // deselect all other items in loadout list
                FlowLayoutPanel loadout_list = (FlowLayoutPanel)this.Parent;
                foreach (Loadout_Item c in loadout_list.Controls.OfType<Loadout_Item>())
                {
                    c.deselect();
                }

                // mark this item as selected
                this.BackColor = Color.LightBlue;
                btn_remove_item.BackColor = Color.LightBlue;
                btn_remove_item.ForeColor = Color.LightBlue;
                _selected = true;
            }
        }

        public void deselect()
        {
            // deselected
            this.BackColor = Color.LightGray;
            btn_remove_item.BackColor = Color.LightGray;
            btn_remove_item.ForeColor = Color.LightGray;
            _selected = false;
        }
    }
}
