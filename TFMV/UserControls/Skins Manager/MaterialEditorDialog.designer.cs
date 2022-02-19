namespace TFMV.MaterialEditor
{
	partial class MaterialEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialEditorDialog));
            this.radioHue = new System.Windows.Forms.RadioButton();
            this.radioSaturation = new System.Windows.Forms.RadioButton();
            this.radioValue = new System.Windows.Forms.RadioButton();
            this.radioRed = new System.Windows.Forms.RadioButton();
            this.radioBlue = new System.Windows.Forms.RadioButton();
            this.radioGreen = new System.Windows.Forms.RadioButton();
            this.numHue = new System.Windows.Forms.NumericUpDown();
            this.numSaturation = new System.Windows.Forms.NumericUpDown();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.numRed = new System.Windows.Forms.NumericUpDown();
            this.numGreen = new System.Windows.Forms.NumericUpDown();
            this.numBlue = new System.Windows.Forms.NumericUpDown();
            this.labelOld = new System.Windows.Forms.Label();
            this.labelNew = new System.Windows.Forms.Label();
            this.panel_paint_picker = new System.Windows.Forms.Panel();
            this.numUpDwn_blendtintColor = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.vtab_paint_colorpicker = new System.Windows.Forms.Panel();
            this.lab_paint_colorPicker = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cb_phong_enable = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_phong = new System.Windows.Forms.Panel();
            this.numUpDwn_PhongExp = new System.Windows.Forms.NumericUpDown();
            this.numUpDwn_ExpBoost = new System.Windows.Forms.NumericUpDown();
            this.panel_rimlight = new System.Windows.Forms.Panel();
            this.numUpDwn_RimBoost = new System.Windows.Forms.NumericUpDown();
            this.numUpDwn_RimExp = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cb_rim_enable = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_get_vmt_code = new System.Windows.Forms.Button();
            this.colorPanel = new TFMV.MaterialEditor.ColorPanel();
            this.colorSlider = new TFMV.MaterialEditor.ColorSlider();
            this.colorShowBox = new TFMV.MaterialEditor.ColorShowBox();
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
            this.panel_paint_picker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_blendtintColor)).BeginInit();
            this.vtab_paint_colorpicker.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel_phong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_PhongExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_ExpBoost)).BeginInit();
            this.panel_rimlight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_RimBoost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_RimExp)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioHue
            // 
            resources.ApplyResources(this.radioHue, "radioHue");
            this.radioHue.Checked = true;
            this.radioHue.Name = "radioHue";
            this.radioHue.TabStop = true;
            this.radioHue.UseVisualStyleBackColor = true;
            this.radioHue.CheckedChanged += new System.EventHandler(this.radioHue_CheckedChanged);
            // 
            // radioSaturation
            // 
            resources.ApplyResources(this.radioSaturation, "radioSaturation");
            this.radioSaturation.Name = "radioSaturation";
            this.radioSaturation.UseVisualStyleBackColor = true;
            this.radioSaturation.CheckedChanged += new System.EventHandler(this.radioSaturation_CheckedChanged);
            // 
            // radioValue
            // 
            resources.ApplyResources(this.radioValue, "radioValue");
            this.radioValue.Name = "radioValue";
            this.radioValue.UseVisualStyleBackColor = true;
            this.radioValue.CheckedChanged += new System.EventHandler(this.radioValue_CheckedChanged);
            // 
            // radioRed
            // 
            resources.ApplyResources(this.radioRed, "radioRed");
            this.radioRed.Name = "radioRed";
            this.radioRed.UseVisualStyleBackColor = true;
            this.radioRed.CheckedChanged += new System.EventHandler(this.radioRed_CheckedChanged);
            // 
            // radioBlue
            // 
            resources.ApplyResources(this.radioBlue, "radioBlue");
            this.radioBlue.Name = "radioBlue";
            this.radioBlue.UseVisualStyleBackColor = true;
            this.radioBlue.CheckedChanged += new System.EventHandler(this.radioBlue_CheckedChanged);
            // 
            // radioGreen
            // 
            resources.ApplyResources(this.radioGreen, "radioGreen");
            this.radioGreen.Name = "radioGreen";
            this.radioGreen.UseVisualStyleBackColor = true;
            this.radioGreen.CheckedChanged += new System.EventHandler(this.radioGreen_CheckedChanged);
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
            resources.ApplyResources(this.numHue, "numHue");
            this.numHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numHue.Name = "numHue";
            this.numHue.ValueChanged += new System.EventHandler(this.numHue_ValueChanged);
            this.numHue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
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
            resources.ApplyResources(this.numSaturation, "numSaturation");
            this.numSaturation.Name = "numSaturation";
            this.numSaturation.ValueChanged += new System.EventHandler(this.numSaturation_ValueChanged);
            this.numSaturation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
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
            resources.ApplyResources(this.numValue, "numValue");
            this.numValue.Name = "numValue";
            this.numValue.ValueChanged += new System.EventHandler(this.numValue_ValueChanged);
            this.numValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // numRed
            // 
            this.numRed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.numRed, "numRed");
            this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRed.Name = "numRed";
            this.numRed.ValueChanged += new System.EventHandler(this.numRed_ValueChanged);
            this.numRed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // numGreen
            // 
            this.numGreen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.numGreen, "numGreen");
            this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGreen.Name = "numGreen";
            this.numGreen.ValueChanged += new System.EventHandler(this.numGreen_ValueChanged);
            this.numGreen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // numBlue
            // 
            this.numBlue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numBlue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.numBlue, "numBlue");
            this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBlue.Name = "numBlue";
            this.numBlue.ValueChanged += new System.EventHandler(this.numBlue_ValueChanged);
            this.numBlue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsv_rgb_KeyUp);
            // 
            // labelOld
            // 
            resources.ApplyResources(this.labelOld, "labelOld");
            this.labelOld.Name = "labelOld";
            // 
            // labelNew
            // 
            resources.ApplyResources(this.labelNew, "labelNew");
            this.labelNew.Name = "labelNew";
            // 
            // panel_paint_picker
            // 
            this.panel_paint_picker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_paint_picker.Controls.Add(this.numUpDwn_blendtintColor);
            this.panel_paint_picker.Controls.Add(this.colorPanel);
            this.panel_paint_picker.Controls.Add(this.colorSlider);
            this.panel_paint_picker.Controls.Add(this.label4);
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
            resources.ApplyResources(this.panel_paint_picker, "panel_paint_picker");
            this.panel_paint_picker.Name = "panel_paint_picker";
            // 
            // numUpDwn_blendtintColor
            // 
            this.numUpDwn_blendtintColor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numUpDwn_blendtintColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDwn_blendtintColor.DecimalPlaces = 2;
            this.numUpDwn_blendtintColor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDwn_blendtintColor, "numUpDwn_blendtintColor");
            this.numUpDwn_blendtintColor.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numUpDwn_blendtintColor.Name = "numUpDwn_blendtintColor";
            this.numUpDwn_blendtintColor.Tag = "blendtintcoloroverbase";
            this.numUpDwn_blendtintColor.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged_to_VMT);
            this.numUpDwn_blendtintColor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDwn_KeyUp);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // vtab_paint_colorpicker
            // 
            this.vtab_paint_colorpicker.BackColor = System.Drawing.Color.DimGray;
            this.vtab_paint_colorpicker.Controls.Add(this.lab_paint_colorPicker);
            resources.ApplyResources(this.vtab_paint_colorpicker, "vtab_paint_colorpicker");
            this.vtab_paint_colorpicker.Name = "vtab_paint_colorpicker";
            this.vtab_paint_colorpicker.Click += new System.EventHandler(this.vtab_paint_colorpicker_Click);
            this.vtab_paint_colorpicker.MouseLeave += new System.EventHandler(this.vtab_paint_colorpicker_MouseLeave);
            this.vtab_paint_colorpicker.MouseHover += new System.EventHandler(this.vtab_paint_colorpicker_MouseHover);
            // 
            // lab_paint_colorPicker
            // 
            resources.ApplyResources(this.lab_paint_colorPicker, "lab_paint_colorPicker");
            this.lab_paint_colorPicker.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lab_paint_colorPicker.Name = "lab_paint_colorPicker";
            this.lab_paint_colorPicker.Click += new System.EventHandler(this.vtab_paint_colorpicker_Click);
            this.lab_paint_colorPicker.MouseHover += new System.EventHandler(this.vtab_paint_colorpicker_MouseLeave);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Controls.Add(this.cb_phong_enable);
            this.panel2.Controls.Add(this.label6);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // cb_phong_enable
            // 
            resources.ApplyResources(this.cb_phong_enable, "cb_phong_enable");
            this.cb_phong_enable.Checked = true;
            this.cb_phong_enable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_phong_enable.Name = "cb_phong_enable";
            this.cb_phong_enable.Tag = "phong";
            this.cb_phong_enable.UseVisualStyleBackColor = true;
            this.cb_phong_enable.CheckedChanged += new System.EventHandler(this.cb_phong_enable_CheckedChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Name = "label6";
            // 
            // panel_phong
            // 
            this.panel_phong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_phong.Controls.Add(this.numUpDwn_PhongExp);
            this.panel_phong.Controls.Add(this.numUpDwn_ExpBoost);
            this.panel_phong.Controls.Add(this.panel2);
            this.panel_phong.Controls.Add(this.label1);
            this.panel_phong.Controls.Add(this.label2);
            resources.ApplyResources(this.panel_phong, "panel_phong");
            this.panel_phong.Name = "panel_phong";
            // 
            // numUpDwn_PhongExp
            // 
            this.numUpDwn_PhongExp.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numUpDwn_PhongExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDwn_PhongExp.DecimalPlaces = 2;
            this.numUpDwn_PhongExp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDwn_PhongExp, "numUpDwn_PhongExp");
            this.numUpDwn_PhongExp.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numUpDwn_PhongExp.Name = "numUpDwn_PhongExp";
            this.numUpDwn_PhongExp.Tag = "phongexponent";
            this.numUpDwn_PhongExp.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged_to_VMT);
            this.numUpDwn_PhongExp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDwn_KeyUp);
            // 
            // numUpDwn_ExpBoost
            // 
            this.numUpDwn_ExpBoost.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numUpDwn_ExpBoost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDwn_ExpBoost.DecimalPlaces = 2;
            this.numUpDwn_ExpBoost.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDwn_ExpBoost, "numUpDwn_ExpBoost");
            this.numUpDwn_ExpBoost.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numUpDwn_ExpBoost.Name = "numUpDwn_ExpBoost";
            this.numUpDwn_ExpBoost.Tag = "phongboost";
            this.numUpDwn_ExpBoost.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged_to_VMT);
            this.numUpDwn_ExpBoost.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDwn_KeyUp);
            // 
            // panel_rimlight
            // 
            this.panel_rimlight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_rimlight.Controls.Add(this.numUpDwn_RimBoost);
            this.panel_rimlight.Controls.Add(this.numUpDwn_RimExp);
            this.panel_rimlight.Controls.Add(this.panel5);
            this.panel_rimlight.Controls.Add(this.label5);
            this.panel_rimlight.Controls.Add(this.label7);
            resources.ApplyResources(this.panel_rimlight, "panel_rimlight");
            this.panel_rimlight.Name = "panel_rimlight";
            // 
            // numUpDwn_RimBoost
            // 
            this.numUpDwn_RimBoost.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numUpDwn_RimBoost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDwn_RimBoost.DecimalPlaces = 2;
            this.numUpDwn_RimBoost.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDwn_RimBoost, "numUpDwn_RimBoost");
            this.numUpDwn_RimBoost.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numUpDwn_RimBoost.Name = "numUpDwn_RimBoost";
            this.numUpDwn_RimBoost.Tag = "rimlightboost";
            this.numUpDwn_RimBoost.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged_to_VMT);
            this.numUpDwn_RimBoost.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDwn_KeyUp);
            // 
            // numUpDwn_RimExp
            // 
            this.numUpDwn_RimExp.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numUpDwn_RimExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUpDwn_RimExp.DecimalPlaces = 2;
            this.numUpDwn_RimExp.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDwn_RimExp, "numUpDwn_RimExp");
            this.numUpDwn_RimExp.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numUpDwn_RimExp.Name = "numUpDwn_RimExp";
            this.numUpDwn_RimExp.Tag = "rimlightexponent";
            this.numUpDwn_RimExp.ValueChanged += new System.EventHandler(this.NumUpDown_ValueChanged_to_VMT);
            this.numUpDwn_RimExp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numUpDwn_KeyUp);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DimGray;
            this.panel5.Controls.Add(this.cb_rim_enable);
            this.panel5.Controls.Add(this.label3);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // cb_rim_enable
            // 
            resources.ApplyResources(this.cb_rim_enable, "cb_rim_enable");
            this.cb_rim_enable.Checked = true;
            this.cb_rim_enable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_rim_enable.Name = "cb_rim_enable";
            this.cb_rim_enable.Tag = "rimlight";
            this.cb_rim_enable.UseVisualStyleBackColor = true;
            this.cb_rim_enable.CheckedChanged += new System.EventHandler(this.cb_rim_enable_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btn_get_vmt_code
            // 
            resources.ApplyResources(this.btn_get_vmt_code, "btn_get_vmt_code");
            this.btn_get_vmt_code.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btn_get_vmt_code.Name = "btn_get_vmt_code";
            this.btn_get_vmt_code.UseVisualStyleBackColor = true;
            this.btn_get_vmt_code.Click += new System.EventHandler(this.btn_get_vmt_code_Click);
            // 
            // colorPanel
            // 
            this.colorPanel.BottomLeftColor = System.Drawing.Color.Black;
            this.colorPanel.BottomRightColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.colorPanel, "colorPanel");
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.TopLeftColor = System.Drawing.Color.White;
            this.colorPanel.TopRightColor = System.Drawing.Color.Red;
            this.colorPanel.Value = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorPanel.ValuePercentual = ((System.Drawing.PointF)(resources.GetObject("colorPanel.ValuePercentual")));
            this.colorPanel.PercentualValueChanged += new System.EventHandler(this.colorPanel_PercentualValueChanged);
            this.colorPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseUp);
            // 
            // colorSlider
            // 
            resources.ApplyResources(this.colorSlider, "colorSlider");
            this.colorSlider.Name = "colorSlider";
            this.colorSlider.Value = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.colorSlider.PercentualValueChanged += new System.EventHandler(this.colorSlider_PercentualValueChanged);
            this.colorSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.colorSlider_MouseUp);
            // 
            // colorShowBox
            // 
            this.colorShowBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorShowBox.Color = System.Drawing.Color.DarkRed;
            resources.ApplyResources(this.colorShowBox, "colorShowBox");
            this.colorShowBox.LowerColor = System.Drawing.Color.Maroon;
            this.colorShowBox.Name = "colorShowBox";
            this.colorShowBox.UpperColor = System.Drawing.Color.DarkRed;
            this.colorShowBox.UpperClick += new System.EventHandler(this.colorShowBox_UpperClick);
            // 
            // MaterialEditorDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_get_vmt_code);
            this.Controls.Add(this.vtab_paint_colorpicker);
            this.Controls.Add(this.panel_rimlight);
            this.Controls.Add(this.panel_phong);
            this.Controls.Add(this.panel_paint_picker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MaterialEditorDialog_FormClosed);
            this.Load += new System.EventHandler(this.MaterialEditorDialog_Load);
            this.Shown += new System.EventHandler(this.MaterialEditorDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
            this.panel_paint_picker.ResumeLayout(false);
            this.panel_paint_picker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_blendtintColor)).EndInit();
            this.vtab_paint_colorpicker.ResumeLayout(false);
            this.vtab_paint_colorpicker.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel_phong.ResumeLayout(false);
            this.panel_phong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_PhongExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_ExpBoost)).EndInit();
            this.panel_rimlight.ResumeLayout(false);
            this.panel_rimlight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_RimBoost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwn_RimExp)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private ColorPanel colorPanel;
		private ColorSlider colorSlider;
		private ColorShowBox colorShowBox;
		private System.Windows.Forms.RadioButton radioHue;
		private System.Windows.Forms.RadioButton radioSaturation;
		private System.Windows.Forms.RadioButton radioValue;
		private System.Windows.Forms.RadioButton radioRed;
		private System.Windows.Forms.RadioButton radioBlue;
		private System.Windows.Forms.RadioButton radioGreen;
		private System.Windows.Forms.NumericUpDown numHue;
		private System.Windows.Forms.NumericUpDown numSaturation;
		private System.Windows.Forms.NumericUpDown numValue;
		private System.Windows.Forms.NumericUpDown numRed;
		private System.Windows.Forms.NumericUpDown numGreen;
        private System.Windows.Forms.NumericUpDown numBlue;
		private System.Windows.Forms.Label labelOld;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.Panel panel_paint_picker;
        private System.Windows.Forms.Panel vtab_paint_colorpicker;
        private System.Windows.Forms.Label lab_paint_colorPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cb_phong_enable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel_phong;
        private System.Windows.Forms.NumericUpDown numUpDwn_ExpBoost;
        private System.Windows.Forms.NumericUpDown numUpDwn_PhongExp;
        private System.Windows.Forms.Panel panel_rimlight;
        private System.Windows.Forms.NumericUpDown numUpDwn_RimBoost;
        private System.Windows.Forms.NumericUpDown numUpDwn_RimExp;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox cb_rim_enable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numUpDwn_blendtintColor;
        private System.Windows.Forms.Button btn_get_vmt_code;
    }
}