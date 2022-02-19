using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace TFMV.SourceEngine
{
    /// <summary>
    /// 2022: this is a complete mess and unreliable method to detect the steam/game directories
    /// requires a total overhaul to properly detect Steam/game install folders
    /// </summary>
    public partial class SteamGameConfig : UserControl
    {

        private int appid = 440;

        public string steam_dir { get; set; }

        public string tf_dir { get; set; }
        public string tf2_dir { get; set; }
        public string bin_dir { get; set; }

        public string settings_dir = Main.app_data_dir;

        public bool valid_config;


        public SteamGameConfig()
        {
            InitializeComponent();
#if DEBUG
            settings_dir = "C:\\Users\\Mike\\Desktop\\TFMV\\config\\";
#endif
        }

        // get Steam install directory from the windows registry
        // bool detecting : for when we are searching the dir through the steam config, we don't warn if hlmv.exe is not found
        public bool get_SteamDir(bool detecting)
        {

            // check if hlmv.exe exists with paths from saved config
            if (validate_tf_dir(detecting))
            {
                valid_config = true;
                return true;
            }

            // if not try to find SteamDir and TF2 dir and test if hlmv.exe exists
            try
            {
                #region find steam install dir in windows registry
                //find steam install dir in windows registry
                string tmppath = "";
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.OpenSubKey(@"Software\Valve\Steam");

                if (regKey != null)
                {
                    if (tmppath.Trim().EndsWith("\\"))
                    {
                        tmppath = regKey.GetValue("SteamPath").ToString();
                    }
                    else
                    {
                        tmppath = regKey.GetValue("SteamPath").ToString().Replace("/", "\\") + "\\";
                    }
                }


                //rtxtb.AppendText("\nRedistry path: " + tmppath);
                steam_dir = tmppath.Replace("steamapps", "").Replace(@"\\", @"\");

                #endregion

                #region verify steam dir exists

                //verify that steam dir is valid
                if (Directory.Exists(steam_dir))
                {
                    txtb_steamdir.Text = steam_dir;

                    //get team fortress 2 install dir
                    if (get_app_config(appid))
                    {
                        if (validate_tf_dir(true))
                        {
                            valid_config = true;
                            // successfully found steam dir, tf2 dir and the sdk tools
                            return true;
                        }
                        else
                        {
                            valid_config = false;
                            MessageBox.Show("Error  (1) Steam and TF2 directories where found, but the SDK tools were not, make sure TF2 tools are properly insalled in 'Team Fortress 2\bin\'.");
                            // could not find HLMV
                            return false;
                        }


                    }
                    else
                    {

                        valid_config = false;
                        // tf2 dir not found
                        return false;
                    }
                }
                else
                {
                    valid_config = false;
                    MessageBox.Show("Error (2) could not find the Steam install directory. \nPlease set it manually.");
                    return false;
                }
                #endregion

            }
            catch
            {
                MessageBox.Show("Error  (3) failed to read Steam install directory from the registry. \nPlease set it manually.");
                valid_config = false;
                return false;
            }


        }

        // gets game (tf2) install path from the steam config.vdf
        private bool get_app_config(int appid)
        {
            var steam_config = steam_dir + "config\\config.vdf";

            //if config.vdf exists
            if (!File.Exists(steam_config))
            {
                MessageBox.Show("Error (0) Could not find   " + steam_config);
                return false;
            }


            #region get VDF steam_config data

            TFMV.VDF_parser pr_config = new TFMV.VDF_parser();
            pr_config.file_path = steam_config;
            pr_config.load_VDF_file();

            // get installdir
            TFMV.VDF_parser.VDF_node node_App = pr_config.sGet_NodePath("InstallConfigStore.Software.Valve.Steam.apps." + appid.ToString());
            string installdir_test = pr_config.sGet_KeyVal(node_App, "installdir");
            installdir_test = Regex.Replace(installdir_test, @"\\\\", @"\");

            // get BaseInstallFolder_1
            TFMV.VDF_parser.VDF_node node_steam = pr_config.sGet_NodePath("InstallConfigStore.Software.Valve.Steam");
            string BaseInstallFolder_1 = pr_config.sGet_KeyVal(node_steam, "BaseInstallFolder_1").Replace(@"\\", @"\");

            #endregion

            // test steam_config.vdf installdir exists
            if (Directory.Exists(installdir_test))
            {

                if (sett_tf_dir(installdir_test + "\\tf\\"))
                {
                    return true;
                }
            }


            string BaseInstallFolder_1_test = (BaseInstallFolder_1 + "\\steamapps\\common\\Team Fortress 2\\").Replace(@"\\", @"\");

            //tf2 path not found in the VDF, we try to check if its in the steam directory
            if (Directory.Exists(BaseInstallFolder_1_test) == true)
            {
                if (sett_tf_dir(BaseInstallFolder_1_test + "tf\\"))
                {
                    return true;
                }
            }


            #region get libraryfolders.vdf data

            string libraryfolders_path = (steam_dir + "\\SteamApps\\libraryfolders.vdf").Replace(@"\\", @"\");
            string library_folder = "";

            if (File.Exists(libraryfolders_path))
            {
                TFMV.VDF_parser libraryfolders = new TFMV.VDF_parser();
                pr_config.file_path = libraryfolders_path;
                pr_config.load_VDF_file();

                TFMV.VDF_parser.VDF_node node = pr_config.sGet_NodePath("LibraryFolders");
                library_folder = pr_config.sGet_KeyVal(node, "1").Replace(@"\\", @"\");
            }

            #endregion


            // test LibraryFolders 1  ///////////////  TODO TODO TODO >>> we only load the first folder, check if there's multiple and test each
            if (Directory.Exists(library_folder))
            {
                if (sett_tf_dir(library_folder + @"\SteamApps\common\Team Fortress 2\tf\"))
                {
                    return true;
                }
            }

            // test steam_dir + \SteamApps\common\Team Fortress 2\tf\
            if (Directory.Exists(steam_dir + @"\SteamApps\common\Team Fortress 2\tf\"))
            {
                if (sett_tf_dir(steam_dir + @"\SteamApps\common\Team Fortress 2\tf\"))
                {
                    return true;
                }
            }

            valid_config = false;

            MessageBox.Show("Error (5) Could not find the TF2 install directory, please set the TF directory path manually.");
            return false;

        }

        // checks if hlmv.exe exists
        private bool validate_tf_dir(bool show_error)
        {
            if (File.Exists(tf2_dir + "bin\\hlmv.exe"))
            {
                return true;
            }
            else
            {
                if (show_error)
                {
                    MessageBox.Show("Error: could not find " + tf2_dir + "bin\\hlmv.exe");
                }
                return false;
            }

        }


        // dialog select SteamDir manually
        private void btn_sel_steamdir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;

                if (Path.GetFileNameWithoutExtension(path).ToLower() == "steam")
                {
                    steam_dir = (path + "\\").Replace(@"\\", @"\");

                    txtb_steamdir.Text = steam_dir;

                    save_game_config();

                    if (get_app_config(appid))
                    {
                        valid_config = true;
                        save_game_config();
                    }
                }
                else
                {
                    valid_config = false;
                    MessageBox.Show("Invalid path, please select the \"steam\" folder. \n Example: C:\\Program Files (x86)\\Steam\\");
                }
            }
        }

        // dialog select TF2 Dir manually
        private void btn_sel_mod_dir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;

                if (Path.GetFileNameWithoutExtension(path).ToLower() == "tf")
                {
                    // check if tools exist
                    if (!File.Exists(((path + "\\").Replace(@"\\", @"\")).Replace("\\tf\\", "\\") + "bin\\hlmv.exe"))
                    {
                        MessageBox.Show("Error: invalid TF directory, could not find " + ((path + "\\").Replace(@"\\", @"\")).Replace("\\tf\\", "\\") + "bin\\hlmv.exe");
                        return;
                    }

                    sett_tf_dir(path + "\\");
                    save_game_config();
                }
                else
                {
                    valid_config = false;
                    MessageBox.Show("Error: please select the \"tf\" folder.\nExample: C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Team Fortress 2\\tf\\");
                }
            }
        }


        // detect steam dir button
        private void btn_get_steamdir_Click(object sender, EventArgs e)
        {
            reset_config();
            get_SteamDir(false);
            save_game_config();
        }



        public void save_game_config()
        {
            try
            {
                TextWriter tw = new StreamWriter(settings_dir + "game_config.ini");

                tw.WriteLine("steam_dir|" + steam_dir);
                tw.WriteLine("tf_dir|" + tf_dir);

                tw.Close();
            }
            catch (System.Exception excep)
            {
                MessageBox.Show("Error saving settings " + excep.Message);
            }

            Main.tfmv_dir = (tf_dir + "custom\\TFMV\\").Replace("\\\\", "\\");
            VPK.set_tf_paths(tf_dir, tf2_dir);

        }

        // loads game config, if config is not found and silent_detect = true, try to detect, if tf2 dir found, no need to warn the user the config save file isn't found
        public void load_game_config(bool silent_detect)
        {
            try
            {
                string f = settings_dir + "game_config.ini";
                if (File.Exists(f))
                {
                    List<string> lines = new List<string>();

                    using (StreamReader r = new StreamReader(f))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                            string[] arg = line.Split('|');

                            if (arg[0] == "steam_dir")
                            {
                                steam_dir = arg[1];
                                txtb_steamdir.Text = arg[1];
                            }

                            if (arg[0] == "tf_dir")
                            {
                                //this.tf_dir = arg[1];

                                sett_tf_dir(arg[1]);
                            }
                        }
                    }
                }

            }

            catch (System.Exception excep)
            {
                valid_config = false;
                MessageBox.Show("Error loading settings " + excep.Message);
            }

            if (!validate_tf_dir(false))
            {
                if (!silent_detect)
                {
                    valid_config = false;
                    MessageBox.Show("TF2 directory path loaded from settings file is invalid. \nAttempting to auto-detect TF2 directory.");
                }
                reset_config();
                get_SteamDir(false);
                save_game_config();

                //sett_tf_dir(string tf_path);
                Main.tfmv_dir = (tf_dir + "custom\\TFMV\\").Replace("\\\\", "\\");
                VPK.set_tf_paths(tf_dir, tf2_dir);

            }
            else
            {
                //sett_tf_dir(string tf_path);
                Main.tfmv_dir = (tf_dir + "custom\\TFMV\\").Replace("\\\\", "\\");
                VPK.set_tf_paths(tf_dir, tf2_dir);

                valid_config = true;

            }



        }

        private bool sett_tf_dir(string tf_path)
        {
            string hlmv_exe_path = ((tf_path + "\\").Replace(@"\\", @"\")).Replace("\\tf\\", "\\") + "bin\\hlmv.exe";

            if (File.Exists(hlmv_exe_path))
            {
                tf_dir = (tf_path + "\\").Replace(@"\\", @"\");
                tf2_dir = tf_dir.Replace("\\tf\\", "\\");
                bin_dir = (tf2_dir + "\\bin\\").Replace("\\tf\\", "\\").Replace(@"\\", @"\");
                txtb_moddir.Text = tf_dir;
                valid_config = true;

                return true;
            }

            return false;
        }

        // sets steam_dir and tf_dir to ""
        private void reset_config()
        {
            steam_dir = "";
            txtb_steamdir.Text = "";

            tf_dir = "";
            tf2_dir = "";
            bin_dir = "";
            txtb_moddir.Text = "";
        }


    }
}
