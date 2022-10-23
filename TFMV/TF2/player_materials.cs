using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFMV.TF2
{

    // (used for extract textures to create bodygroup transparency masking)
    #region players materials definitions 

    public class player_materials
    {
        public List<player_material> players_mats = new List<player_material>();

        public player_materials()
        {
            players_mats.Add(new player_material("demoman", @"materials\models\player\demo\", "demoman", 1));
            players_mats.Add(new player_material("engineer", @"materials\models\player\engineer\", "engineer", 2));

            players_mats.Add(new player_material("heavy", @"materials\models\player\hvyweapon\", "hvyweapon", 0));
           // players_mats.Add(new player_material("heavy", @"materials\models\player\hvyweapon\", "hvyweapon_hands"));

            players_mats.Add(new player_material("medic", @"materials\models\player\medic\", "medic", 2));
            players_mats.Add(new player_material("medic", @"materials\models\player\medic\", "medic_backpack", 0));
            players_mats.Add(new player_material("medic", @"materials\models\player\medic\", "medic_head", 0));

            players_mats.Add(new player_material("pyro", @"materials\models\player\pyro\", "pyro", 2));
            players_mats.Add(new player_material("scout", @"materials\models\player\scout\", "scout", 2));
            players_mats.Add(new player_material("sniper", @"materials\models\player\sniper\", "sniper", 1));
            players_mats.Add(new player_material("soldier", @"materials\models\player\soldier\", "soldier", 1));

           // players_mats.Add(new player_material("spy", @"materials\models\player\spy\", "spy"));
        }
    }

    public class player_material
    {
        public string tf_class { get; set; }
        public string mat_dir { get; set; }
        public string mat_name { get; set; }
        public byte texture_res { get; set; } // 0 = does not apply // 1 = 1024x512   // 2  = 2048x1024

        public player_material(string tf_class, string mat_dir, string mat_name, byte _texture_res)
        {
            this.tf_class = tf_class;
            this.mat_dir = mat_dir;
            this.mat_name = mat_name;
            this.texture_res = _texture_res;
        }
    }

    #endregion
}
