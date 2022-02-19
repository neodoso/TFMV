using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TFMV.UserControls.Loadout
{
    // class used to store and save loadout items + selected main model
    [Serializable]
    public class Loadout_File
    {
        public static byte version = 0;
        public string player_class { get; set; }
        public main_model_data main_model { get; set; }
        public List<loadout_item_data> items { get; set; }

        public List<string> bodygroups_off { get; set; }

        public Loadout_File()
        {
            items = new List<loadout_item_data>();
            main_model = new main_model_data();
            bodygroups_off = new List<string>();
        }



        [Serializable]
        public class loadout_item_data
        {
            // version number in case class structure is changed, since this item can be saved as a loadout file
            public Int32 item_id { get; set; }
            public string item_name { get; set; }
            public string item_slot { get; set; }
            public string model_path { get; set; }
            public string workshop_zip_path { get; set; }
            public Image icon { get; set; }

            // if skin_override_all != 255 then it overrides skin_red_override and skin_blue_override
            public byte skin_override_all { get; set; }

            public byte skin_red { get; set; } // visuals_red.skin (overrides skin #)
            public byte skin_blu { get; set; } // visuals_blu.skin (overrides skin #)

            public bool not_paintable { get; set; }

            public loadout_item_data(Loadout_Item _item)
            {
                this.item_id = _item.item_id;
                this.item_name = _item.item_name;
                this.item_slot = _item.item_slot;
                this.model_path = _item.model_path;
                this.workshop_zip_path = _item.workshop_zip_path;
                this.icon = _item.icon;

                this.skin_override_all = _item.skin_override_all;

                this.skin_red = _item.skin_red;
                this.skin_blu = _item.skin_blu;

                this.not_paintable = _item.paintability;
            }
        }

        [Serializable]
        public class main_model_data
        {
            public Int32 item_id { get; set; }
            public string item_name { get; set; }
            public string item_slot { get; set; }
            public string model_path { get; set; }
            public string workshop_zip_path { get; set; }
            public Image icon { get; set; }

            // if skin_override_all != 255 then it overrides skin_red_override and skin_blue_override
            public byte skin_override_all { get; set; }

            public byte skin_red { get; set; } // visuals_red.skin (overrides skin #)
            public byte skin_blu { get; set; } // visuals_blu.skin (overrides skin #)


            public void set_data(Loadout_Item_simple _item)
            {
                this.item_id = _item.item_id;
                this.item_name = _item.item_name;
                this.item_slot = _item.item_slot;
                this.model_path = _item.model_path;

                this.icon = _item.icon;

                this.workshop_zip_path = _item.workshop_zip_path;
                this.skin_override_all = _item.skin_override_all;

                this.skin_red = _item.skin_red;
                this.skin_blu = _item.skin_blu;
            }

        }
    }
}
