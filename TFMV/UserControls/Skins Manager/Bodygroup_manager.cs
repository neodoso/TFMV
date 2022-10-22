using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TFMV.SourceEngine;
using TFMV.Functions;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Reflection;

namespace TFMV.UserControls
{
    public partial class Bodygroup_manager : UserControl
    {
        private TF2.player_bodygroups player_bodygroups = new TF2.player_bodygroups();

        private List<TF2.player_bodygroup> bodygroups_class = new List<TF2.player_bodygroup>();
        private List<TF2.bodygroup_combination> bodygroups_combinations = new List<TF2.bodygroup_combination>();

        private string tfmv_dir,cached_dir,player_class,team_color;


        public Bodygroup_manager()
        {
            InitializeComponent();
        }


        public void setup(List<String> loadout_bodygroups_off, string _tfmv_dir, string _cached_dir, string _player_class, string _team_color)
        {
            tfmv_dir = _tfmv_dir;
            cached_dir = _cached_dir;
            player_class = _player_class;
            team_color = _team_color;

            this.Controls.Clear();
            bodygroups_manager_setup(this.player_class, loadout_bodygroups_off);
        }

        public void switch_skin(string _team_color)
        {
            team_color = _team_color;

            create_bodygroups_mask(true);
        }

        #region forms

        public void add_bodygroups(string name, bool _checked)
        {
            Point checkbox_pos = new Point();
            int posy_increment = 0; //starting position for the first checkbox
            Panel p = (Panel)this.Controls["this"];

            // search checkboxes
            // if checkboxes exist, get the position
            foreach (var checkbox in this.Controls.OfType<CheckBox>())
            {
                checkbox_pos = checkbox.Location;
                posy_increment = 20;
            }

            //add checkbox and set position Y
            add_checkbox(name, checkbox_pos.Y + posy_increment, _checked);
        }

        private void add_checkbox(string name, int posy, bool _checked)
        {
            CheckBox cb = new CheckBox();

            cb.Checked = _checked;

            cb.Location = new Point(5, posy);
            cb.Text = name;

            //neodement: fix for long bodygroup names getting cut off (such as medic_backpack)
            cb.Width = 110;

            this.Controls.Add(cb);

            cb.CheckedChanged += new EventHandler(this.checkbox_event);
        }

        private void checkbox_event(object sender, EventArgs e)
        {
            create_bodygroups_mask(true);
        }

        #endregion


        #region bodygroups manager

        // creates the bodygroup manager user control within the control "bodygroups_panel"
        // and adds checkboxes for each existing bodygroup for a TF2 player/class model
        private void bodygroups_manager_setup(string tf_class, List<String> loadout_bodygroups_off)
        {

            if ((tf_class == "") || (tfmv_dir == "") || (cached_dir == "") || (team_color == "")) { return; }

            // clear bodygroups panel
            this.Controls.Clear();


            // get classes's bodygroups from the lists "player_bodygroups"
            #region load player class bodygroups

            bodygroups_class = new List<TF2.player_bodygroup>();
            bodygroups_combinations = new List<TF2.bodygroup_combination>();

            //**HERE** TODO: engy isnt loading rightarm

            // object bodygroup_class = player_bodygroups.GetType().GetMember("scout");
            // TODO
            // we could get the bodygroup from "player_bodygroups" by name through reflection instead of using this switch
            switch (tf_class)
            {
                case "scout":
                    bodygroups_class = player_bodygroups.scout;
                    bodygroups_combinations = player_bodygroups.scout_combinations;
                    break;
                case "soldier":
                    bodygroups_class = player_bodygroups.soldier;
                    bodygroups_combinations = player_bodygroups.soldier_combinations;
                    break;
                case "pyro":
                    bodygroups_class = player_bodygroups.pyro;
                    bodygroups_combinations = player_bodygroups.pyro_combinations;
                    break;
                case "demoman":
                    bodygroups_class = player_bodygroups.demoman;
                    bodygroups_combinations = player_bodygroups.demoman_combinations;
                    break;
                case "demo":
                    bodygroups_class = player_bodygroups.demoman;
                    bodygroups_combinations = player_bodygroups.demoman_combinations;
                    break;
                case "medic":
                    bodygroups_class = player_bodygroups.medic;
                    break;
                case "heavy":
                    bodygroups_class = player_bodygroups.heavy;
                    break;
                case "sniper":
                    bodygroups_class = player_bodygroups.sniper;
                    break;
                case "engineer":
                    bodygroups_class = player_bodygroups.engineer;
                    bodygroups_combinations = player_bodygroups.engineer_combinations;
                    break;
            }

            #endregion

            if (tf_class == "heavy") { tf_class = "hvyweapon"; }

            // for each TF2.player_bodygroups.tfmv_bodygroup add a checkbox for the bodygroup-name
            foreach (var item in bodygroups_class)
            {
                if (item.mask_name != "")
                {
                    bool _bodygroup_on = true;
                    string bodygroupname_without_class = item.mask_name.Replace(tf_class + "_", "");

                    if (loadout_bodygroups_off.Contains(bodygroupname_without_class)) //item.mask_name.Split('_')[1]
                    {
                        _bodygroup_on = false;
                    }

                    this.add_bodygroups(item.name, _bodygroup_on);
                }
            }

            create_bodygroups_mask(true);
        }


