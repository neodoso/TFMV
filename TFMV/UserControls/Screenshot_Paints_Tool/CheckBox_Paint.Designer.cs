namespace TFMV.UserControls.Screenshot_Paints_Tool
{
    partial class CheckBox_Paint
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
            this.cb_paint = new System.Windows.Forms.CheckBox();
            this.label_paint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cb_paint
            // 
            this.cb_paint.AutoSize = true;
            this.cb_paint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_paint.Location = new System.Drawing.Point(8, 7);
            this.cb_paint.Name = "cb_paint";
            this.cb_paint.Size = new System.Drawing.Size(12, 11);
            this.cb_paint.TabIndex = 0;
            this.cb_paint.UseVisualStyleBackColor = true;
            // 
            // label_paint
            // 
            this.label_paint.AutoSize = true;
            this.label_paint.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label_paint.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_paint.Location = new System.Drawing.Point(20, 5);
            this.label_paint.Name = "label_paint";
            this.label_paint.Size = new System.Drawing.Size(59, 13);
            this.label_paint.TabIndex = 1;
            this.label_paint.Text = "paint name";
            this.label_paint.Click += new System.EventHandler(this.check_box);
            this.label_paint.MouseLeave += new System.EventHandler(this.exit_hover);
            this.label_paint.MouseHover += new System.EventHandler(this.on_hover);
            // 
            // CheckBox_Paint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.label_paint);
            this.Controls.Add(this.cb_paint);
            this.Name = "CheckBox_Paint";
            this.Size = new System.Drawing.Size(160, 25);
            this.Click += new System.EventHandler(this.check_box);
            this.MouseLeave += new System.EventHandler(this.exit_hover);
            this.MouseHover += new System.EventHandler(this.on_hover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox cb_paint;
        public System.Windows.Forms.Label label_paint;
    }
}
