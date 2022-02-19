namespace TFMV.UserControls
{
    partial class ModelSelect
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
            this.button1 = new System.Windows.Forms.Button();
            this.listBox_models = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(941, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add selected models";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox_models
            // 
            this.listBox_models.FormattingEnabled = true;
            this.listBox_models.Location = new System.Drawing.Point(13, 39);
            this.listBox_models.Name = "listBox_models";
            this.listBox_models.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_models.Size = new System.Drawing.Size(941, 173);
            this.listBox_models.TabIndex = 2;
            this.listBox_models.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_models_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select one or multiple items (hold Shift + left click) to add to the models list." +
    "";
            // 
            // ModelSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 260);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_models);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Model";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox_models;
        private System.Windows.Forms.Label label1;
    }
}