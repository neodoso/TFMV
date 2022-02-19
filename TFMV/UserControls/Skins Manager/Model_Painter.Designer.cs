namespace TFMV.UserControls
{
    partial class Model_Painter
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
            this.components = new System.ComponentModel.Container();
            this.lab_mdl = new System.Windows.Forms.Label();
            this.panel_header = new System.Windows.Forms.Panel();
            this.cb_lock_skin = new System.Windows.Forms.CheckBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel_header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_mdl
            // 
            this.lab_mdl.AutoSize = true;
            this.lab_mdl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.lab_mdl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lab_mdl.Location = new System.Drawing.Point(3, 3);
            this.lab_mdl.Name = "lab_mdl";
            this.lab_mdl.Size = new System.Drawing.Size(41, 13);
            this.lab_mdl.TabIndex = 0;
            this.lab_mdl.Text = "Model";
            this.lab_mdl.MouseHover += new System.EventHandler(this.lab_mdl_MouseHover);
            // 
            // panel_header
            // 
            this.panel_header.BackColor = System.Drawing.Color.Silver;
            this.panel_header.Controls.Add(this.cb_lock_skin);
            this.panel_header.Controls.Add(this.lab_mdl);
            this.panel_header.Location = new System.Drawing.Point(-1, -1);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(879, 20);
            this.panel_header.TabIndex = 1;
            // 
            // cb_lock_skin
            // 
            this.cb_lock_skin.AutoSize = true;
            this.cb_lock_skin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_lock_skin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cb_lock_skin.Location = new System.Drawing.Point(798, 1);
            this.cb_lock_skin.Name = "cb_lock_skin";
            this.cb_lock_skin.Size = new System.Drawing.Size(76, 17);
            this.cb_lock_skin.TabIndex = 1;
            this.cb_lock_skin.Text = "Lock Skins";
            this.cb_lock_skin.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.ErrorImage = global::TFMV.Properties.Resources.icon_missing;
            this.pictureBox.Image = global::TFMV.Properties.Resources.icon_mdl_item;
            this.pictureBox.InitialImage = global::TFMV.Properties.Resources.icon_mdl_item;
            this.pictureBox.Location = new System.Drawing.Point(5, 5);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(64, 64);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Location = new System.Drawing.Point(0, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(75, 76);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Location = new System.Drawing.Point(0, 88);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(876, 10);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Location = new System.Drawing.Point(873, -1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 97);
            this.panel3.TabIndex = 5;
            // 
            // Model_Painter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_header);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.ForeColor = System.Drawing.Color.Silver;
            this.Name = "Model_Painter";
            this.Size = new System.Drawing.Size(875, 90);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_header;
        public System.Windows.Forms.Label lab_mdl;
        public System.Windows.Forms.CheckBox cb_lock_skin;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}
