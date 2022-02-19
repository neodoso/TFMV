namespace TFMV.UserControls.Loadout
{
    partial class Loadout_Item
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
            this.label = new System.Windows.Forms.Label();
            this.btn_remove_item = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.label.Location = new System.Drawing.Point(0, 51);
            this.label.Margin = new System.Windows.Forms.Padding(0);
            this.label.Name = "label";
            this.label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label.Size = new System.Drawing.Size(65, 16);
            this.label.TabIndex = 0;
            this.label.Text = "no name";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label.Click += new System.EventHandler(this.select_Click);
            this.label.DoubleClick += new System.EventHandler(this.action_DoubleClick);
            // 
            // btn_remove_item
            // 
            this.btn_remove_item.BackColor = System.Drawing.Color.LightGray;
            this.btn_remove_item.BackgroundImage = global::TFMV.Properties.Resources.btn_x;
            this.btn_remove_item.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_remove_item.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_remove_item.ForeColor = System.Drawing.Color.LightGray;
            this.btn_remove_item.Location = new System.Drawing.Point(50, 1);
            this.btn_remove_item.Name = "btn_remove_item";
            this.btn_remove_item.Size = new System.Drawing.Size(12, 12);
            this.btn_remove_item.TabIndex = 2;
            this.btn_remove_item.UseVisualStyleBackColor = false;
            this.btn_remove_item.Click += new System.EventHandler(this.btn_remove_item_Click);
            // 
            // PictureBox
            // 
            this.PictureBox.ErrorImage = global::TFMV.Properties.Resources.icon_missing;
            this.PictureBox.Image = global::TFMV.Properties.Resources.icon_mdl_item;
            this.PictureBox.InitialImage = global::TFMV.Properties.Resources.icon_mdl_item;
            this.PictureBox.Location = new System.Drawing.Point(7, 5);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(50, 50);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 1;
            this.PictureBox.TabStop = false;
            this.PictureBox.Click += new System.EventHandler(this.select_Click);
            this.PictureBox.DoubleClick += new System.EventHandler(this.action_DoubleClick);
            // 
            // Loadout_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(this.btn_remove_item);
            this.Controls.Add(this.label);
            this.Controls.Add(this.PictureBox);
            this.Name = "Loadout_Item";
            this.Size = new System.Drawing.Size(65, 70);
            this.Click += new System.EventHandler(this.select_Click);
            this.DoubleClick += new System.EventHandler(this.action_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label;
        public System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Button btn_remove_item;
    }
}
