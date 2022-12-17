﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using AnimatedGif;
using TFMV.Functions;
using System.IO;
using System.Collections.Generic;

namespace TFMV.UserControls
{

    public partial class Turntable_GIF_Generator : UserControl
    {

        //settings stuff taken from Main.cs
        private List<string> settings = new List<string>();

        //don't save settings if you haven't loaded settings yet
        private bool settings_loaded = false;

        //test/placeholder
        private Form parentform;

        /*
        
        //DITHERING FUNCTIONS!
        public void DoAtkinsonDithering()
        {
            //AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(TrueColorBytesToWebSafeColorBytes);

            //using (FileStream pngStream = new FileStream("half.png", FileMode.Open, FileAccess.Read))
            using (var image = new Bitmap(pngStream))
            {
                byte[,,] bytes = ReadBitmapToColorBytes(image);

                TempByteImageFormat temp = new TempByteImageFormat(bytes);
                atkinson.DoDithering(temp);

                WriteToBitmap(image, temp.GetPixelChannels);

                image.Save("test.png");
            }
        }

        static private Color roundColor(Color color, int factor)
        {
            double R = (double)factor * color.R / 255.0;
            double newR = Math.Round(R) * (255 / factor);
            double G = (double)factor * color.G / 255.0;
            double newG = Math.Round(G) * (255 / factor);
            double B = (double)factor * color.B / 255.0;
            double newB = Math.Round(B) * (255 / factor);
            return Color.FromArgb((int)newR, (int)newG, (int)newB);
        }

        private static void TrueColorBytesToWebSafeColorBytes(in byte[] input, ref byte[] output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(Math.Round(input[i] / 51.0) * 51);
            }
        }

        private static byte[,,] ReadBitmapToColorBytes(Bitmap bitmap)
        {
            byte[,,] returnValue = new byte[bitmap.Width, bitmap.Height, 3];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    returnValue[x, y, 0] = color.R;
                    returnValue[x, y, 1] = color.G;
                    returnValue[x, y, 2] = color.B;
                }
            }
            return returnValue;
        }

        private static void WriteToBitmap(Bitmap bitmap, Func<int, int, byte[]> reader)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte[] read = reader(x, y);
                    Color color = Color.FromArgb(read[0], read[1], read[2]);
                    bitmap.SetPixel(x, y, color);
                }
            }
        }

        */


        bool cancel_capture;
        Process proc_HLMV;
        string screemshots_dir;

        public Turntable_GIF_Generator(Process p, string screenshots_dir)
        {
            InitializeComponent();

            proc_HLMV = p;
            screemshots_dir = screenshots_dir;
        }


