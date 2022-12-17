using System;
using System.Collections.Generic;

namespace TFMV.TF2
{
    [Serializable]
    public class items_game
    {
        [Serializable]
        public class item
        {
            public string prefab { get; set; }
            public string item_class { get; set; }

            public string item_type_name { get; set; }
            public string item_name_var { get; set; } // variable that we use to search the real name in tf_english.txt
            public string Name_str { get; set; } // internal name

            public string icon_url { get; set; }
            public string model_path { get; set; }

            public string show_in_armory { get; set; }

            public string item_slot { get; set; }
            public string anim_slot { get; set; }

            // item_slot from schema.vdf = "misc" since in items_game.vdf it can be anything (head, feet, etc) and wee
            // we get the category (misc, primary,secondary, melee etc) from schema.vdf
            public string item_slot_schema { get; set; } 
            public string equip_rgn { get; set; }
            public bool not_paintable { get; set; }




            public List<used_by_class> used_by_classes = new List<used_by_class>();
            public List<string> visuals_red_attached_models { get; set; } // attached models for visuals_red.attache_models (see item: The Kritzkrieg)

            public extra_wearable extra_wearable { get; set; } //model path for weapon attachement model (MVM Botkiller weapons, festives...)


            public visuals visuals = new visuals();


            public List<models> model_player_per_class = new List<models>();

            public void models_allclass_ADD(string Class, string Model)
            {
                models model = new models();
                
                model.tfclass = Class;
                model.model = Model;

                model_player_per_class.Add(model);
            }

            public void used_by_class_ADD(string Class, string Slot)
            {
                used_by_class usedby = new used_by_class();

                usedby.tfclass = Class;
                usedby.slot = Slot;

                used_by_classes.Add(usedby);
            }
        }

        [Serializable]
        public class equip_regions
        {
            public List<region_list> equip_conflicts;

            public List<String> shared;
            public List<String> global;

            public equip_regions()
            {
                global = new List<string>();
                shared = new List<string>();
                equip_conflicts = new List<region_list>();
            }

        }

        [Serializable]
        public class region_list
        {
            public List<String> regions;
            public String region;
            


            public region_list()
            {
                regions = new List<string>();
            }
        }


        [Serializable]
        public class models
        {
            public string model;
            public string tfclass;
            
        }
        [Serializable]
        public class used_by_class
        {
            public string tfclass;
            public string slot;
        }


        #region visuals (material styles, models attachements, bodygroups)
        [Serializable]
        public class visuals
        {
            public List<player_bodygroup> player_bodygroups { get; set; }
            public List<attached_model> attached_models { get; set; }
            public List<style> styles { get; set; }

            public visuals()
            {
                 player_bodygroups = new List<player_bodygroup>();
                 attached_models = new List<attached_model>();
                 styles = new List<style>();
            }
        }

        [Serializable]
        public class model_player_per_class
        {
            public string class_name { get; set; }
            public string model { get; set; }

            public model_player_per_class(string _class_name, string _model)
            {
                this.class_name = _class_name; this.model = _model;
            }
        }

        [Serializable]
        public class attached_model
        {
            public byte model_display_flags { get; set; }
            public string model { get; set; }

            public attached_model(byte _model_display_flags, string _model)
            {
                this.model_display_flags = _model_display_flags; this.model = _model;
            }
        }

        //material style
        [Serializable]
        public class style
        {
            public byte skin { get; set; } //skin number

            public string name { get; set; }

            public string skin_red { get; set; }
            public string skin_blu { get; set; }

            public string model_player { get; set; } // "c_persian_shield" has model styles

            public List<style_hidden_bodygroup> additional_hidden_bodygroups { get; set; }

            public List<model_player_per_class> model_player_per_class_list { get; set; }

            public string image_inventory { get; set; }

            public style() //string _key, string _value
            {
                additional_hidden_bodygroups = new List<style_hidden_bodygroup>();
                model_player_per_class_list = new List<model_player_per_class>();
            }
        }


        [Serializable]
        public class style_hidden_bodygroup
        {
           public string bodygrop_name { get; set; }
           public byte toggle { get; set; }

           public style_hidden_bodygroup(string _bodygrop_name, byte _toggle)
           {
               this.bodygrop_name = _bodygrop_name;
               this.toggle = _toggle;
           }
        }


        [Serializable]
        public class player_bodygroup
        {
            public string key { get; set; }
            public string value { get; set; }

            public player_bodygroup(string _key, string _value)
            {
                this.key = _key; this.value = _value;
            }
        }

        [Serializable] // used for botkilelr weapons and festive weapons
        public class extra_wearable
        {
            public string mdl_path;
            public byte skin_red { get; set; } // visuals_red.skin (overrides skin #)
            public byte skin_blu { get; set; } // visuals_blu.skin (overrides skin #)

            public bool skin_override { get; set; }

            public extra_wearable()
            {

            }

            public extra_wearable(string _mdl_path, byte _skin_red, byte _skin_blu, bool _skin_override)
            {
                this.mdl_path = _mdl_path;
                this.skin_red = _skin_red;
                this.skin_blu = _skin_blu;
                this.skin_override = _skin_override;
            }
        }

        #endregion

    }
}
