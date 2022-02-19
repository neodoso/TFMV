namespace TFMV.SourceEngine
{
    partial class SteamGameConfig
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_sel_tf2_dir = new System.Windows.Forms.Button();
            this.lab_steamdir = new System.Windows.Forms.Label();
            this.txtb_moddir = new System.Windows.Forms.TextBox();
            this.txtb_steamdir = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_get_steamdir = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_sel_steamdir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_sel_tf2_dir);
            this.groupBox4.Controls.Add(this.lab_steamdir);
            this.groupBox4.Controls.Add(this.txtb_moddir);
            this.groupBox4.Controls.Add(this.txtb_steamdir);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.btn_get_steamdir);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.btn_sel_steamdir);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(758, 117);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Steam";
            // 
            // btn_sel_tf2_dir
            // 
            this.btn_sel_tf2_dir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sel_tf2_dir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_sel_tf2_dir.Location = new System.Drawing.Point(575, 80);
            this.btn_sel_tf2_dir.Name = "btn_sel_tf2_dir";
            this.btn_sel_tf2_dir.Size = new System.Drawing.Size(82, 23);
            this.btn_sel_tf2_dir.TabIndex = 8;
            this.btn_sel_tf2_dir.Text = "Select Directory";
            this.btn_sel_tf2_dir.UseVisualStyleBackColor = true;
            this.btn_sel_tf2_dir.Click += new System.EventHandler(this.btn_sel_mod_dir_Click);
            // 
            // lab_steamdir
            // 
            this.lab_steamdir.AutoSize = true;
            this.lab_steamdir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lab_steamdir.Location = new System.Drawing.Point(13, 22);
            this.lab_steamdir.Name = "lab_steamdir";
            this.lab_steamdir.Size = new System.Drawing.Size(82, 13);
            this.lab_steamdir.TabIndex = 2;
            this.lab_steamdir.Text = "Steam Directory";
            // 
            // txtb_moddir
            // 
            this.txtb_moddir.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtb_moddir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtb_moddir.Enabled = false;
            this.txtb_moddir.Location = new System.Drawing.Point(13, 81);
            this.txtb_moddir.Name = "txtb_moddir";
            this.txtb_moddir.Size = new System.Drawing.Size(556, 20);
            this.txtb_moddir.TabIndex = 7;
            // 
            // txtb_steamdir
            // 
            this.txtb_steamdir.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtb_steamdir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtb_steamdir.Enabled = false;
            this.txtb_steamdir.Location = new System.Drawing.Point(13, 39);
            this.txtb_steamdir.Name = "txtb_steamdir";
            this.txtb_steamdir.Size = new System.Drawing.Size(556, 20);
            this.txtb_steamdir.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(13, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "TF Directory";
            // 
            // btn_get_steamdir
            // 
            this.btn_get_steamdir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_get_steamdir.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_get_steamdir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_get_steamdir.Location = new System.Drawing.Point(575, 38);
            this.btn_get_steamdir.Name = "btn_get_steamdir";
            this.btn_get_steamdir.Size = new System.Drawing.Size(82, 23);
            this.btn_get_steamdir.TabIndex = 1;
            this.btn_get_steamdir.Text = "Detect";
            this.btn_get_steamdir.UseVisualStyleBackColor = true;
            this.btn_get_steamdir.Click += new System.EventHandler(this.btn_get_steamdir_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(101, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(393, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Example: C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Team Fortress 2\\tf\\";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(101, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Example: C:\\Program Files (x86)\\Steam";
            // 
            // btn_sel_steamdir
            // 
            this.btn_sel_steamdir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_sel_steamdir.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_sel_steamdir.Location = new System.Drawing.Point(663, 38);
            this.btn_sel_steamdir.Name = "btn_sel_steamdir";
            this.btn_sel_steamdir.Size = new System.Drawing.Size(82, 23);
            this.btn_sel_steamdir.TabIndex = 1;
            this.btn_sel_steamdir.Text = "Select Directory";
            this.btn_sel_steamdir.UseVisualStyleBackColor = true;
            this.btn_sel_steamdir.Click += new System.EventHandler(this.btn_sel_steamdir_Click);
            // 
            // SteamGameConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "SteamGameConfig";
            this.Size = new System.Drawing.Size(765, 125);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_sel_tf2_dir;
        private System.Windows.Forms.Label lab_steamdir;
        private System.Windows.Forms.TextBox txtb_moddir;
        private System.Windows.Forms.TextBox txtb_steamdir;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_get_steamdir;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_sel_steamdir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
