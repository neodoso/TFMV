using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TFMV.SourceEngine
{
    public static class VPK
    {

        public static string tf_dir, tf2_dir;

        //converts long paths to short paths, for "dos" console commands
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)] public static extern int GetShortPathName(
        [MarshalAs(UnmanagedType.LPTStr)] string path,
        [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);

        private static string tools_dir = Application.StartupPath + "\\tools\\";
        private static string app_data_dir = Main.app_data_dir;
        private static string tmp_dir = app_data_dir + "\\tmp\\";


        //gets tf/steam cpaths from the main form
        public static void set_tf_paths(string tf, string tf2)
        {
            tf_dir = tf; tf2_dir = tf2;
        }


        //check if vpk.exe exists
        public static bool vpk_tool_check()
        {
            bool result = true;

            if (File.Exists(tf2_dir + "bin/vpk.exe") == false)
            {
                MessageBox.Show("Error: " + tf2_dir + "bin/vpk.exe is missing! \n SDK Tools are needed to extract from the game VPKs.");
                result = false;
            }

            return result;
        }

        //for skins or workshop skins dragged and dropped into TFMV
        public static bool Extract_ALL(string vpk_file, string extract_dir)
        {

            if (File.Exists(tf2_dir + "bin/vpk.exe") == false)
            {
                MessageBox.Show("Error: " + tf2_dir + "bin/vpk.exe is missing! \n SDK Tools are needed to extract from the game VPKs.");
                return false;
            }

            //create temporary directory if it doesn't exist
            if (Directory.Exists(tmp_dir) == false) { Directory.CreateDirectory(tmp_dir); }

            extract_dir = extract_dir.Replace("\\", "/");

            if (Directory.Exists(extract_dir) == false) { Directory.CreateDirectory(extract_dir); }

            StringBuilder shortPath = new StringBuilder(512);
            GetShortPathName(@extract_dir, shortPath, shortPath.Capacity);
            @extract_dir = shortPath.ToString();


            StringBuilder shortPath1 = new StringBuilder(512);
            GetShortPathName(vpk_file, shortPath1, shortPath1.Capacity);
            vpk_file = shortPath1.ToString();

            string args = vpk_file;


            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = extract_dir, // + "team fortress 2\\tf\\",
                    FileName = tf2_dir + "bin/vpk.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            //get process output to check for errors
            while (!proc.StandardOutput.EndOfStream)
            {
                string str = proc.StandardOutput.ReadLine();

                // if (str.Contains("not found in package"))
                // {
                // MessageBox.Show("Error extracting from VPK: file " + file_path + " not found in the VPK!");
                // }
            }
            return true;

        }


        //Extract file from gcf...
        public static bool Extract(string file_path, string extract_path, int vpk_selection)
        {

            #region check if tools files exist

            if (File.Exists(tools_dir + "HLExtract.exe") == false)
            {
                MessageBox.Show("Error: tools\\HLExtract.exe is missing! \n Tools are needed to extract from the game VPKs.");
                return false;
            }

            if (File.Exists(tools_dir + "HLLib.dll") == false)
            {
                MessageBox.Show("Error: tools\\HLLib.dll is missing!\n Tools are needed to extract from the game VPKs.");
                return false;
            }

            if ((extract_path.Replace(" ", "") == "") || (file_path.Replace(" ", "") == ""))
            {
                MessageBox.Show("Error: VPKExtract: invalid file or directory paths.");
            }

            #endregion

            string vpk_file = "tf2_misc_dir.vpk";
            if (vpk_selection == 0) { vpk_file = "tf2_misc_dir.vpk"; } //models, vmts, pcf ...
            if (vpk_selection == 1) { vpk_file = "tf2_textures_dir.vpk"; } // VTF only

            if (file_path == "")
            {
                MessageBox.Show("Error extracting from vpk: file path is undefined.");
                return false;
            }

            string filename = Path.GetFileName(file_path);

            //create temporary directory if it doesn't exist
            if (!Directory.Exists(tmp_dir)) 
            {
                Directory.CreateDirectory(tmp_dir); 
            }

            string VPK = (tf_dir + vpk_file).Replace("\\", "/");
            
            string filename_export = file_path.Replace("\\", "/");
            string extract_dir = extract_path.Replace("\\", "/");
            

            if (Directory.Exists(extract_dir) == false) 
            {         
                try
                {
                     Directory.CreateDirectory(extract_dir); 

                }
                catch //(Exception ex)
	            {
	                MessageBox.Show("Error: (VPK extract) could not create directory: " + extract_dir );
                    return false;
	            }
            }

            StringBuilder shortPath = new StringBuilder(512);
            GetShortPathName(@extract_dir, shortPath, shortPath.Capacity);
            @extract_dir = shortPath.ToString();


            VPK = '"' + VPK + '"';
            extract_dir = '"' + extract_dir + '"';
            filename_export = '"' + filename_export + '"';


            string args = " -v -p " + VPK + " -d " + extract_dir + " -e " + filename_export;

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = app_data_dir, // + "team fortress 2\\tf\\",
                    FileName = tools_dir + "HLExtract.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            //get process output to check for errors
            while (!proc.StandardOutput.EndOfStream)
            {
                string str = proc.StandardOutput.ReadLine();


                if (str.Contains("not found in package"))
                {
                    // MessageBox.Show("Error extracting from VPK: file " + file_path + " not found in the VPK!");
                }
            }

            //check if file was extracted
            if (File.Exists(extract_dir + filename))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Validate that file exists in GCF
        public static bool Find(string path, int vpk_selection)
        {

            #region check if tools files exist

            if (!File.Exists(tools_dir + "HLExtract.exe"))
            {
                MessageBox.Show("Error: tools\\HLExtract.exe is missing! \n Tools are needed to extract from the game VPKs.");
                return false;
            }

            if (!File.Exists(tools_dir + "HLLib.dll"))
            {
                MessageBox.Show("Error: tools\\HLLib.dll is missing!\n Tools are needed to extract from the game VPKs.");
                return false;
            }

            #endregion

            string vpk_file = "tf2_misc_dir.vpk";
            if (vpk_selection == 0) { vpk_file = "tf2_misc_dir.vpk"; } //models, vmts, pcf ...
            if (vpk_selection == 1) { vpk_file = "tf2_textures_dir.vpk"; } // VTF only

            bool found = false;

            if (path == "")
            {
                MessageBox.Show("Error reading from VPK: file path is undefined.");
                return false;
            }


            string VPK = '"' + tf_dir + vpk_file + '"';
            string model = '"' + path + '"';
            string extract_dir = tmp_dir;
            string args = " -v -p " + VPK + " -t " + model;

            StringBuilder shortPath = new StringBuilder(512);
            GetShortPathName(@extract_dir, shortPath, shortPath.Capacity);
            @extract_dir = shortPath.ToString();

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = app_data_dir, // + "team fortress 2\\tf\\",
                    FileName = tools_dir + "HLExtract.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            //get process output to check for errors
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine() + "\n";

                if (line.Contains(": OK  "))
                {
                    found = true;
                }


                if (line.Contains("Error (0x00000002):"))
                {
                    MessageBox.Show("Error: could not access to the VPK file.\n" + VPK);
                    return false;

                }

            }

            return found;
        }


    }
}
