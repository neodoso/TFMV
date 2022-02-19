using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFMV.TF2
{

    // (used for extracting textures to create bodygroup transparency masking)
    #region players materials definitions 

    public class player_all_materials
    {
        public List<player_mats> players_mats_red = new List<player_mats>();
        public List<player_mats> players_mats_blue = new List<player_mats>();

        public player_all_materials()
        {
            #region demoman

            var demoman_red = new player_mats("demoman");
            demoman_red.materials.Add(new player_mat(@"materials\models\player\demo\demoman_red", 1, true, true));
            demoman_red.materials.Add(new player_mat(@"materials\models\player\demo\demoman_head_red", 0, true, true));
            demoman_red.materials.Add(new player_mat(@"materials\models\player\demo\eyeball_r", 0, false, false));
            players_mats_red.Add(demoman_red);

            var demoman_blue = new player_mats("demoman");
            demoman_blue.materials.Add(new player_mat(@"materials\models\player\demo\demoman_blue", 1, true, true));
            demoman_red.materials.Add(new player_mat(@"materials\models\player\demo\demoman_head_blue", 0, true, true));
            demoman_blue.materials.Add(new player_mat(@"materials\models\player\demo\eyeball_r", 0, false, false));

            players_mats_blue.Add(demoman_blue);

            #endregion


            #region engineer
    
            var engineer_red = new player_mats("engineer");
            engineer_red.materials.Add(new player_mat(@"materials\models\player\engineer\engineer_red", 2, true, true));
            engineer_red.materials.Add(new player_mat(@"materials\models\player\engineer\engineer_head_red", 0, true, true));
            players_mats_red.Add(engineer_red);

            var engineer_blue = new player_mats("engineer");
            engineer_blue.materials.Add(new player_mat(@"materials\models\player\engineer\engineer_blue", 2, true, true));
            engineer_blue.materials.Add(new player_mat(@"materials\models\player\engineer\engineer_head_blue", 0, true, true));
            players_mats_blue.Add(engineer_blue);

            #endregion

            #region heavy

            var heavy_red = new player_mats("heavy");
            heavy_red.materials.Add(new player_mat(@"materials\models\player\hvyweapon\hvyweapon_red", 0, true, true));
            heavy_red.materials.Add(new player_mat(@"materials\models\player\hvyweapon\heavy_head_red", 0, true, true));
            heavy_red.materials.Add(new player_mat(@"materials\models\player\hvyweapon\eyeball_r", 0, false, false));
            heavy_red.materials.Add(new player_mat(@"materials\models\player\hvyweapon\eyeball_l", 0, false, false));
            heavy_red.materials.Add(new player_mat(@"materials\models\player\hvyweapon\hvyweapon_red_sheen", 0, true, true));
            players_mats_red.Add(heavy_red);

            var heavy_blue = new player_mats("heavy");
            heavy_blue.materials.Add(new player_mat(@"materials\models\player\hvyweapon\hvyweapon_blue", 0, true, true));
            heavy_blue.materials.Add(new player_mat(@"materials\models\player\hvyweapon\heavy_head_blue", 0, true, true));
            heavy_blue.materials.Add(new player_mat(@"materials\models\player\hvyweapon\eyeball_r", 0, false, false));
            heavy_blue.materials.Add(new player_mat(@"materials\models\player\hvyweapon\eyeball_l", 0, false, false));
            heavy_blue.materials.Add(new player_mat(@"materials\models\player\hvyweapon\hvyweapon_red_sheen", 0, true, true));
            players_mats_blue.Add(heavy_blue);

            #endregion

            #region medic

            var medic_red = new player_mats("medic");
            medic_red.materials.Add(new player_mat(@"materials\models\player\medic\medic_red", 2, true, true));
            medic_red.materials.Add(new player_mat(@"materials\models\player\medic\medic_head_red", 0, true, true));
            medic_red.materials.Add(new player_mat(@"materials\models\player\medic\medic_backpack_red", 0, false, false));
            medic_red.materials.Add(new player_mat(@"materials\models\player\medic\eyeball_l", 0, false, false));
            medic_red.materials.Add(new player_mat(@"materials\models\player\medic\eyeball_r", 0, false, false));
            players_mats_red.Add(medic_red);

            var medic_blue = new player_mats("medic");
            medic_blue.materials.Add(new player_mat(@"materials\models\player\medic\medic_blue", 2, true, true));
            medic_blue.materials.Add(new player_mat(@"materials\models\player\medic\medic_head_blue", 0, true, true)); ;
            medic_blue.materials.Add(new player_mat(@"materials\models\player\medic\medic_backpack_blue", 0, false, false));
            medic_blue.materials.Add(new player_mat(@"materials\models\player\medic\eyeball_l", 0, false, false));
            medic_blue.materials.Add(new player_mat(@"materials\models\player\medic\eyeball_r", 0, false, false));
            players_mats_blue.Add(medic_blue);

            #endregion


            #region pyro

            var pyro_red = new player_mats("pyro");
            pyro_red.materials.Add(new player_mat(@"materials\models\player\pyro\pyro_red", 2, true, true));
            players_mats_red.Add(pyro_red);

            var pyro_blue = new player_mats("pyro");
            pyro_blue.materials.Add(new player_mat(@"materials\models\player\pyro\pyro_blue", 2, true, true));
            players_mats_blue.Add(pyro_blue);

            #endregion

            #region scout

            var scout_red = new player_mats("scout");
            scout_red.materials.Add(new player_mat(@"materials\models\player\scout\scout_red", 2, true, true));
            scout_red.materials.Add(new player_mat(@"materials\models\player\scout\scout_head_red", 0, true, true));
            scout_red.materials.Add(new player_mat(@"materials\models\player\scout\eyeball_r", 0, false, false));
            scout_red.materials.Add(new player_mat(@"materials\models\player\scout\eyeball_l", 0, false, false));
            players_mats_red.Add(scout_red);

            var scout_blue = new player_mats("scout");
            scout_blue.materials.Add(new player_mat(@"materials\models\player\scout\scout_blue", 2, true, true));
            scout_blue.materials.Add(new player_mat(@"materials\models\player\scout\scout_head_blue", 0, true, true));
            scout_blue.materials.Add(new player_mat(@"materials\models\player\scout\eyeball_r", 0, false, false));
            scout_blue.materials.Add(new player_mat(@"materials\models\player\scout\eyeball_l", 0, false, false));
            players_mats_blue.Add(scout_blue);

            #endregion


            #region sniper

            var sniper_red = new player_mats("sniper");
            sniper_red.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_red", 1, true, true));
            sniper_red.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_head_red", 0, true, true));
            sniper_red.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_lens", 0, false, false));
            sniper_red.materials.Add(new player_mat(@"materials\models\player\sniper\eyeball_r", 0, false, false));
            sniper_red.materials.Add(new player_mat(@"materials\models\player\sniper\eyeball_l", 0, false, false));
            players_mats_red.Add(sniper_red);

            var sniper_blue = new player_mats("sniper");
            sniper_blue.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_blue", 1, true, true));
            sniper_blue.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_head_blue", 0, true, true));
            sniper_blue.materials.Add(new player_mat(@"materials\models\player\sniper\sniper_lens", 0, false, false));
            sniper_blue.materials.Add(new player_mat(@"materials\models\player\sniper\eyeball_r", 0, false, false));
            sniper_blue.materials.Add(new player_mat(@"materials\models\player\sniper\eyeball_l", 0, false, false));
            players_mats_blue.Add(sniper_blue);

            #endregion

            #region soldier

            var soldier_red = new player_mats("soldier");
            soldier_red.materials.Add(new player_mat(@"materials\models\player\soldier\soldier_red", 1, true, true));
            soldier_red.materials.Add(new player_mat(@"materials\models\player\soldier\soldier_head_red", 0, true, true));
            soldier_red.materials.Add(new player_mat(@"materials\models\player\soldier\eyeball_r", 0, false, false));
            soldier_red.materials.Add(new player_mat(@"materials\models\player\soldier\eyeball_l", 0, false, false));
            players_mats_red.Add(soldier_red);

            var soldier_blue = new player_mats("soldier");
            soldier_blue.materials.Add(new player_mat(@"materials\models\player\soldier\soldier_blue", 1, true, true));
            soldier_blue.materials.Add(new player_mat(@"materials\models\player\soldier\soldier_head_blue", 0, true, true));
            soldier_blue.materials.Add(new player_mat(@"materials\models\player\soldier\eyeball_r", 0, false, false));
            soldier_blue.materials.Add(new player_mat(@"materials\models\player\soldier\eyeball_l", 0, false, false));
            players_mats_blue.Add(soldier_blue);

            #endregion

            #region spy

            var spy_red = new player_mats("spy");
            spy_red.materials.Add(new player_mat(@"materials\models\player\spy\spy_red", 0, true, true));
            spy_red.materials.Add(new player_mat(@"materials\models\player\spy\spy_head_red", 0, true, true));
            spy_red.materials.Add(new player_mat(@"materials\models\player\spy\eyeball_r", 0, false, false));
            spy_red.materials.Add(new player_mat(@"materials\models\player\spy\eyeball_l", 0, false, false));
            players_mats_red.Add(spy_red);

            var spy_blue = new player_mats("spy");
            spy_blue.materials.Add(new player_mat(@"materials\models\player\spy\spy_blue", 0, true, true));
            spy_blue.materials.Add(new player_mat(@"materials\models\player\spy\spy_head_blue", 0, true, true));
            spy_blue.materials.Add(new player_mat(@"materials\models\player\spy\eyeball_r", 0, false, false));
            spy_blue.materials.Add(new player_mat(@"materials\models\player\spy\eyeball_l", 0, false, false));
            players_mats_blue.Add(spy_blue);

            #endregion
        }
    }


    public class player_mats
    {
        public string class_name { get; set; }
        public List<player_mat> materials { get; set; }

        public player_mats(string _class_name)
        {
            class_name = _class_name;
            materials = new List<player_mat>();
        }

    }

    public class player_mat
    {
        public string material { get; set; }
        public byte texture_res { get; set; }  // 0 = do not rewrite VTF      // 1 = 1024x512   // 2 = 2048x1024
    
        //  (if true, we keep the VMT's original bump texture for the  player grey material
        public bool keep_bumptexture { get; set; }

        // grey material should have phong and player lightwarp
        public bool phong_lightwarp { get; set; }

        public player_mat(string _material, byte _texture_res, bool _keep_bumptexture, bool _phong_lightwarp)
        {
            this.material = _material;
            this.texture_res = _texture_res;
            this.keep_bumptexture = _keep_bumptexture;
            this.phong_lightwarp = _phong_lightwarp;
        }
    }

    #endregion
}
