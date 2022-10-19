using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TFMV.SourceEngine;
using System.IO;
using System.Diagnostics;

namespace TFMV.UserControls
{
    public partial class Model_Painter : UserControl
    {
        #region declarations

        public byte skin_red_override = 0;
        public byte skin_blu_override = 1;
        public byte skin_override_all = 255;
        public bool not_paintable = false;

        public string paint_dir = "";
        public string tf_dir = "";

        public List<List<string>> skins;

        private string[] main_skins = new string[4];
        private string[] main_skins_backups = new string[4];

        private int skin_num = 0;

        public List<VMT_Painter> vmt_Painterslist = new List<VMT_Painter>();

        #endregion

        public Model_Painter()
        {
            InitializeComponent();
        }

        // closes VMT editor forms if they exist
        public void close_forms()
        {
            foreach (var vmtPainter in this.Controls.OfType<VMT_Painter>())
            {
                vmtPainter.close_VMT_editor();
            }
        }

        public void Load_Skins(int in_skin_num)
        {
            vmt_Painterslist = new List<VMT_Painter>();
            skin_num = in_skin_num;

            //SET main_skins_backups 
            int max_skins = 4;
            if (skin_num >= skins.Count) { skin_num = skins.Count - 1; }
            if (skins.Count == 0) { return; }

            #region Back/Restore the base VMT (skin 0)

            //make vmt lists and backup list for the skins
            //for (int s = 0; s < skins.Count; s++)
            int s = 0;
            if (s > 4) { return; }

            //if (skins[s].Count > 4) { System.Windows.Forms.MessageBox.Show("Note: only the first 4 materials of a model will be loaded, " + this.lab_mdl.Text + " has " + skins[s].Count + " materials."); }
            //byte skin_count = 4 - ;
            // maximum skins allowed = 4

            for (int m = 0; m < 4; m++) //(int m = 0; m < 1; m++) //skins[s].Count
            {
                if (m == skins[s].Count) { break; }
                main_skins[m] = skins[s][m];
                main_skins_backups[m] = skins[s][m].Replace(".vmt", "") + "_skin" + s + ".vmt";
                max_skins -= 1;
            }

            // we can't switch the skin for attached models in HLMV
            // so instead we replace the VMT that the model uses as skin 0
            // we need to make a copy of the original VMT for skin: 0, so we won't need to re-extract or import it again when we restore the skin 0's VMT
            // this better than re-extracting from VPK, or for workshop zip files
            // where we would need to re-extract the specific file from the zip

            //restore skin 0 VMT to its original if skin 0 is loaded by the user
            if (skin_num == 0)
            {
                for (int i = 0; i < main_skins_backups.Length; i++)
                {
                    if (main_skins_backups[i] == null) { continue; }
                    //main_skins_backups[skin_num] = main_skins[skin_num].Replace(".vmt", "") + "_skin" + skin_num + ".vmt";
                    if (File.Exists(main_skins_backups[i]))
                    {
                        File.Copy(paint_dir + main_skins_backups[i], paint_dir + main_skins[i], true);
                    } 
                }
            }


            for (int i = 0; i < main_skins_backups.Length; i++)
            {
                if (main_skins_backups[i] == null) { continue; }

                //  main_skins_backups[skin_num] = main_skins[skin_num].Replace(".vmt", "") + "_skin" + skin_num + ".vmt";
                if ((File.Exists(paint_dir + main_skins[i])) && (main_skins[i] != "")) //&& File.Exists((paint_dir + main_vmt))
                {
                    File.Copy(paint_dir + main_skins[i], paint_dir + main_skins_backups[i], true);
                }
            }

            #endregion

            //for each skin Add_VMT()
            for (byte i = 0; i < skins[skin_num].Count; i++) // skins[skin_num].Count
            {
                if (i == 4) { break; }
                Add_VMT(skins[skin_num][i]);
            }

            //add VMT_Painters from vmt_Painterslist to form
            for (byte i = 0; i < vmt_Painterslist.Count; i++)
            {
                VMT_Painter vmtP = vmt_Painterslist[i];
                vmtP.Location = new Point(this.Location.X + i * 200 + 75, this.Location.Y + 20);
                if (IsOdd(i)) { vmtP.BackColor = Color.FromArgb(230, 230, 230); }

                vmtP.id = i;

                this.Controls.Add(vmtP);
                //vmtP.SendToBack();
            }
        }