        private void panel_close_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_start_turntable_Click(object sender, EventArgs e)
        {
            #region error checks

            if(!Main.Process_IsRunning(proc_HLMV))
            {
              System.Windows.MessageBox.Show("Could not run task, HLMV is closed.");
                return;
            }

            if(!Directory.Exists(screemshots_dir))
            {
                try
                {
                    Directory.CreateDirectory(screemshots_dir);
                } catch
                {
                    System.Windows.MessageBox.Show("Could not create screenshots folder, verify the path is correct.");
                    return;
                }
            }

            #endregion
            btn_start_turntable.Visible = false;
            cancel_capture = false;


            int loop_count = 360 / Convert.ToInt32(txtb_move_x_factor.Text);
            int move_x_factor = Convert.ToInt32(txtb_move_x_factor.Text);

            //clockwise/counterclockwise
            if (lstTurnDirection.SelectedIndex == 0) 
            {
                move_x_factor = move_x_factor * -1;
            }

            progressBar.Maximum = loop_count;
            progressBar.Minimum = 0;

            #region calculate HLMV window center position and move mouse on position

            HLMV_window_pos window_pos = new HLMV_window_pos(proc_HLMV);
            // calculate center of HLMV viewport
            System.Drawing.Point center_pos = new System.Drawing.Point(window_pos.rect_left + (window_pos.width / 2), window_pos.rect_top + (window_pos.height / 2));

            MouseSimulator.X = center_pos.X;
            MouseSimulator.Y = center_pos.Y;

            // hold Left Mouse Button Down
            MouseSimulator.MouseDown(MouseButton.Left);

            #endregion

            //neodement: todo: add gif dithering options! and image as png etc options!

            //todo: better naming options than this
            string date = DateTime.UtcNow.AddTicks(Stopwatch.StartNew().Elapsed.Ticks).ToString().Replace(" ", "").Replace("/", "_").Replace(":", "");


            if (lstOutputFormat.SelectedIndex == 1)
            {

                //// SAVE AS STATIC IMAGE ////

                if (!Directory.Exists(screemshots_dir + @"\TurnTable_" + date))
                {
                    try
                    {
                        Directory.CreateDirectory(screemshots_dir + @"\TurnTable_" + date);
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Could not create subfolder in screenshots folder, verify the path is correct.");
                        return;
                    }
                }

                for (int i = 0; i < loop_count; i++)
                {
                    if (cancel_capture)
                    {
                        break;
                    }

                    MouseSimulator.X += move_x_factor;

                    Bitmap img = CaptureApplication(proc_HLMV.ProcessName);
                    img.Save(screemshots_dir + @"\TurnTable_" + date + "/" + i + ".png");

                    progressBar.Value = i;

                }
            }
            else
            {


                //// SAVE AS ANIMATED GIF ////
                // 33ms delay (~30fps)
                using (var gif = AnimatedGif.AnimatedGif.Create(screemshots_dir + @"\TurnTable_" + date + ".gif", 33))
                {
                    for (int i = 0; i < loop_count; i++)
                    {
                        if (cancel_capture)
                        {
                            break;
                        }

                        MouseSimulator.X += move_x_factor;

                        Bitmap image = CaptureApplication(proc_HLMV.ProcessName);



                        var img = CaptureApplication(proc_HLMV.ProcessName);
                        gif.AddFrame(img, delay: 1, quality: GifQuality.Bit8);

                        progressBar.Value = i;

                    }
                }
            }

            //neodement: fixed turntable never reporting it's finished

            // release Left Mouse Button 
            MouseSimulator.MouseUp(MouseButton.Left);

            progressBar.Value = 0;

            btn_start_turntable.Visible = true;

            //cheeky hack to make sure the user realises the turntable is done

            Form mainForm = this.FindForm();

            mainForm.WindowState = FormWindowState.Minimized;
            //mainForm.Show();
            mainForm.WindowState = FormWindowState.Normal;

            System.Windows.Forms.MessageBox.Show("Done!");

        }

        private class HLMV_window_pos
        {
            public int rect_left { get; set; }
            public int rect_top { get; set; }
            public int width { get; set; }
            public int height { get; set; }

