using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using TFMV.MaterialEditor;

namespace TFMV.UserControls
{

    public partial class ColorPicker : Form
    {

        Color color_original;
        bool form_loaded;

        #region declarations global

        public string vmt_path { get; set; }
        public string vmt_name { get; set; }
        public int vmt_painter_id { get; set; }


        #endregion

        #region numeirc updown value changes delay timers
        // timer for when numericUpdown values are changed by user, so we only get the new value once the user is done typing
        // or using the up/down buttons, so we also do not spam the HLMV refresh and VMT file editing

        System.Timers.Timer refresh_timer;
        private System.Timers.Timer _Timer;
        private System.Timers.Timer _Timer_paintChange;

        bool numBox_typed_value;
        bool numBox_rgb_hsv_typed_value;
        bool refresh_busy;

        Object last_updated_input;

        #endregion

        public ColorPicker(Color _color_input)
        {
            InitializeComponent();

            vmt_path = Main.tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt";
            vmt_name = "tfmv_bg";

            color_original = _color_input;

            refresh_timer = new System.Timers.Timer();
            refresh_timer.Interval = 1800; //In milliseconds here
            refresh_timer.AutoReset = true; //Stops it from repeating
            refresh_timer.Elapsed += new ElapsedEventHandler(TimerElapsed);

            _Timer = new System.Timers.Timer() { AutoReset = false, Interval = 700 };
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);
            numBox_typed_value = false;

            _Timer_paintChange = new System.Timers.Timer() { AutoReset = false, Interval = 700 };
            _Timer_paintChange.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_paintChange_Elapsed);
            numBox_typed_value = false;

