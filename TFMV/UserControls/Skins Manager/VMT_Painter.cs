using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TFMV
{
    public partial class VMT_Painter : UserControl
    {
        public byte id;
        public string vmt_path;
        public MaterialEditor.MaterialEditorDialog MatEditor;

        public byte flat_mat_switch = 1;

        // for weapon items, we don't have a skin backup
        // so we use this bool to know if we need to restore the vmt or not when using btn_flatmat_Click()
        private bool material_backup;

        public VMT_Painter()
        {
            InitializeComponent();
        }

        public void close_VMT_editor()
        {
            if (MatEditor != null)
            {
                MatEditor.Close();
            }
        }

        private void btn_edit_vmt_Click_1(object sender, EventArgs e)
        {
            if (MatEditor != null)
            {
                // MessageBox.Show("The editor for this material is already open.");
                MatEditor.Focus();
                return;
            }

            MatEditor = new MaterialEditor.MaterialEditorDialog(this);
            MatEditor.vmt_painter_id = this.id;
            MatEditor.vmt_path = Main.tfmv_dir + this.vmt_path;
            MatEditor.set_params(this.vmt_path, color_picker.SelectedItem.Color);
            MatEditor.StartPosition = FormStartPosition.Manual;
            MatEditor.Location = new Point(Cursor.Position.X  + 15, Cursor.Position.Y-15);

            MatEditor.Show();
        }

        // edit VMT and switch between original material / white mask / black mask
        private void btn_flatmat_Click(object sender, EventArgs e)
        {
            Color c = new Color();

            if(!material_backup)
            { 
                if (File.Exists(Main.tfmv_dir + vmt_path.Replace(".vmt", "_skin0.vmt")))
                {
                    material_backup = true;
                }
            }

            if (flat_mat_switch == 0)
            {

                string vmt_original = Main.tfmv_dir + vmt_path.Replace(".vmt", "_skin0.vmt");

                try
                {
                    if(File.Exists(vmt_original))
                    {
                        File.Copy(vmt_original, Main.tfmv_dir + vmt_path, true);  
                    }

                    // for weapon items (that don't have a backup _skin0.vmt)
                    // they just load the original vmt from the game's files
                    if(!material_backup)
                    {
                        if(File.Exists(Main.tfmv_dir + vmt_path))
                        File.Delete(Main.tfmv_dir + vmt_path);
                    }

                } catch {
                    MessageBox.Show("Failed to restore original material.\nPlease try switching team colors to restore materials.");
                }
            }

            if (flat_mat_switch == 1) { c = Color.White; }
            if (flat_mat_switch == 2) {  c = Color.Black; }

            btn_flatmat.BackColor = c;
            btn_flatmat.ForeColor = c;

            if(flat_mat_switch!=0)
            write_flat_mat(Main.tfmv_dir + this.vmt_path, c.R + " " + c.G + " " + c.B);

            // refresh HLMV
            Main.refresh_hlmv(false);

            flat_mat_switch++;

            if (flat_mat_switch > 2) { flat_mat_switch = 0; }    
        }

        // write VMT with flat constant color
        private void write_flat_mat(string vmt_path, string rgb)
        {

            if (!Directory.Exists(Path.GetDirectoryName(vmt_path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(vmt_path));
            }

            System.IO.File.WriteAllText(vmt_path, "\"UnlitGeneric\" \n{\n\t" + "\"$color2\" \"{ " + rgb + " }\"\n}");
        }
    }
}
