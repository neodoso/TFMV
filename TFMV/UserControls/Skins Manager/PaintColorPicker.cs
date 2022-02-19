using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

namespace TFMV
{
    public partial class PaintColorPicker : ComboBox
    {
        public string VMT { get; set; }
        // Data for each color in the list
        public class ColorInfo
        {
            public string Text { get; set; }
            public Color Color { get; set; }

            public ColorInfo(string text, Color color)
            {
                Text = text;
                Color = color;
            }
        }

        public PaintColorPicker()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;

            Font = new Font("Verdana", 7);
            FlatStyle = FlatStyle.Flat;

            Height = 30;
            Width = 200;

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;

            AddPaint(":255 255 255");
        }

        public void enabled(bool param)
        {
            this.Enabled = param;
            if (!param) { DisableControls(this); }
            if (param) { EnableControls(this); }
        }


        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            con.Enabled = false;
        }

        private void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }

        // Populate control with standard colors
        public void AddPaints(List<TFMV.TF2.paints.paint> paints) //List<string> paints_list
        {
            for (int i = 0; i < paints.Count; i++)
            {
                TFMV.TF2.paints.paint paint = (TFMV.TF2.paints.paint)paints[i];
                Items.Add(new ColorInfo(paint.name, paint.color));
            }
        }

        // add a paint invidivudally
        public void AddPaint(string paint_)
        {
            string[] paint = paint_.Split(':');
            string[] color = paint[1].Split(' ');
            Items.Add(new ColorInfo(paint[0], Color.FromArgb(Convert.ToInt16(color[0]), Convert.ToInt16(color[1]), Convert.ToInt16(color[2]))));
        }

        // edit a paint invidivudally
        public void EditPaint(string paint_)
        {
            // cancel void if the this. color picker combobox is disabled
            // means its not paintable
            if (!this.Enabled)  {  return;  }

            if (paint_.Contains(":"))
            {
                string[] paint = paint_.Split(':');
                string[] color = paint[1].Split(' ');

                if (color[0] == "") { color = new string[] { "255", "255", "255" }; }

                Items[0] = new ColorInfo(paint[0], Color.FromArgb(Convert.ToInt16(color[0]), Convert.ToInt16(color[1]), Convert.ToInt16(color[2])));
            }
            else
            {

                Items[0] = new ColorInfo(paint_, Color.White);
            }
        }

        // draw list item
        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ColorInfo color = (ColorInfo)Items[e.Index];

                e.DrawBackground();

                Rectangle rect = new Rectangle();
                rect.X = e.Bounds.X + 2;
                rect.Y = e.Bounds.Y + 2;
                rect.Width = 10;
                rect.Height = e.Bounds.Height - 5;
                e.Graphics.FillRectangle(new SolidBrush(color.Color), rect);
                e.Graphics.DrawRectangle(SystemPens.WindowText, rect);

                Brush brush;
                if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                    brush = SystemBrushes.HighlightText;
                else
                    brush = SystemBrushes.WindowText;
                e.Graphics.DrawString(color.Text, Font, brush,
                    e.Bounds.X + rect.X + rect.Width + 2,
                    e.Bounds.Y + ((e.Bounds.Height - Font.Height) / 2));

                if ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None)
                    e.DrawFocusRectangle();
            }
        }

   
        // get/set selected item.
        public new ColorInfo SelectedItem
        {
            get
            {
                return (ColorInfo)base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
            }
        }


        // get text of selected item, or sets the selection to the item with the specified text.
        public new string SelectedText
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Text;
                return String.Empty;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((ColorInfo)Items[i]).Text == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        // Gets the value of the selected item, or sets the selection to the item with the specified value.
        public new Color SelectedValue
        {
            get
            {
                if (SelectedIndex >= 0)
                    return SelectedItem.Color;
                return Color.White;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (((ColorInfo)Items[i]).Color == value)
                    {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }
    }
}