            this.ShowInTaskbar = true;
        }

        private void update_bg_color(Color c)
        {
            Main.set_bgColor(c);
        }


        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //events of values changes fire while form is being setup
            // so we disable refreshing, so VMT isn't edit and HLMV not refreshed
            refresh_busy = false;
        }

        private void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // update stuff
            if ((!refresh_busy) && (form_loaded))
            {
                if (last_updated_input != null)
                    refresh_busy = true;

                update_bg_color(colorPanel.Value);

                refresh_timer.Start();
            }
            numBox_typed_value = false;
        }

        private void _Timer_paintChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if ((!refresh_busy) && (form_loaded))
            {
                string R = "0"; string G = "0"; string B = "0";

                this.Invoke((MethodInvoker)delegate { R = this.numRed.Value.ToString(); });
                this.Invoke((MethodInvoker)delegate { G = this.numGreen.Value.ToString(); });
                this.Invoke((MethodInvoker)delegate { B = this.numBlue.Value.ToString(); });

                refresh_busy = true;
                SourceEngine.VMT.set_color2(vmt_path, R + " " + G + " " + B);
                Main.refresh_hlmv(false);

                refresh_timer.Start();

                numBox_rgb_hsv_typed_value = false;
            }
        }

        #region color picker

        public enum PrimaryAttrib
        {
            Hue,
            Saturation,
            Brightness,
            Red,
            Green,
            Blue
        }

        private struct InternalColor
        {
            public float h;
            public float s;
            public float v;
            public float a;

            public InternalColor(float h, float s, float v, float a)
            {
                this.h = h;
                this.s = s;
                this.v = v;
                this.a = a;
            }
            public InternalColor(Color c)
            {
                this.h = c.GetHSVHue();
                this.s = c.GetHSVSaturation();
                this.v = c.GetHSVBrightness();
                this.a = c.A / 255.0f;
            }

            public Color ToColor()
            {
                return Color.FromArgb((int)Math.Round(this.a * 255.0f), ExtMethodsSystemDrawingColor.ColorFromHSV(this.h, this.s, this.v));
            }
        }



        private bool alphaEnabled = true;
        private InternalColor oldColor = new InternalColor(Color.Red);
        private InternalColor selColor = new InternalColor(Color.Red);
        private PrimaryAttrib primAttrib = PrimaryAttrib.Hue;
        private bool suspendTextEvents = false;

        public bool AlphaEnabled
        {
            get { return this.alphaEnabled; }
            set
            {
                this.alphaEnabled = value;
                //this.alphaSlider.Enabled = this.alphaEnabled;
                //this.numAlpha.Enabled = this.alphaEnabled;
            }
        }
        public Color OldColor
        {
            get { return this.oldColor.ToColor(); }
            set { this.oldColor = new InternalColor(value); this.UpdateColorShowBox(); }
        }
        public Color SelectedColor
        {
            get { return this.selColor.ToColor(); }
            set { this.selColor = new InternalColor(value); this.UpdateColorControls(); }
        }
        public PrimaryAttrib PrimaryAttribute
        {
            get { return this.primAttrib; }
            set { this.primAttrib = value; this.UpdateColorControls(); }
        }


        private void UpdateColorControls()
        {
            this.UpdatePrimaryAttributeRadioBox();
            this.UpdateText();
            this.UpdateColorShowBox();
            this.UpdateColorPanelGradient();
            this.UpdateColorSliderGradient();

            this.UpdateColorPanelValue();
            this.UpdateColorSliderValue();

        }
        private void UpdatePrimaryAttributeRadioBox()
        {
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.radioHue.Checked = true;
                    break;
                case PrimaryAttrib.Saturation:
                    this.radioSaturation.Checked = true;
                    break;
                case PrimaryAttrib.Brightness:
                    this.radioValue.Checked = true;
                    break;
                case PrimaryAttrib.Red:
                    this.radioRed.Checked = true;
                    break;
                case PrimaryAttrib.Green:
                    this.radioGreen.Checked = true;
                    break;
                case PrimaryAttrib.Blue:
                    this.radioBlue.Checked = true;
                    break;
            }
        }
        private void UpdateText()
        {
            Color tmp = this.selColor.ToColor();
            this.suspendTextEvents = true;

            this.numRed.Value = tmp.R;
            this.numGreen.Value = tmp.G;
            this.numBlue.Value = tmp.B;

            try
            {
                this.numHue.Value = (decimal)(this.selColor.h * 360.0f);
                this.numSaturation.Value = (decimal)(this.selColor.s * 100.0f);
                this.numValue.Value = (decimal)(this.selColor.v * 100.0f);

            }
            catch
            {
            }

            this.suspendTextEvents = false;
        }

        private void UpdateColorShowBox()
        {
            this.colorShowBox.UpperColor = this.alphaEnabled ? this.oldColor.ToColor() : Color.FromArgb(255, this.oldColor.ToColor());
            this.colorShowBox.LowerColor = this.alphaEnabled ? this.selColor.ToColor() : Color.FromArgb(255, this.selColor.ToColor());
        }
        private void UpdateColorPanelGradient()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.colorPanel.SetupXYGradient(
                        Color.White,
                        ExtMethodsSystemDrawingColor.ColorFromHSV(this.selColor.h, 1.0f, 1.0f),
                        Color.Black,
                        Color.Transparent);
                    break;
                case PrimaryAttrib.Saturation:
                    this.colorPanel.SetupHueBrightnessGradient(this.selColor.s);
                    break;
                case PrimaryAttrib.Brightness:
                    this.colorPanel.SetupHueSaturationGradient(this.selColor.v);
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.SetupGradient(
                        Color.FromArgb(255, tmp.R, 255, 0),
                        Color.FromArgb(255, tmp.R, 255, 255),
                        Color.FromArgb(255, tmp.R, 0, 0),
                        Color.FromArgb(255, tmp.R, 0, 255),
                        32);
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.SetupGradient(
                        Color.FromArgb(255, 255, tmp.G, 0),
                        Color.FromArgb(255, 255, tmp.G, 255),
                        Color.FromArgb(255, 0, tmp.G, 0),
                        Color.FromArgb(255, 0, tmp.G, 255),
                        32);
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.SetupGradient(
                        Color.FromArgb(255, 255, 0, tmp.B),
                        Color.FromArgb(255, 255, 255, tmp.B),
                        Color.FromArgb(255, 0, 0, tmp.B),
                        Color.FromArgb(255, 0, 255, tmp.B),
                        32);
                    break;
            }
        }
        private void UpdateColorPanelValue()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.colorPanel.ValuePercentual = new PointF(
                        this.selColor.s,
                        this.selColor.v);
                    break;
                case PrimaryAttrib.Saturation:
                    this.colorPanel.ValuePercentual = new PointF(
                        this.selColor.h,
                        this.selColor.v);
                    break;
                case PrimaryAttrib.Brightness:
                    this.colorPanel.ValuePercentual = new PointF(
                        this.selColor.h,
                        this.selColor.s);
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.ValuePercentual = new PointF(
                        tmp.B / 255.0f,
                        tmp.G / 255.0f);
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.ValuePercentual = new PointF(
                        tmp.B / 255.0f,
                        tmp.R / 255.0f);
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.colorPanel.ValuePercentual = new PointF(
                        tmp.G / 255.0f,
                        tmp.R / 255.0f);
                    break;
            }
        }
        private void UpdateColorSliderGradient()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.colorSlider.SetupHueGradient(/*this.selColor.GetHSVSaturation(), this.selColor.GetHSVBrightness()*/);
                    break;
                case PrimaryAttrib.Saturation:
                    this.colorSlider.SetupGradient(
                        ExtMethodsSystemDrawingColor.ColorFromHSV(this.selColor.h, 0.0f, this.selColor.v),
                        ExtMethodsSystemDrawingColor.ColorFromHSV(this.selColor.h, 1.0f, this.selColor.v));
                    break;
                case PrimaryAttrib.Brightness:
                    this.colorSlider.SetupGradient(
                        ExtMethodsSystemDrawingColor.ColorFromHSV(this.selColor.h, this.selColor.s, 0.0f),
                        ExtMethodsSystemDrawingColor.ColorFromHSV(this.selColor.h, this.selColor.s, 1.0f));
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.SetupGradient(
                        Color.FromArgb(255, 0, tmp.G, tmp.B),
                        Color.FromArgb(255, 255, tmp.G, tmp.B));
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.SetupGradient(
                        Color.FromArgb(255, tmp.R, 0, tmp.B),
                        Color.FromArgb(255, tmp.R, 255, tmp.B));
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.SetupGradient(
                        Color.FromArgb(255, tmp.R, tmp.G, 0),
                        Color.FromArgb(255, tmp.R, tmp.G, 255));
                    break;
            }
        }
        private void UpdateColorSliderValue()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.colorSlider.ValuePercentual = this.selColor.h;
                    break;
                case PrimaryAttrib.Saturation:
                    this.colorSlider.ValuePercentual = this.selColor.s;
                    break;
                case PrimaryAttrib.Brightness:
                    this.colorSlider.ValuePercentual = this.selColor.v;
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.ValuePercentual = tmp.R / 255.0f;
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.ValuePercentual = tmp.G / 255.0f;
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.colorSlider.ValuePercentual = tmp.B / 255.0f;
                    break;
            }
        }


        private void UpdateSelectedColorFromSliderValue()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.selColor.h = this.colorSlider.ValuePercentual;
                    break;
                case PrimaryAttrib.Saturation:
                    this.selColor.s = this.colorSlider.ValuePercentual;
                    break;
                case PrimaryAttrib.Brightness:
                    this.selColor.v = this.colorSlider.ValuePercentual;
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        (int)Math.Round(this.colorSlider.ValuePercentual * 255.0f),
                        tmp.G,
                        tmp.B));
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        tmp.R,
                        (int)Math.Round(this.colorSlider.ValuePercentual * 255.0f),
                        tmp.B));
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        tmp.R,
                        tmp.G,
                        (int)Math.Round(this.colorSlider.ValuePercentual * 255.0f)));
                    break;
            }
        }
        private void UpdateSelectedColorFromPanelValue()
        {
            Color tmp;
            switch (this.primAttrib)
            {
                default:
                case PrimaryAttrib.Hue:
                    this.selColor.s = this.colorPanel.ValuePercentual.X;
                    this.selColor.v = this.colorPanel.ValuePercentual.Y;
                    break;
                case PrimaryAttrib.Saturation:
                    this.selColor.h = this.colorPanel.ValuePercentual.X;
                    this.selColor.v = this.colorPanel.ValuePercentual.Y;
                    break;
                case PrimaryAttrib.Brightness:
                    this.selColor.h = this.colorPanel.ValuePercentual.X;
                    this.selColor.s = this.colorPanel.ValuePercentual.Y;
                    break;
                case PrimaryAttrib.Red:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        tmp.R,
                        (int)Math.Round(this.colorPanel.ValuePercentual.Y * 255.0f),
                        (int)Math.Round(this.colorPanel.ValuePercentual.X * 255.0f)));
                    break;
                case PrimaryAttrib.Green:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        (int)Math.Round(this.colorPanel.ValuePercentual.Y * 255.0f),
                        tmp.G,
                        (int)Math.Round(this.colorPanel.ValuePercentual.X * 255.0f)));
                    break;
                case PrimaryAttrib.Blue:
                    tmp = this.selColor.ToColor();
                    this.selColor = new InternalColor(Color.FromArgb(
                        tmp.A,
                        (int)Math.Round(this.colorPanel.ValuePercentual.Y * 255.0f),
                        (int)Math.Round(this.colorPanel.ValuePercentual.X * 255.0f),
                        tmp.B));
                    break;
            }
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.selColor = this.oldColor;
            this.UpdateColorControls();
        }

        private void radioHue_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioHue.Checked) this.PrimaryAttribute = PrimaryAttrib.Hue;
        }
        private void radioSaturation_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioSaturation.Checked) this.PrimaryAttribute = PrimaryAttrib.Saturation;
        }
        private void radioValue_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioValue.Checked) this.PrimaryAttribute = PrimaryAttrib.Brightness;
        }
        private void radioRed_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioRed.Checked) this.PrimaryAttribute = PrimaryAttrib.Red;
        }
        private void radioGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioGreen.Checked) this.PrimaryAttribute = PrimaryAttrib.Green;
        }
        private void radioBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBlue.Checked) this.PrimaryAttribute = PrimaryAttrib.Blue;
        }

        private void colorPanel_PercentualValueChanged(object sender, EventArgs e)
        {
            if (this.ContainsFocus) this.UpdateSelectedColorFromPanelValue();
            this.UpdateColorSliderGradient();
            this.UpdateColorShowBox();
            this.UpdateText();

            // update_vmt_paint();
        }
        private void colorSlider_PercentualValueChanged(object sender, EventArgs e)
        {
            if (this.ContainsFocus) this.UpdateSelectedColorFromSliderValue();
            this.UpdateColorPanelGradient();
            this.UpdateColorShowBox();
            this.UpdateText();


        }
        private void alphaSlider_PercentualValueChanged(object sender, EventArgs e)
        {
            this.UpdateColorSliderGradient();
            this.UpdateColorPanelGradient();
            this.UpdateColorShowBox();
            this.UpdateText();
        }

        private void numHue_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            this.selColor.h = (float)this.numHue.Value / 360.0f;
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }

        }
        private void numSaturation_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            this.selColor.s = (float)this.numSaturation.Value / 100.0f;
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }
        }
        private void numValue_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            this.selColor.v = (float)this.numValue.Value / 100.0f;
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }
        }
        private void numRed_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            Color tmp = this.selColor.ToColor();
            this.selColor = new InternalColor(Color.FromArgb(tmp.A, (byte)this.numRed.Value, tmp.G, tmp.B));
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }
        }
        private void numGreen_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            Color tmp = this.selColor.ToColor();
            this.selColor = new InternalColor(Color.FromArgb(tmp.A, tmp.R, (byte)this.numGreen.Value, tmp.B));
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }
        }
        private void numBlue_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            Color tmp = this.selColor.ToColor();
            this.selColor = new InternalColor(Color.FromArgb(tmp.A, tmp.R, tmp.G, (byte)this.numBlue.Value));
            this.UpdateColorControls();

            if (!numBox_rgb_hsv_typed_value)
            {
                _Timer_paintChange.Stop();
                _Timer_paintChange.Start();
            }
        }
        private void numAlpha_ValueChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            Color tmp = this.selColor.ToColor();
            this.selColor = new InternalColor(Color.FromArgb(255, tmp.R, tmp.G, tmp.B));
            this.UpdateColorControls();
        }
        private void textBoxHex_TextChanged(object sender, EventArgs e)
        {
            if (this.suspendTextEvents) return;
            int argb;
            if (int.TryParse("", System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentUICulture, out argb))
            {
                Color tmp = Color.FromArgb(argb);
                this.selColor = new InternalColor(tmp);
                this.UpdateColorControls();
            }
        }

        private void colorShowBox_UpperClick(object sender, EventArgs e)
        {
            this.selColor = this.oldColor;
            this.UpdateColorControls();
        }

        #endregion

        private void ColorPicker_Load(object sender, EventArgs e)
        {
            Color bc = Color.FromArgb(color_original.R, color_original.G, color_original.B);
            InternalColor base_color = new InternalColor();
            base_color.h = bc.GetHue();
            base_color.s = bc.GetSaturation();
            base_color.v = bc.GetBrightness();

            this.selColor = base_color;

            oldColor = new InternalColor(color_original);
            selColor = new InternalColor(color_original);

            this.UpdateColorControls();
        }

        private void ColorPicker_Shown(object sender, EventArgs e)
        {
            form_loaded = true;
        }



        #region numeric updown events

        // even for NumericUpDown control value changes, saves value/parameter onto VMT based on the NumericUpDown.Tag (tag gives the VMT $parameterName)
        private void NumUpDown_ValueChanged_to_VMT(object sender, EventArgs e)
        {
            if (!numBox_typed_value)
            {
                _Timer.Stop();
                last_updated_input = sender;
                _Timer.Start();
            }
        }


        private void numUpDwn_KeyUp(object sender, KeyEventArgs e)
        {
            _Timer.Stop();
            numBox_typed_value = true;

            last_updated_input = sender;
            _Timer.Start();
        }

        #endregion


        private void colorPanel_MouseUp(object sender, MouseEventArgs e)
        {
            update_bg_color(colorPanel.Value);
        }

        private void colorSlider_MouseUp(object sender, MouseEventArgs e)
        {
            update_bg_color(colorPanel.Value);
        }


        #region color picker panel  open/close

        private void vtab_paint_colorpicker_MouseHover(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.Gray;
            Cursor = Cursors.Hand;
        }

        private void vtab_paint_colorpicker_MouseLeave(object sender, EventArgs e)
        {
            if ((sender is Panel)) { ((Panel)sender).BackColor = Color.DimGray; }

            Cursor = Cursors.Arrow;
        }

        private void vtab_paint_colorpicker_Click(object sender, EventArgs e)
        {

            if (this.Size.Height > 150)
            {
                this.Size = new System.Drawing.Size(416, 150);
            }
            else
            {
                this.Size = new System.Drawing.Size(416, 416);
            }
        }

        #endregion



        private void hsv_rgb_MouseUp(object sender, MouseEventArgs e)
        {
            if (!refresh_busy)
            {
                update_bg_color(colorPanel.BackColor);
                refresh_busy = true;

                refresh_timer.Start();
            }
        }

        private void hsv_rgb_KeyUp(object sender, KeyEventArgs e)
        {
            _Timer_paintChange.Stop();
            numBox_rgb_hsv_typed_value = true;
            _Timer_paintChange.Start();
        }

    }
}