            public HLMV_window_pos(Process proc)
            {

                Rect rect = new Rect();
                IntPtr error = GetWindowRect(proc.MainWindowHandle, ref rect);

                // sometimes it gives error.
                while (error == (IntPtr)0)
                {
                    error = GetWindowRect(proc.MainWindowHandle, ref rect);
                }

                OS_Settings.Point4 hlmv_padding = new OS_Settings.Point4(18, 8, 50, 300);
                OS_Settings os_settings = new OS_Settings();

                rect_left = (int)((rect.left + hlmv_padding.right) * Main.dpi_scale_factor);
                rect_top = (int)((rect.top + hlmv_padding.top) * Main.dpi_scale_factor);

                width = (int)(((rect.right - rect.left) - hlmv_padding.left) * Main.dpi_scale_factor);
                height = (int)(((rect.bottom - rect.top) - hlmv_padding.bottom) * Main.dpi_scale_factor);
                if (Main.dpi_scale_factor > 1) { rect_left += 2; rect_top += 2; }
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public Bitmap CaptureApplication(string procName)
        {
            Process proc;

            // Cater for cases when the process can't be located.
            try
            {
                proc = Process.GetProcessesByName(procName)[0];
            }
            catch // (IndexOutOfRangeException e)
            {
                System.Windows.MessageBox.Show("CpatureApplication() : Unable to get process " + procName);
                return null;
            }

            HLMV_window_pos window_pos = new HLMV_window_pos(proc);

            Bitmap bmp = new Bitmap(window_pos.width, window_pos.height, PixelFormat.Format32bppRgb);

            Graphics.FromImage(bmp).CopyFromScreen(window_pos.rect_left, window_pos.rect_top, 0, 0, new System.Drawing.Size(window_pos.width, window_pos.height), CopyPixelOperation.SourceCopy);
            return bmp;
        }

        private void btn_cancel_turntable_Click(object sender, EventArgs e)
        {
            cancel_capture = true;
        }

        private void txtb_move_x_factor_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void settings_save(object sender, EventArgs e)
        {

            //don't save settings if you haven't loaded settings yet
            if (!settings_loaded) { return; }

            //settings stuff taken from Main.cs
            try
            {
                // Properties ob =  (Object)sender;
                string obj_name = "";
                string arg = "";

                if (sender.GetType() == typeof(CheckBox))
                {
                    CheckBox obj = (CheckBox)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.Checked.ToString();
                }

                if (sender.GetType() == typeof(TextBox))
                {
                    TextBox obj = (TextBox)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.Text.ToString();
                }

                if (sender.GetType() == typeof(ComboBox))
                {
                    ComboBox obj = (ComboBox)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.SelectedIndex.ToString();
                }

                if (sender.GetType() == typeof(NumericUpDown))
                {
                    NumericUpDown obj = (NumericUpDown)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.Value.ToString();
                }


                TextWriter tw = new StreamWriter(TFMV.Main.settings_dir + "settings_turntable.ini");

                Boolean set_found = false;
                for (int i = 0; i < settings.Count; i++)
                {
                    if (settings[i].Split('<')[1] == obj_name)
                    {
                        settings[i] = arg + "<" + obj_name;
                        set_found = true;
                    }
                    tw.WriteLine(settings[i]);
                }

                if ((settings.Count == 0) || (!set_found) && (arg != "") && (obj_name != ""))
                {
                    settings.Add(arg + "<" + obj_name);
                    tw.WriteLine(arg + "<" + obj_name);
                }


                tw.Close();

            }

            catch (System.Exception excep)
            {
                System.Windows.MessageBox.Show("Error saving turntable settings.\n\n" + excep.Message);
            }


            /*
            if(!File.Exists(Main.settings_dir + "settings_turntable.ini"))
            {
                using (File.Create(Main.settings_dir + "settings_turntable.ini"));
            }
  
            string[] lines = File.ReadAllLines(Main.settings_dir + "settings_turntable.ini", System.Text.Encoding.UTF8);
            List<String> settings = new List<string>(lines);

            bool found_set_a = false; bool found_set_b = false;
            for (int i = 0; i < settings.Count; i++)
            {
                if (settings[i].Split('<')[1] == "txtb_move_x_factor")
                {
                    found_set_a = true;
                    settings[i] = txtb_move_x_factor.Text.ToString() + "<" + "txtb_move_x_factor" ;
                }
                
                //if (settings[i].Split('<')[1] == "cb_invert_rotation")
                //{
                //    found_set_b = true;
                //    settings[i] = cb_invert_rotation.Checked.ToString() + "<" + "cb_invert_rotation" ;
                //}
                
            }

            if(!found_set_a)
            {
                settings.Add(txtb_move_x_factor.Text.ToString() + "<" + "txtb_move_x_factor"  );
            }

            
            //if (!found_set_b)
            //{
            //    settings.Add(cb_invert_rotation.Checked.ToString() + "<" + "cb_invert_rotation" );
            //}
            

            try
            {
                System.IO.File.WriteAllLines(Main.settings_dir + "settings_turntable.ini", settings);
            } catch { 
            }
*/
        }

        private void load_settings()          
        {


            //settings stuff taken from Main.cs
            {
                settings.Clear();

                try
                {
                    if (File.Exists(TFMV.Main.settings_dir + "settings_turntable.ini"))
                    {
                        string f = TFMV.Main.settings_dir + "settings_turntable.ini";
                        List<string> lines = new List<string>();

                        using (StreamReader r = new StreamReader(f))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                lines.Add(line);
                            }
                        }

                        foreach (string s in lines)
                        {
                            string[] arg = s.Split('<');

                            // Object prop = this.GetType().GetProperty(arg[1].GetType().GetProperties();
                            // string  test = this.GetType().GetProperty(arg[1]).GetType().ToString();
                            Object prop = null;
                            try
                            {
                                Control[] ctrl = this.Controls.Find(arg[1], true);

                                if (ctrl.Length > 0)
                                {
                                    prop = ctrl[0];
                                }
                            }
                            catch
                            {
                                settings.Add(s);
                                //string test_fail = "";
                                continue;
                            }


                            settings.Add(s);

                            if (prop != null)
                            {
                                if (prop.GetType() == typeof(CheckBox))
                                {
                                    CheckBox obj = (CheckBox)prop;

                                    if (arg[0].ToLower() == "true") { obj.Checked = true; }
                                    if (arg[0].ToLower() == "false") { obj.Checked = false; }
                                }

                                if (prop.GetType() == typeof(TextBox))
                                {
                                    TextBox obj = (TextBox)prop;
                                    obj.Text = arg[0];
                                }

                                if (prop.GetType() == typeof(ComboBox))
                                {
                                    ComboBox obj = (ComboBox)prop;
                                    obj.SelectedIndex = Convert.ToInt32(arg[0]);
                                }

                                if (prop.GetType() == typeof(NumericUpDown))
                                {
                                    NumericUpDown obj = (NumericUpDown)prop;
                                    obj.Value = Convert.ToDecimal(arg[0]);
                                }
                            }
                        }
                    }
                }

                catch (System.Exception excep)
                {
                    System.Windows.MessageBox.Show("Error loading turntable settings.\n\n" + excep.Message);
                }
            }            



