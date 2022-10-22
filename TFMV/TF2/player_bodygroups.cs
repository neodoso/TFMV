using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFMV.TF2
{
    public class player_bodygroups
    {
        public List<player_bodygroup> demoman = new List<player_bodygroup>();
        public List<bodygroup_combination> demoman_combinations = new List<bodygroup_combination>();

        public List<player_bodygroup> engineer = new List<player_bodygroup>();
        public List<bodygroup_combination> engineer_combinations = new List<bodygroup_combination>();


        public List<player_bodygroup> heavy = new List<player_bodygroup>();
        public List<player_bodygroup> medic = new List<player_bodygroup>();

        public List<player_bodygroup> pyro = new List<player_bodygroup>();
        public List<bodygroup_combination> pyro_combinations = new List<bodygroup_combination>();

        public List<player_bodygroup> scout = new List<player_bodygroup>();
        public List<bodygroup_combination> scout_combinations = new List<bodygroup_combination>();


        public List<player_bodygroup> sniper = new List<player_bodygroup>();

        public List<player_bodygroup> soldier = new List<player_bodygroup>();
        public List<bodygroup_combination> soldier_combinations = new List<bodygroup_combination>();
        // public player_bodygroups spy = new player_bodygroups(); //spy has no bodygroups we'd need to modify

        public player_bodygroups()
        {

            this.scout.Add(new player_bodygroup("scout", 0, ""));
            this.scout.Add(new player_bodygroup("hat", 1, "scout_hat"));
            this.scout.Add(new player_bodygroup("headphones", 0, "scout_headphones"));
            this.scout.Add(new player_bodygroup("shoes_socks", 0, "scout_shoes_socks"));
            this.scout.Add(new player_bodygroup("dogtags", 0, "scout_dogtags"));

            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "hat", "dogtags" }, "scout_hat_dogtags"));
            this.scout_combinations.Add(new bodygroup_combination(new string[3] { "hat", "dogtags", "headphones" }, "scout_hat_dogtags_headphones"));
            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "hat", "headphones" }, "scout_hat_headphones"));
            
            this.scout_combinations.Add(new bodygroup_combination(new string[3] { "hat", "shoes_socks", "dogtags" }, "scout_hat_shoes_socks_dogtags"));
            this.scout_combinations.Add(new bodygroup_combination(new string[4] { "hat", "shoes_socks", "dogtags", "headphones" }, "scout_hat_shoes_socks_dogtags_headphones"));
            this.scout_combinations.Add(new bodygroup_combination(new string[3] { "hat", "shoes_socks", "headphones" }, "scout_hat_shoes_socks_headphones"));


            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "hat", "shoes_socks" }, "scout_hat_shoes_socks"));
            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "shoes_socks", "dogtags" }, "scout_shoes_socks_dogtags"));
            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "shoes_socks", "headphones" }, "scout_shoes_socks_headphones"));
            this.scout_combinations.Add(new bodygroup_combination(new string[2] { "headphones", "dogtags" }, "scout_headphones_dogtags"));

            this.scout_combinations.Add(new bodygroup_combination(new string[3] { "headphones", "shoes_socks", "dogtags" }, "scout_headphones_shoes_socks_dogtags"));



            this.soldier.Add(new player_bodygroup("soldier", 0, ""));
            this.soldier.Add(new player_bodygroup("rocket", 1, ""));
            this.soldier.Add(new player_bodygroup("hat", 0, "soldier_hat"));
            this.soldier.Add(new player_bodygroup("medal", 0, ""));
            this.soldier.Add(new player_bodygroup("grenades", 0, "soldier_grenades"));

            this.soldier_combinations.Add(new bodygroup_combination(new string[2] { "hat", "grenades" }, "soldier_grenades_hat"));



            this.pyro.Add(new player_bodygroup("pyro", 0, ""));
            this.pyro.Add(new player_bodygroup("head", 0, "pyro_head"));
            this.pyro.Add(new player_bodygroup("grenades", 0, "pyro_grenades"));
            this.pyro.Add(new player_bodygroup("backpack", 0, "pyro_backpack"));

            this.pyro_combinations.Add(new bodygroup_combination(new string[2] { "backpack", "grenades" }, "pyro_backpack_grenades"));
            this.pyro_combinations.Add(new bodygroup_combination(new string[2] { "backpack", "head" }, "pyro_backpack_head"));
            this.pyro_combinations.Add(new bodygroup_combination(new string[2] { "head", "grenades" }, "pyro_head_grenades"));
            this.pyro_combinations.Add(new bodygroup_combination(new string[3] { "backpack", "head", "grenades" }, "pyro_head_grenades_backpack"));



            this.demoman.Add(new player_bodygroup("demo", 0, ""));
            this.demoman.Add(new player_bodygroup("smile", 0, ""));
            this.demoman.Add(new player_bodygroup("shoes", 0, "demoman_shoes"));
            this.demoman.Add(new player_bodygroup("grenades", 0, "demoman_grenades"));

            this.demoman_combinations.Add(new bodygroup_combination(new string[2] { "shoes", "grenades" }, "demoman_shoes_grenades"));

            this.engineer.Add(new player_bodygroup("engineer", 0, ""));
            this.engineer.Add(new player_bodygroup("hat", 0, "engineer_hat"));

            this.engineer.Add(new player_bodygroup("rightarm", 0, "engineer_rightarm"));

            this.engineer_combinations.Add(new bodygroup_combination(new string[2] { "hat", "rightarm" }, "engineer_rightarm_hat"));


            this.heavy.Add(new player_bodygroup("heavy", 0, ""));
            this.heavy.Add(new player_bodygroup("hands", 0, "hvyweapon_hands"));

            this.medic.Add(new player_bodygroup("medic", 0, ""));
            this.medic.Add(new player_bodygroup("medic_backpack", 0, "medic_backpack"));

            this.sniper.Add(new player_bodygroup("sniper", 0, ""));
            this.sniper.Add(new player_bodygroup("arrows", 1, ""));
            this.sniper.Add(new player_bodygroup("hat", 0, "sniper_hat"));
            this.sniper.Add(new player_bodygroup("bullets", 0, ""));

        }
    }

    public class player_bodygroup
    {
        public string name { get; set; }
        public byte submodel_num { get; set; } //0 = visible // 1 = off if there is no more than two submodels
        public string mask_name { get; set; }

        // public string alpha_mask_name { get; set; }
        // alpha_mask_name is for the player.vtf alpha, so we can make the part of the model transparent

        public player_bodygroup(string name, byte submodel_num, string mask_name)
        {
            this.name = name;
            this.submodel_num = submodel_num;
            this.mask_name = mask_name;
        }
    }

    // possible combination of masks and the mask file name
    public class bodygroup_combination
    {
        public String[] mask_names { get; set; }
        public string mask_filename { get; set; }

        public bodygroup_combination(String[] _masks, string _mask_filename)
        {
            this.mask_names = _masks;
            this.mask_filename = _mask_filename;
        }
    }
}