        public void Add_VMT(string vmt_path)
        {
            VMT_Painter vmtPainter = new VMT_Painter();

            vmtPainter.label_vmt_name.Text = Path.GetFileNameWithoutExtension(vmt_path) + ".vmt";
            vmtPainter.label_vmt_name.Tag = paint_dir + vmt_path;
            vmtPainter.vmt_path = vmt_path;

            vmtPainter.label_vmt_name.Click += new EventHandler(vmt_textEdit);
            vmtPainter.color_picker.SelectedIndexChanged += new EventHandler(vmt_PaintChanged);

            PaintColorPicker cp = vmtPainter.color_picker;
            cp.enabled(true);
            cp.SelectedIndex = 0;

            // check if material is paintable
            // add tint base color
            if ((VMT.check_paintability(paint_dir + vmt_path)) && (!not_paintable))
            {
                string colorbase = VMT.get_colorbase(paint_dir + vmt_path);
                VMT.set_color2(paint_dir + vmt_path, colorbase);

                //get paints list from Form1
                cp.AddPaints(TFMV.TF2.paints.red);
                //add color base
                cp.EditPaint("VMT ColorTint Base:" + colorbase);
                cp.enabled(true);
                cp.Refresh();
            }
            else if ((VMT.check_paintability(paint_dir + vmt_path)) && (not_paintable))
            {
                string colorbase = VMT.get_colorbase(paint_dir + vmt_path);
                VMT.set_color2(paint_dir + vmt_path, colorbase);
                cp.EditPaint("VMT ColorTint Base:" + colorbase);
                cp.enabled(false);
                cp.Refresh();
            }
            else  //not paintable
            {
                cp.BackColor = Color.DarkGray;
                cp.EditPaint("Not paintable");
                cp.enabled(false);
                cp.Refresh();
            }

            vmtPainter.Location = new Point(this.Location.X, this.Location.Y + 5);
            vmt_Painterslist.Add(vmtPainter);
        }

        public void switch_Skin(int in_skin_num)
        {           
           byte vmt_num = 0; // index for VMT_painter (4 VMTs per VMT_Painter)

           // override skins for red/blu
           if ((skin_red_override != 0) && (skin_blu_override != 1) )
           {
               if ((skin_override_all == 255))
               { 
                   if (in_skin_num == 0) { in_skin_num = skin_red_override; }
                   if (in_skin_num == 1) { in_skin_num = skin_blu_override; }
               }
           }

            // override all team skins
           if (skin_override_all != 255)
           {
               in_skin_num = skin_override_all;
           }

            //for selected VMT style
           foreach (var vmtPainter in this.Controls.OfType<VMT_Painter>())
           {
               vmtPainter.label_vmt_name.Text = "";
               vmtPainter.vmt_path = "";

               //loop through selected skin(in_skin_num) and add VMT editor
               //from TF folder or from the VPKs
               int max_skins = 4;

               // if skin number higher than number of skins, set it to last skin
               if (in_skin_num >= skins.Count) { in_skin_num = skins.Count - 1; }

               vmtPainter.vmt_path = skins[in_skin_num][vmt_num];

               vmtPainter.label_vmt_name.Text = Path.GetFileName(skins[in_skin_num][vmt_num]);
               vmtPainter.label_vmt_name.Tag = paint_dir + skins[in_skin_num][vmt_num];
               vmtPainter.label_vmt_name.Click += new EventHandler(vmt_textEdit);

               max_skins -= 1;

               bool is_paintable = false;
               is_paintable = VMT.check_paintability(paint_dir + vmtPainter.vmt_path);

               // if its  skin0
               if (vmtPainter.vmt_path == main_skins[vmt_num])
               {
                   if (File.Exists(paint_dir + main_skins_backups[vmt_num]))
                   {
                       File.Copy(paint_dir + main_skins_backups[vmt_num], paint_dir + main_skins[vmt_num], true);
                       is_paintable = VMT.check_paintability(paint_dir + vmtPainter.vmt_path);
                   }
               }

               // if its not skin0
               if (vmtPainter.vmt_path != main_skins[vmt_num])
               {
                   if (File.Exists(paint_dir + vmtPainter.vmt_path))
                   {
                       File.Copy(paint_dir + vmtPainter.vmt_path, paint_dir + main_skins[vmt_num], true);
                       is_paintable = VMT.check_paintability(paint_dir + main_skins[vmt_num]);
                   }
               }

               // set base color
               if ((vmtPainter.color_picker.Enabled) && (vmtPainter.color_picker.SelectedIndex > 0))
               { 
                   if (is_paintable)
                   {
                       string colorbase = VMT.get_colorbase(paint_dir + vmtPainter.vmt_path);
                       vmtPainter.color_picker.EditPaint("VMT ColorTint Base:" + colorbase);
                   }
               }

               //if color picker is enabled (is paintable) and selected paint is not colorbase
               if ((vmtPainter.color_picker.Enabled) && (vmtPainter.color_picker.SelectedIndex > 0))
               {
                   Color c = vmtPainter.color_picker.SelectedItem.Color;
                   VMT.set_color2(paint_dir + main_skins[0], c.R.ToString() + " " + c.G.ToString() + " " + c.B.ToString());
               }
               else
               {
                   if (is_paintable)
                   {
                       vmtPainter.color_picker.enabled(true);
                       string colorbase = VMT.get_colorbase(paint_dir + vmtPainter.vmt_path);
                       VMT.set_color2(paint_dir + main_skins[vmt_num], colorbase);
                       vmtPainter.color_picker.EditPaint("VMT ColorTint Base:" + colorbase);

                       vmtPainter.color_picker.Refresh();
                   }
                   else
                   {
                       //vmtPainter.BackColor = Color.DarkGray;
                       vmtPainter.color_picker.EditPaint("Not paintable");
                       vmtPainter.color_picker.enabled(false);
                       vmtPainter.color_picker.Refresh();
                   }
               }

               vmt_num++;
           }
        }