            //original load method
            /*
            try
            {
                if (!File.Exists(Main.settings_dir + "settings_turntable.ini"))
                {
                    return;
                }

                string f = Main.settings_dir + "settings_turntable.ini";
                List<string> lines = new List<string>();

                using (StreamReader r = new StreamReader(f))
                {

                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                    
                foreach (string s in lines)
                {

                    string[] arg = s.Split('<');

                    if(arg[1] == "txtb_move_x_factor")
                    {
                        txtb_move_x_factor.Text = arg[0];
                    }

                    /*
                    if (arg[1] == "cb_invert_rotation")
                    {
                        if (arg[0].ToLower() == "true")
                        {
                            cb_invert_rotation.Checked = true;
                        } else {
                            cb_invert_rotation.Checked = false;
                        }
                           
                    }
                    

                }
            }
            catch (System.Exception excep)
            {
                //System.Windows.MessageBox.Show("Error loading settings " + excep.Message);
            }
            */

            //settings are loaded, now we can save them
            settings_loaded = true;
        }


        private void Turntable_GIF_Generator_Load(object sender, EventArgs e)
        {
            load_settings();

#if DEBUG
    System.Windows.MessageBox.Show("finish this!");
#endif
            btn_Options.Visible = false;
            lstOutputFormat.Width = lstTurnDirection.Width;


            //set default state of dropdowns
            if (lstOutputFormat.SelectedIndex == -1)
            {
                lstOutputFormat.SelectedIndex = 0;
            }

            if (lstTurnDirection.SelectedIndex == -1)
            {
                lstTurnDirection.SelectedIndex = 0;
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtb_move_x_factor.Value = 7;
        }

        //todo: idk
        private void btnOpenOutputFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(screemshots_dir))
            {
                try
                {
                    Directory.CreateDirectory(screemshots_dir);
                    Process.Start(screemshots_dir);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Could not create screenshots folder, verify the path is correct.");
                    return;
                }
            }
            else
            {
                Process.Start(screemshots_dir);
            }
        }

        private void lstOutputFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstOutputFormat.SelectedIndex == 0)
            {
                lbl_Info_1.Text = "This tool captures a 360 degree horizontal turn around the object and generates an animated GIF.";
                lbl_Info_2.Text = "You will find the generated GIF in the screenshots folder.";
            }
            else
            {
                lbl_Info_1.Text = "This tool captures a 360 degree horizontal turn around the object and generates an image sequence.";
                lbl_Info_2.Text = "You will find the image sequence in a subfolder of the screenshots folder.";
            }

            settings_save(sender, e);

        }
    }
}