        public void create_bodygroups_mask(bool refresh_hlmv)
        {
            List<string> bodygroups_off = new List<string>();

            // loop through bodygroup_manager_panel checkboxes
            // if bodygroup on, add to list
            foreach (var cb in this.Controls.OfType<CheckBox>())
            {
                if (!cb.Checked) bodygroups_off.Add(cb.Text);
            }

            gen_player_material(bodygroups_off);

            if(refresh_hlmv)
            {
                Main.refresh_hlmv(false);
            }
        }

        public int bodygroups_off_count()
        {
            int count = 0;
            // loop through bodygroup_manager_panel checkboxes
            // if bodygroup on, add to list
            foreach (var cb in this.Controls.OfType<CheckBox>())
            {
                if (!cb.Checked) count++;
            }

            return count;
        }



        // generates player's VTF with transparency alpha mask for the bodygroups
        // and the VMT with transparency parameters
        public void gen_player_material(List<string> bodygroups_off)
        {
            #region checks and setup vars

            string tf_class = player_class;

            if (tf_class == "demo") { tf_class = "demoman"; }
          
            #region search for player material

            TF2.player_material player_material = null;
            TF2.player_materials player_mats = new TF2.player_materials();

            // search for the class material
            for (int i = 0; i < player_mats.players_mats.Count; i++)
            {
                if (player_mats.players_mats[i].tf_class == tf_class)
                {
                    player_material = player_mats.players_mats[i];
                    break;
                }
            }

            #endregion

            // force use of red material for the grey skin
            if (Main.grey_material) { team_color = "red"; }

            // if player material was no found, exit
            if (player_material == null) { return; }
            string mat_name = player_material.mat_name + "_" + team_color;
            string vtf_name = player_material.mat_name + "_" + team_color;


            #region player class exception for material name

            if (tf_class == "medic")
            {
                mat_name = "medic_backpack_" + team_color;
                vtf_name = "medic_backpack_" + team_color;
            }

            if (tf_class == "heavy")
            {
                mat_name = "hvyweapon_red_sheen";
            }

            #endregion

            // extract VMT of player's team color
            if (!Main.grey_material)
            { 
                VPK.Extract(player_material.mat_dir + mat_name + ".vmt", tfmv_dir + player_material.mat_dir, 0);
            }

            #endregion

            // if there's no bodygroups to hide, return
            if ((bodygroups_off.Count == 0) && (!Main.grey_material))
            {
                miscFunc.delete_safe(tfmv_dir + player_material.mat_dir + vtf_name + ".vtf");
                miscFunc.delete_safe(tfmv_dir + player_material.mat_dir + mat_name + ".vmt");
                return;
            }


            VTFedit vtf_edit = new VTFedit();

            #region single bodygroup mask

            // if only one mask
            if (bodygroups_off.Count == 1)
            {
                #region get alpha mask from resources

                System.Reflection.Assembly asm = Assembly.GetExecutingAssembly();

                string mask_name = "";
                for (int i = 0; i < bodygroups_class.Count; i++)
			    {
                    if (bodygroups_class[i].name == bodygroups_off[0])
                    {
                        mask_name = bodygroups_class[i].mask_name;
                    }
			    }
                System.IO.Stream fs;
                if (mask_name != "")
                {
                    fs = asm.GetManifestResourceStream("TFMV.Resources.bodygroup_masks." + mask_name + ".bin");
                } else {
                     fs = asm.GetManifestResourceStream("TFMV.Resources.bodygroup_masks." + tf_class + "_" + bodygroups_off[0] + ".bin");
                }
                

                if (fs == null) { return; }

                // convert stream to byte array
                byte[] result;
                using (var streamReader = new MemoryStream())
                {
                    fs.CopyTo(streamReader);
                    result = streamReader.ToArray();
                }

                #endregion

                // decompress alpha mask  // add to list
                byte[] alpha_mask = vtf_edit.decompress_byte_array(result);

                string cached_vtf = cached_dir + vtf_name + "_mask" + ".vtf";
                if ((player_material.texture_res == 1) && (Main.grey_material)) { cached_vtf = cached_dir + "rgba_grey_1024_512.vtf"; }
                if((player_material.texture_res == 2) && (Main.grey_material)) { cached_vtf = cached_dir + "rgba_grey_2048_1024.vtf"; }


                // inject alpha
                // copy paste vtf file to tf/custom/tfmv/materials
                if (File.Exists(cached_vtf))
                {
                    vtf_edit.write_alpha(cached_vtf, alpha_mask);
                    File.Copy(cached_vtf, tfmv_dir + player_material.mat_dir + vtf_name + ".vtf", true);
                }

            }

            #endregion


            #region multiple bodygroups masks
            // if more than one mask search for combined mask
            if (bodygroups_off.Count > 1)
            {
                // search some same combination of bodygroups
                int match_count = 0;
                string match = "";

                #region search matching mask combination

                List<TF2.bodygroup_combination> masks_combinations = new List<TF2.bodygroup_combination>();

                // pick arrays that have the same number of masks
                for (int i = 0; i < bodygroups_combinations.Count; i++)
                {
                    if (bodygroups_combinations[i].mask_names.Length == bodygroups_off.Count)
                    {
                        masks_combinations.Add(bodygroups_combinations[i]);
                    }
                }

                foreach (var masks in masks_combinations)
                {
                    match_count = 0;
                    for (int i = 0; i < masks.mask_names.Length; i++)
                    {
                        for (int b = 0; b < bodygroups_off.Count; b++)
                        {
                            if (masks.mask_names[i] == bodygroups_off[b])
                            {
                                match_count++;
                                if (match_count == bodygroups_off.Count)
                                {
                                    match = masks.mask_filename;
                                    break;
                                }
                            }
                        }
                    }
                    if (match != "") { break; }
                }

            #endregion


                if (match != "")
                {

                    #region get alpha mask from resources
                    System.Reflection.Assembly asm = Assembly.GetExecutingAssembly();
                    System.IO.Stream fs = asm.GetManifestResourceStream("TFMV.Resources.bodygroup_masks." + match + ".bin");

                    if (fs == null) { return; }

                    // convert stream to byte array
                    byte[] result;
                    using (var streamReader = new MemoryStream())
                    {
                        fs.CopyTo(streamReader);
                        result = streamReader.ToArray();
                    }

                    #endregion
                    // decompress alpha mask // add to list
                    byte[] alpha_mask = vtf_edit.decompress_byte_array(result);

                    string cached_vtf = cached_dir + vtf_name + "_mask" + ".vtf";
                    if ((player_material.texture_res == 1) && (Main.grey_material)) { cached_vtf = cached_dir + "rgba_grey_1024_512.vtf"; }
                    if ((player_material.texture_res == 2) && (Main.grey_material)) { cached_vtf = cached_dir + "rgba_grey_2048_1024.vtf"; }

                    // inject alpha
                    // copy paste vtf file to tf/custom/tfmv/materials
                    if (File.Exists(cached_vtf))
                    {
                        vtf_edit.write_alpha(cached_vtf, alpha_mask);

                        File.Copy(cached_vtf, tfmv_dir + player_material.mat_dir + vtf_name + ".vtf", true);
                    }

                }

            } // end if

            #endregion



            #region edit VMT to add transparency

            List<string> vmt_lines = new List<string>();

            if (tf_class == "heavy")
            {
                mat_name = "hvyweapon_red_sheen";
            }

            try
            {
                // if there's no bodygroups to hide but we're using the grey material
                if ((bodygroups_off.Count == 0) && (Main.grey_material))
                {
                    VMT.set_parameter(tfmv_dir + player_material.mat_dir + mat_name + ".vmt", "basetexture", "tfmv\\flat_color.vtf");
                    return;
                }
                else if ((bodygroups_off.Count > 0) && (Main.grey_material))
                {
                    VMT.set_parameter(tfmv_dir + player_material.mat_dir + mat_name + ".vmt", "basetexture", player_material.mat_dir.ToLower().Replace("materials\\models\\", "models\\") + mat_name + ".vtf");
                    return;
                }

                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(tfmv_dir + player_material.mat_dir + mat_name + ".vmt");
                while ((rline = file.ReadLine()) != null)
                {
                    vmt_lines.Add(rline);
                }

                file.Close();

                // remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    // make sure its not a line that's commented out
                    if (line.Contains("{"))
                    {
                       
                        if (tf_class == "medic")
                        {
                            vmt_lines.Insert(i + 1, "\t$translucent 1\n\t$alphatest 1\n" + "\t$basemapalphaphongmask 0");
                        } else {
                            vmt_lines.Insert(i + 1, "\t$translucent 1\n\t$alphatest 1\n");
                        }
                        break;
                    }
                }

                // make VMT red, since we can't switch the model skin in HLMV
                if (team_color == "blue") { mat_name = mat_name.Replace("blue", "red"); }
             
                File.WriteAllLines(tfmv_dir + player_material.mat_dir + mat_name + ".vmt", vmt_lines);
            }
            catch
            {
            }

            #endregion
        }

        #endregion
    }
}