        // if material_name_skin0.vmt copy exists, rewrite material_name.vmt as (the original skin) material_name_skin0.vmt
        public void restore_original_skin0()
        {
            // for selected VMT style
            foreach (var vmtPainter in this.Controls.OfType<VMT_Painter>())
            {
                if (vmtPainter.vmt_path == main_skins[0])
                {
                    if (File.Exists(paint_dir + main_skins_backups[0]))
                    {
                        File.Copy(paint_dir + main_skins_backups[0], paint_dir + main_skins[0], true);
                    }
                }
            }
        }


        private void vmt_PaintChanged(object sender, EventArgs e)
        {

            if (!TFMV.Main.auto_refresh_paints) { return; }
            // do not update if "paint all" or "team skin switch" is doing it so we don't unecesarily rewrite the vmt multiple times
            if (TFMV.Main.auto_refresh_busy) { return; } 

            PaintColorPicker cp = (PaintColorPicker)sender;
            VMT_Painter vmt = (VMT_Painter)cp.Parent;

            if (vmt.vmt_path == main_skins[vmt.id])
            {
                if (File.Exists(paint_dir + main_skins_backups[vmt.id]))
                {
                    File.Copy(paint_dir + main_skins_backups[vmt.id], paint_dir + vmt.vmt_path, true);
                }
            }

            // make sure we aren't copying the same file (in the case where its the VMT0)
            if (vmt.vmt_path != main_skins[vmt.id])
            {
                if (File.Exists(paint_dir + vmt.vmt_path))
                {
                    File.Copy(paint_dir + vmt.vmt_path, paint_dir + main_skins[vmt.id], true);
                }
                else
                {
                   // MessageBox.Show("Error: could not find material: " + paint_dir + vmt.vmt_path);
                }
            }

            bool paintable = vmt.color_picker.Text.ToLower().Contains("not paintable");

            // if color picker is enabled aka if VMT is paintable, then we update the VMT paint $color2
            // for some reason its not possible to set the color_picker "Enabled" property
            // maybe it needs to be exposed since its a partial class derived from ComboBox not sure how to do it

            if (paintable)
            {
                vmt.color_picker.enabled(false);
                vmt.color_picker.Refresh();
            }
            else
            {
                Color c = vmt.color_picker.SelectedItem.Color;
                VMT.set_color2(paint_dir + main_skins[vmt.id], c.R.ToString() + " " + c.G.ToString() + " " + c.B.ToString());
            }

            TFMV.Main.refresh_hlmv(false);
        }


        private void vmt_textEdit(object sender, EventArgs e)
        {
            Label vmt_label = (Label)sender;

            string vmt_path = vmt_label.Tag.ToString();

            if (File.Exists(vmt_path))
            {
                Process.Start(vmt_path);
            }
        }

        //loop through VMT's and set paints and styles
        public void update_paints()
        {
            byte vmt_num = 0;

            //for selected VMT style
            foreach (var vmt in this.Controls.OfType<VMT_Painter>())
            {
                // copy selected VMT skin style as skin0
               // if (vmt.cb_use_style.Checked)
                
                // if selecting VMT skin = 0, 
                // retstore original VMT skin
                if (vmt.vmt_path == main_skins[vmt_num])
                {
                        if (File.Exists(paint_dir + main_skins_backups[vmt_num]))
                        {
                            File.Copy(paint_dir + main_skins_backups[vmt_num], paint_dir + vmt.vmt_path, true);
                        }
                }

                //make sure we aren't copying the same file (in the case where its the VMT0)
                if (vmt.vmt_path != main_skins[vmt_num])
                {
                        if (File.Exists(paint_dir + vmt.vmt_path))
                        {
                            File.Copy(paint_dir + vmt.vmt_path, paint_dir + main_skins[vmt_num], true);
                        } else{
                          //  MessageBox.Show("Error: could not find material: " + paint_dir + vmt.vmt_path);
                        }
                }

                bool paintable = vmt.color_picker.Text.ToLower().Contains("not paintable");

                // if color picker is enabled (so, if VMT is paintable) then we update the VMT paint $color2
                // for some reason its not possible to set the color_picker "Enabled" property
                // maybe it needs to be exposed since its a partial class derived from ComboBox not sure how to do it

                if (paintable)
                {
                    vmt.color_picker.enabled(false);
                    vmt.color_picker.Refresh();
                }
                else
                {
                    Color c = vmt.color_picker.SelectedItem.Color;
                    VMT.set_color2(paint_dir + main_skins[vmt_num], c.R.ToString() + " " + c.G.ToString() + " " + c.B.ToString());
                }

                // return; //exit since we found and updated the selected VMT style
                vmt_num++;
            }
        }


        private static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        private void lab_mdl_MouseHover(object sender, EventArgs e)
        {
        }
    }
}
