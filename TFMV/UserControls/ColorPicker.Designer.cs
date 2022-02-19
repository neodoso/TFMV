namespace TFMV.UserControls
{
    partial class ColorPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.panel_paint_picker = new System.Windows.Forms.Panel();
            this.labelNew = new System.Windows.Forms.Label();
            this.radioHue = new System.Windows.Forms.RadioButton();
            this.labelOld = new System.Windows.Forms.Label();
            this.radioSaturation = new System.Windows.Forms.RadioButton();
            this.numBlue = new System.Windows.Forms.NumericUpDown();
            this.radioValue = new System.Windows.Forms.RadioButton();
            this.numGreen = new System.Windows.Forms.NumericUpDown();
            this.radioRed = new System.Windows.Forms.RadioButton();
            this.numRed = new System.Windows.Forms.NumericUpDown();
            this.radioBlue = new System.Windows.Forms.RadioButton();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.radioGreen = new System.Windows.Forms.RadioButton();
            this.numSaturation = new System.Windows.Forms.NumericUpDown();
            this.numHue = new System.Windows.Forms.NumericUpDown();
            this.colorPanel = new TFMV.MaterialEditor.ColorPanel();
            this.colorSlider = new TFMV.MaterialEditor.ColorSlider();
            this.colorShowBox = new TFMV.MaterialEditor.ColorShowBox();
            this.panel_paint_picker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_paint_picker
            // 
            this.panel_paint_picker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_paint_picker.Controls.Add(this.colorPanel);
            this.panel_paint_picker.Controls.Add(this.colorSlider);
            this.panel_paint_picker.Controls.Add(this.colorShowBox);
            this.panel_paint_picker.Controls.Add(this.labelNew);
            this.panel_paint_picker.Controls.Add(this.radioHue);
            this.panel_paint_picker.Controls.Add(this.labelOld);
            this.panel_paint_picker.Controls.Add(this.radioSaturation);
            this.panel_paint_picker.Controls.Add(this.numBlue);
            this.panel_paint_picker.Controls.Add(this.radioValue);
            this.panel_paint_picker.Controls.Add(this.numGreen);
            this.panel_paint_picker.Controls.Add(this.radioRed);
            this.panel_paint_picker.Controls.Add(this.numRed);
            this.panel_paint_picker.Controls.Add(this.radioBlue);
            this.panel_paint_picker.Controls.Add(this.numValue);
            this.panel_paint_picker.Controls.Add(this.radioGreen);
            this.panel_paint_picker.Controls.Add(this.numSaturation);
            this.panel_paint_picker.Controls.Add(this.numHue);
            this.panel_paint_picker.Location = new System.Drawing.Point(0, 1);
            this.panel_paint_picker.Name = "panel_paint_picker";
            this.panel_paint_picker.Size = new System.Drawing.Size(405, 279);
            this.panel_paint_picker.TabIndex = 23;
            // 
            // labelNew
            // 
            this.labelNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNew.Location = new System.Drawing.Point(300, 37);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(33, 26);
            this.labelNew.TabIndex = 19;
            this.labelNew.Text = "New";
            this.labelNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioHue
            // 
            this.radioHue.AutoSize = true;
            this.radioHue.Checked = true;
            this.radioHue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioHue.Location = new System.Drawing.Point(301, 95);
            this.radioHue.Name = "radioHue";
            this.radioHue.Size = new System.Drawing.Size(33, 17);
            this.radioHue.TabIndex = 3;
            this.radioHue.TabStop = true;
            this.radioHue.Text = "H";
            this.radioHue.UseVisualStyleBackColor = true;
            this.radioHue.CheckedChanged += new System.EventHandler(this.radioHue_CheckedChanged);
            // 
            // labelOld
            // 
            this.labelOld.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelOld.Location = new System.Drawing.Point(300, 13);
            this.labelOld.Name = "labelOld";
            this.labelOld.Size = new System.Drawing.Size(33, 26);
            this.labelOld.TabIndex = 18;
            this.labelOld.Text = "Old";
            this.labelOld.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioSaturation
            // 
            this.radioSaturation.AutoSize = true;
            this.radioSaturation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioSaturation.Location = new System.Drawing.Point(301, 118);
            this.radioSaturation.Name = "radioSaturation";
            this.radioSaturation.Size = new System.Drawing.Size(32, 17);
            this.radioSaturation.TabIndex = 4;
            this.radioSaturation.Text = "S";
            this.radioSaturation.UseVisualStyleBackColor = true;
            this.radioSaturation.CheckedChanged += new System.EventHandler(this.radioSaturation_CheckedChanged);
            // 
            // numBlue
            // 
            this.numBlue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numBlue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numBlue.Location = new System.Drawing.Point(338, 242);
            this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBlue.Name = "numBlue";
            this.numBlue.Size = new System.Drawing.Size(54, 20);
            this.numBlue.TabIndex = 11;
            this.numBlue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numBlue.ValueChanged += new System.EventHandler(this.numBlue_ValueChanged);
            this.numBlue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // radioValue
            // 
            this.radioValue.AutoSize = true;
            this.radioValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioValue.Location = new System.Drawing.Point(301, 141);
            this.radioValue.Name = "radioValue";
            this.radioValue.Size = new System.Drawing.Size(32, 17);
            this.radioValue.TabIndex = 5;
            this.radioValue.Text = "V";
            this.radioValue.UseVisualStyleBackColor = true;
            this.radioValue.CheckedChanged += new System.EventHandler(this.radioValue_CheckedChanged);
            // 
            // numGreen
            // 
            this.numGreen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numGreen.Location = new System.Drawing.Point(338, 219);
            this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGreen.Name = "numGreen";
            this.numGreen.Size = new System.Drawing.Size(54, 20);
            this.numGreen.TabIndex = 10;
            this.numGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numGreen.ValueChanged += new System.EventHandler(this.numGreen_ValueChanged);
            this.numGreen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // radioRed
            // 
            this.radioRed.AutoSize = true;
            this.radioRed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioRed.Location = new System.Drawing.Point(301, 196);
            this.radioRed.Name = "radioRed";
            this.radioRed.Size = new System.Drawing.Size(33, 17);
            this.radioRed.TabIndex = 6;
            this.radioRed.Text = "R";
            this.radioRed.UseVisualStyleBackColor = true;
            this.radioRed.CheckedChanged += new System.EventHandler(this.radioRed_CheckedChanged);
            // 
            // numRed
            // 
            this.numRed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRed.Location = new System.Drawing.Point(338, 196);
            this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRed.Name = "numRed";
            this.numRed.Size = new System.Drawing.Size(54, 20);
            this.numRed.TabIndex = 9;
            this.numRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numRed.ValueChanged += new System.EventHandler(this.numRed_ValueChanged);
            this.numRed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // radioBlue
            // 
            this.radioBlue.AutoSize = true;
            this.radioBlue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioBlue.Location = new System.Drawing.Point(301, 242);
            this.radioBlue.Name = "radioBlue";
            this.radioBlue.Size = new System.Drawing.Size(32, 17);
            this.radioBlue.TabIndex = 7;
            this.radioBlue.Text = "B";
            this.radioBlue.UseVisualStyleBackColor = true;
            this.radioBlue.CheckedChanged += new System.EventHandler(this.radioBlue_CheckedChanged);
            // 
            // numValue
            // 
            this.numValue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numValue.Location = new System.Drawing.Point(338, 141);
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(54, 20);
            this.numValue.TabIndex = 8;
            this.numValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numValue.ValueChanged += new System.EventHandler(this.numValue_ValueChanged);
            this.numValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // radioGreen
            // 
            this.radioGreen.AutoSize = true;
            this.radioGreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioGreen.Location = new System.Drawing.Point(301, 219);
            this.radioGreen.Name = "radioGreen";
            this.radioGreen.Size = new System.Drawing.Size(33, 17);
            this.radioGreen.TabIndex = 8;
            this.radioGreen.Text = "G";
            this.radioGreen.UseVisualStyleBackColor = true;
            this.radioGreen.CheckedChanged += new System.EventHandler(this.radioGreen_CheckedChanged);
            // 
            // numSaturation
            // 
            this.numSaturation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numSaturation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSaturation.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSaturation.Location = new System.Drawing.Point(338, 118);
            this.numSaturation.Name = "numSaturation";
            this.numSaturation.Size = new System.Drawing.Size(54, 20);
            this.numSaturation.TabIndex = 7;
            this.numSaturation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSaturation.ValueChanged += new System.EventHandler(this.numSaturation_ValueChanged);
            this.numSaturation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // numHue
            // 
            this.numHue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numHue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numHue.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numHue.Location = new System.Drawing.Point(338, 95);
            this.numHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numHue.Name = "numHue";
            this.numHue.Size = new System.Drawing.Size(54, 20);
            this.numHue.TabIndex = 6;
            this.numHue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numHue.ValueChanged += new System.EventHandler(this.numHue_ValueChanged);
            this.numHue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // colorPanel
            // 
            this.colorPanel.BottomLeftColor = System.Drawing.Color.Black;
            this.colorPanel.BottomRightColor = System.Drawing.Color.Black;
            this.colorPanel.Location = new System.Drawing.Point(5, 4);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(260, 260);
            this.colorPanel.TabIndex = 0;
            this.colorPanel.TopLeftColor = System.Drawing.Color.White;
            this.colorPanel.TopRightColor = System.Drawing.Color.Red;
            this.colorPanel.Value = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorPanel.ValuePercentual = ((System.Drawing.PointF)(resources.GetObject("colorPanel.ValuePercentual")));
            this.colorPanel.PercentualValueChanged += new System.EventHandler(this.colorPanel_PercentualValueChanged);
            this.colorPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseUp);
            // 
            // colorSlider
            // 
            this.colorSlider.Location = new System.Drawing.Point(265, -1);
            this.colorSlider.Name = "colorSlider";
            this.colorSlider.Size = new System.Drawing.Size(32, 270);
            this.colorSlider.TabIndex = 1;
            this.colorSlider.Value = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.colorSlider.PercentualValueChanged += new System.EventHandler(this.colorSlider_PercentualValueChanged);
            this.colorSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorSlider_MouseUp);
            // 
            // colorShowBox
            // 
            this.colorShowBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorShowBox.Color = System.Drawing.Color.DarkRed;
            this.colorShowBox.Location = new System.Drawing.Point(337, 17);
            this.colorShowBox.LowerColor = System.Drawing.Color.Maroon;
            this.colorShowBox.Name = "colorShowBox";
            this.colorShowBox.Size = new System.Drawing.Size(55, 46);
            this.colorShowBox.TabIndex = 2;
            this.colorShowBox.UpperColor = System.Drawing.Color.DarkRed;
            this.colorShowBox.UpperClick += new System.EventHandler(this.colorShowBox_UpperClick);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 279);
            this.Controls.Add(this.panel_paint_picker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ColorPicker";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color Picker";
            this.Load += new System.EventHandler(this.ColorPicker_Load);
            this.Shown += new System.EventHandler(this.ColorPicker_Shown);
            this.panel_paint_picker.ResumeLayout(false);
            this.panel_paint_picker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_paint_picker;
        private MaterialEditor.ColorSlider colorSlider;
        private MaterialEditor.ColorShowBox colorShowBox;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.RadioButton radioHue;
        private System.Windows.Forms.Label labelOld;
        private System.Windows.Forms.RadioButton radioSaturation;
        private System.Windows.Forms.NumericUpDown numBlue;
        private System.Windows.Forms.RadioButton radioValue;
        private System.Windows.Forms.NumericUpDown numGreen;
        private System.Windows.Forms.RadioButton radioRed;
        private System.Windows.Forms.NumericUpDown numRed;
        private System.Windows.Forms.RadioButton radioBlue;
        private System.Windows.Forms.NumericUpDown numValue;
        private System.Windows.Forms.RadioButton radioGreen;
        private System.Windows.Forms.NumericUpDown numSaturation;
        private System.Windows.Forms.NumericUpDown numHue;
        public MaterialEditor.ColorPanel colorPanel;
    }
}