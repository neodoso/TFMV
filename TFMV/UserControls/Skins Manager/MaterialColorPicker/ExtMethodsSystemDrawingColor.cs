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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFMV.MaterialEditor
{
	public static class ExtMethodsSystemDrawingColor
	{
		public static float GetLuminance(this System.Drawing.Color color)
		{
			return (0.2126f * color.R + 0.7152f * color.G + 0.0722f * color.B) / 255.0f;
		}
		public static float GetHSVHue(this System.Drawing.Color color)
		{
			return color.GetHue() / 360.0f;
		}
		public static float GetHSVBrightness(this System.Drawing.Color color)
		{
			return Math.Max(Math.Max(color.R, color.G), color.B) / 255.0f;
		}
		public static float GetHSVSaturation(this System.Drawing.Color color)
		{
			int max = Math.Max(color.R, Math.Max(color.G, color.B));
			int min = Math.Min(color.R, Math.Min(color.G, color.B));

			return (max == 0) ? 0.0f : 1.0f - (1.0f * (float)min / (float)max);
		}

		public static System.Drawing.Color ColorFromHSV(float hue, float saturation, float value)
		{
			hue *= 360.0f;

			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			double f = hue / 60 - Math.Floor(hue / 60);

			value = value * 255;
			int v = Convert.ToInt32(value);
			int p = Convert.ToInt32(value * (1 - saturation));
			int q = Convert.ToInt32(value * (1 - f * saturation));
			int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

			if (hi == 0)
				return System.Drawing.Color.FromArgb(255, v, t, p);
			else if (hi == 1)
				return System.Drawing.Color.FromArgb(255, q, v, p);
			else if (hi == 2)
				return System.Drawing.Color.FromArgb(255, p, v, t);
			else if (hi == 3)
				return System.Drawing.Color.FromArgb(255, p, q, v);
			else if (hi == 4)
				return System.Drawing.Color.FromArgb(255, t, p, v);
			else
				return System.Drawing.Color.FromArgb(255, v, p, q);
		}
	}
}
