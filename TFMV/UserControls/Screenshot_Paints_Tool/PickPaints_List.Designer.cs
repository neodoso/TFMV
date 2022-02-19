namespace TFMV.UserControls
{
    partial class PickPaints_List
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_paints = new System.Windows.Forms.Panel();
            this.cb_check_all = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_paints.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Location = new System.Drawing.Point(505, 327);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(160, 29);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select the paints to be included in the paints mosaic.";
            // 
            // panel_paints
            // 
            this.panel_paints.Controls.Add(this.cb_check_all);
            this.panel_paints.Location = new System.Drawing.Point(13, 13);
            this.panel_paints.Name = "panel_paints";
            this.panel_paints.Size = new System.Drawing.Size(652, 308);
            this.panel_paints.TabIndex = 3;
            // 
            // cb_check_all
            // 
            this.cb_check_all.AutoSize = true;
            this.cb_check_all.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_check_all.Location = new System.Drawing.Point(567, 284);
            this.cb_check_all.Name = "cb_check_all";
            this.cb_check_all.Size = new System.Drawing.Size(78, 17);
            this.cb_check_all.TabIndex = 2;
            this.cb_check_all.Text = "uncheck all";
            this.cb_check_all.UseVisualStyleBackColor = true;
            this.cb_check_all.CheckedChanged += new System.EventHandler(this.cb_check_all_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Location = new System.Drawing.Point(0, 319);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(678, 2);
            this.panel1.TabIndex = 4;
            // 
            // PickPaints_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 363);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.panel_paints);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PickPaints_List";
            this.Text = "Paints selection";
            this.panel_paints.ResumeLayout(false);
            this.panel_paints.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_paints;
        public System.Windows.Forms.CheckBox cb_check_all;
        private System.Windows.Forms.Panel panel1;
    }
}