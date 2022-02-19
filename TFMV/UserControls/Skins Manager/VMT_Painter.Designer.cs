namespace TFMV
{
    partial class VMT_Painter
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
            this.label_vmt_name = new System.Windows.Forms.Label();
            this.btn_edit_vmt = new System.Windows.Forms.Button();
            this.color_picker = new TFMV.PaintColorPicker();
            this.btn_flatmat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_vmt_name
            // 
            this.label_vmt_name.AutoSize = true;
            this.label_vmt_name.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_vmt_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label_vmt_name.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_vmt_name.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label_vmt_name.Location = new System.Drawing.Point(9, 14);
            this.label_vmt_name.Name = "label_vmt_name";
            this.label_vmt_name.Size = new System.Drawing.Size(28, 13);
            this.label_vmt_name.TabIndex = 1;
            this.label_vmt_name.Tag = " ";
            this.label_vmt_name.Text = "VMT";
            // 
            // btn_edit_vmt
            // 
            this.btn_edit_vmt.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_edit_vmt.BackgroundImage = global::TFMV.Properties.Resources.pencil_32;
            this.btn_edit_vmt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_edit_vmt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_edit_vmt.ForeColor = System.Drawing.Color.Gainsboro;
            this.btn_edit_vmt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_edit_vmt.Location = new System.Drawing.Point(166, 6);
            this.btn_edit_vmt.Name = "btn_edit_vmt";
            this.btn_edit_vmt.Size = new System.Drawing.Size(27, 27);
            this.btn_edit_vmt.TabIndex = 3;
            this.btn_edit_vmt.Tag = "Material Editor";
            this.btn_edit_vmt.UseVisualStyleBackColor = false;
            this.btn_edit_vmt.Click += new System.EventHandler(this.btn_edit_vmt_Click_1);
            // 
            // color_picker
            // 
            this.color_picker.BackColor = System.Drawing.Color.WhiteSmoke;
            this.color_picker.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.color_picker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.color_picker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.color_picker.Font = new System.Drawing.Font("Verdana", 7F);
            this.color_picker.FormattingEnabled = true;
            this.color_picker.Location = new System.Drawing.Point(9, 37);
            this.color_picker.Name = "color_picker";
            this.color_picker.SelectedItem = null;
            this.color_picker.SelectedValue = System.Drawing.Color.White;
            this.color_picker.Size = new System.Drawing.Size(184, 20);
            this.color_picker.TabIndex = 0;
            this.color_picker.VMT = "";
            // 
            // btn_flatmat
            // 
            this.btn_flatmat.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_flatmat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_flatmat.ForeColor = System.Drawing.Color.LightGray;
            this.btn_flatmat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_flatmat.Location = new System.Drawing.Point(147, 13);
            this.btn_flatmat.Name = "btn_flatmat";
            this.btn_flatmat.Size = new System.Drawing.Size(15, 15);
            this.btn_flatmat.TabIndex = 33;
            this.btn_flatmat.UseVisualStyleBackColor = false;
            this.btn_flatmat.Click += new System.EventHandler(this.btn_flatmat_Click);
            // 
            // VMT_Painter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.btn_flatmat);
            this.Controls.Add(this.btn_edit_vmt);
            this.Controls.Add(this.label_vmt_name);
            this.Controls.Add(this.color_picker);
            this.Name = "VMT_Painter";
            this.Size = new System.Drawing.Size(200, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label_vmt_name;
        public PaintColorPicker color_picker;
        private System.Windows.Forms.Button btn_edit_vmt;
        private System.Windows.Forms.Button btn_flatmat;
    }
}
