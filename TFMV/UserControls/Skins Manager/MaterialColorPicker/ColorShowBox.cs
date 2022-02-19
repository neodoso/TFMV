// based on: https://github.com/AdamsLair/winforms/tree/master/WinForms/ColorControls

/*
	The MIT License (MIT)

	Copyright (c) 2014 Adam

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TFMV.MaterialEditor
{
    public class ColorShowBox : UserControl
    {
        private Color upperColor = Color.Transparent;
        private Color lowerColor = Color.Transparent;


        public event EventHandler UpperClick = null;
        public event EventHandler LowerClick = null;


        public Color Color
        {
            get { return this.upperColor; }
            set { this.upperColor = this.lowerColor = value; this.Invalidate(); }
        }
        public Color UpperColor
        {
            get { return this.upperColor; }
            set { this.upperColor = value; this.Invalidate(); }
        }
        public Color LowerColor
        {
            get { return this.lowerColor; }
            set { this.lowerColor = value; this.Invalidate(); }
        }


        public ColorShowBox()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        protected void OnUpperClick()
        {
            if (this.UpperClick != null)
                this.UpperClick(this, null);
        }
        protected void OnLowerClick()
        {
            if (this.LowerClick != null)
                this.LowerClick(this, null);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Y > (this.ClientRectangle.Top + this.ClientRectangle.Bottom) / 2)
                this.OnLowerClick();
            else
                this.OnUpperClick();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(new HatchBrush(HatchStyle.LargeCheckerBoard, Color.LightGray, Color.Gray), this.ClientRectangle);

            e.Graphics.FillRectangle(new SolidBrush(this.upperColor),
                this.ClientRectangle.X,
                this.ClientRectangle.Y,
                this.ClientRectangle.Width,
                this.ClientRectangle.Height / 2 + 1);
            e.Graphics.FillRectangle(new SolidBrush(this.lowerColor),
                this.ClientRectangle.X,
                this.ClientRectangle.Y + this.ClientRectangle.Height - this.ClientRectangle.Height / 2,
                this.ClientRectangle.Width,
                this.ClientRectangle.Height / 2);
        }
    }
}
