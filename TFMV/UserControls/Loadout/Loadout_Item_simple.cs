using System;
using System.Drawing;
using System.Windows.Forms;

namespace TFMV.UserControls.Loadout
{
    [Serializable]
    public class Loadout_Item_simple : PictureBox
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

        public bool selected { get; set; }


        public Loadout_Item_simple()
        {
            skin_blu = 1;
            skin_override_all = 255;
        }

    }
}
