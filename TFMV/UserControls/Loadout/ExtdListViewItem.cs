using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace TFMV
{
    public class ExtdListViewItem : ListViewItem
    {
        public Int32 item_id { get; set; }

        public string model_path { get; set; }
        public string item_name { get; set; }
        public string anim_slot { get; set; }
        public string equip_region { get; set; }
        public bool not_paintable { get; set; }

        public string workshop_zip_path { get; set; }

        public List<string> bodygroups_off { get; set; }


        // if skin_override_all != 255 then it overrides skin_red_override and skin_blue_override
        public byte skin_override_all { get; set; }

        public byte skin_red_override { get; set; }
        public byte skin_blu_override { get; set; }

        public List<TF2.items_game.attached_model> model_attachements { get; set; }

        public TF2.items_game.extra_wearable extra_wearable { get; set; }

        public List<string> visuals_red_attached_models { get; set; } // attached models for visuals_red.attache_models (see item: The Kritzkrieg)


        public ExtdListViewItem()
        {
            skin_blu_override = 1;
            skin_override_all = 255;
        }

    }
}
