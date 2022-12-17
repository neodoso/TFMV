using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Imaging;
using System.Media;
using System.Security.Cryptography;
using System.Linq.Expressions;

using Ionic.Zip;

using TFMV.UserControls;
using TFMV.SourceEngine;
using TFMV.Functions;
using TFMV.UserControls.Loadout;


namespace TFMV
{
    public partial class Main : Form
    {
        #region WARNING: Set your own Steam API key and windows username folder
        // Steam api key 
        // this key is necesary in order to download the TF2's item schema from Valve's servers
        // you can get your steam api key here: https://steamcommunity.com/dev/apikey
        // http://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key=" + steam_api_key +"&format=vdf"
        //        private string steam_api_key = "<API KEY>";
        //        private string steam_api_key = "http://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key=<API KEY>";
        //        http://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key=


        //todo: enter an API key from a dummy account here
        private string internal_steam_api_key = "INVALID KEY";
        private string steam_api_key = "";



        string DirUserName = "jburn";

        #endregion

        #region Global vars



        public static string tfmv_version = "";

        private bool adding_workshop_item_toLoadout = false;
        private string adding_workshop_item_zip_path;

        Image missing_icon = Properties.Resources.icon_workshop_item;

        private String items_game_URL;


        #region process hooks (for HLMV)

        public static Process proc_HLMV;

        [DllImport("User32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_F5 = 0x74;


        #region HLMV hook win32 Form controls adresses

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;

        #endregion

        #region hooks for screenshots capture functionality

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;

        [DllImport("User32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);


        // windows size

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        [DllImport("User32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // / <summary>
        // / 
        // / </summary>
        // / 
        private static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        internal enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }

        #endregion

        #endregion


        /*
        //todo: get this working, it's for the TURNTABLE GIF GENERATOR
        public void DoAtkinsonDithering()
        {
            AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(TrueColorBytesToWebSafeColorBytes);

            using (FileStream pngStream = new FileStream("half.png", FileMode.Open, FileAccess.Read))
            using (var image = new Bitmap(pngStream))
            {
                byte[,,] bytes = ReadBitmapToColorBytes(image);

                TempByteImageFormat temp = new TempByteImageFormat(bytes);
                atkinson.DoDithering(temp);

                WriteToBitmap(image, temp.GetPixelChannels);

                image.Save("test.png");
            }
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

        // list of items/models attachments
        public static ListBox listbox_mdls = new ListBox();

        #region configuration directories paths

        public static string tfmv_dir = "";

        public static string vpk_tmp_path = "";

        public static string app_data_dir = Application.StartupPath + "\\config\\";

        string schema_dir = app_data_dir + "tf2_schema\\";
        public static string settings_dir = app_data_dir;

        //todo: neodement: this is where bodygroups are breaking i guess, maybe?
        //neodement: made tmp_dir public, jigglebone editor needed access
        public static string tmp_dir = app_data_dir + "tmp\\";
        string cached_dir = app_data_dir + "cached\\";
        string tmp_loadout_dir = app_data_dir + "tmp_loadout\\";
        string tmp_workshop_zip_dir = app_data_dir + "\\tmp_workshop_zip\\";

        //neodement: cubemaps_dir for cubemap functions
        string cubemaps_dir = app_data_dir + "cubemaps\\";

        //neodement: special variable so we know not to trigger the dialog when changing medal setting if a user didn't click it.
        bool cb_allow_tournament_medals_SupressCheckedChange = false;

        private static string tools_dir = Application.StartupPath + "\\tools\\";

        #endregion



        bool items_loading;//skins_first_load,
        public static bool auto_refresh_paints, auto_refresh_busy;

        //for saving/loading settings
        private List<string> settings = new List<string>();

        //don't save settings if you haven't loaded settings yet
        private bool settings_loaded = false;


        //selection of paints (indices) that the user selects to have for the "screenshot paints tool" mosaic generation
        public static List<byte> paints_selection = new List<byte>();
        public static TFMV.TF2.paints paints_list = new TFMV.TF2.paints();
        private Panel skins_manager_control;
        private byte selected_team_skin_index = 0;


        private string[] mdl_files, mat_files;
        private string tf_mdldir, tf_matdir;

        public static List<string> model_list = new List<string>();

        #region hlmv settings

        private Color bg_color = Color.FromArgb(63, 63, 63);
        private Color aColor = Color.FromArgb(75, 75, 75);
        private Color lColor = Color.FromArgb(255, 255, 255);
        private bool bg_toggle = false;

        #endregion

        private string selected_player_class = "", selected_item_slot = "primary", item_type_name = "";

        List<TF2.items_game.item> items_game = new List<TF2.items_game.item>();

        private List<string> banned_items = new List<string>();


        //this variable stops right click context menu popping up when the Workshop tab is open
        private bool supress_TF2Item_ContextMenu = false;


        // drag and drop handle file
        private delegate void DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;
        private bool DragNDrop_on_MainModel = false;


        private TF2.player_bodygroups player_bodygroups = new TF2.player_bodygroups();
        public List<String> loadout_bodygroups_off;

        private TF2.player_all_materials player_all_mats = new TF2.player_all_materials();
        public static bool grey_material = false;


        List<equip_region_tfmv> equip_regions_list;

        //neodement: array of bytes needed to store hlmv.rf (Recent files list)
        byte[] hlmv_rf_backup = new byte[0];

        private class equip_region_tfmv
        {
            public string name { get; set; }
            public int count { get; set; }

            public equip_region_tfmv(string _name)
            {
                this.name = _name;
                this.count = 1;
            }
        }

        // hlmv window padding needed for taking screenshots (cropping area of HLMV's window)
        // number of pixels (takes account for HLMV's form pannels, we only want to get the 3D render window region)
        //todo: i think this number is wrong on windows 11
        OS_Settings.Point4 hlmv_padding = new OS_Settings.Point4(18, 8, 50, 300);
        OS_Settings OS_settings = new OS_Settings();

        //  DPI scaling factor, we use take this into account for the HLMV screenshot region
        public static float dpi_scale_factor = 1;


        #region background workers

        private BackgroundWorker bgWorker_ScreenPaintsTool = new BackgroundWorker();
        private BackgroundWorker bgWorker_load_items_to_listView = new BackgroundWorker();
        private BackgroundWorker bgWorker_load_workshop_items_to_listView = new BackgroundWorker();

        private BackgroundWorker bgWorker_load_schema = new BackgroundWorker();
        private BackgroundWorker bgWorker_download_schema = new BackgroundWorker();

        #endregion


        #endregion

        #region ui scale

        //neodement: fixed numbers being wrong here??

        private const int options_panel_closed_width = 915; //905;
        private const int options_panel_open_width = 1250;

        // private int this_width = 915;
        private const int this_height = 590;
        private const int this_height_extended = 869;

        private const int vtab_loadout_open_height = 525; //todo: fix this number too, if you need to

        #endregion


        #region Main Form

        public Main()
        {


            InitializeComponent();


            // for debugging: set config / schema directories to TFMV in desktop folder
            //(this is to stop you writing crap like updated schema pngs to the debug folder)
#if DEBUG


            app_data_dir = "C:\\Users\\" + DirUserName + "\\Desktop\\TFMV\\config\\";
            tools_dir = "C:\\Users\\" + DirUserName + "\\Desktop\\TFMV\\tools\\";

            schema_dir = app_data_dir + "tf2_schema\\";
            settings_dir = app_data_dir;

            tmp_dir = app_data_dir + "tmp\\";
            cached_dir = app_data_dir + "cached\\";
            tmp_loadout_dir = app_data_dir + "tmp_loadout\\";
            tmp_workshop_zip_dir = app_data_dir + "\\tmp_workshop_zip\\";

            //neodement: cubemaps_dir for cubemap functions
            cubemaps_dir = app_data_dir + "cubemaps\\";

#endif




            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //tfmv_version = fvi.FileVersion;

            #region bgWorkers event handlers

            bgWorker_ScreenPaintsTool.DoWork += new DoWorkEventHandler(bgWorker_ScreenPaintsTool_DoWork);
            bgWorker_ScreenPaintsTool.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_ScreenPaintsTool_RunWorkerCompleted);
            bgWorker_ScreenPaintsTool.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ScreenPaintsTool_ProgressChanged);
            bgWorker_ScreenPaintsTool.WorkerReportsProgress = true;
            bgWorker_ScreenPaintsTool.WorkerSupportsCancellation = true;

            bgWorker_load_items_to_listView.DoWork += new DoWorkEventHandler(bgWorker_load_items_to_listView_DoWork);
            bgWorker_load_items_to_listView.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_load_items_to_listView_RunWorkerCompleted);
            bgWorker_load_items_to_listView.ProgressChanged += new ProgressChangedEventHandler(bgWorker_load_items_to_listView_ProgressChanged);
            bgWorker_load_items_to_listView.WorkerReportsProgress = true;
            bgWorker_load_items_to_listView.WorkerSupportsCancellation = true;


            bgWorker_load_workshop_items_to_listView.DoWork += new DoWorkEventHandler(bgWorker_load_workshop_items_to_listView_DoWork);
            bgWorker_load_workshop_items_to_listView.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_load_workshop_items_to_listView_RunWorkerCompleted);
            bgWorker_load_workshop_items_to_listView.ProgressChanged += new ProgressChangedEventHandler(bgWorker_load_workshop_items_to_listView_ProgressChanged);
            bgWorker_load_workshop_items_to_listView.WorkerReportsProgress = true;
            bgWorker_load_workshop_items_to_listView.WorkerSupportsCancellation = true;


            bgWorker_load_schema.DoWork += new DoWorkEventHandler(bgWorker_load_schema_DoWork);
            bgWorker_load_schema.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_load_schema_RunWorkerCompleted);
            bgWorker_load_schema.ProgressChanged += new ProgressChangedEventHandler(bgWorker_load_schema_ProgressChanged);
            bgWorker_load_schema.WorkerReportsProgress = true;
            bgWorker_load_schema.WorkerSupportsCancellation = true;

            bgWorker_download_schema.DoWork += new DoWorkEventHandler(bgWorker_download_schema_DoWork);
            bgWorker_download_schema.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_download_schema_RunWorkerCompleted);
            bgWorker_download_schema.ProgressChanged += new ProgressChangedEventHandler(bgWorker_download_schema_ProgressChanged);
            bgWorker_download_schema.WorkerReportsProgress = true;
            bgWorker_download_schema.WorkerSupportsCancellation = true;

            #endregion


            #region set form controls params

            //string[] ver = tfmv_version.Split('.');

            //old version number stuff
            //            string version = ver[0] + "." + ver[1] + "  [v." + ver[2] + "]";


            // version info for titlebar is now just taken from assembly name.
            this.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

            #region progress / status controls


            progressBar_dl.Visible = false;
            progressBar_dl.BringToFront();

            skins_manager_control.HorizontalScroll.Enabled = false;
            skins_manager_control.HorizontalScroll.Visible = false;

            panel_paints.Enabled = false;

            #endregion


            // get Os version and set var (used to determine padding amount for cropping hlmv.exe window's screenshots)
            hlmv_padding = OS_settings.get_hlmv_padding();

            loadout_bodygroups_off = new List<string>();

            #region skins paint vars/forms setup

            selected_team_skin_index = 0;

            // get paints control
            skins_manager_control = (Panel)panel_paints.Controls["skins_manager_control"];

            colorPicker_master.AddPaints(TFMV.TF2.paints.red);
            colorPicker_master.EditPaint("ColorTint Base:" + "255 255 255");
            colorPicker_master.SelectedIndex = 0;

            #endregion

            // create delegate used for asynchronous call
            m_DelegateOpenFile = new DelegateOpenFile(this.filter_droped_file);

            #endregion


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // SETTINGS LOAD  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            // disable HWM checkbox checked event
            cb_hwm.CheckedChanged -= new EventHandler(cb_hwm_CheckedChanged);

            // disable fix hlp bone checkbox checked event
            cb_fix_hlp_bones.CheckedChanged -= new EventHandler(cb_fix_hlp_bones_CheckedChanged);

            // disable disable jigglebone + cubemap checkbox checked event
            cb_disable_jigglebones.CheckedChanged -= new EventHandler(cb_disable_jigglebones_CheckedChanged);
            cb_cubemap.CheckedChanged -= new EventHandler(cb_cubemap_CheckedChanged);

            settings_load();



            #region API KEY STUFF

            //use the built-in one unless you were told not to
            //if (chk_API_Key.Checked)
            //{
                steam_api_key = LoadAPIKey();
            //}
            //else
            //{
            //    steam_api_key = internal_steam_api_key;
            //}

            //neodement:
            //if api key was empty the above function will have thrown out an appropriate error message explaining why (not anymore)
            if (steam_api_key == "")
            {
                //tabControl.SelectedIndex = 1;
                panel_APIKey.Visible = false;
                steam_api_key = internal_steam_api_key;

            }
            else
            {
                panel_APIKey.Visible = true;
                txtb_API_Key.Text = steam_api_key;
                boxL.Visible = false;
                boxT.Visible = false;
                boxR.Visible = false;
                boxB.Visible = false;
            }
            #endregion




            // CHECK DUPLICATE PROCESSES  ////////////////////////////////////////////////////////////////////////////////////////////////////
            // check if another TFMV or HLMV is running
            check_duplicate_processes();


            #region load text file parameters

            load_banned_items();
            load_hint_modelspaths();
            load_hint_main_modelspaths();
            load_pose_names();

            #endregion

            #region clear // create resources dirs

            #region clear temporary directories/content

            // clear tfmv mod directory  tf/custom/TFMV/   

            miscFunc.DeleteDirectoryContent(tmp_dir);
            miscFunc.DeleteDirectoryContent(tmp_loadout_dir);

            #endregion

            #region create missing directories

            miscFunc.create_missing_dir(app_data_dir);

            miscFunc.create_missing_dir(schema_dir);
            miscFunc.create_missing_dir(schema_dir + "icons");

            miscFunc.create_missing_dir(tmp_dir);
            miscFunc.create_missing_dir(tmp_loadout_dir);

            #endregion

            #endregion

            #region set vars AFTER settings_load();



            // paint mosaic preferred paints
            load_paints_selection();

            auto_refresh_paints = cb_refresh_SkinPaintChange.Checked;

            #endregion


            #region create EventHanlders for controls

            // we do this after loading the settings so it doesn't trigger settings_save() before settings are first loaded
            cb_hwm.CheckedChanged += new EventHandler(cb_hwm_CheckedChanged);
            cb_fix_hlp_bones.CheckedChanged += new EventHandler(cb_fix_hlp_bones_CheckedChanged);
            cb_disable_jigglebones.CheckedChanged += new EventHandler(cb_disable_jigglebones_CheckedChanged);
            cb_cubemap.CheckedChanged += new EventHandler(cb_cubemap_CheckedChanged);

            #endregion



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // see form show for next operations
            separator.Size = new Size(1, separator.Size.Height);
            separator1.Size = new Size(1, separator1.Size.Height);


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // DETECT STEAM & TF game DIRECTORIES  /////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // check steam dir and tf2 dir
            steamGameConfig.load_game_config(true);


            #region install TFMV mods (null fire overlay, for HLMV anti aliasing)

            if (Directory.Exists(steamGameConfig.tf_dir))
            {
                miscFunc.DeleteDirectoryContent(tfmv_dir);

                install_tfmv_mods();

                // force HLMV anti aliasing
                if (cb_hlmv_antialias.Checked)
                {
                    set_hlmv_antialias("8");
                }
            }

            #endregion


            #region unpack icons.zip if it exists and if icons directory has no files

            if (Directory.Exists(schema_dir + "/icons/"))
            {
                int fCount = Directory.GetFiles(schema_dir + "/icons/", "*", SearchOption.AllDirectories).Length;

                // if icons.zip exists (from installation) decompress/extract and delete files
                if ((File.Exists(schema_dir + "/icons.zip")) && (fCount == 0))
                {
                    try
                    {
                        using (ZipFile zip = ZipFile.Read(schema_dir + "icons.zip"))
                        {
                            foreach (ZipEntry zfile in zip)
                            {
                                zfile.Extract(schema_dir + "icons");
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

            #endregion

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (check_if_workshopuser())
            {
                btn_workshop_items.Visible = true;
            }

            string workshop_dir = steamGameConfig.tf_dir + "workshop";
            string workshop_import_dir = steamGameConfig.tf_dir + "workshop\\import_source\\";

            if (!Directory.Exists(workshop_import_dir)) { btn_workshop_items.Visible = false; }

        }

        // after form is loaded
        private void Main_Shown(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(options_panel_closed_width, this_height);

            // if checked, open adv options panel
            if (cb_adv_settings_start_open.Checked)
            {
                panel_adv_settings_btn.Location = new System.Drawing.Point(860, -1);
                panel_adv_settings_btn.Size = new System.Drawing.Size(30, 25);
                this.Size = new System.Drawing.Size(options_panel_open_width, this_height);
                lab_expand_options.Text = "<<";
            }

            btn_back_to_skins_tab.Location = new Point(751, 0);

            equip_regions_list = new List<equip_region_tfmv>();


            //neodement: auto-expand on startup
            if (cb_autoexpandonstartup.Checked)
            {
                btn_expand_item_list.PerformClick();
            }


            //neodement: set up buttons to make room for the medal button, if need be
            set_filter_button_sizes();

            this.BringToFront();


            // cache player textures for bodygroup masking if needed
            check_player_texture_cache();
            check_player_grey_texture_cache();

            #region custom mods / disable mods

            check_custom_mods();

            if (cb_disable_custom_mods.Checked)
            {
                disable_custom_mods();
            }
            else
            {
                restore_custom_mods();
            }

            #endregion

            #region install TFMV mods

            install_tfmv_mods();

            // write flat material if needed
            if (cb_screenshot_transparency.Checked)
            {
                write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", bg_color.R + " " + bg_color.G + " " + bg_color.B);
            }

            #endregion

            // get DPI scaling factor
            // we use take this into account for the HLMV screenshot region
            dpi_scale_factor = getScalingFactor();


            if (cb_screenshot_transparency.Checked)
            {
                btn_bg_color1.Visible = true;
                panel_Bgcolor1.Visible = true;
                lab_trsp_scrn.Visible = true;
                lab_trsp_scrn1.Visible = true;
                numUpDown_screenshot_delay.Visible = true;

                btn_bg_color1.Visible = true;
                panel_Bgcolor1.Visible = true;
            }

            //btn_primary.PerformClick();


            switch (lstStartupTab_Slot.Text)
            {
                case "Primary":
                    btn_primary.PerformClick();
                    break;
                case "Secondary":
                    btn_secondary.PerformClick();
                    break;
                case "Melee":
                    btn_melee.PerformClick();
                    break;
                case "Cosmetic":
                    btn_misc.PerformClick();
                    break;
                case "Building":
                    btn_building.PerformClick();
                    break;
                case "PDA":
                    btn_pda.PerformClick();
                    break;
                case "Taunt Prop":
                    btn_tauntprop.PerformClick();
                    break;
                //if it isn't set, set it to Primary
                default:

                    if (lstStartupTab_Slot.Text == "Workshop" && btn_workshop_items.Visible)
                    {
                        btn_workshop_items.PerformClick();
                    }
                    else if (lstStartupTab_Slot.Text == "Medal" && btn_medal.Visible)
                    {
                        btn_medal.PerformClick();
                    }
                    else
                    {
                        {

                            lstStartupTab_Slot.SelectedIndex = 0;
                            btn_primary.PerformClick();
                        }
                    }

                    break;

            }



        }

        #region dpi scaling info

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }

        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

        #endregion

        #region panels open/close methods

        #region advanced options pannel

        // expand or contract advanced settings pannel
        private void close_options_panel(object sender, EventArgs e)
        {
            // Form1.Size = new System.Drawing.Size(100, 100); // 890 // 1181
            collapsable_options_panel();
        }

        private void collapsable_options_panel()
        {

            int fheight = this_height;
            if (btn_expand_item_list.Text == "-") { fheight = this_height_extended; }

            if (btn_expand_item_list.Text != "+")
            {
                fheight = this_height_extended + 10;

            }

            // advanced settings panel open
            if (this.Size.Width <= options_panel_closed_width)
            {
                panel_adv_settings_btn.Location = new System.Drawing.Point(860, -1);
                panel_adv_settings_btn.Size = new System.Drawing.Size(30, 25);

                this.Size = new System.Drawing.Size(options_panel_open_width, fheight);
                lab_expand_options.Text = "<<";
            }
            else // advanced settings panel closed
            {

                // while in skins panel
                if ((btn_clear_clist.Visible) || (btn_back_to_skins_tab.Visible))
                {
                    panel_adv_settings_btn.Location = new System.Drawing.Point(860, -1);
                    panel_adv_settings_btn.Size = new System.Drawing.Size(30, 25);
                    lab_expand_options.Text = ">>";
                }
                else // while in loadouts panel
                {
                    panel_adv_settings_btn.Location = new System.Drawing.Point(752, -1);
                    panel_adv_settings_btn.Size = new System.Drawing.Size(138, 25);
                    lab_expand_options.Text = "Advanced Settings";
                }

                this.Size = new System.Drawing.Size(options_panel_closed_width, fheight);
            }
        }


        #endregion


        #region vtab loadout

        // collapse/expand loadout panel
        private void vtab_loadout_Click(object sender, EventArgs e)
        {
            if (!panel_paints.Enabled)
            {
                btn_back_to_skins_tab.Visible = false;
                vtab_loadout_close();
                panel_paints.Enabled = true;
                panel_paints.Show();

                // turntable tool
                foreach (var c in tab_items.Controls)
                {
                    if (c.GetType() == typeof(Turntable_GIF_Generator))
                    {
                        ((Turntable_GIF_Generator)c).Visible = true;
                    }
                }
            }
            else // edit loadout
            {
                // turntable tool
                foreach (var c in tab_items.Controls)
                {
                    if (c.GetType() == typeof(Turntable_GIF_Generator))
                    {
                        ((Turntable_GIF_Generator)c).Visible = false;
                    }
                }

                if (selected_team_skin_index != 0)
                {
                    selected_team_skin_index = 0;
                    switch_team_skin(0);
                    btn_red_team.ForeColor = Color.FromArgb(255, 30, 30, 30);
                    btn_blu_team.ForeColor = Color.FromArgb(255, 91, 122, 140);
                }

                btn_back_to_skins_tab.Visible = true;
                vtab_loadout_open();

                // reset team skin to red
                panel_paints.Enabled = false;
            }
        }

        private void vtab_loadout_open()
        {
            panel_paints.Hide();

            panel_screen_paints_tool.Visible = false;

            btn_clear_clist.Visible = false;
            btn_clear_clist.Enabled = false;
            btn_open_loadout_tab.Visible = false;

            int add_height = 290;
            int vheight = vtab_loadout_open_height;
            if (btn_expand_item_list.Text == "+")
            {
                vheight = vheight - add_height;
            }
            else
            {
                vheight = vheight + add_height;
            }

            vtab_loadout.MouseHover -= vtab_MouseHover;
            vtab_loadout.MouseLeave -= vtab_MouseLeave;
        }

        private void vtab_loadout_close()
        {
            panel_paints.SuspendLayout();

            panel_screen_paints_tool.Visible = true;

            btn_clear_clist.Visible = true;
            btn_clear_clist.Enabled = true;
            btn_open_loadout_tab.Visible = true;

            int add_height = 290;
            int vheight = vtab_loadout_open_height;
            if (btn_expand_item_list.Text == "+")
            {
                vheight = vheight - add_height;
            }
            else
            {
                vheight = vheight + add_height;
            }

            panel_paints.Location = new System.Drawing.Point(0, 25);

            vtab_loadout.MouseHover += vtab_MouseHover;
            vtab_loadout.MouseLeave += vtab_MouseLeave;

            panel_paints.Show();

            panel_paints.ResumeLayout();
        }



        #endregion

        private void vtab_MouseHover(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.Gray;
            Cursor = Cursors.Hand;

        }

        private void vtab_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.DimGray;
            Cursor = Cursors.Arrow;
        }


        private void vlabel_MouseHover(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.LightBlue;
            Cursor = Cursors.Hand;

        }

        private void vlabel_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.WhiteSmoke;
            Cursor = Cursors.Arrow;
        }


        private void btn_expand_item_list_Click_1(object sender, EventArgs e)
        {
            ui_scale();
        }

        // restore UI compact height
        private void ui_compact_vertical()
        {
            int padding = 10;
            int add_height = 300;
            int width = options_panel_closed_width;
            if (lab_expand_options.Text == "<<") { width = options_panel_open_width; }

            this.Size = new System.Drawing.Size(width, this_height);

            panel_loadout.Size = new Size(panel_loadout.Width, panel_loadout.Height - add_height + 10);


            panel_hlmv_settings.Size = new Size(panel_hlmv_settings.Width, panel_hlmv_settings.Height - add_height + padding);
            panel_items.Size = new Size(panel_items.Width, panel_items.Height - add_height);

            panel2.Size = new Size(panel2.Width, panel2.Height - add_height + padding);
            list_view.Size = new Size(list_view.Width, list_view.Height - add_height + padding);

            progressBar_item_list.Location = new Point(progressBar_item_list.Location.X, progressBar_item_list.Location.Y - add_height + padding);
            btn_expand_item_list.Location = new Point(btn_expand_item_list.Location.X, btn_expand_item_list.Location.Y - add_height + padding);

            panel_paints.Size = new Size(panel_paints.Width, panel_paints.Height - add_height + 15);
            skins_manager_control.Size = new Size(skins_manager_control.Width, skins_manager_control.Height - add_height);
            panel_tools.Size = new Size(skins_manager_control.Width, panel_tools.Height - add_height + 15);
        }

        // expand UI height
        private void ui_expand_vertical()
        {

            int padding = 10;
            int add_height = 300;
            int width = options_panel_closed_width;
            if (lab_expand_options.Text == "<<") { width = options_panel_open_width; }

            this.Size = new System.Drawing.Size(width, this_height_extended + 10);

            panel_loadout.Size = new Size(panel_loadout.Width, panel_loadout.Height + add_height - 10);


            panel_hlmv_settings.Size = new Size(panel_hlmv_settings.Width, panel_hlmv_settings.Height + add_height - padding);
            panel_items.Size = new Size(panel_items.Width, panel_items.Height + add_height);

            panel2.Size = new Size(panel2.Width, panel2.Height + add_height - padding);
            list_view.Size = new Size(list_view.Width, list_view.Height + add_height - padding);

            progressBar_item_list.Location = new Point(progressBar_item_list.Location.X, progressBar_item_list.Location.Y + add_height - padding);
            btn_expand_item_list.Location = new Point(btn_expand_item_list.Location.X, btn_expand_item_list.Location.Y + add_height - padding);


            panel_paints.Size = new Size(panel_paints.Width, panel_paints.Height + add_height - 10);
            skins_manager_control.Size = new Size(skins_manager_control.Width, skins_manager_control.Height + add_height);
            panel_tools.Size = new Size(skins_manager_control.Width, skins_manager_control.Height + add_height - 10);
        }


        // scale UI
        private void ui_scale()
        {
            if (btn_expand_item_list.Text == "+")
            {

                ui_expand_vertical();
                btn_expand_item_list.Text = "-";
            }
            else
            {

                ui_compact_vertical();
                btn_expand_item_list.Text = "+";
            }
        }

        #endregion

        // loads, verifies schema version, verifies integrity and tries to load items if schema is valid
        private void check_load_items()
        {
            // check if schema cache file version matches with TFMV version, if not, delete it to it gets re-generated with the new version
            bool schema_cache_valid = check_schema_cache_validity();

            if (!schema_cache_valid)
            {
                if (bgWorker_download_schema.IsBusy)
                {
                    return;
                }

                var result = MessageBox.Show("The item list needs to be updated.\n(this might take a few minutes to download)", "Download items list?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    download_schemas();
                }

            } else {

                load_schema(false);

                //freeze the whole program (oops)
                //while (bgWorker_load_schema.IsBusy)
                //{
                //    Thread.Sleep(1000);
                //}
                

                check_schema_version(true);

                //set a default value to the class startup tab if it wasn't set before
                if (lstStartupTab_Class.Text == "")
                {
                    lstStartupTab_Class.SelectedIndex = 0;
                }

                // set class and slot and load items
                //set_class("scout", false);
                set_class(lstStartupTab_Class.Text.ToLower(), true);



            }
        }



        // check that no other TFMV.exe or HLMV.exe processes are running
        private void check_duplicate_processes()
        {
            if (cb_AllowMultipleProc.Checked)
            {
                return;
            }

            int TFMV_process_id = Process.GetCurrentProcess().Id;

            List<Process> HLMV_processlist = new List<Process>();
            List<Process> TFMV_processlist = new List<Process>();

            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName == "hlmv")
                {
                    HLMV_processlist.Add(theprocess);
                }

                if ((theprocess.ProcessName == "TFMV") && (theprocess.Id != TFMV_process_id))
                {

                    TFMV_processlist.Add(theprocess);
                }
            }

            if (HLMV_processlist.Count > 0)
            {

#if DEBUG

                #region close other HLMV instances
                for (int i = 0; i < HLMV_processlist.Count; i++)
                {
                    try
                    {

                        bool process_exists = Process.GetProcesses().Any(x => x.Id == HLMV_processlist[i].Id);

                        if (process_exists)
                        {
                            HLMV_processlist[i].Kill();
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Could not close hlmv process: " + e);
                    }
                }

                return;

                #endregion
#endif

                DialogResult dialogResult = MessageBox.Show("Other HLMV (Half Life Model Viewer) are running. \nNo other instances should be running while using TFMV.\n\n Close other HLMV processes? ", "TFMV", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    for (int i = 0; i < HLMV_processlist.Count; i++)
                    {
                        try
                        {
                            bool process_exists = Process.GetProcesses().Any(x => x.Id == HLMV_processlist[i].Id);

                            if (process_exists)
                            {
                                HLMV_processlist[i].Kill();
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Could not close hlmv process: " + e);
                        }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Please, close all HLMV windows so TFMV can run HLMV properly.");
                    this.Close();
                }

            }

            if (TFMV_processlist.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Other TFMV are running. \nNo other instances should be running while using TFMV.\n\n Close other TFMV processes? ", "TFMV", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    for (int i = 0; i < TFMV_processlist.Count; i++)
                    {
                        try
                        {
                            bool process_exists = Process.GetProcesses().Any(x => x.Id == TFMV_processlist[i].Id);

                            if (process_exists)
                            {
                                TFMV_processlist[i].Kill();
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Could not close TFMV process: " + e);
                        }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Only one TFMV instance should run at once.");
                    this.Close();
                }

            }
        }


        //neodement: this is the function that is breaking

        public static void WriteResourceToFile(string resourceName, string fileName)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream file = assembly.GetManifestResourceStream(resourceName); // my resource

                //if (file == null) { return; }
                BinaryReader bReader = new BinaryReader(file);

                FileStream fStream = new FileStream(fileName, FileMode.Create);  // where i want to copy the resource

                using (BinaryWriter bWriter = new BinaryWriter(fStream))
                {

                    bWriter.Write(bReader.ReadBytes((int)file.Length));
                    bReader.Close();
                    bWriter.Close();

                }

                file.Close();
            }
            catch
            {
                MessageBox.Show("failed to write resource " + resourceName + " to file " + fileName);
            }
        }



        #endregion




        #region Panels and functionality

        public static void set_bgColor(Color c)
        {
            Main.write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", c.R + " " + c.G + " " + c.B);
            refresh_hlmv(false);
        }

        #region LOADOUT Panel


        // loadout to HLMV + paints
        private void btn_loadout_to_HLMV_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(tfmv_dir);

            #region check if parameters are set

            close_hlmv();


            if (!Directory.Exists(steamGameConfig.tf_dir))
            {
                MessageBox.Show("Error: You need to set the Team Fortress 2 directory.\nExample: C:\\Program Files(x86)\\Steam\\SteamApps\\common\\Team Fortress 2\\tf\\");
                tabControl.SelectedIndex = 1;

                return;
            }

            if ((steamGameConfig.steam_dir == "") || (steamGameConfig.steam_dir == null) || (!Directory.Exists(steamGameConfig.steam_dir)))
            {
                MessageBox.Show("Error: You need to set the Steam directory.");
                tabControl.SelectedIndex = 1;

                return;
            }

            if ((steamGameConfig.tf_dir == "") || (steamGameConfig.tf_dir == null) || (!Directory.Exists(steamGameConfig.tf_dir)))
            {
                MessageBox.Show("Error: You need to set the Team Fortress 2 directory.\nExample: C:\\Program Files(x86)\\Steam\\SteamApps\\common\\Team Fortress 2\\tf\\");
                tabControl.SelectedIndex = 1;

                return;
            }


            if (txtb_main_model.Text == "") { set_class("scout", true); selected_player_class = "scout"; }


            foreach (Loadout_Item item in loadout_list.Controls)
            {
                string mdl_path = item.model_path;

                if (mdl_path == null)
                {
                    MessageBox.Show("Error: (" + item.item_name + ") item's model path is undefined.");
                    return;
                }

                // if ((!miscFunc.IsValidPath(mdl_path)) || (!mdl_path.Contains(".mdl")))
                if (!mdl_path.Contains(".mdl"))
                {
                    MessageBox.Show("Error: (" + item.item_name + ") item model path is invalid\n(" + mdl_path + ")");
                    return;
                }
            }


            // if class not found, we search one from the models paths
            if ((selected_player_class == "") && (txtb_main_model.Text == ""))
            {

                #region find the class that appears the model in the models
                int[] ClassCount = new int[9];


                foreach (Loadout_Item item in loadout_list.Controls)
                {
                    string classx = find_class_inString(item.model_path);

                    if (classx == "scout") { ClassCount[0]++; }
                    if (classx == "demo") { ClassCount[1]++; }
                    if (classx == "engineer") { ClassCount[2]++; }
                    if (classx == "medic") { ClassCount[3]++; }
                    if (classx == "heavy") { ClassCount[4]++; }
                    if (classx == "pyro") { ClassCount[5]++; }
                    if (classx == "sniper") { ClassCount[6]++; }
                    if (classx == "soldier") { ClassCount[7]++; }
                    if (classx == "spy") { ClassCount[8]++; }
                }


                int highest_num = -1;
                int highest_class = -1;
                for (int i = 0; i < 8; i++)
                {
                    if (ClassCount[i] > highest_num + 1)
                    {
                        highest_num = ClassCount[i];
                        highest_class = i;
                    }
                }

                string found_class = "";

                if (highest_class == 0) { found_class = "scout"; }
                if (highest_class == 1) { found_class = "demo"; }
                if (highest_class == 2) { found_class = "engineer"; }
                if (highest_class == 3) { found_class = "medic"; }
                if (highest_class == 4) { found_class = "heavy"; }
                if (highest_class == 5) { found_class = "pyro"; }
                if (highest_class == 6) { found_class = "sniper"; }
                if (highest_class == 7) { found_class = "soldier"; }
                if (highest_class == 8) { found_class = "spy"; }


                #endregion

                if (found_class != "")
                {

                    set_class(found_class, true);
                }
                else
                {
                    MessageBox.Show("Select a Class.\n(could not determine class from model paths).");
                    return;
                }
            }


            if (txtb_main_model.Text == "")
            {

                MessageBox.Show("Error: main model is not set!");
                // mdlpath = cb_main_model.Text;
            }

            #endregion


            close_hlmv();

            //set_hlmv_fov();

            // if adv options tab is not open
            if (this.Size.Width <= options_panel_closed_width)
            {
                lab_expand_options.Text = ">>";
                panel_adv_settings_btn.Location = new System.Drawing.Point(860, -1);
                panel_adv_settings_btn.Size = new System.Drawing.Size(30, 25);
                lab_expand_options.Refresh();
                panel_adv_settings_btn.Refresh();
                panel_adv_settings_btn.Refresh();
            }

            if (btn_expand_item_list.Text == "-")
            {
                ui_scale();
            }


            #region reset skins manager form

            SkinsManager_Form_reset();

            colorPicker_master.EditPaint("ColorTint Base:" + "255 255 255");
            colorPicker_master.SelectedIndex = 0;

            selected_team_skin_index = 0;

            #endregion

            pictureBox_screen_paints_preview.Image = null;

            // maybe do not delete files if workshop zips are loaded??
            // or reload the zip item
            miscFunc.DeleteDirectoryContent(tfmv_dir + "materials\\models\\player\\");

            #region delete player materials (to make sure we reset skins to RED & bodygroup mask )

            // delete player materials (to make sure we reset skins to RED & bodygroup mask )
            // don't do this if models/materials are loaded from workshop zip!!!
            if (Directory.Exists(tfmv_dir + "materials\\models\\player\\"))
            {
                string[] player_mat_files = Directory.GetDirectories(tfmv_dir + "materials\\models\\player\\");

                foreach (var item in player_mat_files)
                {
                    if (!item.Contains("items"))


                        try
                        {
                            Directory.Delete(item, true);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Warning: could not delete directory for temporary player materials\nMake sure no file is being used by another process in \\Team Fortress 2\\tf\\custom\\TFMV\\" + exception.ToString());
                        }
                }
            }

            #endregion



            if (cb_ref_pose.Checked) { txtb_pose.Text = "ref"; }

            //THIS is where models are extracted to the tmp folder!!!!
            load_skins();

            // set player model skin
            switch_player_skin(selected_team_skin_index);

            // if main model is a TF2 player model, load the bodygroups
            if ((txtb_main_model.Text.ToLower().Contains(selected_player_class + ".mdl")) || ((txtb_main_model.Text.ToLower().Contains("demo.mdl"))))
            {
                // set bodygroups pannel and create bodygroups mask
                bodygroup_manager_panel.setup(loadout_bodygroups_off, tfmv_dir, cached_dir, selected_player_class, (selected_team_skin_index == 0) ? "red" : "blue");
            }

            // set TFMV background model color
            if (cb_screenshot_transparency.Checked)
            {
                write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", panel_Bgcolor.BackColor.R.ToString() + " " + panel_Bgcolor.BackColor.G.ToString() + " " + panel_Bgcolor.BackColor.B.ToString());
            }

            loadout_to_hlmv();


            vtab_loadout_close();
            panel_paints.Enabled = true;
        }


        // reset loadout
        private void btn_reset_loadout_Click(object sender, EventArgs e)
        {
            reset_loadout(false);
        }

        // closes hlmv, resets and closes skins panel, resets loadout unless specified to keep_loadout = true
        private void reset_loadout(bool keep_loadout)
        {
            close_hlmv();

            // reset skins manager form
            SkinsManager_Form_reset();

            pictureBox_screen_paints_preview.Image = null;

            // reset grey button
            flat_mat_switch = 0;
            btn_mainModel_material.ForeColor = Color.Gray;
            grey_material = false;


            // adv settings button make big
            panel_adv_settings_btn.Location = new System.Drawing.Point(752, -1);
            panel_adv_settings_btn.Size = new System.Drawing.Size(138, 25);
            // lab_expand_options.Text = "Advanced Settings";


            // advanced settings panel open
            if (this.Size.Width <= options_panel_closed_width)
            {
                // lab_expand_options.Text = "<<";
                lab_expand_options.Text = "Advanced Settings";
            }
            else // advaced settings panel closed
            {

                // while in skins panel
                if ((btn_clear_clist.Visible) || (btn_back_to_skins_tab.Visible))
                {
                    panel_adv_settings_btn.Location = new System.Drawing.Point(860, -1);
                    panel_adv_settings_btn.Size = new System.Drawing.Size(30, 25);
                    lab_expand_options.Text = "<<";
                }
                else // while in loadouts panel
                {
                    lab_expand_options.Text = "Advanced Settings";
                }
            }


            // hide screenshot paints tool and disable its bg worker
            panel_screen_paints_tool.Location = new Point(0, 1024);
            bgWorker_ScreenPaintsTool.Dispose();

            // reset bodygroups
            loadout_bodygroups_off = new List<string>();
            bodygroup_manager_panel.Controls.Clear();


            // reset paints team color button to red
            btn_red_team.ForeColor = Color.WhiteSmoke;
            btn_blu_team.ForeColor = Color.FromArgb(255, 91, 122, 140);

            #region reset paints pickers

            colorPicker_master.EditPaint("ColorTint Base:" + "255 255 255");
            colorPicker_master.SelectedIndex = 0;

            selected_team_skin_index = 0;

            skins_manager_control.Controls.Clear();
            miscFunc.DeleteDirectoryContent(tfmv_dir + "materials\\models\\");


            #endregion


            if (!keep_loadout)
            {
                // uncheck items from schema items list
                foreach (ListViewItem itemRow in this.list_view.Items)
                {
                    for (int i = 0; i < itemRow.SubItems.Count; i++)
                    {
                        itemRow.Checked = false;
                    }
                }

                // reset loadout item list
                loadout_list.Controls.Clear();

                // reset class
                set_class(selected_player_class, false);

                txtb_pose.Text = "ref";

                btn_back_to_skins_tab.Visible = false;
                lab_info_dragndrop.Visible = true;
                lab_info_dragndrop1.Visible = true;
                lab_info_dragndrop2.Visible = true;
                btn_example.Visible = true;

            }

            // remove turntable tool
            foreach (var c in tab_items.Controls)
            {
                if (c.GetType() == typeof(Turntable_GIF_Generator))
                {
                    ((Turntable_GIF_Generator)c).Dispose();
                }
            }

            // open loadout tab
            vtab_loadout_open();

            panel_paints.Enabled = false;
        }

        // save loadout
        private void btn_save_loadout_Click(object sender, EventArgs e)
        {

            if (Directory.Exists(settings_dir + "loadouts/") == false) { Directory.CreateDirectory(app_data_dir + "loadouts/"); }

            if (loadout_list.Controls.Count == 0) { MessageBox.Show("No items in loadout."); return; }

            Loadout_File loadout_file = new Loadout_File();

            loadout_file.player_class = selected_player_class;
            loadout_file.bodygroups_off = loadout_bodygroups_off;

            //(Loadout_Item_simple)main_model_panel.Controls[0]
            Loadout_Item_simple c = new Loadout_Item_simple();
            foreach (var pb in main_model_panel.Controls.OfType<Loadout_Item_simple>())
            {
                c = pb;
            }

            loadout_file.main_model.set_data(c);

            foreach (Loadout_Item item in loadout_list.Controls)
            {
                loadout_file.items.Add(new Loadout_File.loadout_item_data(item));
            }


            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = "Loadout file|*.bin";
            saveFileDialog1.Title = "Save a loadout";

            saveFileDialog1.InitialDirectory = Path.GetFullPath(settings_dir + "loadouts/");
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                try
                {
                    TFMV.Functions.Serializer.WriteToBinaryFile(saveFileDialog1.FileName, loadout_file);
                }
                catch (System.Exception excep)
                {
                    MessageBox.Show("Error saving settings " + excep.Message);
                }
            }



        }
        // load loadout
        private void btn_load_loadout_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(settings_dir + "loadouts/") == false) { Directory.CreateDirectory(app_data_dir + "loadouts/"); }

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = settings_dir + "loadouts/";

            openFileDialog1.Filter = "Loadout files (*.bin) | *.bin";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    Loadout_File loadout_file = TFMV.Functions.Serializer.ReadFromBinaryFile<Loadout_File>(openFileDialog1.FileName);
                    Loadout_Item main_model = new Loadout_Item();

                    // set class
                    if (items_loading) { return; }

                    btn_reset_loadout.PerformClick();
                    set_class(loadout_file.player_class, true);

                    main_model.icon = loadout_file.main_model.icon;

                    main_model.item_id = loadout_file.main_model.item_id;
                    main_model.item_name = loadout_file.main_model.item_name;
                    main_model.item_slot = loadout_file.main_model.item_slot;
                    main_model.skin_blu = loadout_file.main_model.skin_blu;
                    main_model.skin_red = loadout_file.main_model.skin_red;
                    main_model.skin_override_all = loadout_file.main_model.skin_override_all;
                    main_model.model_path = loadout_file.main_model.model_path;
                    main_model.workshop_zip_path = loadout_file.main_model.workshop_zip_path;

                    set_main_model(main_model, main_model.model_path, new Bitmap(main_model.icon)); //new Bitmap(main_model.icon)

                    foreach (var item in loadout_file.items)
                    {
                        loadout_addItem(item.icon, item.item_name, item.model_path, 0, 1, item.item_id, false);
                    }

                    // set bodygroups
                    loadout_bodygroups_off = loadout_file.bodygroups_off;
                }
                catch (Exception)
                {
                }
            }
        }


        // sets mian model item (thumbnail) and model path
        private void set_main_model(Loadout_Item _item, string model_path, Bitmap icon)
        {
            if ((_item == null) && (model_path == "") && (icon == null)) { MessageBox.Show("Invalid item."); return; }

            Loadout_Item_simple item_new = new Loadout_Item_simple();

            // create loadout item from model path and icon if item is undefined
            if (_item == null)
            {
                if (model_path == "") { MessageBox.Show("Invalid item: model path undefined."); return; }
                if (icon == null) { icon = Properties.Resources.icon_mdl_item; }

                item_new.icon = icon;
                item_new.Image = icon;
                item_new.item_name = "";
                item_new.model_path = model_path;
                item_new.item_id = 0;
                item_new.skin_red = 0;
                item_new.skin_blu = 1;
            }
            else
            {
                Loadout_Item_simple item = new Loadout_Item_simple();

                item_new.icon = _item.icon;
                item_new.Image = _item.icon;
                item_new.item_name = "";//_item.item_name;
                item_new.model_path = _item.model_path;
                item_new.item_slot = _item.item_slot;
                item_new.item_id = _item.item_id;
                item_new.skin_red = _item.skin_red;
                item_new.skin_blu = _item.skin_blu;
                item_new.skin_override_all = _item.skin_override_all;
            }

            item_new.ClientSize = new Size(77, 77);
            item_new.SizeMode = PictureBoxSizeMode.StretchImage;

            main_model_panel.Controls.Clear();
            item_new.Parent = main_model_panel;

            txtb_main_model.Text = item_new.model_path;
        }


        private void loadout_list_ControlAdded(object sender, ControlEventArgs e)
        {
            lab_info_dragndrop.Visible = false;
            lab_info_dragndrop1.Visible = false;
            lab_info_dragndrop2.Visible = false;
            btn_example.Visible = false;
        }

        private void btn_loadout_item_to_mainModel_Click(object sender, EventArgs e)
        {
            // for every model add a VMT  painter
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {
                Loadout_Item item = (Loadout_Item)loadout_list.Controls[i];
                if (item._selected)
                {
                    set_main_model(item, "", null);
                    item.Dispose();
                    break;
                }
            }
        }

        #endregion

        #region ITEMS Panel

        private void set_class(string playerx, bool load_items)
        {
            // if class name is invalid, return

            if (!miscFunc.is_valid_tf_class_name(playerx)) { return; }

            #region button color reset

            Color clr = Color.LightBlue;

            if (playerx == "scout") { btn_scout.BackColor = clr; } else { btn_scout.BackColor = Color.Gainsboro; }
            if (playerx == "demoman") { btn_demoman.BackColor = clr; } else { btn_demoman.BackColor = Color.Gainsboro; }
            if (playerx == "engineer") { btn_engineer.BackColor = clr; } else { btn_engineer.BackColor = Color.Gainsboro; }
            if (playerx == "medic") { btn_medic.BackColor = clr; } else { btn_medic.BackColor = Color.Gainsboro; }
            if (playerx == "heavy") { btn_heavy.BackColor = clr; } else { btn_heavy.BackColor = Color.Gainsboro; }
            if (playerx == "pyro") { btn_pyro.BackColor = clr; } else { btn_pyro.BackColor = Color.Gainsboro; }
            if (playerx == "sniper") { btn_sniper.BackColor = clr; } else { btn_sniper.BackColor = Color.Gainsboro; }
            if (playerx == "soldier") { btn_soldier.BackColor = clr; } else { btn_soldier.BackColor = Color.Gainsboro; }
            if (playerx == "spy") { btn_spy.BackColor = clr; } else { btn_spy.BackColor = Color.Gainsboro; }

            btn_badge.BackColor = Color.Gainsboro;

            #endregion

            selected_player_class = playerx.ToLower();

            string mdlpath = "models\\player\\" + selected_player_class + ".mdl";

            // use high quality model (hwm)
            if (cb_hwm.Checked) { mdlpath = "models\\player\\hwm\\" + selected_player_class + ".mdl"; }
            // hwm for demoman
            if ((selected_player_class.ToLower() == "demoman") && (!cb_hwm.Checked)) { mdlpath = "models\\player\\demo.mdl"; }
            if ((selected_player_class.ToLower() == "demoman") && (cb_hwm.Checked)) { mdlpath = "models\\player\\hwm\\demo.mdl"; }

            string player_class = selected_player_class;

            if (player_class == "all_class") { player_class = "scout"; }
            if (player_class == "demo") { player_class = "demoman"; }

            // set player icon
            set_main_model(null, mdlpath, new Bitmap(this.GetType().Assembly.GetManifestResourceStream("TFMV.Resources.player_icons." + player_class + ".png")));


            txtb_main_model.Text = mdlpath;

            if ((load_items) && (!adding_workshop_item_toLoadout))
            {
                load_items_to_listView(items_game, "", "", false);
            }

            // listb_cmdls.Items.Clear();
        }

        private void set_slot(string _slot)
        {
            item_type_name = "";

            #region button color reset

            Color clr = Color.LightBlue;

            if (_slot == "primary") { btn_primary.BackColor = clr; } else { btn_primary.BackColor = Color.Gainsboro; }
            if (_slot == "secondary") { btn_secondary.BackColor = clr; } else { btn_secondary.BackColor = Color.Gainsboro; }
            if (_slot == "melee") { btn_melee.BackColor = clr; } else { btn_melee.BackColor = Color.Gainsboro; }
            if (_slot == "misc") { btn_misc.BackColor = clr; } else { btn_misc.BackColor = Color.Gainsboro; }
            if (_slot == "building") { btn_building.BackColor = clr; } else { btn_building.BackColor = Color.Gainsboro; }
            if (_slot == "pda") { btn_pda.BackColor = clr; } else { btn_pda.BackColor = Color.Gainsboro; }
            //neodement: removed unnecessary pda2 slot
            if (_slot == "pda2") { btn_pda2.BackColor = clr; } else { btn_pda2.BackColor = Color.Gainsboro; }
            //neodement: added medal slot
            if (_slot == "medal") { btn_medal.BackColor = clr; } else { btn_medal.BackColor = Color.Gainsboro; }
            //neodement: added taunt prop slot
            if (_slot == "taunt") { btn_tauntprop.BackColor = clr; } else { btn_tauntprop.BackColor = Color.Gainsboro; }

            btn_badge.BackColor = Color.Gainsboro;

            #endregion

            selected_item_slot = _slot;

            if (items_game.Count == 0)
            {
                check_load_items();
            }

            load_items_to_listView(items_game, "", "", false);
        }

        private void reset_class()
        {
            btn_scout.BackColor = Color.Gainsboro;
            btn_soldier.BackColor = Color.Gainsboro;
            btn_demoman.BackColor = Color.Gainsboro;
            btn_pyro.BackColor = Color.Gainsboro;
            btn_medic.BackColor = Color.Gainsboro;
            btn_heavy.BackColor = Color.Gainsboro;
            btn_sniper.BackColor = Color.Gainsboro;
            btn_spy.BackColor = Color.Gainsboro;
            btn_engineer.BackColor = Color.Gainsboro;
            btn_badge.BackColor = Color.Gainsboro;

            selected_player_class = "";
            selected_player_class = "";
        }




        #region load workshop zip file items

        private void btn_workshop_items_Click(object sender, EventArgs e)
        {

            //disable multiclass checkbox, it doesn't work on this page
            cb_allclass_only.Enabled = false;

            //this variable stops right click context menu popping up when the Workshop tab is open
            supress_TF2Item_ContextMenu = true;

            //hide equip region filter because it doesn't work on this page
            comboBox_equip_region_filter.Visible = false;
            lab_region_filter.Visible = false;

            items_loading = true;

            list_view.BeginUpdate();

            try
            {
                string workshop_dir = steamGameConfig.tf_dir + "workshop";
                string workshop_import_dir = steamGameConfig.tf_dir + "workshop\\import_source\\";

                if (!Directory.Exists(workshop_dir)) { MessageBox.Show("Could not find directory: " + workshop_dir); items_loading = false; return; }
                if (!Directory.Exists(workshop_import_dir)) { MessageBox.Show("Could not find directory: " + workshop_import_dir); items_loading = false; return; }

                txtb_pose.Text = "ref";


                #region button color reset

                Color clr = Color.LightBlue;

                string _slot = "workshop";
                if (_slot == "primary") { btn_primary.BackColor = clr; } else { btn_primary.BackColor = Color.Gainsboro; }
                if (_slot == "secondary") { btn_secondary.BackColor = clr; } else { btn_secondary.BackColor = Color.Gainsboro; }
                if (_slot == "melee") { btn_melee.BackColor = clr; } else { btn_melee.BackColor = Color.Gainsboro; }
                if (_slot == "misc") { btn_misc.BackColor = clr; } else { btn_misc.BackColor = Color.Gainsboro; }
                if (_slot == "building") { btn_building.BackColor = clr; } else { btn_building.BackColor = Color.Gainsboro; }
                if (_slot == "pda") { btn_pda.BackColor = clr; } else { btn_pda.BackColor = Color.Gainsboro; }
                //neodement: removed unnecessary pda2 slot
                if (_slot == "pda2") { btn_pda2.BackColor = clr; } else { btn_pda2.BackColor = Color.Gainsboro; }
                //neodement: added medal slot
                if (_slot == "medal") { btn_medal.BackColor = clr; } else { btn_medal.BackColor = Color.Gainsboro; }
                //neodement: added taunt prop slot
                if (_slot == "taunt") { btn_tauntprop.BackColor = clr; } else { btn_tauntprop.BackColor = Color.Gainsboro; }

                btn_badge.BackColor = Color.Gainsboro;

                #endregion

                DirectoryInfo info = new DirectoryInfo(workshop_import_dir);
                FileInfo[] filesinfo = info.GetFiles("*.zip").OrderBy(p => p.CreationTime).ToArray();


                List<String> fp = new List<string>();
                foreach (FileInfo f in filesinfo)
                {
                    fp.Add(f.FullName);
                }

                string[] filePaths = fp.ToArray();

                if (filePaths.Length == 0)
                {
                    MessageBox.Show("No workshop zip files found.");
                }

                list_view.Items.Clear();
                imgList.Images.Clear();

                object[] arrObjects = new object[] { filePaths };


                if (!bgWorker_load_workshop_items_to_listView.IsBusy)
                {
                    btn_cancel_screen_paints.Visible = true;
                    // call background worker
                    bgWorker_load_workshop_items_to_listView.RunWorkerAsync(arrObjects);
                }

            }
            catch
            {
                Invoke(new Action(() => { items_loading = false; }));
            }
        }



        private void bgWorker_load_workshop_items_to_listView_DoWork(object sender, DoWorkEventArgs e)
        {
            #region bg worker setup

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects = (object[])e.Argument;//Collect the array of objects the we recived from the main thread

            String[] filePaths = (String[])arrObjects[0];
            StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            #endregion

            Invoke(new Action(() => { items_loading = true; }));

            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Visible = true; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Maximum = filePaths.Length; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Value = 0; }));

            while ((bgWorker_download_schema.IsBusy) || (bgWorker_load_items_to_listView.IsBusy) || (bgWorker_load_schema.IsBusy))
            {
                Thread.Sleep(300);
            }

            int list_index = 0;

            // reverse sorting of items list, to place most recent first
            Array.Reverse(filePaths);

            // for each workshop file
            foreach (var zip_path in filePaths)
            {
                #region check if header is corrupted, fix it

                Object modDate = File.GetLastWriteTime(zip_path);

                try
                {
                    miscFunc.DeleteDirectoryContent(tmp_workshop_zip_dir);


                    int flag = -1;
                    using (BinaryReader b = new BinaryReader(File.Open(zip_path, FileMode.Open)))
                    {
                        b.BaseStream.Position = 24;
                        flag = b.ReadInt32();
                    }

                    // if corrupted, fix it
                    if (flag == 0)
                    {
                        using (FileStream stream = new FileStream(zip_path, FileMode.Open))
                        {
                            using (BinaryWriter br = new BinaryWriter(stream))
                            {
                                br.BaseStream.Position = 24;
                                br.Write(786432);
                            }
                        }
                    }
                }
                catch { }

                #endregion


                #region  get_workshop_zip_as_item


                string item_name = "";

                Image icon = null;

                #region extract zip file data

                if (Path.GetExtension(zip_path).StartsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    // clear tmp dir
                    miscFunc.DeleteDirectoryContent(tmp_workshop_zip_dir);

                    if (!Directory.Exists(tmp_workshop_zip_dir)) { Directory.CreateDirectory(tmp_workshop_zip_dir); }

                    //neodement: wrapped this in a try statement to stop error message while debugging
                    //todo: get rid of this try statement probably?
                    //try
                    //{ 
                    // extract to tmp
                    using (ZipFile zip1 = ZipFile.Read(zip_path))
                    {

                        // extract files in "content" folder for backpack icon
                        foreach (ZipEntry z in zip1.Where(x => x.FileName.StartsWith("content\\materialsrc\\backpack")))
                        {
                            // if ((!e.FileName.Contains(".tga")) && (!e.FileName.Contains(".dmx")) && (!e.FileName.Contains(".qc")))
                            try
                            {
                                z.Extract(tmp_workshop_zip_dir, ExtractExistingFileAction.OverwriteSilently);
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        try
                        {
                            if (zip1["manifest.txt"] == null) { continue; }

                            // extract manifest
                            zip1["manifest.txt"].Extract(tmp_workshop_zip_dir, ExtractExistingFileAction.OverwriteSilently);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    //}
                    //catch
                    //{
                    //}


                }

                #endregion

                #region search tga file

                // vdf parser is crap or Valve's manifests are broken
                // so just do this crappy workaround >_>
                string icon_path = "";
                string tga_name = "";

                try
                {
                    var fileStream = new FileStream(tmp_workshop_zip_dir + "manifest.txt", FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {

                            if (line.ToLower().Contains("\"name\""))
                            {
                                item_name = line.Split('"')[3];
                            }

                            if (line.ToLower().Contains("\"icon\""))
                            {
                                icon_path = line.Split('"')[3];
                                break;
                            }
                        }
                    }

                }
                catch
                {
                }


                if (File.Exists(tmp_workshop_zip_dir + icon_path))
                {
                    Functions.TargaImage tf = null;
                    try
                    {
                        tf = new Functions.TargaImage(tmp_workshop_zip_dir + icon_path);
                        icon = tf.Image;
                    }
                    catch //(Exception ex)
                    {
                        //  MessageBox.Show(ex.ToString());
                    }
                }


                #endregion


                imgList.ImageSize = new Size(64, 64);

                if (icon == null) { icon = Properties.Resources.icon_workshop_item; }

                Invoke(new Action(() => { imgList.Images.Add(icon); }));

                // add item to item list
                try
                {
                    ExtdListViewItem item_ListView = new ExtdListViewItem();

                    // set workshop zip file path
                    item_ListView.workshop_zip_path = zip_path;

                    item_ListView.item_id = list_index;
                    item_ListView.item_name = item_name;
                    item_ListView.Text = item_name;

                    item_ListView.item_id = list_index;


                    item_ListView.ImageIndex = list_index;

                    Invoke(new Action(() => { list_view.LargeImageList = imgList; }));
                    Invoke(new Action(() => { list_view.Items.Add(item_ListView); }));
                }
                catch
                {
                    MessageBox.Show("Failed to load: " + Path.GetFileName(zip_path));
                }

                #endregion

                list_index++;

                sendingWorker.ReportProgress(list_index);
                progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Value = list_index; }));
            }

            e.Result = sb.ToString();
        }

        protected void bgWorker_load_workshop_items_to_listView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;

                list_view.Enabled = true;
                list_view.EndUpdate();

            }
            else if (e.Cancelled)
            {
                list_view.Enabled = true;
                list_view.EndUpdate();
            }
            else
            {
                list_view.Enabled = true;
                list_view.EndUpdate();
            }

            Invoke(new Action(() => { items_loading = false; }));

            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Value = progressBar_item_list.Maximum; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Visible = false; }));

            Invoke(new Action(() => { items_loading = false; }));
        }


        protected void bgWorker_load_workshop_items_to_listView_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            // lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        #endregion



        // search item click
        private void txtb_searchitem_Click(object sender, EventArgs e)
        {
            if (txtb_searchitem.Text == "Search (Press Enter)")
            {
                txtb_searchitem.Text = "";
            }
        }

        // combobox event : select and filter items by equip region
        private void comboBox_equip_region_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox_equip_region_filter
            if (items_loading) { return; }
            adding_workshop_item_toLoadout = false;

            string region_filter = (comboBox_equip_region_filter.Text.Split(']')[1]).TrimStart();

            load_items_to_listView(items_game, "", region_filter, false);
        }

        // checkbox event : load all class items only
        private void cb_allclass_only_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_allclass_only.Checked)
            {
                //cb_no_load_allclass.Checked = false;
                // settings_save(sender, e);
            }

            load_items_to_listView(items_game, "", "", false);
        }

        // buttons load class
        private void btn_class_load_Click(object sender, EventArgs e)
        {

            if (bgWorker_download_schema.IsBusy) { return; }
            if (bgWorker_load_schema.IsBusy) { return; }
            if (items_loading) { return; }

            if (items_game.Count == 0)
            {
                check_load_items();
            }

            // clear equip regions filter list
            if (comboBox_equip_region_filter.Items != null)
            {
                comboBox_equip_region_filter.Items.Clear();
            }

            if (equip_regions_list != null)
            {
                equip_regions_list.Clear();
            }

            comboBox_equip_region_filter.Text = "Equip region filter";

            Button btn = (Button)sender;
            set_class(btn.Text.ToLower(), true);

        }


        //neodement: todo: removed badge button...
        private void btn_badge_Click(object sender, EventArgs e)
        {

            #region button color reset

            btn_scout.BackColor = Color.Gainsboro;
            btn_soldier.BackColor = Color.Gainsboro;
            btn_demoman.BackColor = Color.Gainsboro;
            btn_pyro.BackColor = Color.Gainsboro;
            btn_medic.BackColor = Color.Gainsboro;
            btn_heavy.BackColor = Color.Gainsboro;
            btn_sniper.BackColor = Color.Gainsboro;
            btn_spy.BackColor = Color.Gainsboro;
            btn_engineer.BackColor = Color.Gainsboro;

            #endregion

            #region slots button color reset

            Color clr = Color.LightBlue;

            btn_primary.BackColor = Color.Gainsboro;
            btn_secondary.BackColor = Color.Gainsboro;
            btn_melee.BackColor = Color.Gainsboro;
            btn_misc.BackColor = Color.Gainsboro;
            btn_building.BackColor = Color.Gainsboro;
            btn_pda.BackColor = Color.Gainsboro;
            btn_pda2.BackColor = Color.Gainsboro;

            #endregion

            Button btn = (Button)sender;
            btn.BackColor = Color.LightBlue;

            // Button btn = (Button)sender;
            item_type_name = "#TF_Wearable_Badge";
            selected_item_slot = "misc";
        }

        // buttons load item slot type
        private void btn_slot_load_Click(object sender, EventArgs e)
        {

            //enable multiclass checkbox, workshop button may have disabled it
            cb_allclass_only.Enabled = true;

            //this variable stops right click context menu popping up when the Workshop tab is open
            supress_TF2Item_ContextMenu = false;

            if (bgWorker_download_schema.IsBusy) { return; }
            if (bgWorker_load_schema.IsBusy) { return; }
            if (items_loading) { return; }

            if (items_game.Count == 0)
            {
                check_load_items();
            }

            adding_workshop_item_toLoadout = false;

            comboBox_equip_region_filter.Text = "Equip region filter";
            Button btn = (Button)sender;


            if (btn.Tag.ToString() == "misc")
            {
                cb_allclass_only.Enabled = true;

                comboBox_equip_region_filter.Visible = true;
                lab_region_filter.Visible = true;
                equip_regions_list = new List<equip_region_tfmv>();

                // clear equip regions filter list
                comboBox_equip_region_filter.Items.Clear();
            }
            else if (btn.Tag.ToString() == "medal")
            {
                cb_allclass_only.Checked = true;
                cb_allclass_only.Enabled = false;
            }
            else
            {
                cb_allclass_only.Enabled = true;

                comboBox_equip_region_filter.Visible = false;
                lab_region_filter.Visible = false;
            }

            set_slot(btn.Tag.ToString());
        }

        #endregion

        #region  TOOLS Panel (advanced settings)

        private void cb_fix_hlp_bones_CheckedChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);

            if (cb_fix_hlp_bones.Checked)
            {
                mdl_disable_hlp_bones(tfmv_dir + txtb_main_model.Text);

                refresh_hlmv(false);
            }
            else
            {
                //restore main model 

                #region extract/copy main model to tf/custom/TFMV

                string mdlpath = txtb_main_model.Text;

                // check if main .mdl exists in tf/models, in VPK or in tf/custom/TFMV/models
                // if it doesn't exist in drive, we copy it to TF/custom/models
                // so we can load the model directly to HLMV by argument (without having to manually load recent files or F5)

                // if exists in tf/models/ copy to tf/custom/TFMV/models/
                if (File.Exists(steamGameConfig.tf_dir + mdlpath))
                {
                    string mdl_name = mdlpath.Replace(".mdl", "");
                    string mdl_in = steamGameConfig.tf_dir + mdl_name;
                    string mdl_out = tfmv_dir + mdl_name;

                    miscFunc.copy_safe(mdl_in + ".mdl", mdl_out + ".mdl");
                }

                else
                { // if found in VPK, extract it to TF/custom/TFMV/...

                    #region extract model

                    if (VPK.Find(mdlpath, 0))
                    {
                        string mdl_name = mdlpath.Replace(".mdl", "");
                        string mdl_in = mdl_name;
                        string mdl_out = tfmv_dir + mdl_name;
                        VPK.Extract(mdl_in + ".mdl", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    }

                    #endregion
                }

                #endregion

                refresh_hlmv(false);
            }
        }


        private void cb_disable_jigglebones_CheckedChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);

            // disable jigglebones
            if (cb_disable_jigglebones.Checked)
            {
                mdl_disable_all_jigglebones_in_folder(tmp_dir, tfmv_dir);

                refresh_hlmv(false);
            }
            else
            {
                restore_mdls_in_folder(tmp_dir, tfmv_dir);

                refresh_hlmv(false);
            }

        }


        // button event : set HLMV window size
        private void btn_window_scale_Click(object sender, EventArgs e)
        {
            if ((proc_HLMV == null) || (proc_HLMV.HasExited))
            {
                //MessageBox.Show("Cannot resize window, HLMV is not running.");
                return;
            }

            // make number integer
            if (txtb_hlmv_wsize_x.Text.Contains(".")) { txtb_hlmv_wsize_x.Text = txtb_hlmv_wsize_x.Text.Split('.')[0]; }
            if (txtb_hlmv_wsize_y.Text.Contains(".")) { txtb_hlmv_wsize_y.Text = txtb_hlmv_wsize_y.Text.Split('.')[0]; }

            int x = Convert.ToInt32(txtb_hlmv_wsize_x.Text);
            int y = Convert.ToInt32(txtb_hlmv_wsize_y.Text);

            var rect = new Rect();
            GetWindowRect(proc_HLMV.MainWindowHandle, ref rect);


            if ((x > 10000) || (y > 10000))
            {
                MessageBox.Show("Woah there! that's a big-ass monitor ya got! \nAre we in the future? with +10k monitors!? \nWhy are you still using TFMV in the future?\n\nSo many questions! \n\nTry a lower resolution :)");
                return;
            }

            if ((x <= 0) || (y <= 0))
            {
                MessageBox.Show("Now you're just trying to break things.\nTry a higher resolution :)");
                return;
            }

            if ((x < 25) || (y < 25))
            {
                MessageBox.Show("What's that? I can't see anything >.<");
                // return;
            }

            // padding of the HLMV panels so we scale the model viewer area
            int width = x + hlmv_padding.left;
            int height = y + hlmv_padding.bottom;

            //neodement: apply scale even if tfmv window size is disabled
            //if (!cb_disable_window.Checked)
            //{
                SetWindowPos(proc_HLMV.MainWindowHandle, HWND_TOP, rect.left, rect.top, width, height, 0);
            //}
        }
        // textbox event : set HLMV window size X
        private void txtb_hlmv_wsize_x_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }
        // textbox event : set HLMV window size Y
        private void txtb_hlmv_wsize_y_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        // button event : show paints chart tool panel
        private void btn_paints_chart_tool_Click(object sender, EventArgs e)
        {
            panel_screen_paints_tool.Enabled = true;
            panel_screen_paints_tool.Visible = true;
            panel_screen_paints_tool.Location = new Point(0, 25);
            panel_screen_paints_tool.BringToFront();
        }

        #endregion

        #region SKINS panel


        #region switch team skin

        private void btn_red_team_Click(object sender, EventArgs e)
        {
            // reset grey button
            flat_mat_switch = 0;
            btn_mainModel_material.ForeColor = Color.Gray;

            selected_team_skin_index = 0;
            switch_team_skin(0);
            btn_red_team.ForeColor = Color.FromArgb(255, 30, 30, 30);
            btn_blu_team.ForeColor = Color.FromArgb(255, 91, 122, 140);
            btn_mainModel_material.ForeColor = Color.Gray;

        }

        private void btn_blu_team_Click(object sender, EventArgs e)
        {
            // reset grey button
            flat_mat_switch = 0;
            btn_mainModel_material.ForeColor = Color.Gray;

            selected_team_skin_index = 1;
            switch_team_skin(1);
            btn_blu_team.ForeColor = Color.FromArgb(255, 30, 30, 30);
            btn_red_team.ForeColor = Color.FromArgb(255, 189, 59, 59);
            btn_mainModel_material.ForeColor = Color.Gray;
        }

        private void switch_team_skin(byte skin_index)
        {
            skins_manager_control.SuspendLayout();

            if (grey_material) { remove_player_grey_material(); grey_material = false; }


            #region set paint_pickers to team specific paint list
            List<TFMV.TF2.paints.paint> team_paints = new List<TFMV.TF2.paints.paint>();

            if (skin_index == 0) { team_paints = TFMV.TF2.paints.red; } else { team_paints = TFMV.TF2.paints.blu; }

            // master paint picker
            int p_index = colorPicker_master.SelectedIndex;
            colorPicker_master.Items.Clear();
            colorPicker_master.AddPaint("ColorTint Base:" + "255 255 255");
            colorPicker_master.AddPaints(team_paints);

            colorPicker_master.SelectedIndex = p_index;

            #endregion

            auto_refresh_busy = true;

            #region reload skins

            // for each model painter
            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    // if model paint is not locked we switch the skins
                    if (!mp.cb_lock_skin.Checked)
                    {

                        // switch paints team color
                        if (vmt.color_picker.Items.Count > 1)
                        {
                            p_index = vmt.color_picker.SelectedIndex;

                            // var base_color_item = vmt.color_picker.Items[0];
                            vmt.color_picker.Items.Clear();
                            vmt.color_picker.AddPaint("ColorTint Base:" + "255 255 255");
                            // restore base color paint
                            // vmt.color_picker.Items[0] = base_color_item;
                            vmt.color_picker.AddPaints(team_paints);
                            vmt.color_picker.SelectedIndex = p_index;
                        }

                        mp.switch_Skin(skin_index);

                    }
                }
            }

            #endregion

            update_skins(false);

            switch_player_skin(skin_index);

            auto_refresh_busy = false;

            // regenerate bodygroup mask for team-color texture
            bodygroup_manager_panel.switch_skin((selected_team_skin_index == 0) ? "red" : "blue");

            skins_manager_control.ResumeLayout();
        }

        private void lab_exit_ScreenPaintsTool_MouseHover(object sender, EventArgs e)
        {
            lab_exit_ScreenPaintsTool.ForeColor = Color.DarkRed;
            Cursor = Cursors.Hand;
        }

        private void lab_exit_ScreenPaintsTool_MouseLeave(object sender, EventArgs e)
        {
            lab_exit_ScreenPaintsTool.ForeColor = Color.WhiteSmoke;
            Cursor = Cursors.Arrow;
        }

        #endregion


        private void btn_sort_order_Click(object sender, EventArgs e)
        {

            if (items_game.Count > 0) // && (items_badges.Count > 0) 
            {
                // items_badges.Reverse();
                items_game.Reverse();

                //if (item_type_name == "#TF_Wearable_Badge")
                //{
                // load_items_to_listView(items_badges, "", true);
                //}
                //else
                //{
                load_items_to_listView(items_game, "", "", false);
                //}
            }
        }

        private void btn_item_sorting_order_Click(object sender, EventArgs e)
        {
            if (items_game.Count > 0) // && (items_badges.Count > 0) 
            {
                comboBox_equip_region_filter.Text = "Equip region filter";

                items_game.Reverse();

                //if (item_type_name == "#TF_Wearable_Badge")
                //{
                // load_items_to_listView(items_badges, "", true);
                //}
                //else
                //{
                load_items_to_listView(items_game, "", "", false);
                //}

                if (cb_sort_order.Checked)
                {
                    cb_sort_order.Checked = false;
                    btn_item_sorting_order.Image = TFMV.Properties.Resources.up;
                }
                else
                {
                    cb_sort_order.Checked = true;
                    btn_item_sorting_order.Image = TFMV.Properties.Resources.down;

                }
            }
        }
        #endregion


        #region ADVANCED SETTINGS Panel

        // use HMW player models // checkbox event
        private void cb_hwm_CheckedChanged(object sender, EventArgs e)
        {
            // use high quality model (hwm)
            if (cb_hwm.Checked) { txtb_main_model.Text = "models\\player\\hwm\\" + selected_player_class + ".mdl"; }
            if (!cb_hwm.Checked) { txtb_main_model.Text = "models\\player\\" + selected_player_class + ".mdl"; }
            // hwm for demoman
            if ((selected_player_class.ToLower() == "demoman") && (!cb_hwm.Checked)) { txtb_main_model.Text = "models\\player\\demo.mdl"; }
            if ((selected_player_class.ToLower() == "demoman") && (cb_hwm.Checked)) { txtb_main_model.Text = "models\\player\\hwm\\demo.mdl"; }

            settings_save(sender, e);

            if (cb_hwm.Checked)
            {
                MessageBox.Show("If you are using TFMV to create workshop items, do not use HWM models. \n\nHWM models are not used in-game, if you create a head/face item with the HWM player model, the texture and flexes will not match ingame.", "Warning");
            }

            // reset loadout
            btn_reset_loadout_Click(null, EventArgs.Empty);
        }

        byte flat_mat_switch = 0;

        // switches main model material to a (128 128 128) diffuse
        private void btn_mainModel_material_Click(object sender, EventArgs e)
        {
            Color c = new Color();

            if (flat_mat_switch == 0) { c = Color.Gray; set_player_grey_material(); }
            if (flat_mat_switch == 1) { c = Color.White; set_player_flat_material("255 255 255"); }
            if (flat_mat_switch == 2) { c = Color.Black; set_player_flat_material("0 0 0"); }

            btn_mainModel_material.BackColor = c;
            btn_mainModel_material.ForeColor = c;

            flat_mat_switch++;

            if (flat_mat_switch > 2) { flat_mat_switch = 0; }
        }


        #endregion


        #region Settings


        private void settings_save(object sender, EventArgs e)
        {

        //don't save settings if you haven't loaded settings yet
        if (!settings_loaded) { return; }

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

                if (sender.GetType() == typeof(RadioButton))
                {
                    RadioButton obj = (RadioButton)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.Checked.ToString();
                }


                if (sender.GetType() == typeof(Panel))
                {
                    Panel obj = (Panel)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.BackColor.R + ":" + obj.BackColor.G + ":" + obj.BackColor.B;
                }

                if (sender.GetType() == typeof(NumericUpDown))
                {
                    NumericUpDown obj = (NumericUpDown)sender;
                    obj_name = obj.Name.ToString();
                    arg = obj.Value.ToString();
                }


                TextWriter tw = new StreamWriter(settings_dir + "settings.ini");

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
                MessageBox.Show("Error saving settings.\n\n" + excep.Message);
            }
        }


        private void settings_load()
        {
            settings.Clear();

            try
            {
                if (File.Exists(settings_dir + "settings.ini"))
                {
                    string f = settings_dir + "settings.ini";
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

                                if (arg[0].ToLower() == "true") {

                                    if (obj.Name == "cb_allow_tournament_medals")
                                    {
                                        //special variable so we know not to trigger the dialog as a user didn't click it.
                                        //this is only set when changing the checkbox value to TRUE, as it's FALSE by default and we don't want the SupressCheckedChange getting stuck.
                                        cb_allow_tournament_medals_SupressCheckedChange = true;
                                    }

                                    obj.Checked = true;
                                }
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

                            if (prop.GetType() == typeof(RadioButton))
                            {
                                RadioButton obj = (RadioButton)prop;
                                obj.Checked = Convert.ToBoolean(arg[0]);
                            }

                            if (prop.GetType() == typeof(Panel))
                            {
                                Panel obj = (Panel)prop;
                                Color clr = Color.FromArgb(Convert.ToByte(arg[0].Split(':')[0]), Convert.ToByte(arg[0].Split(':')[1]), Convert.ToByte(arg[0].Split(':')[2]));
                                obj.BackColor = clr;

                                if (obj.Name == "panel_Bgcolor")
                                {
                                    bg_color = clr;
                                }

                                if (obj.Name == "panel_aColor")
                                {
                                    aColor = clr;
                                }

                                if (obj.Name == "panel_lColor")
                                {
                                    lColor = clr;
                                }
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
                MessageBox.Show("Error loading settings.\n\n" + excep.Message);
            }

        //settings are loaded, now we can save them
        settings_loaded = true;
    }

        static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = (MemberExpression)expr.Body;

            return body.Member.Name;
        }


        private void load_banned_items()
        {
            try
            {
                if (File.Exists(settings_dir + "banned_items.txt"))
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(settings_dir + "banned_items.txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        banned_items.Add(line.Trim());

                        counter++;
                    }

                    file.Close();
                }
            }
            catch (Exception)
            {
                //    MessageBox.Show("Error loading settings! (banned_items.txt)");
            }
        }

        private void load_hint_modelspaths()
        {
            try
            {
                if (File.Exists(settings_dir + "hint_models.txt"))
                {
                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file = new System.IO.StreamReader(settings_dir + "hint_models.txt");


                    while ((line = file.ReadLine()) != null)
                    {

                        txtb_cmdl_path.Items.Add(line.Trim());

                    }

                    file.Close();
                }
                else
                { // file doesn't exist

                    if (!Directory.Exists(settings_dir))
                    {
                        Directory.CreateDirectory(settings_dir);
                    }

                    File.Create(settings_dir + "hint_models.txt");
                }
            }
            catch (Exception)
            {
                //   MessageBox.Show("Error loading settings! (hint_models.txt)");
            }
        }

        private void load_pose_names()
        {
            try
            {
                if (File.Exists(settings_dir + "pose_names.txt"))
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(settings_dir + "pose_names.txt");
                    while ((line = file.ReadLine()) != null)
                    {

                        txtb_pose.Items.Add(line.Trim());

                        counter++;
                    }

                    file.Close();
                }
                else
                { // file doesn't exist
                    File.Create(settings_dir + "pose_names.txt");
                }
            }
            catch (Exception)
            {
                //  MessageBox.Show("Error loading settings! (pose_names.txt)");
            }
        }


        private void save_paints_selection()
        {
            using (BinaryWriter b = new BinaryWriter(File.Open(settings_dir + "\\paints_selection.bin", FileMode.Create)))
            {
                foreach (var item in paints_selection)
                {
                    b.Write(item);
                }
            }
        }

        private void load_paints_selection()
        {
            string path = settings_dir + "\\paints_selection.bin";
            if (!File.Exists(path)) { return; }

            byte[] fileBytes = File.ReadAllBytes(path);
            StringBuilder sb = new StringBuilder();

            paints_selection = new List<byte>();

            foreach (byte b in fileBytes)
            {
                paints_selection.Add(b);
            }
        }


        private void load_hint_main_modelspaths()
        {
            try
            {
                if (File.Exists(settings_dir + "hint_main_models.txt"))
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(settings_dir + "hint_main_models.txt");
                    while ((line = file.ReadLine()) != null)
                    {

                        txtb_main_model.Items.Add(line.Trim());

                        counter++;
                    }

                    file.Close();
                }
                else
                { // file doesn't exist
                    File.Create(settings_dir + "hint_main_models.txt");
                }
            }
            catch (Exception)
            {
                //  MessageBox.Show("Error loading settings! (hint_main_models.txt)");
            }
        }

        private bool check_sdk_tools()
        {
            // MessageBox.Show("Checking if sdk tools beta is installed in: \n" + steam_dir + tf_common_path + "bin\\");
            // return true;
            // steam_dir + tf_common_path + "tf\\"
            if (File.Exists(steamGameConfig.tf2_dir + "bin\\hlmv.exe"))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Error: could not find " + steamGameConfig.tf2_dir + "bin\\hlmv.exe");

                return false;
            }
        }

        #endregion

        #endregion


        #region screenshots



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
                MessageBox.Show("CpatureApplication() : Unable to get process " + procName);
                return null;
            }

            // You need to focus on the application
            SetForegroundWindow(proc.MainWindowHandle);

            // You need some amount of delay, but 1 second may be overkill
            Thread.Sleep(1000);

            Rect rect = new Rect();
            IntPtr error = GetWindowRect(proc.MainWindowHandle, ref rect);

            // sometimes it gives error.
            while (error == (IntPtr)0)
            {
                error = GetWindowRect(proc.MainWindowHandle, ref rect);
            }

            int rect_left = (int)((rect.left + hlmv_padding.right) * dpi_scale_factor);
            int rect_top = (int)((rect.top + hlmv_padding.top) * dpi_scale_factor);

            int width = (int)(((rect.right - rect.left) - hlmv_padding.left) * dpi_scale_factor);
            int height = (int)(((rect.bottom - rect.top) - hlmv_padding.bottom) * dpi_scale_factor);

            // correct cropping for higher DPI scaling
            if (dpi_scale_factor > 1) { rect_left += 2; rect_top += 2; }

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            Graphics.FromImage(bmp).CopyFromScreen(rect_left, rect_top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            return bmp;
        }


        public Bitmap screenshot_capture(bool export_to_file, bool opaque)
        {
            // create screenshots dir if it doesn't exist
            miscFunc.create_missing_dir(txtb_screenshots_dir.Text);

            // check if HLMV process is running
            if (Process_IsRunning(proc_HLMV))
            {
                try
                {
                    // we restore window to normal state in case its minimized
                    ShowWindow(proc_HLMV.MainWindowHandle, SW_SHOW);

                    // check if its minimized we restore it
                    // we only restore it if minized because it tends to modify the scale/placement when doing so
                    var placement = GetPlacement(proc_HLMV.MainWindowHandle);
                    if (placement.showCmd.ToString() == "Minimized")
                    {
                        ShowWindow(proc_HLMV.MainWindowHandle, SW_RESTORE);
                        proc_HLMV.WaitForInputIdle();
                    }

                    ImageCodecInfo formatEncoder = GetEncoder(ImageFormat.Png);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    string date = _starttime.AddTicks(_stopwatch.Elapsed.Ticks).ToString().Replace(" ", "").Replace("/", "_").Replace(":", "");
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    if (cb_screenshot_transparency.Checked && !opaque)
                    {
                        int delay = Convert.ToInt16(numUpDown_screenshot_delay.Value) * 1000;

                        // set black background
                        write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", "0 0 0");
                        Thread.Sleep(delay / 4);
                        refresh_hlmv(true, delay);
                        // take screenshot and store bitmap
                        Bitmap black_bg = screenshot_capture(false, true);

                        // set white background
                        write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", "255 255 255");
                        Thread.Sleep(delay / 4);
                        refresh_hlmv(true, delay);
                        // take screenshot and store bitmap
                        Bitmap white_bg = screenshot_capture(false, true);

                        // generate transparency from black and white masks
                        Bitmap image_transparent = new BitmapProcessing().get_transp_bitmap(white_bg, black_bg);

                        // reset background color
                        write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", panel_Bgcolor1.BackColor.R + " " + panel_Bgcolor1.BackColor.G + " " + panel_Bgcolor1.BackColor.B);

                        // save transparent image
                        image_transparent.Save(txtb_screenshots_dir.Text + "\\TFMV_" + date + ".png", formatEncoder, myEncoderParameters);

                        return image_transparent;
                    }
                    else // opaque image
                    {
                        // Get a bitmap.
                        Bitmap image = CaptureApplication(proc_HLMV.ProcessName); // new Bitmap(@"c:\TestPhoto.jpg");

                        if (export_to_file)
                        {
                            image.Save(txtb_screenshots_dir.Text + "\\tfmv_" + date + ".png", formatEncoder, myEncoderParameters);
                        }

                        return image;
                    }



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Cannot take a screenshot, HLMV is not running.");
                return null;
            }
        }

        // export bitmap as PNG file in screenshots folder
        private void bitmap_export(Bitmap bmp)
        {
            try
            {
                ImageCodecInfo formatEncoder = GetEncoder(ImageFormat.Png);

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);

                string date = _starttime.AddTicks(_stopwatch.Elapsed.Ticks).ToString().Replace(" ", "").Replace("/", "_").Replace(":", "");

                myEncoderParameters.Param[0] = myEncoderParameter;

                bmp.Save(txtb_screenshots_dir.Text + "\\tfmv_" + date + ".png", formatEncoder, myEncoderParameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //return null;
            }
        }


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        DateTime _starttime = DateTime.UtcNow;
        Stopwatch _stopwatch = Stopwatch.StartNew();

        public class HiResDateTime
        {
            private static DateTime _startTime;
            private static Stopwatch _stopWatch = null;
            private static TimeSpan _maxIdle =
                TimeSpan.FromSeconds(10);

            public static DateTime UtcNow
            {
                get
                {
                    if ((_stopWatch == null) ||
                        (_startTime.Add(_maxIdle) < DateTime.UtcNow))
                    {
                        Reset();
                    }
                    return _startTime.AddTicks(_stopWatch.Elapsed.Ticks);
                }
            }

            private static void Reset()
            {
                _startTime = DateTime.UtcNow;
                _stopWatch = Stopwatch.StartNew();
            }
        }


        private void btn_sel_screensdir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;

                txtb_screenshots_dir.Text = (path + "\\").Replace(@"\\", @"\").ToLower();
            }
        }


        #endregion

        #region screenshot paints

        #region form events

        private void btn_close_screen_paints_tool_Click(object sender, EventArgs e)
        {
            panel_screen_paints_tool.Location = new Point(0, 1024);
            bgWorker_ScreenPaintsTool.Dispose();
        }

        // screenshots paint tool "Start" button
        private void btn_start_screen_paints_Click(object sender, EventArgs e)
        {
            miscFunc.create_missing_dir(txtb_screenshots_dir.Text);
            // verify screenshots directory is set and is valid
            if (!Directory.Exists(txtb_screenshots_dir.Text))
            {
                MessageBox.Show("Error: the screenshots directory is not set, define it in the settings.");
                tabControl.SelectedIndex = 1;
                return;
            }

            miscFunc.create_missing_dir(txtb_screenshots_dir.Text);

            if (!cb_paintschart_notice.Checked)
            {
                TFMV.UserControls.Screenshot_Paints_Tool.MessageBox_PaintsChartNotice PaintsChartNotice_dialog = new TFMV.UserControls.Screenshot_Paints_Tool.MessageBox_PaintsChartNotice();
                PaintsChartNotice_dialog.StartPosition = FormStartPosition.CenterParent;
                var result = PaintsChartNotice_dialog.ShowDialog();

                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }


            if (!Directory.Exists(txtb_screenshots_dir.Text))
            {
                MessageBox.Show("Error: the screenshots directory is not set, define it in the settings.");
                tabControl.BeginInvoke((Action)(() => tabControl.SelectedIndex = 1));
                return;
            }


            int paintable_materials = 0;
            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    if (vmt.color_picker.Enabled)
                    {
                        paintable_materials++;
                    }
                }
            }

            if (paintable_materials == 0)
            {
                MessageBox.Show("There are no models to paint or paintable materials.");
                return;
            }

            var hlmv_window_rect = new Rect();
            GetWindowRect(proc_HLMV.MainWindowHandle, ref hlmv_window_rect);

            // get HLMV render area size
            Point hlmv_window_size = new Point(hlmv_window_rect.right - hlmv_window_rect.left - hlmv_padding.left, hlmv_window_rect.bottom - hlmv_window_rect.top - hlmv_padding.bottom);


            #region disable UI panels

            skins_manager_control.Enabled = false;
            panel_loadout.Enabled = false;
            numUpDown_screen_paints_delay.Enabled = false;
            btn_cancel_screen_paints.Visible = true;

            btn_rdo_PlayMat_TeamColor.Enabled = false;
            btn_rdo_PlayMat_Grey.Enabled = false;
            btn_rdo_PaintsChart_mosaic_expt.Enabled = false;
            btn_rdo_PaintsChart_indiv_expt.Enabled = false;

            panel_tools.Enabled = false;
            panel_hlmv_settings.Enabled = false;
            vtab_loadout.Enabled = false;
            btn_close_paints_chart.Enabled = false;

            btn_start_screen_paints.Enabled = false;
            btn_pick_paints.Visible = false;

            lab_warn_refresh.Visible = true;
            lab_warn_refresh1.Visible = true;
            lab_warn_refresh2.Visible = true;
            lab_warn_refresh3.Visible = true;

            #endregion

            int delay = Convert.ToInt16(numUpDown_screen_paints_delay.Value);
            bool mosaic = btn_rdo_PaintsChart_mosaic_expt.Checked ? true : false;

            float scalingFactor = getScalingFactor();

            object[] arrObjects = new object[] { delay, mosaic, hlmv_window_size, scalingFactor };


            if (!bgWorker_ScreenPaintsTool.IsBusy)
            {
                btn_cancel_screen_paints.Visible = true;
                // call background worker
                bgWorker_ScreenPaintsTool.RunWorkerAsync(arrObjects);
            }
        }

        #endregion


        // do work
        protected void bgWorker_ScreenPaintsTool_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginInvoke((Action)(() => lab_PaintsChart_descr.Visible = false));

            #region bg worker setup

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects = (object[])e.Argument;//Collect the array of objects the we recived from the main thread

            int delay = (int)arrObjects[0] * 1000;
            bool mosaic = (bool)arrObjects[1];
            Point hlmv_window_size = (Point)arrObjects[2];
            float dpi_scaling_factor = (float)arrObjects[3];
            StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            #endregion


            #region setup

            int icons_per_row = 10;
            int icons_per_collumn = 4;
            int icons_count = icons_per_row * icons_per_collumn;

            // calculate image resolution for paints mosaic (9x4 images of hlmv_size X by Y)
            Point mosaic_image_resolution = new Point((int)((hlmv_window_size.X * icons_per_row) * dpi_scaling_factor), (int)((hlmv_window_size.Y * icons_per_collumn) * dpi_scaling_factor));
            BitmapProcessing BitmProc = new BitmapProcessing();

            // check mosaic export dimensions do not exceed 15k x 15k pixels
            if (btn_rdo_PaintsChart_mosaic_expt.Checked)
            {
                if ((mosaic_image_resolution.X > 15000) || (mosaic_image_resolution.Y > 15000))
                {
                    MessageBox.Show("The maxmimum image resolution supported for the paints mosaic is 15 000px by 15 000px\n\nPlease scale down the HLMV window size.\n\nFor higher resolutions generate indivudal images instead (untick \"merge screenshots into a mosaic\").");
                    bgWorker_ScreenPaintsTool.CancelAsync();
                    return;
                }
            }

            List<Bitmap> bitmaps = new List<Bitmap>();

            // enable painters
            panel_paints.BeginInvoke((Action)(() => panel_paints.Enabled = false));

            string screenshot_dir = "";
            txtb_screenshots_dir.Invoke(new MethodInvoker(delegate { screenshot_dir = txtb_screenshots_dir.Text; }));
            string image_file_format = ".png";

            // set progress bar size
            progressBar_screen_paints_tool.Invoke(new MethodInvoker(delegate { progressBar_screen_paints_tool.Maximum = icons_count; }));

            #endregion


            #region switch through paints list & take screenshots

            Bitmap preview_moasic_image = new Bitmap(mosaic_image_resolution.X, mosaic_image_resolution.Y, PixelFormat.Format32bppPArgb);
            Graphics graphics_preview = Graphics.FromImage(preview_moasic_image);
            int img_row = 0;
            int img_collumn = 0;

            Bitmap mask = new Bitmap(100, 100, PixelFormat.Format32bppPArgb);


            #region reset paints

            colorPicker_master.BeginInvoke((Action)(() => colorPicker_master.SelectedIndex = 0));

            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    vmt.color_picker.BeginInvoke((Action)(() => vmt.color_picker.SelectedIndex = 0));
                }
            }

            // refresh paints (to set base color) if skin is 0 (RED) since the following condition only refreshes HLMV if its not skin 0
            if (selected_team_skin_index == 0)
            {
                this.BeginInvoke((Action)(() => update_skins(false)));
            }

            #endregion


            #region set player model skins

            // switch skin to RED 0 if its not set to red
            if ((selected_team_skin_index != 0)) //&& (!btn_rdo_PlayMat_Grey.Checked)
            {
                // switch to blu skin
                selected_team_skin_index = 0;
                this.BeginInvoke((Action)(() => switch_team_skin(0)));
                Thread.Sleep(delay); // wait for hlmv refresh
            }

            // switch skin to Grey material if selected
            if (btn_rdo_PlayMat_Grey.Checked)
            {
                this.BeginInvoke((Action)(() => set_player_grey_material()));
                Thread.Sleep(delay); // wait for hlmv refresh
            }

            #endregion


            #region generate transparency alpha mask

            // if transparency enabled
            if (cb_screenshot_transparency.Checked)
            {
                // set black background and capture window
                this.BeginInvoke((Action)(() => write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", "0 0 0")));
                Thread.Sleep(delay); // wait for hlmv refresh
                Bitmap black_bg = screenshot_capture(false, true); // capture screenshot

                // set white background and capture window
                this.BeginInvoke((Action)(() => write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", "255 255 255")));
                Thread.Sleep(delay); // wait for hlmv refresh
                Bitmap white_bg = screenshot_capture(false, true);

                // set black background again
                this.BeginInvoke((Action)(() => write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", "0 0 0")));

                // generate bitmap with alpha mask from black/white background bitmaps
                mask = BitmProc.get_transp_bitmap(black_bg, white_bg);
            }

            #endregion

            #region generate RED & BLU base color
            // generate base paints and team colors (RED & BLU)

            #region RED

            // refresh HLMV window
            PostMessage(proc_HLMV.MainWindowHandle, WM_KEYDOWN, VK_F5, 0);
            SetForegroundWindow(proc_HLMV.MainWindowHandle); // set to foreground so it refreshes
            // wait for HLMV to refresh before we take the screenshot
            Thread.Sleep(delay);

            #region save screenshot & add to bitmaps list

            // mosaic screenshot
            if (btn_rdo_PaintsChart_mosaic_expt.Checked)
            {
                Bitmap capture = screenshot_capture(false, true);

                // copy transparency alpha mask if transparency enabled
                if (cb_screenshot_transparency.Checked)
                {
                    capture = BitmProc.transfer_rgb(capture, mask);
                }

                bitmaps.Add(capture);
            }
            else // single screenshot
            {
                Bitmap capture = screenshot_capture(false, true);

                // copy transparency alpha mask if transparency enabled
                if (cb_screenshot_transparency.Checked)
                {
                    capture = BitmProc.transfer_rgb(capture, mask);
                }

                bitmap_export(capture);
                bitmaps.Add(capture);
            }


            #region generate preview mosaic

            // return to x position = 0 if we completed a row
            // also move to the next line (Y++) img_collumn
            if (img_row >= icons_per_row) { img_row = 0; img_collumn++; }

            // add latest screenshot to (picturebox) grid of images // draw preview
            graphics_preview.DrawImage(bitmaps[bitmaps.Count - 1], bitmaps[bitmaps.Count - 1].Width * img_row, bitmaps[bitmaps.Count - 1].Height * img_collumn);
            pictureBox_screen_paints_preview.Invoke(new MethodInvoker(delegate { pictureBox_screen_paints_preview.Image = preview_moasic_image; }));

            // next row
            img_row++;

            #endregion

            #endregion

            // increment progress bar
            progressBar_screen_paints_tool.Invoke(new MethodInvoker(delegate { progressBar_screen_paints_tool.Value++; }));

            sendingWorker.ReportProgress(1);

            #endregion

            #region BLU

            // switch to blu skin
            selected_team_skin_index = 1;
            colorPicker_master.BeginInvoke((Action)(() => switch_team_skin(1)));


            // set grey player material if checked
            if (btn_rdo_PlayMat_Grey.Checked)
            {
                this.BeginInvoke((Action)(() => set_player_grey_material()));
            }

            // srefresh HLMV window
            PostMessage(proc_HLMV.MainWindowHandle, WM_KEYDOWN, VK_F5, 0);
            SetForegroundWindow(proc_HLMV.MainWindowHandle); // set to foreground so it refreshes
            Thread.Sleep(delay);// wait for HLMV to refresh

            #region save screenshot & add to bitmaps list

            // mosaic screenshot
            if (btn_rdo_PaintsChart_mosaic_expt.Checked)
            {
                Bitmap capture = screenshot_capture(false, true);

                // copy transparency alpha mask if transparency enabled
                if (cb_screenshot_transparency.Checked)
                {
                    capture = BitmProc.transfer_rgb(capture, mask);
                }

                bitmaps.Add(capture);
            }
            else // single screenshot
            {
                Bitmap capture = screenshot_capture(false, true);

                // copy transparency alpha mask if transparency enabled
                if (cb_screenshot_transparency.Checked)
                {
                    capture = BitmProc.transfer_rgb(capture, mask);
                }

                bitmap_export(capture);
                bitmaps.Add(capture);
            }

            //pictureBox_screen_paints_preview
            #region generate preview mosaic

            // return to x position = 0 if we completed a row
            // also move to the next line (Y++)
            if (img_row >= icons_per_row) { img_row = 0; img_collumn++; }

            // draw preview
            graphics_preview.DrawImage(bitmaps[bitmaps.Count - 1], bitmaps[bitmaps.Count - 1].Width * img_row, bitmaps[bitmaps.Count - 1].Height * img_collumn);
            pictureBox_screen_paints_preview.Invoke(new MethodInvoker(delegate { pictureBox_screen_paints_preview.Image = preview_moasic_image; }));

            img_row++;

            #endregion

            #endregion

            // increment progress bar
            progressBar_screen_paints_tool.Invoke(new MethodInvoker(delegate { progressBar_screen_paints_tool.Value++; }));

            sendingWorker.ReportProgress(2);

            #endregion

            #endregion


            // switch to red skin
            // if Grey player material is NOT selected
            if (!btn_rdo_PlayMat_Grey.Checked)
            {
                selected_team_skin_index = 0;
                this.BeginInvoke((Action)(() => switch_team_skin(0)));
                Thread.Sleep(delay);
            }

            #region for each paint

            // for each paint, change selected paint in each vmt_painter of each model_painter  // and take a screenshot 
            // start at 1 to skip the vmt_base color
            // 38 and not 40 because we already captured RED & BLU icons for the basecolor
            // there's 36 paints, +2 empty icons for the bottom row

            int team_paint_index = 22;

            // up to paint number 22 its neutral paints
            // starting 22 we do RED then switch to BLU
            for (int i = 1; i < 37; i++)
            {

                // if user cancelled or HLMV was closed
                if (sendingWorker.CancellationPending || proc_HLMV.HasExited)//At each iteration of the loop, check if there is a cancellation request pending 
                {
                    //  cancel background worker
                    e.Cancel = true;//If a cancellation request is pending,assgine this flag a value of true
                    break;
                }

                #region switch paints

                if (i > 22)
                {
                    // blue paints
                    if ((i == 24) || (i == 26) || (i == 28) || (i == 30) || (i == 32) || (i == 34) || (i == 36))
                    {
                        // switch to blue
                        selected_team_skin_index = 1;
                        this.BeginInvoke((Action)(() => switch_team_skin(1)));

                        // set player grey material
                        if (btn_rdo_PlayMat_Grey.Checked)
                        {
                            this.BeginInvoke((Action)(() => set_player_grey_material()));
                        }
                    }
                    else // red paints  // 25 // 27 // 29 // 
                    {

                        // switch to red
                        selected_team_skin_index = 0;
                        this.BeginInvoke((Action)(() => switch_team_skin(0)));

                        // set player grey material
                        if (btn_rdo_PlayMat_Grey.Checked)
                        {
                            this.BeginInvoke((Action)(() => set_player_grey_material()));
                        }

                        // change to next paint
                        #region update each model painter and VMT paint

                        foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
                        {
                            foreach (var vmt_painter in mp.vmt_Painterslist)
                            {
                                // make sure the color picker has the paints (by checking the max index)
                                if (team_paint_index < vmt_painter.color_picker.Items.Count - 1)
                                {   // set paint
                                    vmt_painter.BeginInvoke((Action)(() => vmt_painter.color_picker.SelectedIndex = team_paint_index));
                                    // update paint
                                    mp.BeginInvoke((Action)(() => mp.update_paints()));
                                }
                            }
                        }

                        #endregion

                        team_paint_index++;
                    }

                }
                else  // switch paints from i=1 to i= 24;
                {     // set paint color (i)

                    #region update each model painter and VMT paint

                    foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
                    {
                        foreach (var vmt_painter in mp.vmt_Painterslist)
                        {
                            // make sure the color picker has the paints (by checking the max index)
                            if (i < vmt_painter.color_picker.Items.Count)
                            {   // set paint
                                vmt_painter.BeginInvoke((Action)(() => vmt_painter.color_picker.SelectedIndex = i));
                                // update paint
                                mp.BeginInvoke((Action)(() => mp.update_paints()));
                            }
                        }
                    }

                    #endregion

                }
                #endregion


                PostMessage(proc_HLMV.MainWindowHandle, WM_KEYDOWN, VK_F5, 0); // refresh HLMV window
                SetForegroundWindow(proc_HLMV.MainWindowHandle);   // set to foreground so it refreshes
                Thread.Sleep(delay); // wait for refresh (user specified delay)


                #region take screenshot & preview


                // mosaic screenshot
                if (btn_rdo_PaintsChart_mosaic_expt.Checked)
                {
                    Bitmap capture = screenshot_capture(false, true);

                    // copy transparency alpha mask if transparency enabled
                    if (cb_screenshot_transparency.Checked)
                    {
                        capture = BitmProc.transfer_rgb(capture, mask);
                    }

                    bitmaps.Add(capture);
                }
                else // single screenshot
                {
                    Bitmap capture = screenshot_capture(false, true);

                    // copy transparency alpha mask if transparency enabled
                    if (cb_screenshot_transparency.Checked)
                    {
                        capture = BitmProc.transfer_rgb(capture, mask);
                    }

                    bitmap_export(capture);
                    bitmaps.Add(capture);
                }

                #endregion


                #region check duplicate bitmaps // if HLMV didn't have time to refresh
                /*
                // check if icon bitmpap is the same as previous
                // if it is, it means HLMV didn't have enough time to refresh and be focused
                // cancel work and warn user to increase delay
                if ((bitmaps.Count == 4))
                {
                    Bitmap img_diff_test_1 = bitmaps[2];
                    Bitmap img_diff_test_2 = bitmaps[3];

                    // scale down so its faster to compare
                    if ((bitmaps[0].Width > 600) || (bitmaps[0].Height > 600))
                    {
                        int rescale_y = img_diff_test_1.Height / (img_diff_test_1.Width / 600);
                        img_diff_test_1 = ResizeImage(img_diff_test_1, 600, rescale_y);
                        img_diff_test_2 = ResizeImage(img_diff_test_2, 600, rescale_y);
                    }

                    int difference_percent = CompareBitmapsDiffPercent(img_diff_test_1, img_diff_test_2);
                    if (difference_percent < 1)
                    {
                        this.BeginInvoke((Action)(() => paints_refresh_failed = true));
                        //e.Result = "refresh_failure";
                        sendingWorker.CancelAsync();
                        e.Cancel = true;

                        return;
                    }
                }
                */
                #endregion

                #region generate preview mosaic

                // add blank space on 4th line for first picture
                if (i == 29)
                {
                    if (img_row >= icons_per_row)
                    {
                        img_row = 1; img_collumn++;
                    }

                    // update preview
                    if (bitmaps[bitmaps.Count - 1] != null)
                    {
                        graphics_preview.DrawImage(bitmaps[bitmaps.Count - 1], bitmaps[bitmaps.Count - 1].Width * img_row, bitmaps[bitmaps.Count - 1].Height * img_collumn);
                        pictureBox_screen_paints_preview.Invoke(new MethodInvoker(delegate { pictureBox_screen_paints_preview.Image = preview_moasic_image; }));
                    }

                    img_row++;
                }
                else
                {
                    if (img_row >= icons_per_row)
                    {
                        img_row = 0; img_collumn++;
                    }

                    // update preview
                    if (bitmaps[bitmaps.Count - 1] != null)
                    {
                        graphics_preview.DrawImage(bitmaps[bitmaps.Count - 1], bitmaps[bitmaps.Count - 1].Width * img_row, bitmaps[bitmaps.Count - 1].Height * img_collumn);
                        pictureBox_screen_paints_preview.Invoke(new MethodInvoker(delegate { pictureBox_screen_paints_preview.Image = preview_moasic_image; }));
                    }

                    img_row++;
                }

                #endregion

                // increment progress bar
                progressBar_screen_paints_tool.Invoke(new MethodInvoker(delegate { progressBar_screen_paints_tool.Value++; }));
                sendingWorker.ReportProgress(i);
            }

            #endregion

            #endregion


            #region save mosaic image

            // merge images
            if (btn_rdo_PaintsChart_mosaic_expt.Checked)
            {
                string format = image_file_format;
                ImageCodecInfo formatEncoder = GetEncoder(ImageFormat.Png);

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                string date = _starttime.AddTicks(_stopwatch.Elapsed.Ticks).ToString().Replace(" ", "_").Replace("/", "_").Replace(":", "");

                // txtb_debug.AppendText("date: " + date);
                miscFunc.create_missing_dir(txtb_screenshots_dir.Text);
                myEncoderParameters.Param[0] = myEncoderParameter;

                preview_moasic_image.Save(screenshot_dir + "\\tfmv_" + date + format, ImageFormat.Png);
            }


            #endregion

            e.Result = sb.ToString();// Send our result to the main thread!
        }



        protected void bgWorker_ScreenPaintsTool_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            // lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        // work completed
        protected void bgWorker_ScreenPaintsTool_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            if (!e.Cancelled && e.Error == null)//Check if the worker has been cancelled or if an error occured
            {
                string result = (string)e.Result;//Get the result from the background thread
                                                 //txtResult.Text = result;//Display the result to the user
                                                 //lblStatus.Text = "Done";
        }
            else if (e.Cancelled)
            {

            }
            else
            {
                // lblStatus.Text = "An error has occured";
            }

            progressBar_screen_paints_tool.Value = 0;
            skins_manager_control.Enabled = true;
            panel_loadout.Enabled = true;
            numUpDown_screen_paints_delay.Enabled = true;

            btn_rdo_PlayMat_TeamColor.Enabled = true;
            btn_rdo_PlayMat_Grey.Enabled = true;
            btn_rdo_PaintsChart_mosaic_expt.Enabled = true;
            btn_rdo_PaintsChart_indiv_expt.Enabled = true;

            btn_cancel_screen_paints.Visible = false;
            btn_start_screen_paints.Enabled = true;
            panel_tools.Enabled = true;
            panel_hlmv_settings.Enabled = true;
            vtab_loadout.Enabled = true;
            btn_close_paints_chart.Enabled = true;

            lab_warn_refresh.Visible = false;
            lab_warn_refresh1.Visible = false;
            lab_warn_refresh2.Visible = false;
            lab_warn_refresh3.Visible = false;
            lab_PaintsChart_descr.Visible = true;


            #region reset paints

            colorPicker_master.SelectedIndex = 0;

            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    vmt.color_picker.SelectedIndex = 0;
                }
            }

            if (selected_team_skin_index != 0)
            {
                selected_team_skin_index = 0;
                switch_team_skin(0);
            }

            #endregion

            if (!e.Cancelled)
            {

                //cheeky hack to make sure the user realises the paint chart is done

                Form mainForm = this.FindForm();

                mainForm.WindowState = FormWindowState.Minimized;
                //mainForm.Show();
                mainForm.WindowState = FormWindowState.Normal;

                System.Windows.Forms.MessageBox.Show("Done!");

                //it used to just play a sound
                //SystemSounds.Exclamation.Play();
            }

        }

        // cancel
        private void btn_cancel_screen_paints_Click(object sender, EventArgs e)
        {
            bgWorker_ScreenPaintsTool.CancelAsync();
        }

        private void btn_pick_paints_Click(object sender, EventArgs e)
        {
            TFMV.UserControls.PickPaints_List PickPaints_dialog = new TFMV.UserControls.PickPaints_List();
            PickPaints_dialog.populate_paints_panel(paints_selection);

            var result = PickPaints_dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                paints_selection = PickPaints_dialog.paints_selection;
                save_paints_selection();
            }
        }


        private void numUpDown_screen_paints_delay_ValueChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);
        }


        private bool CompareBitmaps(Image left, Image right)
        {
            if (object.Equals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (!left.Size.Equals(right.Size) || !left.PixelFormat.Equals(right.PixelFormat))
                return false;

            Bitmap leftBitmap = left as Bitmap;
            Bitmap rightBitmap = right as Bitmap;
            if (leftBitmap == null || rightBitmap == null)
                return true;

            #region Optimized code for performance

            int bytes = left.Width * left.Height * (Image.GetPixelFormatSize(left.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bmd1 = leftBitmap.LockBits(new Rectangle(0, 0, leftBitmap.Width - 1, leftBitmap.Height - 1), ImageLockMode.ReadOnly, leftBitmap.PixelFormat);
            BitmapData bmd2 = rightBitmap.LockBits(new Rectangle(0, 0, rightBitmap.Width - 1, rightBitmap.Height - 1), ImageLockMode.ReadOnly, rightBitmap.PixelFormat);

            Marshal.Copy(bmd1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bmd2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            leftBitmap.UnlockBits(bmd1);
            rightBitmap.UnlockBits(bmd2);

            #endregion

            return result;
        }

        private int CompareBitmapsDiffPercent(Bitmap img1, Bitmap img2)
        {
            if (img1.Size != img2.Size)
            {

                return 0;
            }

            float diff = 0;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    diff += (float)Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R) / 255;
                    diff += (float)Math.Abs(img1.GetPixel(x, y).G - img2.GetPixel(x, y).G) / 255;
                    diff += (float)Math.Abs(img1.GetPixel(x, y).B - img2.GetPixel(x, y).B) / 255;
                }
            }

            return Convert.ToInt32(100 * diff / (img1.Width * img1.Height * 3));

        }


        #endregion


        private void open_tfmv_guide()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string htmlCode = client.DownloadString("http://steamcommunity.com/sharedfiles/filedetails/?id=158547475");
                }
            } catch {
                MessageBox.Show("Could not open web browser to load the TFMV guide.\nPlease check the TFMV guide for new versions of the tool at: https://steamcommunity.com/sharedfiles/filedetails/?id=158547475 ");
            }
        }


        // checks for files in \\Team Fortress 2\\tf\\workshop\\ to see if user is a workshop artist
        // returns true if files found, is workshop user
        private bool check_if_workshopuser()
        {
            string workshop_dir = steamGameConfig.tf_dir + "workshop";
            string workshop_import_dir = steamGameConfig.tf_dir + "workshop\\import_source\\";

            if (Directory.Exists(workshop_dir))
            {
                if (Directory.Exists(workshop_import_dir))
                {
                    if (Directory.EnumerateFileSystemEntries(workshop_import_dir).Any())
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        #region Schema & item loading


        #region items

        #region load schema and items_game into list of Structs.items_game.item

        private void load_schema(bool _updating_schema)
        {
            object[] arrObjects = new object[] { _updating_schema };

            if (!bgWorker_load_schema.IsBusy)
            {
                bgWorker_load_schema.RunWorkerAsync(arrObjects);
            }

        }

        protected void bgWorker_load_schema_DoWork(object sender, DoWorkEventArgs e)
        {
            #region bg worker setup

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects = (object[])e.Argument;//Collect the array of objects the we recived from the main thread

            bool updating_schema = (bool)arrObjects[0];
            StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            #endregion

            // load localization file for real item names into a string list
            item_names_List = new item_names();
            load_localization_strings();

            #region integrity checks   

            // if cached schema exists
            if (check_schema_cache_validity())
            {
                //if cached schema loaded properly, and we aren't updating it, return and exit load_schema;
                if ((schema_load_cache()) && (!updating_schema))
                {
                    return;
                }
            }

            #endregion

            while (bgWorker_download_schema.IsBusy)
            {
                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);

            #region load schemas and strings

            TFMV.VDF_parser vdf_items_game = new TFMV.VDF_parser();
            vdf_items_game.file_path = schema_dir + "items_game.vdf";
            vdf_items_game.load_VDF_file();

            items_game = new List<TF2.items_game.item>();

            #endregion

            List<TFMV.VDF_parser.VDF_node> items_schema_list = new List<TFMV.VDF_parser.VDF_node>();
            List<TFMV.VDF_parser.VDF_node> items_game_list = new List<TFMV.VDF_parser.VDF_node>();
            List<TFMV.VDF_parser.VDF_node> prefabs_list = new List<TFMV.VDF_parser.VDF_node>();

            bool last_schema_part = false;
            int part_num = 0;

            TFMV.VDF_parser vdf_schema;
            string schema_filepath = schema_dir + "schema.vdf";

            while (!last_schema_part)
            {
                // get first schema.vdf
                if (part_num == 0)
                {
                    vdf_schema = new TFMV.VDF_parser();
                    vdf_schema.file_path = schema_filepath;
                    vdf_schema.load_VDF_file();
                } else { // get schema_x.vdf (schema part file)
                    vdf_schema = new TFMV.VDF_parser();
                    schema_filepath = schema_dir + "schema_" + part_num + ".vdf";
                    if (!File.Exists(schema_filepath))
                    {
                        last_schema_part = true;
                        break;
                    }

                    FileInfo f = new FileInfo(schema_filepath);
                    if (f.Length == 0)
                    {
                        last_schema_part = true;
                        break;
                    }


                    vdf_schema.file_path = schema_filepath;
                    vdf_schema.load_VDF_file();
                }


                #region get "items" List<> and "prefabs" from vdf_items_game

                if (vdf_items_game.RootNode.nkey == "items_game")
                {
                    // get items
                    TFMV.VDF_parser.VDF_node node_items_game = vdf_items_game.sGet_Node(vdf_items_game.RootNode, "items");
                    if (node_items_game.nkey == "items")
                    {
                        items_game_list = node_items_game.nSubNodes;
                        // remove first object which is not an item ("default")
                        items_game_list.RemoveAt(0);
                    }

                    // get prefabs
                    prefabs_list = vdf_items_game.sGet_Node(vdf_items_game.RootNode, "prefabs").nSubNodes;
                }

                #endregion

                #region get items List<> from vdf_schema

                // check that schema is valid if the key name is "items_game"
                if (vdf_schema.RootNode.nkey == "result")
                {
                    // find "items" node
                    TFMV.VDF_parser.VDF_node node_items = vdf_schema.sGet_Node(vdf_schema.RootNode, "items");

                    // check if node is "items"
                    if (node_items.nkey == "items")
                    {
                        items_schema_list = node_items.nSubNodes;
                    }
                }

                #endregion


                List<String> type_name_list = new List<string>();

                #region progress / status info

                // disable UI
                this.BeginInvoke((Action)(() => this.Enabled = false));

                lab_status.BeginInvoke((Action)(() => lab_status.Visible = true));
                lab_status.BeginInvoke((Action)(() => lab_status.Text = "Loading schema items Part[" + part_num.ToString() + "]"));

                progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = 0));
                progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Maximum = items_schema_list.Count));
                progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Visible = true));

                #endregion

                #region for each item in items_schema (from schema.vdf) and items_game

                //keep track of what medal models have already been seen //todo: make this obsolete
                List<string> seen_medal_models = new List<string>();


                // for each item in items_schema
                for (int i = 0; i < items_schema_list.Count; i++)
                {
                    if (!sendingWorker.CancellationPending)//At each iteration of the loop, check if there is a cancellation request pending 
                    {
                        TFMV.VDF_parser.VDF_node node = items_schema_list[i];
                        TF2.items_game.item item = new TF2.items_game.item();
                        item = get_item_info(node, item, "item_schema");

                        // skip "Upgradable" items (generally double stock weapons)
                        if (item.Name_str.Contains("Upgradeable")) { continue; }


                        //neodement: compiled some items hidden inside bodygroups as their own separate mdls. they are added as model paths here.

                        //add fake bodygroup for GENTLE MANNE'S SERVICE MEDAL
                        if (item.Name_str == "Web Easteregg Medal")
                        {
                            item.model_path = "models\\TFMV_bodygroups\\soldier_medal_bodygroup.mdl";
                        }

                        //add fake bodygroup for GUNSLINGER
                        if (item.Name_str == "The Gunslinger")
                        {
                            item.model_path = "models\\TFMV_bodygroups\\engineer_gunslinger_bodygroup.mdl";
                        }

                        //manually disable arm bodygroup for SHORT CIRCUIT
                        if (item.Name_str == "The Short Circuit")
                        {
                            item.visuals.player_bodygroups = new List<TF2.items_game.player_bodygroup>();

                            item.visuals.player_bodygroups.Add(new TF2.items_game.player_bodygroup("rightarm", "1"));
                        }

                        //add fake bodygroup for PURITY FIST (hand)
                        if (item.Name_str == "The Purity Fist")
                        {
                            if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }

                            item.extra_wearable.mdl_path = "models\\TFMV_bodygroups\\heavy_purityfist_bodygroup.mdl";

                            item.visuals.player_bodygroups = new List<TF2.items_game.player_bodygroup>();

                            item.visuals.player_bodygroups.Add(new TF2.items_game.player_bodygroup("hands", "1"));
                        }

                        //neodement: added extra fake bodygroups for the huntsman arrows and sydney sleeper darts
                        if (item.item_class == "tf_weapon_compound_bow")
                        {
                            if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }

                            item.extra_wearable.mdl_path = "models\\TFMV_bodygroups\\sniper_arrows_bodygroup.mdl";
                        }

                        if (item.Name_str == "The Sydney Sleeper")
                        {
                            if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }

                            item.extra_wearable.mdl_path = "models\\TFMV_bodygroups\\sniper_darts_bodygroup.mdl";
                        }



                        string defindex = vdf_schema.sGet_KeyVal(node, "defindex");


                        #region get item data from  items_game_list by defindex

                        // get item by defindex
                        TFMV.VDF_parser.VDF_node item_game = new TFMV.VDF_parser.VDF_node();

                        foreach (var item_game_node in items_game_list)
                        {
                            if (item_game_node.nkey == defindex)
                            {
                                item_game = item_game_node;
                            }
                        }

                        // if item is null go to next item
                        if (item_game == null) { continue; }

                        item = get_item_info(item_game, item, "item_game");

                        // if item is not mean to be displayed in the inventory, skip to next
                        // checking this again, since show_in_armory might be loaded from the prefab
                        if (item.show_in_armory == "0") { continue; }

                        #region get prefab data

                        // get prefab name
                        string prefab_name = vdf_schema.sGet_KeyVal(item_game, "prefab");

                        // sometimes there can be multiple prefabs, separated by a white space
                        string[] prefab_namess = prefab_name.Split(' ');

                        // check if there's multiple prefabs defined
                        if (prefab_namess.Length > 1)
                        {
                            // only get prefab name that starts by weapon_
                            foreach (var name in prefab_namess)
                            {
                                if (name.StartsWith("weapon_"))
                                    prefab_name = name;
                                if ((name == "hat") || (name == "base_hat"))
                                    prefab_name = name;
                                if (name == "cosmetic")
                                    prefab_name = name;
                                if (name == "misc")
                                    prefab_name = name;
                                if (name == "hat_decoration")
                                    prefab_name = name;
                                if (name == "mask")
                                    prefab_name = name;
                                if (name == "beard")
                                    prefab_name = name;
                                if (name == "backpack")
                                    prefab_name = name;
                                if (name == "grenades")
                                    prefab_name = name;
                                if (name == "tournament_medal")
                                    prefab_name = name;
                                if (name == "pyrovision_goggles")
                                    prefab_name = name;
                                if (name == "triad_trinket")
                                    prefab_name = name;
                                if (name == "champ_stamp")
                                    prefab_name = name;
                                if (name == "marxman")
                                    prefab_name = name;
                                if (name == "cannonball")
                                    prefab_name = name;

                                //neodement: todo: let's download taunts too
                                if (name == "taunt")
                                    prefab_name = name;
                            }
                        }


                        // if item has a prefab get the info and replace values by prefab ones (with  get_item_info(prefab, item, "item_prefab");)
                        if (prefab_name != "")
                        {
                            TFMV.VDF_parser.VDF_node prefab = new TFMV.VDF_parser.VDF_node();

                            #region search prefab by name in prefabs_list

                            foreach (var prefab_item in prefabs_list)
                            {
                                if (prefab_item.nkey == prefab_name)
                                {
                                    prefab = prefab_item;
                                }
                            }

                            #endregion

                            // load item data with prefab's data
                            if (prefab.nSubNodes.Count > 0)
                            {
                                item = get_item_info(prefab, item, "item_prefab");
                            }
                        }

                        #endregion
                        #endregion

                        // get real item name
                        string real_name = get_item_name_string(item.item_name_var);
                        if (real_name != "") { item.item_name_var = real_name; }
                        if (banned_item(real_name)) { continue; }

                        #region skip weapon skins

                        // skip weapon skin (that HLMV can't display anyways)
                        if (item.prefab != null)
                        {
                            if (item.prefab.Contains("paintkit_weapon"))
                            {
                                continue;
                            }
                        }

                        #endregion


                        bool valid_model = false;

                        // if item has no model defined, used extra_wearable as model, for instance hte item "The B.A.S.E. Jumper" has no model defined, uses extra_wearable as model
                        if ((item.model_path == "") && (item.extra_wearable != null) && (item.extra_wearable.mdl_path != ""))
                        {
                            // item.model = item.extra_wearable.mdl_path;
                            valid_model = true;

                        }

                        // if it has no model defined, check model styles
                        if (item.model_path == "" && !valid_model)
                        {

                            if (item.visuals != null)
                            {
                                if (item.visuals.styles != null)
                                {
                                    if (item.visuals.styles.Count > 0)
                                    {
                                        // ready each style and look for model styles
                                        foreach (var style in item.visuals.styles)
                                        {
                                            if (style.model_player != null)
                                            {
                                                if (style.model_player != "")
                                                {
                                                    valid_model = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if ((item.model_path != "")) { valid_model = true; }


                        #region add item to item list

                        // add item only if it has a model or at least one all class model definition
                        if ((valid_model) || (item.model_player_per_class.Count >= 1))
                        {
                            //neodement: allowed some medals to load
                            if (item.equip_rgn == "medal") // && (item.item_type_name == "#TF_Wearable_Badge")
                            {
                                //only add medals that are set to "show in armory". this prevents the list getting completely spammed up with tournament medals.
                                if (item.show_in_armory == "1")
                                {

                                    //todo: proper fix for these disabled medals? the styles currently don't work.
                                    if (item.model_path != "models/player/items/all_class/id_badge.mdl" && item.model_path != "models/player/items/all_class/dueling_medal.mdl")
                                    {
                                        items_game.Add(item); // add to items list
                                    }
                                }
                                //for tournament medals, change the item slot to "medal" instead of "misc" so the medal button can see them.
                                else
                                {
                                    //only download tournament medals if you were told to.
                                    if (cb_allow_tournament_medals.Checked == true)
                                    {
                                        //if we already have a medal using this model, don't add it to the list.
                                        if (!seen_medal_models.Contains(item.model_path))
                                        {
                                            seen_medal_models.Add(item.model_path);
                                            item.item_slot_schema = "medal";
                                            items_game.Add(item);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                items_game.Add(item); // add to items list
                            }
                        }

                        #endregion

                        // update progress bar
                        progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = i));
                        sendingWorker.ReportProgress(i);
                    }
                    else
                    {  // cancel background worker
                        e.Cancel = true;
                        break;
                    }

                }

                #endregion

                part_num++;

            }
            e.Result = sb.ToString();// Send our result to the main thread!


            download_icons(sender, e);

            // serializes items_game and saves them as binary files
            schema_save_cache();

        }

        protected void bgWorker_load_schema_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // show the progress to the user based on the input we got from the background worker
            // lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void bgWorker_load_schema_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)//Check if the worker has been cancelled or if an error occured
            {

            }
            else if (e.Cancelled)
            {

            }
            else
            {

            }

            string result = (string)e.Result;//Get the result from the background thread


            this.BeginInvoke((Action)(() => progressBar_dl.Value = 0));
            this.BeginInvoke((Action)(() => progressBar_dl.Visible = false));
            this.BeginInvoke((Action)(() => lab_status.Visible = false));

            this.BeginInvoke((Action)(() => this.Enabled = true));

            //set_class("scout", false);
            //set_slot("primary");
            //set_slot("misc");


            // set class and slot and load items
            //set_class("scout", false);

            set_class(lstStartupTab_Class.Text.ToLower(), true);
            set_class(lstStartupTab_Slot.Text.ToLower(), true);


        }

        private bool check_schema_version(bool promptUser)
        {
            #region download and get items_game URL
            string schemaURL_path_latest = schema_dir + "items_game_URL_latest.txt";
            string items_game_URL_latest = "";

            string schemaURL_path = schema_dir + "items_game_URL.txt";
            string items_game_URL = "";

            // get latest schema version number
            try
            {
                using (WebClient Client = new WebClient())
                {

                    Client.DownloadFile("http://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key=" + steam_api_key + "&format=vdf", schemaURL_path_latest);
                    Client.Dispose();
                }
            }
            catch (WebException e)
            {
                // failed to download

                //slightly different error messages depending if the user is already overriding the default key or not
                if (steam_api_key != internal_steam_api_key)
                {
                    MessageBox.Show("Failed to get items_game URL. The API Key may be invalid.\n\nYou can set an API Key at the bottom of the settings tab.", "Error"); // + e.Message
                }
                else
                {
                MessageBox.Show("Failed to get items_game URL. The built-in API Key may have expired.\n\nYou can set your own API Key at the bottom of the settings tab.", "Error"); // + e.Message
                }

                //dont jump to the tab unless the schema is done loading.
                if (!bgWorker_load_schema.IsBusy)
                {
                    panel_APIKey.Visible = true;

                    tabControl.SelectedIndex = 1;

                    txtb_API_Key.Focus();
                    txtb_API_Key.SelectionStart = txtb_API_Key.TextLength;
                    txtb_API_Key.SelectionLength = 0;
                }

                return false;
            }

            // check that file exists
            if ((File.Exists(schemaURL_path_latest)) && (File.Exists(schemaURL_path)))
            {
                FileInfo f = new FileInfo(schemaURL_path_latest);
                if (f.Length == 0)
                {
                    miscFunc.delete_safe(schemaURL_path_latest);
                    //return;
                }

                TFMV.VDF_parser parser = new TFMV.VDF_parser();
                parser.file_path = schemaURL_path_latest;
                parser.load_VDF_file();
                items_game_URL_latest = parser.RootNode.nSubNodes[1].nvalue;


                f = new FileInfo(schemaURL_path);
                if (f.Length == 0)
                {
                    miscFunc.delete_safe(schemaURL_path);
                    //return;
                }

                parser = new TFMV.VDF_parser();
                parser.file_path = schemaURL_path;
                parser.load_VDF_file();
                items_game_URL = parser.RootNode.nSubNodes[1].nvalue;

                //only ask this if promptUser is set to true
                if (items_game_URL_latest != items_game_URL && promptUser)
                {
                    DialogResult dialogResult = MessageBox.Show("A new version of the schema (item list) is available.\nDo you want to download it now?", "Schema update", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        download_schemas();
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
            }

            #endregion

            return true;
        }


        // loops through an item node's subdnodes
        // gets the item information
        // returns instance of Structs.items_game.item 
        // types: item, prefab
        private TF2.items_game.item get_item_info(TFMV.VDF_parser.VDF_node item_node, TF2.items_game.item item, string type)
        {
            TFMV.VDF_parser.VDF_node visuals = new TFMV.VDF_parser.VDF_node();
            TFMV.VDF_parser.VDF_node used_by_classes = new TFMV.VDF_parser.VDF_node();
            TFMV.VDF_parser.VDF_node model_player_per_class = new TFMV.VDF_parser.VDF_node();

            bool paintability_exists = false;

            foreach (var node in item_node.nSubNodes)
            {
                // todo: why does this loop too many times and the styles get filled 3 times instead of 1?
                string name = node.nkey;
                string value = node.nvalue;

                #region switch search values

                switch (name)
                {
                    case "image_url":
                        item.icon_url = value; continue;

                    case "item_class":
                        item.item_class = value; continue;

                    case "item_name":
                        if (type != "item_prefab") { item.item_name_var = value; } continue;

                    case "item_type_name":
                        if (type != "item_prefab") { item.item_type_name = value; } continue;

                    case "name":
                        if (type != "item_prefab") { item.Name_str = value; } continue;

                    case "model_player":
                        if (item.model_path == null) { item.model_path = value; } continue;

                    case "model_world":
                        item.model_path = value;
                        continue;

                    case "extra_wearable":
                        if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }
                        item.extra_wearable.mdl_path = value;
                        continue;

                    case "item_slot":
                        // item_slot from schema.vdf defines the general slot (misc, primary, secondary, melee etc)
                        if (type == "item_schema") { item.item_slot_schema = value; }

                        // item slot from items_game.vdf defines the equip region
                        if ((item.item_slot == null) || (item.item_slot == ""))
                        {
                            item.item_slot = value;
                        }
                        continue;

                    case "equip_region":
                        item.equip_rgn = value; continue;

                    case "anim_slot":
                        if ((item.anim_slot == null) || (item.anim_slot == ""))
                        {
                            item.anim_slot = value;
                        }
                        continue;

                    case "capabilities":

                        if (node.nSubNodes.Count > 0)
                        {
                            foreach (TFMV.VDF_parser.VDF_node capability in node.nSubNodes)
                            {
                                if (capability.nkey == "paintable")
                                {
                                    paintability_exists = true;

                                    if (capability.nvalue == "0")
                                    {
                                        item.not_paintable = true;
                                        break;
                                    }
                                }
                            }
                        }
                        continue;

                    case "prefab":
                        item.prefab = value; continue;

                    case "show_in_armory":
                        item.show_in_armory = value; continue;

                    // we don't "continue" to the next item on these next cases
                    // since we still need to get extra data after switch

                    // get sub nodes
                    case "used_by_classes":
                        used_by_classes = node;

                        #region used by class

                        if (used_by_classes.nSubNodes.Count > 0)
                        {
                            item.used_by_classes = new List<TF2.items_game.used_by_class>();

                            foreach (TFMV.VDF_parser.VDF_node used_by in used_by_classes.nSubNodes)
                            {
                                if ((type == "item_prefab") || (type == "item_game"))
                                {
                                    item.used_by_class_ADD(used_by.nkey, used_by.nvalue.ToLower());
                                }
                                else if (type == "item_schema")
                                {
                                    item.used_by_class_ADD(used_by.nvalue.ToLower(), "");
                                }

                            }
                        }

                        #endregion

                        continue;

                    // get sub nodes for all class models
                    case "model_player_per_class":
                        // model per class (for all class items)
                        if (node.nSubNodes.Count > 0)
                        {
                            foreach (var player_model in node.nSubNodes)
                            {
                                item.models_allclass_ADD(player_model.nkey, player_model.nvalue);
                            }
                        }

                        continue;

                    //todo: inside this visuals block is probably where the skin fix for medals goes. and also australium weapons.

                    case "visuals":
                        if (type == "item_game") visuals = node;
                        if (type == "item_prefab") visuals = node;

                        #region visuals

                        if (visuals.nSubNodes.Count > 0) // && (node.Value == "visuals"))
                        {
                            foreach (var vis_item in visuals.nSubNodes)
                            {


                                #region player bodygroups

                                if (vis_item.nkey == "player_bodygroups")
                                {
                                    // if item has no bodygroups, create new list
                                    if (item.visuals.player_bodygroups == null) { item.visuals.player_bodygroups = new List<TF2.items_game.player_bodygroup>(); }

                                    // item.visuals.player_bodygroups = new List<TF2.items_game.player_bodygroup>();
                                    foreach (var bodygroup in vis_item.nSubNodes)
                                    {



                                        // if item visuals doesn't have this bodygroup, add it
                                        if (!item.visuals.player_bodygroups.Contains(new TF2.items_game.player_bodygroup(bodygroup.nkey, bodygroup.nvalue)))
                                        {
                                            item.visuals.player_bodygroups.Add(new TF2.items_game.player_bodygroup(bodygroup.nkey, bodygroup.nvalue));
                                        }


                                    }
                                    continue;
                                }


                                #endregion

                                #region attached models

                                // (i.e. festive weapons, australium, pyro's backburner flamethrower)
                                if (vis_item.nkey == "attached_models")
                                {
                                    foreach (var attachements in vis_item.nSubNodes)
                                    {
                                        string attach_model = ""; byte attach_flag = 1;
                                        foreach (var attachement in attachements.nSubNodes)
                                        {
                                            // item.visuals.attached_models.Add(new Structs.items_game.attached_model(attachement.Key, attachement.Value));

                                            if (attachement.nkey == "model") { attach_model = attachement.nvalue; }
                                            if (attachement.nkey == "model_display_flags") { attach_flag = Convert.ToByte(attachement.nvalue); }
                                        }

                                        // add model and or model_display_flags if we found them
                                        if ((attach_model != ""))
                                        {
                                            item.visuals.attached_models.Add(new TF2.items_game.attached_model(attach_flag, attach_model));
                                        }
                                    }

                                    continue;
                                }

                                #endregion

                                #region skin styles

                                if (vis_item.nkey == "styles")
                                {
                                    TF2.items_game.style style_tmp;

                                    // for each style
                                    foreach (var styles in vis_item.nSubNodes)
                                    {
                                        style_tmp = new TF2.items_game.style();

                                        // for each subnode in a style
                                        foreach (var style_ in styles.nSubNodes)
                                        {
                                            string sv = style_.nvalue;


                                            switch (style_.nkey)
                                            {
                                                case "skin":
                                                    style_tmp.skin = Convert.ToByte(sv); break;
                                                case "name":
                                                    style_tmp.name = sv; continue;
                                                case "skin_red":
                                                    style_tmp.skin_red = sv; continue;
                                                case "skin_blu":
                                                    style_tmp.skin_blu = sv; continue;
                                                case "model_player":
                                                    style_tmp.model_player = sv; continue;
                                                case "additional_hidden_bodygroups":
                                                    style_tmp.additional_hidden_bodygroups = new List<TF2.items_game.style_hidden_bodygroup>();
                                                    foreach (var shb in style_.nSubNodes)
                                                    {
                                                        style_tmp.additional_hidden_bodygroups.Add(new TF2.items_game.style_hidden_bodygroup(shb.nkey, Convert.ToByte(shb.nvalue)));
                                                    }
                                                    continue;
                                                //model_player_per_class
                                                case "model_player_per_class":
                                                    style_tmp.model_player_per_class_list = new List<TF2.items_game.model_player_per_class>();
                                                    foreach (var shb in style_.nSubNodes)
                                                    {
                                                        style_tmp.model_player_per_class_list.Add(new TF2.items_game.model_player_per_class(shb.nkey.ToLower(), shb.nvalue));
                                                    }
                                                    continue;
                                            }
                                        }

                                        // add style to item's visuals styles list
                                        item.visuals.styles.Add(style_tmp);
                                    }

                                    #endregion

                                }
                            }
                        }

                        #endregion

                        continue;

                    //neodement: get info from taunts
                    case "taunt":
                        //if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }
                        if (node.nSubNodes.Count > 0)
                        {
                            foreach (var obj in node.nSubNodes)
                            {


                                // neodement: get sub nodes for TAUNT PROPS
                                if (obj.nkey == "custom_taunt_prop_per_class")
                                    // model per class (for all class items)
                                    if (obj.nSubNodes.Count > 0)
                                    {
                                        foreach (var player_model in obj.nSubNodes)
                                        {
                                            item.models_allclass_ADD(player_model.nkey, player_model.nvalue);
                                        }
                                    }
                            }

                        }
                        continue;


                    //neodement: stop medals with paint colour overrides until this is implemented.
                    case "attributes":
                        if (node.nSubNodes.Count > 0)
                        {
                            foreach (var obj in node.nSubNodes)
                            {


                                // neodement: get sub nodes for SET ITEM TINT RGB
                                if (obj.nkey == "set item tint RGB")
                                    // loop through all attributes
                                    if (obj.nSubNodes.Count > 0)
                                    {
                                        foreach (var paintcolor in obj.nSubNodes)
                                        {
                                            item.model_player_per_class.Clear();
                                            item.model_path = "";
                                        }
                                    }
                            }

                        }
                        continue;


                    case "visuals_red":
                        if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }
                        if (node.nSubNodes.Count > 0)
                        {
                            item.extra_wearable.skin_override = true;
                            foreach (var obj in node.nSubNodes)
                            {
                                if (obj.nkey == "skin")
                                {
                                    item.extra_wearable.skin_red = Convert.ToByte(obj.nvalue);
                                }

                                if (obj.nkey == "attached_models") // attached models for visuals_red.attache_models (see item: The Kritzkrieg)
                                {
                                    foreach (var attached_mdl in obj.nSubNodes)
                                    {
                                        if (item.visuals_red_attached_models == null) { item.visuals_red_attached_models = new List<string>(); }
                                        item.visuals_red_attached_models.Add(attached_mdl.nSubNodes[0].nvalue);
                                    }
                                }
                            }
                        }
                        continue;
                    case "visuals_blu":
                        if (item.extra_wearable == null) { item.extra_wearable = new TF2.items_game.extra_wearable(); }
                        if (node.nSubNodes.Count > 0)
                        {
                            item.extra_wearable.skin_override = true;
                            foreach (var obj in node.nSubNodes)
                            {
                                if (obj.nkey == "skin")
                                {
                                    item.extra_wearable.skin_blu = Convert.ToByte(obj.nvalue);
                                }
                            }
                        }
                        continue;
                }

                #endregion

            }

            if ((!paintability_exists) && (type == "item_game")) { item.not_paintable = true; }

            return item;
        }

        // returns true if the item should be ignored, and not added to the list
        private Boolean banned_item(string item_name)
        {
            Boolean banned = false;

            for (int i = 0; i < banned_items.Count; i++)
            {
                if ((item_name.ToLower()).Contains(banned_items[i].ToString().ToLower()))
                {
                    banned = true;
                }
            }

            return banned;
        }



        // serializes items_game and saves as binary files
        private void schema_save_cache()
        {
            TFMV.Functions.Serializer.WriteToBinaryFile(app_data_dir + "tf2_schema\\items_game.bin", items_game);
        }

        // loads serialized files as items_game and items_badges
        private bool schema_load_cache()
        {
            try
            {
                if (miscFunc.IsFileLocked(app_data_dir + "tf2_schema\\items_game.bin")) { MessageBox.Show("TFMV cannot access items_game.bin, the file is locked by another process.\n\nPlease try closing applications that may be using this file:\n " + app_data_dir + "tf2_schema\\items_game.bin"); return false; }

                items_game = TFMV.Functions.Serializer.ReadFromBinaryFile<List<TF2.items_game.item>>(app_data_dir + "tf2_schema\\items_game.bin");

                // if cached items array is empty, re cache schema
                if (items_game.Count == 0) //  || (items_badges.Count == 0)
                {
                    miscFunc.delete_safe(app_data_dir + "tf2_schema\\items_game.bin");
                    schema_save_cache();
                }

            }
            catch // if failed to load delete files and return false
            {
                miscFunc.delete_safe(app_data_dir + "tf2_schema\\items_game.bin");
                return false;
            }

            if (cb_sort_order.Checked)
            {
                items_game.Reverse();
            }

            return true;
        }



        private void download_schemas()
        {
            //if a schema version check succeeds, carry on with the work
            if (check_schema_version(false))
            {

                this.BeginInvoke((Action)(() => progressBar_dl.Visible = true));
                this.BeginInvoke((Action)(() => progressBar_dl.Value = 0));
                this.BeginInvoke((Action)(() => lab_status.Visible = true));

                object[] arrObjects = new object[] { null };

                    if (!bgWorker_load_schema.IsBusy)
                    {


                        // call background worker
                        bgWorker_download_schema.RunWorkerAsync(arrObjects);
                    }
            }
        }

        //todo: if this has an error it never recovers!

        // do work
        protected void bgWorker_download_schema_DoWork(object sender, DoWorkEventArgs eventarg)
        {
            #region bg worker setup

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;
            StringBuilder sb = new StringBuilder();

            #endregion

            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Visible = true));
            lab_status.BeginInvoke((Action)(() => lab_status.Visible = true));
            lab_status.BeginInvoke((Action)(() => lab_status.Text = "Downloading: items_game_URL.txt"));
            this.BeginInvoke((Action)(() => this.Enabled = false));


            #region download and get items_game URL

            string schemaURL_path = schema_dir + "items_game_URL.txt";

            try
            {
                using (WebClient Client = new WebClient())
                {
                    Client.UseDefaultCredentials = true;
                    Client.DownloadFile("http://api.steampowered.com/IEconItems_440/GetSchemaURL/v1/?key=" + steam_api_key + "&format=vdf", schemaURL_path);
                }
            }
            catch (WebException e)
            {
                if (e.Message.Contains("Internal Server Error"))
                {
                    MessageBox.Show("Could not download schema, the item server did not respond, \nserver might be down/offline, please try again in a few minutes.", "Error");
                }
                else
                {
                
                // failed to download

                //slightly different error messages depending if the user is already overriding the default key or not
                if (steam_api_key != internal_steam_api_key)
                {
                    MessageBox.Show("Failed to get items_game URL. The API Key may be invalid.\n\nYou can set an API Key at the bottom of the settings tab.", "Error"); // + e.Message
                }
                else
                {
                    MessageBox.Show("Failed to get items_game URL. The built-in API Key may have expired.\n\nYou can set your own API Key at the bottom of the settings tab.", "Error"); // + e.Message
                }

                //set controls on main thread...
                tabControl.BeginInvoke((Action)(() => tabControl.SelectedIndex = 1));
                txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.Focus()));
                txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionStart = txtb_API_Key.TextLength));
                txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionLength = 0));

                panel_APIKey.BeginInvoke((Action)(() => panel_APIKey.Visible = true));

                bgWorker_download_schema.CancelAsync();
                

                return;

                }

                return;
            }

            #endregion


            TFMV.VDF_parser parser = new TFMV.VDF_parser();
            parser.file_path = schemaURL_path;
            parser.load_VDF_file();
            items_game_URL = parser.RootNode.nSubNodes[1].nvalue;


            #region download items_game.vdf

            lab_status.BeginInvoke((Action)(() => lab_status.Text = "Downloading: items_game.vdf"));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = 0));

            string items_game_path = schema_dir + "items_game.vdf";

            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Minimum = 0));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Maximum = 100));

            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += (s, e) =>
            {

            };

            webClient.DownloadFileCompleted += (s, e) =>
            {
                StreamReader streamReader = new StreamReader(items_game_path);

                string response = streamReader.ReadToEnd();
                streamReader.Close();
            };

            webClient.DownloadFileAsync(new Uri(items_game_URL), items_game_path);

            #endregion


            #region get simple schema from which we can get the web paths for icons

            lab_status.BeginInvoke((Action)(() => lab_status.Text = "Downloading: schema.vdf"));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = 0));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Minimum = 0));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Maximum = 100));

            try
            {
                using (WebClient Client = new WebClient())
                {
                    Client.UseDefaultCredentials = true;
                    Client.DownloadFile(new Uri("http://api.steampowered.com/IEconItems_440/GetSchemaItems/v1/?key=" + steam_api_key + "&format=vdf"), schema_dir + "schema.vdf");
                }
            }
            catch (WebException e)
            {
                if (e.Message.Contains("Internal Server Error"))
                {
                    MessageBox.Show("Could not download schema.vdf, the item server did not respond, \nserver might be down/offline, please try again in a few minutes.", "Error");
                }
                else
                {
                    // failed to download

                    //slightly different error messages depending if the user is already overriding the default key or not
                    if (steam_api_key != internal_steam_api_key)
                    {
                        MessageBox.Show("Failed to get items_game URL. The API Key may be invalid.\n\nYou can set an API Key at the bottom of the settings tab.", "Error"); // + e.Message
                    }
                    else
                    {
                        MessageBox.Show("Failed to get items_game URL. The built-in API Key may have expired.\n\nYou can set your own API Key at the bottom of the settings tab.", "Error"); // + e.Message
                    }

                    //set controls on main thread...
                    tabControl.BeginInvoke((Action)(() => tabControl.SelectedIndex = 1));
                    txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.Focus()));
                    txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionStart = txtb_API_Key.TextLength));
                    txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionLength = 0));

                    panel_APIKey.BeginInvoke((Action)(() => panel_APIKey.Visible = true));

                    bgWorker_download_schema.CancelAsync();
                }
                return;
            }

            bool end_file = false;

            int file_num = 1;

            parser = new TFMV.VDF_parser();
            parser.file_path = schema_dir + "schema.vdf";
            parser.load_VDF_file();
            string next_val = parser.RootNode.nSubNodes[3].nvalue;

            while (!end_file)
            {
                try
                {
                    using (WebClient Client = new WebClient())
                    {
                        Client.UseDefaultCredentials = true;
                        Client.DownloadFile(new Uri("http://api.steampowered.com/IEconItems_440/GetSchemaItems/v1/?key=" + steam_api_key + "&start=" + next_val + "&format=vdf"), schema_dir + "schema_" + file_num.ToString() + ".vdf");
                    }
                }
                catch (WebException e)
                {
                    if (e.Message.Contains("Internal Server Error"))
                    {
                        MessageBox.Show("Could not download schema.vdf, the item server did not respond, \nserver might be down/offline, please try again in a few minutes.", "Error");
                    }
                    else
                    {

                        // failed to download

                        //slightly different error messages depending if the user is already overriding the default key or not
                        if (steam_api_key != internal_steam_api_key)
                        {
                            MessageBox.Show("Failed to get items_game URL. The API Key may be invalid.\n\nYou can set an API Key at the bottom of the settings tab.", "Error"); // + e.Message
                        }
                        else
                        {
                            MessageBox.Show("Failed to get items_game URL. The built-in API Key may have expired.\n\nYou can set your own API Key at the bottom of the settings tab.", "Error"); // + e.Message
                        }

                        //set controls on main thread...
                        tabControl.BeginInvoke((Action)(() => tabControl.SelectedIndex = 1));
                        txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.Focus()));
                        txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionStart = txtb_API_Key.TextLength));
                        txtb_API_Key.BeginInvoke((Action)(() => txtb_API_Key.SelectionLength = 0));

                        panel_APIKey.BeginInvoke((Action)(() => panel_APIKey.Visible = true));

                        bgWorker_download_schema.CancelAsync();

                    }
                    return;
                }

                parser = new TFMV.VDF_parser();
                parser.file_path = schema_dir + "schema_" + file_num.ToString() + ".vdf";
                parser.load_VDF_file();
                // last part doesn't have subnode "next" count
                if (parser.RootNode.nSubNodes.Count == 3)
                {
                    end_file = true;
                    break;
                }

                next_val = parser.RootNode.nSubNodes[3].nvalue;
                file_num++;
            }

            #endregion

            eventarg.Result = sb.ToString();// Send our result to the main thread!
        }

        protected void bgWorker_download_schema_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Show the progress to the user based on the input we got from the background worker
            // lblStatus.Text = string.Format("Counting number: {0}...", e.ProgressPercentage);
        }

        protected void bgWorker_download_schema_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;
            }
            else if (e.Cancelled)
            {

            }
            else
            {
                // lblStatus.Text = "An error has occured";
            }
            #region parse schema files

            string schemaURL_path = schema_dir + "items_game_URL.txt";


            // check that file exists
            if (File.Exists(schemaURL_path))
            {
                TFMV.VDF_parser parser = new TFMV.VDF_parser();
                parser.file_path = schemaURL_path;
                parser.load_VDF_file();
                items_game_URL = parser.RootNode.nSubNodes[1].nvalue;
            }
            else
            {
                MessageBox.Show("\n Failed to get items_game URL.");
                return;
            }
            #endregion

            progressBar_dl.BeginInvoke((Action)(() => load_schema(true)));

        }


        private bool check_schema_validity()
        {
            FileInfo f = new FileInfo(schema_dir + "schema.vdf");
            // check that the file is there
            if (!(File.Exists(schema_dir + "schema.vdf")) || (f.Length == 0))
            {
                // MessageBox.Show("Could not load the items list.");
                miscFunc.delete_safe(app_data_dir + "tf2_schema\\schema.vdf");
                miscFunc.delete_safe(app_data_dir + "tf2_schema\\items_game.vdf");
                return false;
            }

            FileInfo fi = new FileInfo(schema_dir + "items_game.vdf");
            // check that the file is there
            if (!(File.Exists(schema_dir + "schema.vdf")) || (fi.Length == 0))
            {
                //  MessageBox.Show("Could not load the items list");
                miscFunc.delete_safe(app_data_dir + "tf2_schema\\schema.vdf");
                miscFunc.delete_safe(app_data_dir + "tf2_schema\\items_game.vdf");
                return false;
            }

            return true;
        }

        private bool check_schema_cache_validity()
        {
            string filepath = app_data_dir + "tf2_schema\\items_game.bin";

            // check if file doesn't exists
            if (!(File.Exists(filepath)))
            {
                return false;
            }

            // check if file empty
            FileInfo f = new FileInfo(filepath);
            if (f.Length == 0)
            {
                miscFunc.delete_safe(filepath);
                return false;
            }

            // check schema cache version is compatible
            // read first 120 bytes of schema cache file as string
            // string version_string;
            byte[] buffer = new Byte[80];
            FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            stream.Read(buffer, 0, 80);
            stream.Close();

            string version_string = System.Text.Encoding.UTF8.GetString(buffer);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string ver = fvi.FileVersion;

            // if version doesn't match, delete schema file
            if (!version_string.Contains(ver))
            {
                miscFunc.delete_safe(filepath);
                return false;
            }

            return true;
        }


        #endregion

        #region load items_game into item ListView

        private void load_items_to_listView(List<TF2.items_game.item> items_list, string search_string, string equip_region, bool show_progress)
        {
            list_view.Invoke(new MethodInvoker(delegate { list_view.Enabled = false; }));

            // do not load spy watches
            if ((selected_player_class == "spy") && (selected_item_slot == "pda2")) { return; }

            object[] arrObjects = new object[] { items_list, search_string, equip_region, show_progress };

            if ((items_list != null) && (items_list.Count != 0))
            {
                items_loading = true;

                label_model.Invoke(new MethodInvoker(delegate { label_model.Text = ""; }));
                list_view.Invoke(new MethodInvoker(delegate { list_view.Items.Clear(); }));

                Invoke(new Action(() => { imgList.Images.Clear(); }));
            }

            if ((!bgWorker_load_items_to_listView.IsBusy) && (items_list != null) && (items_list.Count != 0))
            {
                bgWorker_load_items_to_listView.RunWorkerAsync(arrObjects);
            }
        }

        private bool CaseContains(string baseString, string textToSearch, StringComparison comparisonMode)
        {
            return (baseString.IndexOf(textToSearch, comparisonMode) != -1);
        }

        // do work : Loads items used by one class only into the item icon list
        protected void bgWorker_load_items_to_listView_DoWork(object sender, DoWorkEventArgs e)
        {
            #region bg worker setup

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event
            object[] arrObjects = (object[])e.Argument;//Collect the array of objects the we recived from the main thread

            List<TF2.items_game.item> items_list = (List<TF2.items_game.item>)arrObjects[0];
            string search_string = (string)arrObjects[1];
            string equip_region_filter = (string)arrObjects[2];
            bool show_progress = (bool)arrObjects[3];
            StringBuilder sb = new StringBuilder();//Declare a new string builder to store the result.

            #endregion

            show_progress = true;

            #region setup/clear variables

            items_loading = true;

            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Visible = show_progress; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Maximum = items_list.Count; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { imgList.ImageSize = new Size(64, 64); }));

            Int32 item_id = 0;
            int list_index = 0;
            bool validated = false;
            TF2.items_game.item the_item;

            string class_item, slot_item, model;

            #endregion


            Invoke(new Action(() => { list_view.BeginUpdate(); }));

            int progress_perct = 0;

            #region for each item in item_list

            // For each item in items_list
            // foreach (var the_item in items_list) // items_list.Count
            for (int i = 0; i < items_list.Count; i++)
            {
                try
                {
                    #region setup variables

                    item_id = i;

                    validated = false;
                    the_item = items_list[i];

                    // load only items of selected class and item slot
                    class_item = "";
                    slot_item = the_item.item_slot_schema;
                    model = the_item.model_path;

                    #endregion

                    // if item is not the same slot type
                    if (!slot_item.Eq(selected_item_slot))
                    {
                        continue;
                    }

                    #region search item criteria

                    // search item by name.contains, skip if it doesn't match
                    if ((search_string != null) || (search_string != ""))
                    {
                        //neodement: this extra bit at the end makes it search the displayed name instead of just the internal one
                        if ((the_item.Name_str != null) && (!the_item.Name_str.ToLower().Contains(search_string.ToLower())) && (!the_item.item_name_var.ToLower().Contains(search_string.ToLower())))
                        {
                            continue;
                        }
                    }
                    #endregion


                    #region Load: all other items

                    // check if item is used by mutiple classes
                    // if that's the case, since its possible one class may use a different item slot (compared to another class)
                    if (the_item.used_by_classes != null)
                    {

                        // if its only one class and not the user selected class, skip to nex item
                        if (the_item.used_by_classes.Count == 1)
                        {
                            if (!the_item.used_by_classes[0].tfclass.Eq(selected_player_class))
                                continue;
                        }

                        // for each class
                        for (int eobj = 0; eobj < the_item.used_by_classes.Count; eobj++)
                        {
                            // if its the user selected class
                            if (the_item.used_by_classes[eobj].tfclass.ToLower().Equals(selected_player_class))
                            {
                                class_item = the_item.used_by_classes[eobj].tfclass.ToLower();

                                // in some cases multi-class weapons have different slots defined in used_by_class secondary value
                                string slot_test = the_item.used_by_classes[eobj].slot.ToLower();
                                if ((slot_test.Eq("primary")) || (slot_test.Eq("secondary")) || (slot_test.Eq("melee"))) { slot_item = slot_test; }
                                break;
                            }
                        }

                        // if the item has many all class model, get the model for the user selected class
                        if (the_item.model_player_per_class.Count > 0)
                        {
                            // for each class's model
                            foreach (var item in the_item.model_player_per_class)
                            {

                                // if model path uses class variant %s
                                if (item.tfclass.Eq("basename"))
                                {
                                    string bclass = selected_player_class;
                                    if (selected_player_class.Eq("demoman")) { bclass = "demo"; }

                                    model = item.model.Replace("%s", bclass);
                                }

                                // if its the user selected class
                                if (item.tfclass.Eq(selected_player_class))
                                {
                                    model = item.model;
                                }
                            }
                        }
                        else
                        {
                            model = the_item.model_path;
                        }

                    }
                    #endregion



                    #region validate item's class, slot and if its all class or not

                    // if selected class and item match with the item, validate it to be added into the list
                    if ((class_item.Eq(selected_player_class)) && (slot_item.Eq(selected_item_slot)))
                    {
                        // if checkbox "all class" is checked and item is all class
                        // load only all class
                        if (cb_allclass_only.Checked)
                        {
                            // has more than one class model
                            if (the_item.used_by_classes.Count > 1)
                            {
                                validated = true;
                            }

                        }
                        // load only single-class items
                        else if (!cb_allclass_only.Checked)
                        {
                            // only has one or less class model
                            if (the_item.used_by_classes.Count <= 1)
                            {
                                validated = true;
                            }
                        }
                    }

                    #endregion

                    // filter by equip region
                    if ((equip_region_filter != "") && (the_item.equip_rgn != "")) //all items
                    {

                        if (the_item.equip_rgn == null)
                        {
                            continue;
                        }

                        if (!the_item.equip_rgn.Eq(equip_region_filter)) { validated = false; }

                        if ((equip_region_filter == "all items") && (validated = true)) { validated = true; }
                    }

                    // if item does not match selected class, slot and "all class" skip to next time
                    if (!validated) { continue; }

                    #region equip region

                    if ((the_item.item_slot == "misc") && (the_item.equip_rgn != null) && (equip_region_filter == ""))
                    {
                        bool region_exists = false;

                        foreach (var region in equip_regions_list)
                        {
                            if (region.name == the_item.equip_rgn)
                            {
                                region.count++;
                                region_exists = true;
                                break;
                            }
                        }

                        if (!region_exists)
                        {
                            equip_regions_list.Add(new equip_region_tfmv(the_item.equip_rgn));
                        }
                    }

                    #endregion


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // VALIDATED ITEMS ONLY PAST THIS POINT //
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    // ListViewItem item_ListView = new ListViewItem();
                    ExtdListViewItem item_ListView = new ExtdListViewItem();
                    item_ListView.equip_region = the_item.equip_rgn;
                    item_ListView.not_paintable = the_item.not_paintable;

                    #region bodygroups

                    if (the_item.visuals.player_bodygroups.Count > 0)
                    {
                        item_ListView.bodygroups_off = new List<string>();
                        foreach (var bodygroup in the_item.visuals.player_bodygroups)
                        {
                            if (bodygroup.value == "1")
                            {
                                //todo: remove this when it works...
                                //MessageBox.Show("added " + bodygroup.key);
                                item_ListView.bodygroups_off.Add(bodygroup.key);
                            }
                        }
                    }

                    #endregion

                    // slot animation
                    item_ListView.anim_slot = the_item.anim_slot;

                    #region add item: for model attachments

                    // if model has attachments
                    if (the_item.visuals.attached_models.Count > 0)
                    {
                        item_ListView.model_attachements = the_item.visuals.attached_models;

                        if (item_ListView.model_attachements == null) { item_ListView.model_attachements = new List<TF2.items_game.attached_model>(); }
                    }

                    // extra_wearable - another attachment model parameter (i.e. used by MVM 'botkiller' weapons)
                    item_ListView.extra_wearable = the_item.extra_wearable;

                    #endregion


                    #region add items: for styles

                    // if model has skin style or model style create and add a new item to the list
                    // for australiums make sure to get different backpack icon?

                    if (the_item.visuals.styles.Count > 0)
                    {
                        // for each style
                        for (int s = 0; s < the_item.visuals.styles.Count; s++)
                        {
                            TF2.items_game.style style = the_item.visuals.styles[s];

                            // if its a duplicate item, skip
                            if (style.skin_blu == "3" && style.skin_red == "2" && s == the_item.visuals.styles.Count - 1)
                            {
                                continue;
                            }

                            item_ListView = new ExtdListViewItem();

                            item_ListView.anim_slot = the_item.anim_slot;
                            item_ListView.equip_region = the_item.equip_rgn;
                            item_ListView.not_paintable = the_item.not_paintable;
                            item_ListView.bodygroups_off = new List<string>();

                            #region add to listview

                            #region load icon

                            try // make sure that there's no error loading the image, if there is an error laoding it, we redownload the icon, in case it was corrupted
                            {
                                Invoke(new Action(() => { imgList.Images.Add(get_icon_image(the_item.icon_url)); }));
                                list_view.Invoke(new MethodInvoker(delegate { list_view.LargeImageList = imgList; }));
                            }
                            catch (System.Exception)
                            {
                                Invoke(new Action(() => { imgList.Images.Add(get_icon_image(the_item.icon_url)); }));
                                list_view.Invoke(new MethodInvoker(delegate { list_view.LargeImageList = imgList; }));
                            }

                            #endregion


                            #region set name of the item + (style name) if it has one

                            string style_name = "";
                            // Set the Text property to the cursor name.
                            if (style.name != "")
                            {
                                style_name = get_item_name_string(style.name);
                            }

                            if (style_name != "")
                            {
                                item_ListView.Text = the_item.item_name_var + " (" + style_name + ")";
                            }
                            else
                            {
                                item_ListView.Text = the_item.item_name_var;
                            }

                            #endregion

                            #region bodygroup styles

                            // bodygroups style
                            // some styles define if bodygroups should be turned off
                            if ((style.additional_hidden_bodygroups != null) && (style.additional_hidden_bodygroups.Count > 0))
                            {
                                foreach (var bdygroup_style in style.additional_hidden_bodygroups)
                                {
                                    // if bodygroup is defined as off
                                    if (bdygroup_style.toggle == 1)
                                    {

                                        // add to bodygroups_off list if it isn't already in the list
                                        if (!item_ListView.bodygroups_off.Contains(bdygroup_style.bodygrop_name))
                                        {
                                            item_ListView.bodygroups_off.Add(bdygroup_style.bodygrop_name);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region model styles
                            // if style has a model
                            if (style.model_player != null)
                            {
                                item_ListView.model_path = style.model_player; // model

                            }
                            else if (the_item.model_player_per_class.Count > 0)
                            {
                                foreach (var class_model in the_item.model_player_per_class)
                                {
                                    if (class_model.tfclass.Eq(selected_player_class))
                                    {
                                        item_ListView.model_path = class_model.model;
                                    }

                                    // if model path uses class variant %s
                                    if (class_model.tfclass == "basename")
                                    {
                                        string bclass = selected_player_class;
                                        if (selected_player_class.Eq("demoman")) { bclass = "demo"; }

                                        item_ListView.model_path = class_model.model.Replace("%s", bclass);
                                    }
                                }
                            }
                            else
                            {
                                item_ListView.model_path = the_item.model_path; // model
                            }

                            // for model styles, also get the global bodygroup info
                            #region bodygroups

                            if (the_item.visuals.player_bodygroups.Count > 0)
                            {
                                item_ListView.bodygroups_off = new List<string>();
                                foreach (var bodygroup in the_item.visuals.player_bodygroups)
                                {
                                    if (bodygroup.value == "1")
                                    {
                                        item_ListView.bodygroups_off.Add(bodygroup.key);
                                    }
                                }
                            }

                            #endregion

                            // all class model style
                            foreach (var allClass_mdl_style in style.model_player_per_class_list)
                            {
                                if (selected_player_class.Eq(allClass_mdl_style.class_name))
                                {
                                    item_ListView.model_path = allClass_mdl_style.model;
                                }

                                // if model path uses class variant %s
                                if (allClass_mdl_style.class_name.Eq("basename"))
                                {
                                    string bclass = selected_player_class;
                                    if (selected_player_class.Eq("demoman")) { bclass = "demo"; }

                                    item_ListView.model_path = allClass_mdl_style.model.Replace("%s", bclass);
                                }
                            }

                            #endregion

                            // team colored skin override
                            if ((style.skin_red != null) && (style.skin_blu != null))
                            {
                                item_ListView.skin_red_override = Convert.ToByte(style.skin_red); item_ListView.skin_blu_override = Convert.ToByte(style.skin_blu);
                            }

                            // style.skin (overrides all skins)
                            if ((style.skin_red == null) && (style.skin_blu == null))
                            {
                                // item_ListView.skin_red_override = Convert.ToByte(style.skin); item_ListView.skin_blu_override = Convert.ToByte(style.skin);
                                item_ListView.skin_override_all = style.skin;
                            }

                            item_ListView.ImageIndex = list_index;

                            // Add the ListViewItem to the ListView.
                            item_ListView.item_id = item_id;

                            list_view.Invoke(new MethodInvoker(delegate { list_view.Items.Add(item_ListView); })); //list_view.Items.Add(item_ListView);

                            list_index++;
                            #endregion
                        }
                        continue;
                    }

                    #endregion


                    #region add item: no attachments no styles


                    #region load icon


                    try // make sure that there's no error loading the image, if there is an erorr laoding it, we redownload the icon, in case it was corrupted
                    {
                        Invoke(new Action(() => { imgList.Images.Add(get_icon_image(the_item.icon_url)); }));
                        list_view.Invoke(new MethodInvoker(delegate { list_view.LargeImageList = imgList; }));
                    }
                    catch (System.Exception)
                    {
                        try
                        {
                            Invoke(new Action(() => { imgList.Images.Add(get_icon_image(the_item.icon_url)); }));
                            list_view.Invoke(new MethodInvoker(delegate { list_view.LargeImageList = imgList; }));
                        }
                        catch
                        {

                        }
                    }

                    #endregion

                    #region add to listview

                    // Set the Text property to the cursor name.
                    item_ListView.Text = the_item.item_name_var;

                    // Set the Tag property to the cursor.
                    item_ListView.model_path = model; // model
                    item_ListView.ImageIndex = list_index;

                    item_ListView.visuals_red_attached_models = the_item.visuals_red_attached_models;

                    // if model already is loadout, check the item in the items list
                    foreach (Loadout_Item item in loadout_list.Controls)
                    {
                        if (item.model_path == model)
                        {
                            item_ListView.Checked = true;
                            break;
                        }
                    }

                    // Add the ListViewItem to the ListView.
                    item_ListView.item_id = item_id;

                    list_view.Invoke(new MethodInvoker(delegate { list_view.Items.Add(item_ListView); }));
                    list_index++;

                    #endregion

                    #endregion

                    sendingWorker.ReportProgress(i);

                    progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Value = i; }));
                }
                catch //(System.Exception error)
                {
                    //MessageBox.Show(error.Message);
                }

            }

            #endregion


            if ((equip_regions_list != null) && (equip_region_filter == ""))
            {
                if (selected_item_slot == "misc")

                    foreach (var region in equip_regions_list)
                    {
                        comboBox_equip_region_filter.Invoke(new MethodInvoker(delegate { comboBox_equip_region_filter.Items.Add("[" + region.count + "]  " + region.name); }));
                    }
            }

            Invoke(new Action(() => { list_view.EndUpdate(); }));

            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Value = progressBar_item_list.Maximum; }));
            progressBar_item_list.Invoke(new MethodInvoker(delegate { progressBar_item_list.Visible = false; }));

            Invoke(new Action(() => { items_loading = false; }));
        }

        protected void bgWorker_load_items_to_listView_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        protected void bgWorker_load_items_to_listView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;

                list_view.Invoke(new MethodInvoker(delegate { list_view.Enabled = true; }));
            }
            else if (e.Cancelled)
            {
                list_view.Invoke(new MethodInvoker(delegate { list_view.Enabled = true; }));
                return;
            }
            else
            {
                list_view.Invoke(new MethodInvoker(delegate { list_view.Enabled = true; }));
                return;
            }

            items_loading = false;
        }

        #endregion


        #endregion


        #region load & manage "tf_english.txt" strings
        // loads item names from "tf/resource/tf_english.txt"
        private void load_localization_strings()
        {
            string path = steamGameConfig.tf_dir + "resource\\tf_english.txt";
            item_names_List = new item_names();

            if (!File.Exists(path))
            {
                // MessageBox.Show("Error, could not find file: " + path + "\nFile is required to get the item names.");
                return;

            }
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                // if string has key and value (i.e: "TF_Weapon_Bat"	    "Bat")
                if (line.Count(x => x == '\"') == 4)
                {
                    // ignore descriptions and only take items starting with "TF_"
                    if ((line.Contains("_Desc") == false) && (line.Contains("TF_") || (line.Contains("#TF_"))))
                    {
                        string[] args = line.Split('\"');
                        item_names_List.key.Add(args[1]);
                        item_names_List.value.Add(args[3]);
                    }
                }
                counter++;
            }

            file.Close();
        }

        private item_names item_names_List;

        #region item names class

        private class item_names
        {
            public List<String> key;
            public List<String> value;

            public item_names()
            {
                key = new List<String>();
                value = new List<String>();
            }
        }

        #endregion


        // gets real item name from the language file
        private string get_item_name_string(string item_name)
        {
            if ((item_name == null) || (item_name == "")) { return ""; }
            item_name = item_name.Replace("#", "");
            string result = "";
            //int index = item_names_List.key.FindIndex(x => x.StartsWith(item_name, StringComparison.OrdinalIgnoreCase));

            //fix for wrong string being pulled from TF_English, for example POMSON, Fishcake Fragment
            int index = item_names_List.key.FindIndex(x => x.Equals(item_name, StringComparison.OrdinalIgnoreCase));

            if (index > -1)
            {
                result = item_names_List.value[index];
            }

            return result;
        }


        #endregion


        #region icons

        private void download_icons(object sender, DoWorkEventArgs e)
        {
            if (items_game.Count == 0) { return; }

            if (items_game == null)
            {
                if (MessageBox.Show("TF2 Item schema not found. \n Do you want to download the item schema (might take a few minutes to download)", "Download Schema?", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
                {
                    this.BeginInvoke((MethodInvoker)delegate { download_schemas(); });
                }
                return;
            }

            BackgroundWorker sendingWorker = (BackgroundWorker)sender;//Capture the BackgroundWorker that fired the event

            int items_count = items_game.Count;

            this.BeginInvoke((Action)(() => this.Enabled = false));

            lab_status.BeginInvoke((Action)(() => lab_status.Text = "Downloading: item icons"));

            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Minimum = 0));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Maximum = items_count));
            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = 0));

            // loop through items to get icons
            for (int i = 0; i < items_count; i++)
            {
                if (!sendingWorker.CancellationPending)
                {
                    get_icon_image(items_game[i].icon_url);

                    progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Value = i));

                    sendingWorker.ReportProgress(i);
                }
                else
                {
                    e.Cancel = true;
                    break;
                }
            }

            progressBar_dl.BeginInvoke((Action)(() => progressBar_dl.Visible = false));
        }


        private Image get_icon_image(string image_url)
        {
            Image image = missing_icon;

            // check if url has invalid chars
            if ((image_url.IndexOfAny(Path.GetInvalidPathChars()) == -1) == false)
            {
                //MessageBox.Show("Error: invalid icon path: " + image_url);
                return image;
            }

            string path = schema_dir + "icons/" + Path.GetFileName(image_url);
            string schema_icon_path = schema_dir + "icons/" + Path.GetFileName(image_url);

            try // make sure that there's no error loading the image, if there is an erorr laoding it, we redownload the icon, in case it was corrupted
            {
                // if file doesn't exist or size = 0 then   download
                if (!File.Exists(schema_icon_path) || ((new FileInfo(schema_icon_path)).Length == 0))
                {
                    WebClient webClient = new WebClient();
                    byte[] img_data = webClient.DownloadData(image_url);

                    using (MemoryStream mStream = new MemoryStream(img_data))
                    {
                        image = Image.FromStream(mStream);
                    }

                    image = ResizeImage(image, 64, 64);
                    image.Save(schema_icon_path, ImageFormat.Png);

                } else {

                    // load icon from file
                    image = Image.FromFile(schema_icon_path);
                }

            }
            catch //(System.Exception)
            {
            }

            return image;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        // gets icon web path-name from the schema (since it's not defined in the items_game.vdf)
        private string get_icon_from_schema(string file, string keyword)
        {
            int index = file.IndexOf("/icons/" + keyword + ".", StringComparison.OrdinalIgnoreCase);
            return file.Substring((index + 7), index + keyword.Length + 45 - index);
        }

        #endregion


        #endregion


        #region loadout list

        // add item to loadout list
        private void list_view_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                if (items_loading == false)
                {
                    label_model.Visible = true;

                    // get checked/unchecked item
                    ExtdListViewItem item = (ExtdListViewItem)e.Item;
                    // find if the item is already on the model list
                    int item_listmodel_index = loadout_find_model(item.model_path);

                    #region workshop zip item

                    if ((item.workshop_zip_path != null) && (item.workshop_zip_path != ""))
                    {
                        miscFunc.DeleteDirectoryContent(tmp_workshop_zip_dir);
                        miscFunc.DeleteDirectoryContent(tmp_loadout_dir);

                        #region remove workshop items
                        //  if item gets unchecked / deselected
                        // remove model from loadout
                        if ((item.Checked == false))
                        {
                            bool items_changed = false;
                            // search items in loadout list which have the same item_id as the item that was unchecked
                            for (int i = 0; i < loadout_list.Controls.Count; i++)
                            {

                                Loadout_Item itemx = (Loadout_Item)loadout_list.Controls[i];

                                // remove items in id matches
                                if (itemx.workshop_zip_path == item.workshop_zip_path)
                                {
                                    if ((itemx.item_slot == "primary") || (itemx.item_slot == "secondary") || (itemx.item_slot == "melee") || (itemx.item_slot == "pda") || (itemx.item_slot == "pda2") || (itemx.item_slot == "building"))
                                    {
                                        txtb_pose.Text = "ref";
                                    }

                                    itemx.Dispose();
                                    i--;
                                    items_changed = true;
                                }
                            }
                            // if loadout items changed, reset paint form
                            if (items_changed) { SkinsManager_Form_reset(); }

                            return;
                        }

                        #endregion


                        #region add workshop zip item

                        adding_workshop_item_toLoadout = true;
                        adding_workshop_item_zip_path = item.workshop_zip_path;
                        if (!File.Exists(item.workshop_zip_path)) { MessageBox.Show("Cannot find workshop zip file:\n" + item.workshop_zip_path); return; }

                        filter_droped_file(item.workshop_zip_path);
                        return;

                        #endregion
                    }

                    // if not workshop zip
                    adding_workshop_item_toLoadout = false;

                    #endregion


                    #region set pose

                    if (!cb_ref_pose.Checked)
                    {
                        if ((selected_item_slot == "primary") || (selected_item_slot == "secondary") || (selected_item_slot == "melee"))
                        {
                            txtb_pose.Text = "Stand_" + selected_item_slot.ToUpper();

                            // change animation for demoman depending on the slot
                            if (selected_player_class == "demoman")
                            {
                                if (selected_item_slot == "primary") { txtb_pose.Text = "Stand_SECONDARY"; }
                                if (selected_item_slot == "secondary") { txtb_pose.Text = "Stand_PRIMARY"; }
                            }
                        }
                    }

                    #endregion


                    #region fix stuff

                    if (item.extra_wearable != null)
                    {
                        if (item.model_path == item.extra_wearable.mdl_path) { item.extra_wearable = null; }
                    }

                    #endregion


                    #region add item
                    // if item is checked
                    if (item.Checked)
                    {
                        // make sure item has a model
                        if ((item.model_path == "") && (item.extra_wearable == null)) { MessageBox.Show("Item has no model defined."); item.Checked = false; return; }

                        // enforce HLMV max items attachments (up to 12)
                        if (check_max_loadout_items()) { item.Checked = false; return; }

                        // item strict mode
                        if (cb_strict_equip_regions.Checked)
                        {
                            string item_region_test = loadout_equip_region_free(item.equip_region);

                            if (item_region_test != "")
                            {
                                MessageBox.Show(item_region_test.Split('(')[0] + " is already using this equipment region. \n\nStrict item compability can be disabled in: \nSettings tab > Items > and untick 'Strict equip region'");
                                item.Checked = false;

                                return;
                            }
                        }

                        #region bodygroups
                        // bodygroups
                        if (item.bodygroups_off != null)
                        {
                            // add bodygroup names that need to be turned off for this item
                            foreach (var bodygroup_off in item.bodygroups_off)
                            {
                                if (!loadout_bodygroups_off.Contains(bodygroup_off))
                                {
                                    loadout_bodygroups_off.Add(bodygroup_off);
                                }
                            }
                        }

                        #endregion

                        #region add main item


                        Loadout_Item loadout_item = new Loadout_Item();
                        loadout_item.item_slot = selected_item_slot;

                        if (item.model_path != "")
                        {
                            loadout_item = loadout_addItem(item.ImageList.Images[item.ImageIndex], item.Text, item.model_path, 0, 1, item.item_id, false);
                            loadout_item.item_slot = selected_item_slot;
                            loadout_item.Parent = loadout_list;
                        }
                        #endregion

                        loadout_item.skin_red = item.skin_red_override;
                        loadout_item.skin_blu = item.skin_blu_override;
                        loadout_item.skin_override_all = item.skin_override_all;
                        loadout_item.equip_region = item.equip_region;
                        loadout_item.not_paintable = item.not_paintable;

                        #region add model attachments

                        // if has attachments
                        if (item.model_attachements != null)
                        {
                            // if model has attachment models, add them too
                            foreach (var attachement in item.model_attachements)
                            {
                                // TODO handle number 2 (its probably 1 = skin_0    2 = skin_1 but it looks like no model attachments using 1-2 have different skins, so far)
                                if (attachement.model_display_flags == 1)
                                {
                                    // skip if model is undefined
                                    if (attachement.model == "") { continue; }

                                    // enforce HLMV max items attachements (up to 12)
                                    // exit for if maxed out
                                    if (check_max_loadout_items()) { break; }

                                    // create loadout item
                                    Loadout_Item loadout_item_attachement = loadout_addItem(item.ImageList.Images[item.ImageIndex], "Attachment", attachement.model, 0, 1, item.item_id, false);

                                    loadout_item_attachement.item_slot = selected_item_slot;
                                    // parent item to loadout list
                                    loadout_item_attachement.Parent = loadout_list;
                                }

                            }
                        }
                        #endregion

                        #region add extra wearable item (kind of like an attachment, but different)

                        if (item.extra_wearable != null)
                        {
                            // skip if model is undefined  // enforce HLMV max items attachments (up to 12)
                            if ((item.extra_wearable.mdl_path != null) && (item.extra_wearable.mdl_path != "") && (!check_max_loadout_items()))
                            {
                                byte skin_red = 0, skin_blu = 0;

                                if (item.extra_wearable.skin_override) { skin_red = item.extra_wearable.skin_red; skin_blu = item.extra_wearable.skin_blu; }

                                Loadout_Item loadout_item_wearable = loadout_addItem(item.ImageList.Images[item.ImageIndex], "Attachment", item.extra_wearable.mdl_path, skin_red, skin_blu, item.item_id, false);
                                loadout_item_wearable.item_slot = selected_item_slot;
                                loadout_item_wearable.Parent = loadout_list;
                            }
                        }
                        #endregion

                        #region add visuals_red.attached_models

                        if (item.visuals_red_attached_models != null)
                        {
                            foreach (var attached_model in item.visuals_red_attached_models)
                            {
                                // skip if model is undefined  // enforce HLMV max items attachments (up to 12)
                                if ((attached_model != null) && (attached_model != "") && (!check_max_loadout_items()))
                                {
                                    Loadout_Item loadout_item_wearable = loadout_addItem(item.ImageList.Images[item.ImageIndex], "Attachment", attached_model, 0, 1, item.item_id, false);
                                    loadout_item_wearable.item_slot = selected_item_slot;
                                    loadout_item_wearable.Parent = loadout_list;
                                }
                            }
                        }

                        #endregion

                        SkinsManager_Form_reset();
                    }
                    #endregion

                    #region remove item and attachments
                    //  if item gets unchecked / deselected
                    // remove model from loadout
                    else if ((item.Checked == false) || (item.Selected))
                    {
                        bool items_changed = false;
                        // search items in loadout list which have the same item_id as the item that was unchecked
                        for (int i = 0; i < loadout_list.Controls.Count; i++)
                        {
                            Loadout_Item itemx = (Loadout_Item)loadout_list.Controls[i];

                            // remove items in id matches
                            if (itemx.item_id == item.item_id)
                            {
                                if ((itemx.item_slot == "primary") || (itemx.item_slot == "secondary") || (itemx.item_slot == "melee") || (itemx.item_slot == "pda") || (itemx.item_slot == "pda2") || (itemx.item_slot == "building"))
                                {
                                    txtb_pose.Text = "ref";
                                }

                                itemx.Dispose();
                                i--;
                                items_changed = true;
                            }
                        }
                        // if loadout items changed, reset paint form
                        if (items_changed) { SkinsManager_Form_reset(); }
                    }

                    #endregion

                    #region determine item slot player pose/animation

                    if (item.Checked)
                    {
                        if ((item.anim_slot != null) && (item.anim_slot != ""))
                        {
                            string out_pose = "stand_Primary";
                            out_pose = "stand_" + item.anim_slot;

                            if (item.anim_slot.Eq("primary2")) { out_pose = "stand_PRIMARY"; } // soldier: The Cow Mangler 5000
                            if (item.anim_slot.Eq("building")) { out_pose = "stand_SAPPER"; }
                            if (item.anim_slot.Eq("force_not_used")) { out_pose = "ref"; }

                            if (out_pose != "")
                            {
                                txtb_pose.Text = out_pose;
                            }

                        } else { // if anim_slot is undefined, try to set standard slot animation

                            if (selected_player_class.Eq("spy"))
                            {
                                if (selected_item_slot.Eq("building")) { txtb_pose.Text = "stand_SAPPER"; }
                                if (selected_item_slot.Eq("pda")) { txtb_pose.Text = "stand_PDA"; }
                            }

                            if (selected_player_class.Eq("engineer"))
                            {
                                if ((selected_item_slot.Eq("pda")) || (selected_item_slot.Eq("pda2"))) { txtb_pose.Text = "stand_PDA"; }
                            }
                        }

                        if (list_view.FocusedItem != null)
                        {
                            label_model.Text = item.model_path; //list_view.Focuseditem.Tag.tostring();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private bool check_max_loadout_items()
        {
            // enforce HLMV max items attachments (up to 12)
            if ((loadout_list.Controls.Count > 11))
            {
                MessageBox.Show("HLMV only allows up to 12 model attachments.");
                return true;
            }

            return false;
        }

        // checks items in loadout and their equip_region, if its already in use return false
        private string loadout_equip_region_free(string test_equip_region)
        {
            if (test_equip_region == null) { return ""; }

            foreach (Loadout_Item item in loadout_list.Controls)
            {
                if (item.equip_region == test_equip_region)
                {
                    return item.item_name;
                }
            }

            return "";
        }


        private string selected_item_name = "";
        // The selected PictureBox.
        private Loadout_Item SelectedPictureBox = null;
        // Select the clicked PictureBox.
        private void PictureBox_Click(object sender, EventArgs e)
        {
            Loadout_Item pic = (Loadout_Item)sender;

            // Deselect the previous PictureBox.
            if (SelectedPictureBox != null)
            {
                SelectedPictureBox.BorderStyle = BorderStyle.None;
                SelectedPictureBox.selected = false;
            }

            // Select the clicked PictureBox.
            SelectedPictureBox = pic;
            SelectedPictureBox.BorderStyle = BorderStyle.FixedSingle;
            SelectedPictureBox.selected = true;

            selected_item_name = pic.Name;

            pic.selected = true;
        }

        private void list_view_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ExtdListViewItem item = (ExtdListViewItem)e.Item;

            label_model.Text = item.model_path;
            btn_tf2_wiki.Tag = item.model_path;
            lab_item_id.Text = item.item_id + "  " + item.Index;
        }

        #region loadout functions


        private Loadout_Item loadout_addItem(Image icon, string item_name, string mdl_path, byte _skin_red, byte _skin_blu, Int32 item_id, bool simple_icon)
        {
            if (icon == null)
            {
                var b = new Bitmap(1, 1);
                b.SetPixel(0, 0, Color.Red);
                icon = b;
            }

            Loadout_Item item = new Loadout_Item();
            item.ClientSize = new Size(64, 64);

            // recale icon if needed
            if ((icon.Height != 64) && (icon.Width != 64))
            {
                icon = new Bitmap(icon, new Size(64, 64));
            }

            item.icon = icon;
            item.PictureBox.Image = icon;
            item.item_name = item_name;
            item.model_path = mdl_path;
            item.item_id = item_id;
            item.skin_red = _skin_red;
            item.skin_blu = _skin_blu;

            if (adding_workshop_item_toLoadout)
            {
                item.workshop_zip_path = adding_workshop_item_zip_path;
                adding_workshop_item_zip_path = "";
            }

            // double click event handler
            item.DoubleClick += loadout_item_MouseDoubleClick;

            // add item to list
            item.Parent = loadout_list;

            return item;
        }

        private void loadout_item_MouseDoubleClick(object sender, EventArgs e)
        {
            Loadout_Item item = (Loadout_Item)sender;

            for (int i = (loadout_list.Controls.Count - 1); i >= 0; i--)
            {
                // Control c = loadout_list.Controls[i];
                Loadout_Item item_search = (Loadout_Item)loadout_list.Controls[i];

                if (item_search == item)
                {
                    // remove item from loadout list
                    item_search.Dispose();


                    // for each item in the listview (schema items)
                    for (int c = 0; c < list_view.Items.Count; c++)
                    {
                        ExtdListViewItem listview_item = (ExtdListViewItem)list_view.Items[c];

                        if (listview_item.model_path == item_search.model_path)
                        {
                            listview_item.Checked = false;
                        }

                    }
                }
            }
        }

        // searches item by mdl_path and returns the index
        private int loadout_find_model(string search_mdl_path)
        {
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {

                if (((Loadout_Item)loadout_list.Controls[i]).model_path == search_mdl_path)
                {
                    return i;
                }
            }

            return -1;
        }


        private void loadout_RemoveAt(int index)
        {
            if (index < loadout_list.Controls.Count)
                loadout_list.Controls.RemoveAt(index);
        }

        #endregion


        #endregion

        #region SKINS Manager

        #region switch player model skins

        private string get_player_path_by_class(string tf_class)
        {
            string vmt_path = "";

            if (tf_class == "demoman") { vmt_path = @"models\player\demo\"; }
            if (tf_class == "engineer") { vmt_path = @"models\player\engineer\"; }
            if (tf_class == "heavy") { vmt_path = @"models\player\hvyweapon\"; }
            if (tf_class == "medic") { vmt_path = @"models\player\medic\"; }
            if (tf_class == "pyro") { vmt_path = @"models\player\pyro\"; }
            if (tf_class == "scout") { vmt_path = @"models\player\scout\"; }
            if (tf_class == "sniper") { vmt_path = @"models\player\sniper\"; }
            if (tf_class == "soldier") { vmt_path = @"models\player\soldier\"; }
            if (tf_class == "spy") { vmt_path = @"models\player\spy\"; }

            return vmt_path;
        }

        // switches the player's skin VMT code
        private void switch_player_skin(int skin_num) // 0 = red 1 = blue
        {
            if (skin_num < 0) { skin_num = 0; }
            if (skin_num > 1) { skin_num = 1; }

            string team = "red"; if (skin_num == 1) { team = "blue"; }

            string player_name = selected_player_class;
            if (player_name == "heavy") { player_name = "hvyweapon"; }
            if (selected_player_class == "demo") { selected_player_class = "demoman"; player_name = "demoman"; }

            string player_dir = get_player_path_by_class(selected_player_class);
            string player_mat_path = tfmv_dir + "materials/" + player_dir + player_name;

            // change it after gettubgplayer_dir since demoman has dir name "demoman" but filename = "demo"

            #region reset skin


            // delete hwm materials
            if (Directory.Exists(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\player\\" + player_name + "\\hwm\\"))
            {
                Directory.Delete(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\player\\" + player_name + "\\hwm\\", true);
            }

            // delete red skin
            miscFunc.delete_safe(tfmv_dir + "materials/" + player_dir + player_name + "_red.vmt");

            #region class exception - additional material to switch

            // delete spy head material
            if (player_name == "spy")
            {
                miscFunc.delete_safe(tfmv_dir + "materials/models/player/spy/spy_head_red.vmt");
                if (txtb_main_model.Text.ToLower().Contains("hwm"))
                {
                    miscFunc.delete_safe(tfmv_dir + "materials/models/player/spy/hwm/spy_head_red.vmt");
                }
            }

            if (player_name == "medic")
            {
                miscFunc.delete_safe(tfmv_dir + "materials/models/player/medic/medic_backpack_red.vmt");
            }

            #endregion

            #endregion

            // if player material path is not set, return
            if ((player_dir == "") || (player_name == "")) { return; }

            if (skin_num == 1)
            {
                // extract player vmt to tf/custom/TFMV/ 
                VPK.Extract("materials/" + player_dir + player_name + "_" + team + ".vmt", tfmv_dir + "materials/" + player_dir, 0);

                // rename the _blue.vmt as _red.vmt
                if (File.Exists(tfmv_dir + "materials/" + player_dir + player_name + "_blue.vmt"))
                {
                    System.IO.File.Move(tfmv_dir + "materials/" + player_dir + player_name + "_blue.vmt", tfmv_dir + "materials/" + player_dir + player_name + "_red.vmt");
                }

                #region spy_head_blue & hwm fix

                // exception for the spy, also extract the head material for blue
                if (player_name == "spy")
                {
                    string spy_head_mat_path = "materials/models/player/spy/spy_head_blue.vmt";
                    string spy_head_mat_dir = "materials/models/player/spy/";
                    string spy_head_red_mat_path = "materials/models/player/spy/spy_head_red.vmt";

                    // hwm head material
                    if (txtb_main_model.Text.ToLower().Contains("hwm"))
                    {
                        spy_head_mat_path = "materials/models/player/spy/hwm/spy_head_blue.vmt";
                        Directory.CreateDirectory(tfmv_dir + "materials/models/player/spy/hwm/");
                        spy_head_mat_dir = "materials/models/player/spy/hwm/";
                        spy_head_red_mat_path = "materials/models/player/spy/hwm/spy_head_red.vmt";

                        VPK.Extract(spy_head_mat_path, tfmv_dir + "materials/models/player/spy/", 0);
                    }

                    // extract player vmt to tf/custom/TFMV/ 
                    VPK.Extract(spy_head_mat_path, tfmv_dir + spy_head_mat_dir, 0);



                    // copy the _blue.vmt as _red.vmt
                    if (File.Exists(tfmv_dir + spy_head_mat_path))
                    {
                        System.IO.File.Copy(tfmv_dir + spy_head_mat_path, tfmv_dir + spy_head_red_mat_path, true);
                    }

                }
                #endregion

                #region medic_backpack swap

                if (player_name == "medic")
                {
                    string medic_backpack_mat_path = "materials/models/player/medic/medic_backpack_blue.vmt";
                    string medic_backpack_mat_dir = "materials/models/player/medic/";
                    string medic_backpack_red_mat_path = "materials/models/player/medic/medic_backpack_red.vmt";

                    // extract player vmt to tf/custom/TFMV/ 
                    VPK.Extract(medic_backpack_mat_path, tfmv_dir + medic_backpack_mat_dir, 0);

                    // copy the _blue.vmt as _red.vmt
                    if (File.Exists(tfmv_dir + medic_backpack_mat_path))
                    {
                        System.IO.File.Copy(tfmv_dir + medic_backpack_mat_path, tfmv_dir + medic_backpack_red_mat_path, true);
                    }
                }

                #endregion

            }

        }


        #endregion

        private void SkinsManager_Form_reset()
        {

            // for each model painter close VMT editor
            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                mp.close_forms();
            }

            skins_manager_control.Controls.Clear();

            btn_update_paints.Enabled = false;
            panel_paints.Enabled = false;

            // reset paints team color button to red
            btn_red_team.ForeColor = Color.FromArgb(255, 30, 30, 30);
            btn_blu_team.ForeColor = Color.FromArgb(255, 91, 122, 140);
        }

        // load/get materials of a model and generate interface for painting vmts
        private void btn_load_mdl_mats_Click(object sender, EventArgs e)
        {
            load_skins();
        }

        // loads model's materials and setups the Skins paints manager
        private void load_skins()
        {
            selected_team_skin_index = 0;

            // paints_first_load = true;
            if ((loadout_list.Controls.Count == 0) && (txtb_main_model.Text == ""))
            {
                MessageBox.Show("Error: You first need to load some models in HLMV.");
                return;
            }

            // for every model add a VMT  painter
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {
                Loadout_Item loadout_item = (Loadout_Item)loadout_list.Controls[i];
                string mdlpath = loadout_item.model_path;
                string model_filename = Path.GetFileName(mdlpath);

                Model_Painter mp = new Model_Painter();

                if (loadout_item.item_name != "")
                {
                    mp.lab_mdl.Text = loadout_item.item_name; //.Split('(')[0] disable style name in model name
                    mp.lab_mdl.Tag = model_filename;
                    mp.txt_mdlpath.Text = mdlpath;
                    mp.mdlpath = mdlpath;
                }
                else
                {
                    mp.lab_mdl.Text = model_filename;
                }

                mp.Text = "Model: " + model_filename;
                mp.paint_dir = tfmv_dir;
                mp.tf_dir = steamGameConfig.tf_dir;

                if (cb_strict_paintablity.Checked)
                {
                    mp.not_paintable = loadout_item.not_paintable;
                }

                // set item icon
                mp.pictureBox.Image = loadout_item.PictureBox.Image;
                mp.skins = get_mats_sourcePaths(mdlpath); //THIS function also creates a copy of the mdl in the temp directory.
                mp.skin_override_all = loadout_item.skin_override_all;


                // skin override
                if ((loadout_item.skin_red != 0) || (loadout_item.skin_blu != 1))
                {
                    mp.skin_red_override = loadout_item.skin_red;
                    mp.skin_blu_override = loadout_item.skin_blu;
                }

                if (mp.skin_override_all != 255)
                {
                    mp.skin_red_override = mp.skin_override_all;
                    mp.skin_blu_override = mp.skin_override_all;
                }


                byte skin_index = selected_team_skin_index;

                if (selected_team_skin_index == 0)
                {
                    skin_index = loadout_item.skin_red;
                }

                if (selected_team_skin_index == 1)
                {
                    skin_index = loadout_item.skin_blu;
                }

                if (mp.skin_override_all != 255)
                {
                    skin_index = mp.skin_override_all;
                }

                // loop through skins
                mp.Load_Skins(skin_index);

                mp.Location = new Point(0, i * 105);

                skins_manager_control.Controls.Add(mp);
            }


            // if the are no attachments load the main model materials
            if ((loadout_list.Controls.Count == 0) && (txtb_main_model.Text != ""))
            {
                string mdlpath = txtb_main_model.Text;
                // do not load paints if the main model is a player model
                if (!miscFunc.if_mdl_path_is_playermodel(mdlpath))
                {
                    Model_Painter mp = new Model_Painter();

                    string model_filename = Path.GetFileName(mdlpath);
                    mp.lab_mdl.Text = model_filename; // + "(" + mdlpath + ")"
                    mp.Text = "Model: " + model_filename;
                    mp.txt_mdlpath.Text = model_filename;

                    mp.paint_dir = tfmv_dir;
                    mp.tf_dir = steamGameConfig.tf_dir;
                    mp.skins = get_mats_sourcePaths(mdlpath);

                    // loop through skins
                    mp.Load_Skins(selected_team_skin_index);

                    mp.Location = new Point(0, 0);

                    skins_manager_control.Controls.Add(mp);
                }
            }


            skins_manager_control.HorizontalScroll.Enabled = false;
            skins_manager_control.HorizontalScroll.Visible = false;

            // enable and show skins UI
            btn_update_paints.Enabled = true;
            panel_paints.Visible = true;
            panel_paints.Enabled = true;

            update_skins(true);
        }

        private bool mats_has_paths(List<string> materials)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if ((materials[i] != "") || materials[i] != " ")
                {
                    return true;
                }

            }
            return false;
        }


        // structure to organise the VMT material paths for painting/switching skins
        // we need this so we can easily sort the materials which are in VPK and those in TF


        // searches files in the game directory, "custom" directory and VPK, and copies to "outdir" folder
        private string get_game_files(string search_filepath, string outdir)
        {
            string file_name = Path.GetFileNameWithoutExtension(search_filepath);

            // // TF DIRECTORY  // // if file exists in TF copy it to outdir
            if (File.Exists(steamGameConfig.tf_dir + search_filepath))
            {
                miscFunc.copy_safe(steamGameConfig.tf_dir + search_filepath, outdir + search_filepath);
                return search_filepath;
            }

            else if (File.Exists(search_filepath))
            {
                preload_mdl(search_filepath);
                return (search_filepath.ToLower().Replace("/", "\\")).Replace(steamGameConfig.tf_dir.ToLower(), "");
            }

            // // VPK FILES // // / if file exists in game's VPKs copy it to outdir
            // TODO select materials/-models // / textures VPK???

            else if (VPK.Extract(search_filepath, Path.GetDirectoryName(outdir + search_filepath), 0) == false) // extract it from the vpk
            {
                return search_filepath;
            }

            // else file not found
            else
            {
                MessageBox.Show("Error: could not find model (" + search_filepath + ") in the game VPK, TF or TF/custom/TFMV/.");
            }

            return "";
        }


        // return list of material name/paths and indexes
        private List<List<string>> get_mats_sourcePaths(string modelpath)
        {

            modelpath = get_game_files(modelpath, tmp_dir);
            // split materials in two arrays, those which are in Tf and those which are in the GCF
            List<Object> materials_lists = Get_MDL_Materials(modelpath); // "bonk_helmet.mdl"

            #region Load materials from the raw format and convert to an organised list of skins of VMT_Skins class for TF and VPK paths

            if (materials_lists == null) { return new List<List<string>>(); }

            List<String> materials_raw = (List<string>)materials_lists[0];
            List<List<int>> skins_raw = (List<List<int>>)materials_lists[1];

            // get first material of the skin 0 (hat_red.vmt so we'll rewrite the VMT text to be able to load other skins as it hat_red.vmt containing hat_blue.vmt code)

            List<List<string>> skins = new List<List<string>>();
            // loop through the model's materials list
            for (int s = 0; s < skins_raw.Count; s++)
            {
                // add skin
                skins.Add(new List<string>());

                // if it only has one skin
                if (skins_raw[s].Count == 0)
                {
                    foreach (var skinx in materials_raw)
                    {
                        // check if vmt exists in TF
                        if (File.Exists(tfmv_dir + skinx))
                        {
                            skins[s].Add(skinx);

                        }
                        else
                        { // if vmt does not exist in TF
                          // extract vmt from VPK to TF
                            string[] p = Regex.Split(skinx, "/");
                            string mpath = skinx.Replace("/" + p[p.Length - 1], "");

                            get_game_files(skinx, tfmv_dir);

                            skins[s].Add(skinx);

                        }
                    }
                }

                // loop through the model's materials list
                for (int i = 0; i < skins_raw[s].Count; i++)
                {
                    int mat_index = skins_raw[s][i];
                    // check if vmt exists in TF
                    if (File.Exists(tfmv_dir + materials_raw[mat_index]))
                    {
                        skins[s].Add(materials_raw[mat_index]);
                    }
                    else
                    { // if vmt does not exist in TF
                        // extract vmt from VPK to TF
                        string[] p = Regex.Split(materials_raw[mat_index], "/");
                        string mpath = materials_raw[mat_index].Replace("/" + p[p.Length - 1], "") + "/";

                        get_game_files(materials_raw[mat_index], tfmv_dir);

                        skins[s].Add(materials_raw[mat_index]);
                    }
                }
            }

            return skins;

            #endregion
        }

        // update paints
        private void btn_set_mdl_mats_paints_Click(object sender, EventArgs e)
        {
            update_skins(true);
        }

        // rewrites the VMTs in tf/custom/TFMV/materials for the items and refreshes HLMV (if its running)
        public void update_skins(bool refresh)
        {
            bool has_model_painters = false;

            // loop through VMT painters
            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                mp.update_paints();
                has_model_painters = true;
            }

            // refresh hlmv
            if ((has_model_painters) && (refresh))
            {
                refresh_hlmv(false);
            }
        }



        // switch skins
        private void cb_skins_SelectionChangeCommitted(object sender, EventArgs e)
        {
            #region reload skins

            int mp_count = 0;

            // for each model painter
            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    // if model paint is not locked we switch the skins
                    if (!mp.cb_lock_skin.Checked)
                    {
                        mp.switch_Skin(selected_team_skin_index);
                    }
                }

                mp_count++;
            }

            #endregion

            // refresh hlmv
            if (cb_refresh_SkinPaintChange.Checked)
            {
                update_skins(false);
            }

            switch_player_skin(selected_team_skin_index);

            refresh_hlmv(false);
        }





        private void cb_skins_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush bgcolor = Brushes.Silver;

            if (e.Index == 0) { bgcolor = new SolidBrush(Color.FromArgb(255, 184, 56, 59)); }
            if (e.Index == 1) { bgcolor = new SolidBrush(Color.FromArgb(255, 88, 133, 162)); }

            e.DrawBackground();

            string text = ((ComboBox)sender).Items[e.Index].ToString();

            e.Graphics.FillRectangle(bgcolor, e.Bounds);
            e.Graphics.DrawString(text, ((Control)sender).Font, new SolidBrush(Color.WhiteSmoke), e.Bounds.X, e.Bounds.Y);
        }

        // reset all VMTpainters paints to basecolor
        private void btn_reset_paints_Click(object sender, EventArgs e)
        {
            auto_refresh_busy = true;

            // if HLMV is running
            if (proc_HLMV != null) if (!proc_HLMV.HasExited)

                    colorPicker_master.EditPaint("ColorTint Base:" + "255 255 255");
            colorPicker_master.SelectedIndex = 0;

            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    if (vmt.color_picker.Enabled)
                    {
                        vmt.color_picker.SelectedIndex = 0;
                    }
                }
            }

            switch_team_skin(selected_team_skin_index);

            auto_refresh_busy = false;
        }

        // set all VMTpainters to one paint
        private void colorPicker1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // if HLMV is running
            if (proc_HLMV != null) if (!proc_HLMV.HasExited)

                    // set to busy so the VMT_painter ColorPickers do not try to refresh the paints too (as it would unecesarily try to refresh several times for every VMT)
                    auto_refresh_busy = true;


            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    if (vmt.color_picker.Enabled)
                    {
                        // make sure we don't go out of bounds
                        if (colorPicker_master.SelectedIndex <= vmt.color_picker.Items.Count)
                            vmt.color_picker.SelectedIndex = colorPicker_master.SelectedIndex;
                    }
                }
            }


            update_skins(true);

            auto_refresh_busy = false;
        }

        public void update_vmt_paint(int _id)
        {

            foreach (var mp in skins_manager_control.Controls.OfType<Model_Painter>())
            {
                foreach (var vmt in mp.Controls.OfType<VMT_Painter>())
                {
                    if (vmt.color_picker.Enabled)
                    {
                        if (vmt.id == _id)
                        {
                            // vmt.color_picker.SelectedIndex = colorPicker_master.SelectedIndex;
                        }

                    }
                }
            }

            update_skins(true);
        }


        #endregion



        #region HLMV


        // copy/extract models to tf/custom/TFMV/models
        // set HLMV main model settings and attachments
        // launch HLMV
        private void loadout_to_hlmv()
        {

            string mdlpath = txtb_main_model.Text;

            #region get main model to "tf/custom/TFMV/models"

            // check if main .mdl exists in tf/models, in VPK or in tf/custom/TFMV/models
            // if it doesn't exist in drive, we copy it to TF/custom/models
            // so we can load the model directly to HLMV by argument (without having to manually load recent files or F5)
            if (File.Exists(steamGameConfig.tf_dir + mdlpath))
            {
                string mdl_name = mdlpath.Replace(".mdl", "");
                string mdl_in = steamGameConfig.tf_dir + mdl_name;
                string mdl_out = tfmv_dir + mdl_name;

                miscFunc.copy_safe(mdl_in + ".mdl", mdl_out + ".mdl");
                miscFunc.copy_safe(mdl_in + ".dx80.vtx", mdl_out + ".dx80.vtx");
                miscFunc.copy_safe(mdl_in + ".dx90.vtx", mdl_out + ".dx90.vtx");
                miscFunc.copy_safe(mdl_in + ".sw.vtx", mdl_out + ".sw.vtx");
                miscFunc.copy_safe(mdl_in + ".sw.vtx", mdl_out + ".sw.vtx");
                miscFunc.copy_safe(mdl_in + ".vvd", mdl_out + ".vvd");
                miscFunc.copy_safe(mdl_in + ".phy", mdl_out + ".phy");
            }
            else
            { // if found in VPK, extract it to TF/custom/TFMV/...
                #region extract model

                if (VPK.Find(mdlpath, 0))
                {
                    string mdl_name = mdlpath.Replace(".mdl", "");
                    string mdl_in = mdl_name;
                    string mdl_out = tfmv_dir + mdl_name;
                    VPK.Extract(mdl_in + ".mdl", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    VPK.Extract(mdl_in + ".dx80.vtx", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    VPK.Extract(mdl_in + ".dx90.vtx", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    VPK.Extract(mdl_in + ".sw.vtx", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    VPK.Extract(mdl_in + ".vvd", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);

                    if (VPK.Find(mdl_in + ".phy", 0))
                    {
                        VPK.Extract(mdl_in + ".phy", tfmv_dir + Path.GetDirectoryName(mdlpath), 0);
                    }
                }

                #endregion
            }


            // fix wrist bug
            if (cb_fix_hlp_bones.Checked)
            {
                mdl_disable_hlp_bones(tfmv_dir + txtb_main_model.Text);
            }

            // disable jigglebones
            if (cb_disable_jigglebones.Checked)
            {
                mdl_disable_all_jigglebones_in_folder(tmp_dir, tfmv_dir);
            }
            else
            {
                restore_mdls_in_folder(tmp_dir, tfmv_dir);
            }


            // custom cubemaps
            //todo: allow user to select from a list instead of just using 2fort!
            if (cb_cubemap.Checked)
            {
                install_custom_cubemap("Maps/2fort.vtf");

                refresh_hlmv(false);
            }
            else
            {
                remove_custom_cubemap();

                refresh_hlmv(false);
            }

            #endregion

            #region extract and copy model attachments to tf/custom/TFMV/models

            int max_models = 12;
            // Add TFMV background model if transparent screenshots are enabled in the screenshot settings
            if (cb_screenshot_transparency.Checked) { max_models = 11; }

            // HLMV model attachments 12 max
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {
                if (i > max_models) { break; }

                string model_path = ((Loadout_Item)loadout_list.Controls[i]).model_path.ToString();

                //neodement: todo: rough bit of code to warn the user that zombie souls don't work properly (yet?)
                if (((Loadout_Item)loadout_list.Controls[i]).item_name.ToString().Contains("Voodoo-Cursed "))
                {

                    if (((Loadout_Item)loadout_list.Controls[i]).item_name.ToString() == "Voodoo-Cursed Spy Soul")
                    {
                        MessageBox.Show("WARNING! To view the Voodoo-Cursed Spy Soul correctly in HLMV, click the Model tab and select Skin 22 for Red Team or Skin 23 for Blue team. TFMV Bodygroup selecting is currently not supported for zombie skins but can be overridden using the Submodel dropdowns, at the top of the Model tab.");
                    }
                    else
                    {
                        MessageBox.Show("WARNING! To view Voodoo-Cursed Zombie Souls correctly in HLMV, click the Model tab and select Skin 4 for Red Team or Skin 5 for Blue team. TFMV Bodygroup selecting is currently not supported for zombie skins but can be overridden using the Submodel dropdowns, at the top of the Model tab.");
                    }
                }

                if (model_path != "")
                {

                    string filename = Path.GetFileName(model_path);


                    if (File.Exists(tfmv_dir + model_path))
                    {
                        // copy mdl file to tmp (so we can get the materials paths)
                        File.Copy(tfmv_dir + model_path, tmp_dir + filename, true);
                    }
                    else if (File.Exists(steamGameConfig.tf_dir + model_path))
                    {
                        miscFunc.copy_safe(steamGameConfig.tf_dir + model_path, tmp_dir + Path.GetFileName(model_path));
                    }


                }

            }

            #endregion





            //neodement: extract head texture from vpk and apply lod fix


            miscFunc.create_missing_dir(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\" + get_player_path_by_class(selected_player_class));

            //TODO: by next release! make fixing head textures optional (but on by default)
            //if (cb_lodclamps.Checked)
            //{
            if (selected_player_class == "spy")
            {

                vtf_fix_lod_uv_clamps(get_player_path_by_class(selected_player_class), selected_player_class + "_head_red.vtf");
                vtf_fix_lod_uv_clamps(get_player_path_by_class(selected_player_class), selected_player_class + "_head_blue.vtf");
            }
            else if (selected_player_class != "pyro")
            {
                vtf_fix_lod_uv_clamps(get_player_path_by_class(selected_player_class), selected_player_class + "_head.vtf");
            }

            //}

            reg_create_hlmv_model(tfmv_dir.Replace(":", ".") + mdlpath);

            launch_hlmv(mdlpath);
        }



        // convert 3 textbox to string for HLMV regedit  vector3 values
        private string vector3_TextBox_toString(TextBox X, TextBox Y, TextBox Z)
        {
            return X.Text.Replace(",", ".") + " " + Y.Text.Replace(",", ".") + " " + Z.Text.Replace(",", ".");
        }

        // convert 3 textbox to string for HLMV regedit RGB values
        private string rgb_TextBox_toString(Color clr)
        {
            return (Convert.ToSingle(clr.R) / 255).ToString().Replace(",", ".") + " " + (Convert.ToSingle(clr.G) / 255).ToString().Replace(",", ".") + " " + (Convert.ToSingle(clr.B) / 255).ToString().Replace(",", ".");
        }

        private void reg_create_hlmv_model(string modelname)
        {

            string posx = txtb_hlmv_campos_x.Text.Replace(",", ".");
            string posy = txtb_hlmv_campos_y.Text.Replace(",", ".");
            string posz = txtb_hlmv_campos_z.Text.Replace(",", ".");

            string rotx = txtb_hlmv_camrot_x.Text.Replace(",", ".");
            string roty = txtb_hlmv_camrot_y.Text.Replace(",", ".");
            string rotz = txtb_hlmv_camrot_z.Text.Replace(",", ".");
            // test
            string light_rotx = txtb_hlmv_lightrot_x.Text.Replace(",", ".");
            string light_roty = txtb_hlmv_lightrot_y.Text.Replace(",", ".");
            string light_rotz = txtb_hlmv_lightrot_z.Text.Replace(",", ".");


            string color_R = (Convert.ToSingle(panel_Bgcolor.BackColor.R) / 255).ToString().Replace(",", ".");
            string color_G = (Convert.ToSingle(panel_Bgcolor.BackColor.G) / 255).ToString().Replace(",", ".");
            string color_B = (Convert.ToSingle(panel_Bgcolor.BackColor.B) / 255).ToString().Replace(",", ".");

            string aColor_R = (Convert.ToSingle(panel_aColor.BackColor.R) / 255).ToString().Replace(",", ".");
            string aColor_G = (Convert.ToSingle(panel_aColor.BackColor.G) / 255).ToString().Replace(",", ".");
            string aColor_B = (Convert.ToSingle(panel_aColor.BackColor.B) / 255).ToString().Replace(",", ".");

            string lColor_R = (Convert.ToSingle(panel_lColor.BackColor.R) / 255).ToString().Replace(",", ".");
            string lColor_G = (Convert.ToSingle(panel_lColor.BackColor.G) / 255).ToString().Replace(",", ".");
            string lColor_B = (Convert.ToSingle(panel_lColor.BackColor.B) / 255).ToString().Replace(",", ".");

            modelname = modelname.Replace("\\", ".");
            modelname = modelname.Replace("/", ".");

            // HKEY_CURRENT_USER\Software\Valve\hlmv
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Valve\\hlmv\\" + modelname);
            key.SetValue("aColor", "(" + aColor_R + " " + aColor_G + " " + aColor_B + " " + " 0.000000)");
            key.SetValue("bgColor", "(" + color_R + " " + color_G + " " + color_B + " " + " 0.000000)");// "(0.549020 0.603922 0.654902 0.000000)");
            key.SetValue("cclanguageid", 0);
            key.SetValue("enablenormalmapping", 1);
            key.SetValue("gColor", "(0.850000 0.850000 0.690000 0.000000)");

            if (!cb_disable_light_rotCol.Checked)
            {
                key.SetValue("lColor", "(" + lColor_R + " " + lColor_G + " " + lColor_B + " " + " 0.000000)");
                key.SetValue("lightrot", "(" + light_rotx + " " + light_roty + " " + light_rotz + ")");
            }

            int max_models = 12;
            // Add TFMV background model if its enabled in the screenshot settings
            //neodement: if "Disable Background" is checked, don't load it
            if (cb_screenshot_transparency.Checked && !cb_disable_background.Checked)
            {
                max_models = 11;
                // set 12th attachment as TFMV background
                key.SetValue("merge" + (12), @"models\TFMV\tfmv_bg.mdl");
            }

            // reset attachments // better delete the whole thing though
            for (int i = 0; i < max_models; i++)
            {
                key.SetValue("merge" + (i + 1), "");
            }

            // HLMV model attachments 12 max
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {
                if (i > max_models) { break; }
                key.SetValue("merge" + (i + 1), ((Loadout_Item)loadout_list.Controls[i]).model_path);

            }

            // tested and nope HLMV won't take more than 4 merges
            // key.SetValue("merge4", "models/player/items/medic/medic_goggles.mdl");
            key.SetValue("overlaySequence0", txtb_pose.Text);
            key.SetValue("overlaySequence1", txtb_pose.Text);
            key.SetValue("overlaySequence2", txtb_pose.Text);
            key.SetValue("overlaySequence3", txtb_pose.Text);
            key.SetValue("overlayWeight0", "0.000000");
            key.SetValue("overlayWeight1", "0.000000");
            key.SetValue("overlayWeight2", "0.000000");
            key.SetValue("overlayWeight3", "0.000000");
            key.SetValue("playsounds", 0);
            if (!cb_disable_cam_rotPos.Checked)
            {
                key.SetValue("Rot", "(" + rotx + " " + roty + " " + rotz + ")");
            }

            key.SetValue("Sequence", txtb_pose.Text);
            int showbg = 0;
            if (bg_toggle) { showbg = 1; }
            key.SetValue("showbackground", showbg);
            key.SetValue("showground", 0);
            key.SetValue("showillumpos", 0);
            key.SetValue("showshadow", 0);
            key.SetValue("speechapiindex", 0);
            key.SetValue("speedscale", "0.000000");
            key.SetValue("thumbnailsize", 128);
            key.SetValue("thumbnailsizeanim", 128);
            if (!cb_disable_cam_rotPos.Checked)
            {
                key.SetValue("Trans", "(" + posx + " " + posy + " " + posz + ")");
            }


            key.SetValue("viewermode", 0);
            key.Close();
        }

        private void launch_hlmv(string mdl_path)
        {

            #region setup HLMV process

            string args = "-game \"" + steamGameConfig.tf_dir.Replace("tf\\", "tf") + "\"" + " \"" + tfmv_dir + mdl_path + "\"";

            close_hlmv();

            //MessageBox.Show("backing up rf file now");

            //neodement: don't delete recent files list. back it up instead and then restore it when we close tfmv.
            //(this is to stop the latest entry in the file list being a non-existent player model in the custom folder)
            if (check_sdk_tools())
            {

                //  HLMV recent files
                string rf_path = steamGameConfig.tf2_dir + "bin\\hlmv.rf";

                if (File.Exists(rf_path))
                {
                    //FileInfo fi = new FileInfo(rf_path);
                    //fi.Attributes = FileAttributes.Normal;
                    hlmv_rf_backup = File.ReadAllBytes(rf_path);


                    //delete it if it isn't exactly 2048 bytes, as this causes hlmv to crash on launch
                    if (hlmv_rf_backup.Length != 2048)
                    {
                        //todo: ERROR MAKES NO SENSE! FIX!
                        MessageBox.Show("Warning! Deleted corrupted recent files list! (hlmv.rf)");
                        miscFunc.delete_safe(rf_path);
                    }
                }


            }

            /*
                string temp = "";

                foreach (int val in hlmv_rf_backup)
                {
                    if (val != 0)
                    {
                        temp += (char)val;
                    }
                }

                MessageBox.Show("backup complete! here's what we got:" + temp);
            */

            proc_HLMV = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = steamGameConfig.tf_dir,
                    FileName = steamGameConfig.tf2_dir + "bin\\hlmv.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            if (OS_settings.os_version == "win_xp")
            {
                proc_HLMV.StartInfo.UseShellExecute = true;
            }

            #endregion

            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
            proc_HLMV.Start();

            int WaitTime_max = 8000;
            int WaitTime = 0;

            // wait until HLMV is fully started to scale window
            while (string.IsNullOrEmpty(proc_HLMV.MainWindowTitle))
            {



                if (WaitTime >= WaitTime_max)
                {
                    break;
                }
                WaitTime += 100;
                System.Threading.Thread.Sleep(WaitTime);
                proc_HLMV.Refresh();

                if (proc_HLMV.HasExited)
                {
                    // reset to loadout panel but keep loadout list
                    reset_loadout(true);

                    MessageBox.Show("The process hlmv.exe (model viewer) has closed unexpectedly." +
                        "\n\nMake sure TF2 is updated and not in the middle of an update, verify integrity of files." +
                    "\n\nIf the problem persists, try running 'set_sdk_env.bat' and 'hlmv.bat' located in " + steamGameConfig.bin_dir);

                    return;
                }
            }

            #region set HLMV window size

            if (!cb_disable_window.Checked)
            {
                if ((proc_HLMV == null) || (proc_HLMV.HasExited))
                {
                    //MessageBox.Show("Cannot resize window, HLMV is not running.");
                    return;
                }

                // make number integer
                if (txtb_hlmv_def_wsize_x.Text.Contains(".")) { txtb_hlmv_def_wsize_x.Text = txtb_hlmv_def_wsize_x.Text.Split('.')[0]; }
                if (txtb_hlmv_def_wsize_y.Text.Contains(".")) { txtb_hlmv_def_wsize_y.Text = txtb_hlmv_def_wsize_y.Text.Split('.')[0]; }
                txtb_hlmv_def_wsize_x.Text = Regex.Replace(txtb_hlmv_def_wsize_x.Text, "[^0-9]", "");
                txtb_hlmv_def_wsize_y.Text = Regex.Replace(txtb_hlmv_def_wsize_y.Text, "[^0-9]", "");

                int x = Convert.ToInt32(txtb_hlmv_def_wsize_x.Text);
                int y = Convert.ToInt32(txtb_hlmv_def_wsize_y.Text);

                var rect = new Rect();
                GetWindowRect(proc_HLMV.MainWindowHandle, ref rect);

                // padding of the HLMV panels so we scale the model viewer area
                int width = x + hlmv_padding.left;
                int height = y + hlmv_padding.bottom;

                SetWindowPos(proc_HLMV.MainWindowHandle, HWND_TOP, rect.left, rect.top, width, height, 0);
            }

            #endregion
        }


        private void close_hlmv()
        {
            // check if HLMV process is running
            // and closes it
            if (Process_IsRunning(proc_HLMV))
            {
                try
                {
                    proc_HLMV.CloseMainWindow();
                    proc_HLMV.Close();
                    proc_HLMV.Dispose();

                    //proc_HLMV.WaitForExit();

                    // wait a bit for process to be closed
                    Thread.Sleep(300);

                    /*
                    // if failed to close, kill process
                    if (Process_IsRunning(proc_HLMV))
                    {
                        proc_HLMV.Kill();
                    }
                    */

                    // clear process var
                    proc_HLMV = null;

                    //neodement: todo: ?      wait a bit longer for process to be closed or it overwrites our rf file as soon as we write it...
                    Thread.Sleep(300);

                    //neodement: if we've stored a backup of the .rf file, restore it now
                    if (hlmv_rf_backup.Length == 2048)
                    {

                        /*
                        string temp = "";

                        foreach (int val in hlmv_rf_backup)
                        {
                            if (val != 0)
                            {
                                temp += (char)val;
                            }
                        }

                        MessageBox.Show("restoring rf file: " + temp);
                        */

                        //  HLMV recent files
                        string rf_path = steamGameConfig.tf2_dir + "bin\\hlmv.rf";

                        //make sure it's writable
                        if (File.Exists(rf_path))
                        {
                            FileInfo fi = new FileInfo(rf_path);
                            fi.Attributes = FileAttributes.Normal;
                        }


                        File.WriteAllBytes(rf_path, hlmv_rf_backup);

                        //Array.Clear(hlmv_rf_backup, 0, hlmv_rf_backup.Length);

                        //better way of clearing the array...
                        hlmv_rf_backup = new byte[0];
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while closing HLMV.exe/n" + ex.Message);
                }
            }
        }

        // load loadout to HLMV
        private void btn_refresh_loadout_Click(object sender, EventArgs e)
        {
            loadout_to_hlmv();
        }



        private string find_class_inString(string str)
        {
            string found = "";
            if (str.Contains("scout")) { found = "scout"; }
            if (str.Contains("demo")) { found = "demo"; }
            if (str.Contains("engineer")) { found = "engineer"; }
            if (str.Contains("medic")) { found = "medic"; }
            if (str.Contains("heavy")) { found = "heavy"; }
            if (str.Contains("pyro")) { found = "pyro"; }
            if (str.Contains("sniper")) { found = "sniper"; }
            if (str.Contains("soldier")) { found = "soldier"; }
            if (str.Contains("spy")) { found = "spy"; }

            return found;

        }

        // returns model attachment path from given index
        private string get_attachment(int item_index)
        {
            if (item_index <= loadout_list.Controls.Count)
            {
                return ((Loadout_Item)loadout_list.Controls[item_index]).model_path;
            }

            return "";
        }

        // attempt to force HLMV anti aliasing by editing dxsupport.cfg
        private void set_hlmv_antialias(string level)
        {
            string filepath = steamGameConfig.tf2_dir + "bin\\dxsupport.cfg";
            if ((File.Exists(filepath) == false) || (level == "") || (level == " ")) { return; }

            List<string> cfg_raw_list = new List<string>(), cfg_lines = new List<string>();

            try
            {
                // vmt_raw_list = File.ReadAllLines(filepath);
                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((rline = file.ReadLine()) != null)
                {
                    cfg_raw_list.Add(rline);
                    cfg_lines.Add(rline);
                }

                file.Close();

                // remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < cfg_lines.Count; i++)
                {
                    cfg_lines[i] = ((regex.Replace(cfg_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");
                }

                for (int s = 0; s < cfg_lines.Count; s++)
                {
                    string line = cfg_lines[s].ToLower();
                    // make sure its not a line that's commented out
                    if (line != "")
                    {
                        string[] line_split = line.Split('"');

                        if (line_split.Length > 1)
                        {
                            // find color2 and get it
                            if (line_split[1] == "convar.mat_antialias")
                            {
                                cfg_raw_list[s] = "\t\t\"ConVar.mat_antialias\" \"" + level + "\"";
                                // break;
                            }
                        }


                    }
                }

                File.WriteAllLines(filepath, cfg_raw_list);
            }
            catch (IOException)
            {
                MessageBox.Show("Error: could not write HLMV anti-aliasing setting.");
            }
        }


        public static void refresh_hlmv(bool refresh_delay, int delay_rate = 0)
        {
            // check if HLMV process is running
            if (Process_IsRunning(proc_HLMV) && (proc_HLMV != null) && (!proc_HLMV.HasExited))
            {
                // refreshes fast but only displays changes when the process window is clicked by the user :/
                PostMessage(proc_HLMV.MainWindowHandle, WM_KEYDOWN, VK_F5, 0);

                SetForegroundWindow(proc_HLMV.MainWindowHandle);

                if (refresh_delay)
                    Thread.Sleep(delay_rate);

                /* this method freezes the mouse while HLMV refreshes... :/
                IntPtr h = proc_HLMV.MainWindowHandle;
                SetForegroundWindow(h);
                SendKeys.Send("{F5}");
                 */
            }
        }

        // check if process is running (if has window still open) and return bool
        public static bool Process_IsRunning(Process process)
        {
            if (process == null)
            {
                // throw new ArgumentNullException("process");
                return false;
            }

            try
            {
                if (process == null) { return false; }
                if (process.Id == -1) { return false; }
                if (process.HasExited) { return false; }

                Process.GetProcessById(process.Id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }


        static string CalculateFileMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        // rewrites hard coded FOV value in hlmv.exe
        // note: disabled because if Valve updates hlmv.exe the address for the FOV in the .exe changes
        // causing TFMV to corrupt hlmv.exe
        private void set_hlmv_fov()
        {
            return;

            /*
           if (cb_disable_fov_mod.Checked == true)
           {
               return;
           }



           string filepath = steamGameConfig.tf2_dir + "bin\\hlmv.exe";

           if (!File.Exists(filepath)) { return; }


           // cleanup string to make sure its an integer
           if (txtb_hlmv_fov.Text.Contains(".")) { txtb_hlmv_fov.Text = txtb_hlmv_fov.Text.Split('.')[0]; }
           txtb_hlmv_fov.Text = Regex.Replace(txtb_hlmv_fov.Text, "[^0-9]", "");


           if(proc_HLMV != null)
           {
               proc_HLMV.CloseMainWindow();
               Thread.Sleep(200);
           }

           try
           {
               using (BinaryWriter writer = new BinaryWriter(File.Open(filepath, FileMode.Open)))
               {
                   writer.Seek(190257, SeekOrigin.Begin);
                   writer.Write(Convert.ToSingle(txtb_hlmv_fov.Text));


                   writer.Seek(382144, SeekOrigin.Begin);
                   writer.Write(System.Text.Encoding.UTF8.GetBytes(txtb_hlmv_fov.Text.Trim()));
               }
           }
           catch {   }
           */

        }


        #endregion


        #region .mdl functions


        // Read .mdl file, gets VMT materials paths into a list 
        // Check game directory and the game's VPK to verify paths
        // (Game directory "TF" materials will take priority over the game's VPK materials)   
        private List<Object> Get_MDL_Materials(string model_name)
        {
            string mdl_file = tmp_dir + model_name;
            //string mdl_file = @"C:\models\bonk_helmet.mdl";

            List<string> materials = new List<string>();
            List<string> cd_materials = new List<string>();
            // create list of ints for the family count
            List<List<int>> SkinFamilies = new List<List<int>>();


            if (File.Exists(mdl_file) != true)
            {
                MessageBox.Show("Error: could not find file: " + mdl_file);
                return null;
            }


            #region Read Model file
            using (BinaryReader b = new BinaryReader(File.Open(mdl_file, FileMode.Open)))
            {

                int length = (int)b.BaseStream.Length;

                #region Get Header info

                // jump headers and stuff directly to what we want
                b.BaseStream.Position = 204;

                // materials (textures)
                int mat_count = b.ReadInt32();
                int mat_offset = b.ReadInt32();

                // cd materials (materials directories)
                int cdmat_count = b.ReadInt32();
                int cdmat_offset = b.ReadInt32();

                // skins families
                int SkinReferenceCount = b.ReadInt32();
                int SkinFamilyCount = b.ReadInt32();
                int SkinFamilyOffset = b.ReadInt32();

                b.BaseStream.Position = mat_offset;
                int mat_name_offset = b.ReadInt32();

                #endregion

                #region Get Materials
                // for every material, we read the header (64 bytes)
                // which points to the real offset for the string 
                for (int i = 0; i < mat_count; i++)
                {
                    b.BaseStream.Position = mat_offset + (i * 64);

                    char tmpchar = '#';
                    string tmpstr = "";

                    // read material header to get the material string start offset
                    b.BaseStream.Position = mat_offset + (i * 64) + b.ReadInt32();

                    // read char by char until we hit the null character (0)
                    while (tmpchar != 0)
                    {
                        tmpstr = tmpstr + Convert.ToString(tmpchar);
                        tmpchar = b.ReadChar();
                    }

                    if (tmpstr.Trim('#') != "")
                    {
                        string[] ss = Regex.Split((tmpstr.Trim('#')).Replace("\\", "/"), "/");

                        // add if it doesn't already exist in list
                        if (!materials.Contains(ss[ss.Length - 1]))
                        {
                            materials.Add(ss[ss.Length - 1]);
                        }
                    }
                }

                #endregion

                #region Get CD Materials

                // for every CD material, we get the position every (4 bytes)
                // which points to the real offset for the string 
                for (int i = 0; i < cdmat_count; i++)
                {
                    #region read cdmaterials common offset

                    b.BaseStream.Position = cdmat_offset + (i * 4);
                    // read material header to get the material string start offset
                    b.BaseStream.Position = b.ReadInt32();

                    char tmpchar = '#';
                    string tmpstr = "";

                    // read char by char until we hit the null character (0)
                    while (tmpchar != 0)
                    {
                        tmpstr = tmpstr + Convert.ToString(tmpchar);
                        tmpchar = b.ReadChar();
                    }

                    // make sure that its not null
                    if (tmpstr.Trim('#') != "")
                    {
                        string s = (tmpstr.Trim('#').Replace("\\", "/"));

                        if (s.Substring(0, 1) == "/")
                        {
                            s = s.Remove(0, 1);
                        }

                        // add if it doesn't already exist in list
                        if (!cd_materials.Contains(s))
                        {
                            cd_materials.Add(s);
                        }
                    }

                    #endregion

                    #region read cdmaterials alternative offset

                    b.BaseStream.Position = mat_offset + mat_name_offset;

                    tmpchar = '#';
                    tmpstr = "";

                    // read char by char until we hit the null character (0)
                    while (tmpchar != 0)
                    {
                        if (b.BaseStream.Position >= b.BaseStream.Length) { break; }

                        tmpstr = tmpstr + Convert.ToString(tmpchar);
                        tmpchar = b.ReadChar();
                    }


                    // make sure that its not null
                    if (tmpstr.Trim('#') != "")
                    {
                        string s = (tmpstr.Trim('#').Replace("\\", "/"));

                        if (s.Substring(0, 1) == "/")
                        {
                            s = s.Remove(0, 1);
                        }
                        // add if it doesn't already exist in list
                        if (!cd_materials.Contains(s))
                        {
                            cd_materials.Add(s);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Get Skin Families

                // skins
                // skinreference_count = number of materials
                // skinrfamily_count = number of skin families (red, blue, uber...)
                int skintable_size = SkinReferenceCount * SkinFamilyCount;

                b.BaseStream.Position = SkinFamilyOffset;
                // for every skinFamilyCount
                for (int i = 0; i < SkinFamilyCount; i++)
                {

                    List<int> aSkinFamily = new List<int>();

                    for (int j = 0; j < SkinReferenceCount; j++)
                    {
                        aSkinFamily.Add(b.ReadInt16());
                    }

                    SkinFamilies.Add(aSkinFamily);
                }

                #region remove skinRef from skin families


                int index = 0;
                for (int currentSkinRef = SkinReferenceCount - 1; currentSkinRef >= 0; currentSkinRef += -1)
                {
                    for (index = 0; index < SkinFamilyCount; index++)
                    {
                        int aSkinRef = 0;
                        // aSkinRef = this.theMdlFileData.theSkinFamilies(index)(currentSkinRef);
                        aSkinRef = SkinFamilies[index][currentSkinRef];

                        if (aSkinRef != currentSkinRef)
                        {
                            break;
                        }
                    }

                    if (index == SkinFamilyCount)
                    {
                        for (index = 0; index < SkinFamilyCount; index++)
                        {
                            // this.theMdlFileData.theSkinFamilies(index).RemoveAt(currentSkinRef);
                            SkinFamilies[index].RemoveAt(currentSkinRef);
                        }
                        SkinReferenceCount -= 1;
                    }
                }

                #endregion

                #endregion

            } // end using BinaryReader

            #endregion

            #region Get real path for the materials

            // some materials don't have a directory path, for instance this is what the mdl might give us:
            // models/player/items/scout/bonk_helm  (red)
            // bonk_helm_blu  (blue)

            // the second doesn't have a path, so we try to find it, given the "CD_materials" (materials directory)
            // by searching in the VPK, but if it already exists in "TF"
            // TF takes priority, in this case a custom material, we wouldn't want to overwrite or delete a user custom material...

            string mat;

            foreach (var cd_mat in cd_materials)
            {
                for (int m = 0; m < materials.Count; m++)
                {
                    mat = materials[m];

                    // if material doesn't have a directory
                    if (mat.Contains("/") == false)
                    {

                        string mat_path_guess = "";
                        if (cd_mat.EndsWith("/"))
                        {
                            mat_path_guess = ("materials/" + cd_mat + "/" + mat).Replace("//", "/");
                        } else {

                            mat_path_guess = ("materials/" + cd_mat).Replace("//", "/");
                        }

                        // search material file in TF
                        if (File.Exists(steamGameConfig.tf_dir + mat_path_guess + ".vmt"))
                        {
                            materials[m] = mat_path_guess + ".vmt";
                        }

                        else if (File.Exists(steamGameConfig.tf_dir + "custom/TFMV/" + mat_path_guess + ".vmt"))
                        {
                            materials[m] = mat_path_guess + ".vmt";
                        }
                        else // if we don't find the material in TF, we search it in the VPK
                        {
                            // we add a cd_material path and check if it exists in the VPK
                            if ((VPK.Find(mat_path_guess + ".vmt", 0)) && (!materials.Contains(mat_path_guess + ".vmt")))
                            {
                                // in case the cd materials is correct
                                // models\weapons\c_items\
                                materials[m] = mat_path_guess + ".vmt";
                            }
                        }
                    }
                }
            }

            // if material path is still not found
            // in case where cd_materials is the full material path:
            // models\weapons\c_items\c_lochnload (.vmt)

            for (int c = 0; c < cd_materials.Count; c++)
            {
                for (int m = 0; m < materials.Count; m++)
                {
                    if (materials[m].Contains("/") == false)
                    {
                        string trimmed = cd_materials[c].Substring(0, cd_materials[c].LastIndexOf('/'));
                        string mat_path_guess = "materials/" + trimmed;

                        materials[m] = mat_path_guess + ".vmt";
                    }
                }
            }

            #endregion

            List<Object> materials_list = new List<Object>();

            materials_list.Add(materials);
            materials_list.Add(SkinFamilies);

            return materials_list;
        }

        //VTF
        //this method automatically fixes incorrectly set Clamp U/V values for texture LODs/mipmaps

        //todo: more error checking!

        private void vtf_fix_lod_uv_clamps(string path, string filename)
        {
            //fixup path to include materials subdir
            path = "materials/" + path;

            try
            {
                //extract head texture vtf from vpk to tmp folder
                VPK.Extract(path + filename, tmp_dir + path, 1);

                //MessageBox.Show(tmp_dir + "materials\\" + HeadTexturePath);


                byte[] vtf_data = File.ReadAllBytes(tmp_dir + path + filename);

                //check for LOD block


                if (vtf_data[104] == 0x4C && //L
                    vtf_data[105] == 0x4F && //O
                    vtf_data[106] == 0x44    //D
                    )
                {
                    //108 (0x6C) and 109 (0x6D) are LOD UV clamp values. the ideal values for a 1024x1024 texture are 0x0A (10) and 0x0A (10).

                    vtf_data[108] = 0x0A;
                    vtf_data[109] = 0x0A;
                }


                //copy head texture
                File.WriteAllBytes(tfmv_dir + path + filename, vtf_data);
            }
            catch
            {
            }

        }



        //this method replaces the default hlmv cubemap with a nicer one.

        //todo: expand this to allow selecting custom cubemaps from a list!

        private void install_custom_cubemap(string filename)
        {

            //"Maps/2fort.vtf"

            try
            {
                //copy cubemap texture (don't worry about the copy of cubemap in /editor/, hlmv only needs the /hlmv/ one)
                miscFunc.copy_safe(cubemaps_dir + filename, tfmv_dir + "materials/hlmv/cubemap.vtf");
            }
            catch
            {
            }

        }


        //this method removes any custom cubemaps from the tfmv custom dir.

        private void remove_custom_cubemap()
        {

            try
            {
                //delete cubemap texture if it exists
                miscFunc.delete_safe(tfmv_dir + "materials/hlmv/cubemap.vtf");
            }
            catch
            {
            }

        }


        // this method edits the .mdl file to disable the 'hlp_forearm_' bones, 
        // to get rid of HLMV wrist visual bug where the geometry weighted to this bone renders over itself
        // (aka the wrist area draws on top of the body, even if it's behind it)
        // reads .mdl file, gets string table and looks for bone names "hlp_forearm_"
        // if found, renames hlp_forearm_ to xxx_forearm_ (so the wrist vertices will no longer be weighted to the wrist bone causing the issue)
        private void mdl_disable_hlp_bones(string filepath)
        {
            if (!File.Exists(filepath)) { return; }


            try
            {


                byte[] mdl_data = File.ReadAllBytes(filepath);

                int bone_names_list_offset = BitConverter.ToInt32(mdl_data, 348);
                int bone_names_list_length = mdl_data.Length - bone_names_list_offset;



                byte[] string_list = new byte[bone_names_list_length];
                Array.Copy(mdl_data, bone_names_list_offset, string_list, 0, bone_names_list_length);

                System.Text.Encoding enc = System.Text.Encoding.UTF8;
                string bone_list_strings = enc.GetString(string_list);


                // if model bone list has the bone name "hlp_forearm_"
                if (bone_list_strings.Contains("hlp_forearm_"))
                {
                    bone_list_strings = bone_list_strings.Replace("hlp_forearm_", "xxx_forearm_");

                    byte[] string_list_edited = enc.GetBytes(bone_list_strings);

                    // copy modified strings to original file
                    Buffer.BlockCopy(string_list_edited, 0, mdl_data, bone_names_list_offset, bone_names_list_length);

                    File.WriteAllBytes(filepath, mdl_data);
                }
            }
            catch { }
        }




        // this method gets all .mdl files from the tmp folder and disables jigglebones.
        // could maybe be used to edit other parts of the mdl later.

        private void mdl_disable_all_jigglebones_in_folder(string source, string target)
        {

            //this is based on move_dir_safe(); and could probably be cleaned up more... but it works


            if (!Directory.Exists(source) || !Directory.Exists(target))
            {
                return;
            }

            var stack = new Stack<miscFunc.Folders>();
            stack.Push(new miscFunc.Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.mdl"))
                {
                    if (Path.GetDirectoryName(file) == tmp_dir) return;
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    //File.Move(file, targetFile);

                    //todo:? quick fix to stop a null error (but why is it happening in the first place?)
                    byte[] mdl_data_removed_jigglebones = mdl_disable_jigglebones(file);

                    if (mdl_data_removed_jigglebones != null)
                    {
                        //replace or create the file in tfmv custom directory
                        File.WriteAllBytes(targetFile, mdl_data_removed_jigglebones);
                    }
                    else
                    {
                        //MessageBox.Show(targetFile + " error");
                    }

#if DEBUG
                    //                    MessageBox.Show("attempted to remove some jigglebones from " + targetFile);
#endif
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new miscFunc.Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            //Directory.Delete(source, true);

        }



        // this method edits the .mdl file to disable any jiggle bones, 
        // reads .mdl file, gets bone count and offset and looks for bones marked as jigglebones
        // if found, changes bone type to normal

        //(returns an array of bytes to write back to a file)

        private byte[] mdl_disable_jigglebones(string filepath)
        {


            if (!File.Exists(filepath)) { return null; }

            try
            {

                byte[] mdl_data = File.ReadAllBytes(filepath);

                //156 is the offset to the bone count
                int bone_count = BitConverter.ToInt32(mdl_data, 156);

                //MessageBox.Show("how many bones? this many: " + bone_count);

                //neodement: 160 is the offset to the first bone in the bone data (seems to always be 664)
                int bone_offset = BitConverter.ToInt32(mdl_data, 160);


                //iterate over each bone
                for (int i = 1; i <= bone_count; i++)

                {

                    //check if it's a jigglebone or not
                    //the 164th byte is the procedural bone type.

                    byte bone_type = mdl_data[bone_offset + 164];

                    //(it's intended to be read as an int but easier to just treat it as a single byte)
                    //int bone_type = BitConverter.ToInt32(mdl_data, bone_offset + 164);



                    //if bone type is jigglebone (05), set it to 00 (normal bone).
                    if (bone_type == 0x05)
                    {
                        mdl_data[bone_offset + 164] = 0x00;
                    }


                    //each bone is 216 bytes long. jump to the next bone.
                    bone_offset += 216;
                }

                return mdl_data;
            }
            catch
            {
                return null;
            }
        }


        // this method restores all the original .mdl files from the tmp folder!
        // right now, this is just used to restore jigglebones

        private void restore_mdls_in_folder(string source, string target)
        {

            if (!Directory.Exists(source) || !Directory.Exists(target))
            {
                return;
            }

            var stack = new Stack<miscFunc.Folders>();
            stack.Push(new miscFunc.Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.mdl"))
                {
                    if (Path.GetDirectoryName(file) == tmp_dir) return;
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);

                    //replace or create the original mdl file (deleting it was breaking items from workshop zips)
                    File.Copy(file, targetFile);

#if DEBUG
                    //MessageBox.Show("attempted to delete " + targetFile);
#endif
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new miscFunc.Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }

        }



        #endregion

        #region material tools


        #region grey material

        // sets player model materials to a grey material
        private void set_player_grey_material()
        {
            grey_material = true;

            // copy flat color texture
            miscFunc.create_missing_dir(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\tfmv\\");
            if (!File.Exists(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\tfmv\\flat_color.vtf"))
            {
                try
                {
                    WriteResourceToFile("TFMV.Files.textures.flat_color.vtf", steamGameConfig.tf_dir + "custom\\TFMV\\materials\\tfmv\\flat_color.vtf");
                }
                catch { }
            }

            List<TF2.player_mats> player_mats = player_all_mats.players_mats_red;

            foreach (var tfclass in player_mats)
            {
                if (tfclass.class_name == selected_player_class)
                {
                    foreach (var mat in tfclass.materials)
                    {

                        TF2.player_mat matt = new TF2.player_mat(mat.material, mat.texture_res, mat.keep_bumptexture, mat.phong_lightwarp);

                        //matt = mat;

                        if ((matt.material.Contains("_head_")) && (!matt.material.Contains("hwm")))
                        {
                            if (txtb_main_model.Text.ToLower().Contains("hwm"))
                            {
                                int index = matt.material.LastIndexOf(@"\");

                                matt.material = matt.material.Substring(0, index) + "\\hwm" + matt.material.Substring(index);
                            }
                        }

                        write_grey_material(matt);
                    }
                }
            }

            refresh_hlmv(false);
        }

        // sets player model materials to a solid color
        private void set_player_flat_material(string rgb)
        {
            grey_material = true;

            List<TF2.player_mats> player_mats = player_all_mats.players_mats_red;

            foreach (var tfclass in player_mats)
            {
                if (tfclass.class_name == selected_player_class)
                {
                    foreach (var mat in tfclass.materials)
                    {

                        TF2.player_mat matt = new TF2.player_mat(mat.material, mat.texture_res, mat.keep_bumptexture, mat.phong_lightwarp);

                        //matt = mat;

                        if ((matt.material.Contains("_head_")) && (!matt.material.Contains("hwm")))
                        {
                            if (txtb_main_model.Text.ToLower().Contains("hwm"))
                            {
                                int index = matt.material.LastIndexOf(@"\");

                                matt.material = matt.material.Substring(0, index) + "\\hwm" + matt.material.Substring(index);
                            }
                        }

                        write_flat_mat(tfmv_dir + matt.material + ".vmt", rgb);
                    }
                }
            }

            refresh_hlmv(false);
        }

        // generates grey material .vmt
        private void write_grey_material(TF2.player_mat mat)
        {
            if (mat.material == "") { return; }

            string vmt_filepath = steamGameConfig.tf_dir + "custom\\TFMV\\" + mat.material + ".vmt";
            miscFunc.create_missing_dir(Path.GetDirectoryName(vmt_filepath));

            VPK.Extract(mat.material + ".vmt", tfmv_dir + Path.GetDirectoryName(mat.material + ".vmt"), 0);

            // generate VMT
            List<String> vmt_code = new List<String>();
            vmt_code.Add("\"VertexLitGeneric\"");
            vmt_code.Add("{");

            // make eyeball materials flat shaded with color 70 70 70
            if (mat.material.ToLower().Contains("eyeball_"))
            {
                vmt_code = new List<String>();
                vmt_code.Add("\"UnlitGeneric\"");
                vmt_code.Add("{");
                vmt_code.Add("\"$color2\" \"{ 70 70 70 }\"");
                vmt_code.Add("}");

                System.IO.File.WriteAllLines(vmt_filepath, vmt_code);

                return;
            }

            // if it's a player texture that needs to have bodygroup masking
            if (mat.texture_res >= 1)
            {
                vmt_code.Add("\t$translucent 1\n\t$alphatest 1\n"); // transparency for bodygroups masking

                if (bodygroup_manager_panel.bodygroups_off_count() > 0)
                {
                    vmt_code.Add("\t\"$basetexture\" \"" + mat.material.Replace("materials\\models\\", "models\\") + "\"");
                }
                else
                {
                    vmt_code.Add("\t\"$basetexture\" \"tfmv\\flat_color.vtf\"");
                }
                bodygroup_manager_panel.create_bodygroups_mask(false);
            }
            else
            {
                vmt_code.Add("\t\"$basetexture\" \"tfmv\\flat_color.vtf\"");
            }

            // add/keep original bump if required by material
            if (mat.keep_bumptexture)
            {
                // check original VMT for bump
                string[,] bumpmap = VMT.get_parameters(vmt_filepath, new List<string>(new string[] { "bumpmap" }));

                // make sure paramter exists
                if (bumpmap != null)
                {
                    if (bumpmap.Length > 0)
                    {
                        if (bumpmap[0, 0] == "bumpmap")
                            if (bumpmap[0, 1] != "")
                            {
                                vmt_code.Add("\t\"$bumpmap\" \"" + bumpmap[0, 1] + "\"");
                            }
                    }
                }
            }

            // add phong if required by material
            if (mat.phong_lightwarp)
            {
                vmt_code.Add("");
                string phong_code =
                @"    ""$phong"" ""1""
                ""$phongexponent"" ""10""
                ""$phongboost"" "".1""

                ""$phongfresnelranges""   ""[.3 1 8]""
                ""$halflambert"" ""0""";

                vmt_code.Add(phong_code);
            }

            vmt_code.Add("}");

            System.IO.File.WriteAllLines(vmt_filepath, vmt_code);
        }

        private void remove_player_grey_material()
        {

            foreach (var tfclass in player_all_mats.players_mats_red)
            {
                if (tfclass.class_name == selected_player_class)
                {
                    // delete hwm
                    string classname = tfclass.class_name; if (classname == "demoman") { classname = "demo"; }
                    string hwm_matdir = steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\player\\" + classname + "\\hwm\\";
                    if (Directory.Exists(hwm_matdir)) { Directory.Delete(hwm_matdir, true); }

                    foreach (var mat in tfclass.materials)
                    {
                        miscFunc.delete_safe(steamGameConfig.tf_dir + "custom\\TFMV\\" + mat.material + ".vmt");
                    }
                }
            }


            foreach (var tfclass in player_all_mats.players_mats_blue)
            {

                // delete hwm
                string classname = tfclass.class_name; if (classname == "demoman") { classname = "demo"; }
                string hwm_matdir = steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\player\\" + classname + "\\hwm\\";
                if (Directory.Exists(hwm_matdir)) { Directory.Delete(hwm_matdir, true); }

                if (tfclass.class_name == selected_player_class)
                {
                    foreach (var mat in tfclass.materials)
                    {
                        miscFunc.delete_safe(steamGameConfig.tf_dir + "custom\\TFMV\\" + mat.material + ".vmt");
                    }
                }
            }

        }

        #endregion


        // write VMT with flat constant color
        public static void write_flat_mat(string vmt_path, string rgb)
        {
            if (!Directory.Exists(Path.GetDirectoryName(vmt_path)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(vmt_path));
            }

            System.IO.File.WriteAllText(vmt_path, "\"UnlitGeneric\" \n{\n\t" + "\"$color2\" \"{ " + rgb + " }\"\n}");

            // wait 300ms for HDD to write file, making sure file is written and closed, for HLMV to read it on refresh
            Thread.Sleep(300);

            refresh_hlmv(false);
        }

        #endregion


        #region Drag & Drop item loading methods

        #region Drag and Drop ZIP / VPK / MDL

        private void filter_droped_file(string sFile)
        {
            bool valid_model = false;

            miscFunc.DeleteDirectoryContent(tmp_workshop_zip_dir);

            if (!Directory.Exists(tmp_dir)) { Directory.CreateDirectory(tmp_workshop_zip_dir); }

            if (sFile.ToLower().Contains(".zip"))
            {
                unzip_to_tmp(sFile);
                load_tmp_models();
                valid_model = true;
            }

            if (sFile.ToLower().Contains(".vpk"))
            {
                vpk_to_tmp(sFile);
                load_tmp_models();
                valid_model = true;
            }

            if (sFile.ToLower().Contains(".mdl"))
            {
                #region validate model path

                string mdl_path = (sFile).Replace("\\", "/");
                string cleanPath = String.Join("", mdl_path.Split(Path.GetInvalidPathChars()));

                if (cleanPath != mdl_path)
                {
                    MessageBox.Show("Invalid path.");
                    return;
                }


                if (mdl_path == "custom model path to attach to the player model in HLMV (i.e.: models\\player\\items\\scout\\batter_helmet.mdl)")
                { return; }

                if (loadout_list.Controls.Count >= 12)
                {
                    MessageBox.Show("HLMV only allows up to 12 model attachments.");
                    return;
                }
                if (((mdl_path == "")) || (mdl_path == null))
                {

                    if ((mdl_path != "") || (mdl_path != null))
                    {
                        MessageBox.Show("You must define a model path before adding it to the loadout list.");
                    }

                }
                else
                {
                    if ((!mdl_path.Contains(".mdl")) || (!mdl_path.Contains("/")))
                    {
                        MessageBox.Show("Invalid model path, example: \n\n models\\player\\items\\scout\\batter_helmet.mdl");
                        return;
                    }
                }

                #endregion

                if (preload_mdl(sFile))
                {
                    SkinsManager_Form_reset();

                    loadout_addItem(Properties.Resources.icon_mdl_item, Path.GetFileName(sFile), sFile, 0, 1, -1, false);

                    //load_tmp_models();
                    valid_model = true;
                }
            }

            // if Drag&Drop was on the player icon (because its a Panel and not a FlowLayoutPanel)
            // get last item added to list and add it as main model
            if ((DragNDrop_on_MainModel) && (valid_model))
            {
                if (loadout_list.Controls.Count > 0)
                {
                    #region get icon

                    Image icon = null;
                    #region search tga file

                    if (Directory.Exists(tmp_loadout_dir + "content\\materialsrc\\backpack"))
                    {

                        string icon_path = "";
                        var tga_files = Directory.GetFiles(tmp_loadout_dir + "content\\materialsrc\\backpack", "*.tga", SearchOption.AllDirectories);
                        foreach (var tga_file in tga_files)
                        {
                            if (!tga_file.Contains("_large"))
                            {
                                icon_path = tga_file;

                                if (File.Exists(icon_path))
                                {
                                    Functions.TargaImage tf = null;
                                    try
                                    {
                                        tf = new Functions.TargaImage(icon_path);
                                        icon = tf.Image;
                                    }
                                    catch //(Exception ex)
                                    {
                                        //  MessageBox.Show(ex.ToString());
                                    }
                                }

                            }
                        }
                    }
                    #endregion

                    if (icon == null) { icon = Properties.Resources.icon_workshop_item; }

                    #endregion

                    Loadout_Item item = (Loadout_Item)loadout_list.Controls[loadout_list.Controls.Count - 1];

                    item.PictureBox.Image = icon;


                    set_main_model(item, "", null);

                    item.Dispose();
                }
                DragNDrop_on_MainModel = false;

            }
        }

        private void unzip_to_tmp(string sFile)
        {
            if (Path.GetExtension(sFile).StartsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                // extract to tmp
                using (ZipFile zip1 = ZipFile.Read(sFile))
                {
                    // extract files in "game" folder
                    foreach (ZipEntry e in zip1.Where(x => x.FileName.StartsWith("game")))
                    {
                        e.Extract(tmp_loadout_dir, ExtractExistingFileAction.OverwriteSilently);
                    }


                    foreach (ZipEntry e in zip1.Where(x => x.FileName.StartsWith("content\\materialsrc\\backpack\\")))
                    {
                        e.Extract(tmp_loadout_dir, ExtractExistingFileAction.OverwriteSilently);
                    }

                    // extract manifest
                    zip1["manifest.txt"].Extract(tmp_loadout_dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        private void vpk_to_tmp(string sFile)
        {
            if (File.Exists(sFile)) { File.Copy(sFile, tmp_loadout_dir + Path.GetFileName(sFile)); }
            VPK.Extract_ALL(tmp_loadout_dir + Path.GetFileName(sFile), tmp_loadout_dir);
        }

        // TODO make conditions for each source file type (zip, vpk or mdl)
        // get zip file, materials an models also check in case the zip contains VPKs then extract them
        private void load_tmp_models()
        {
            vpk_tmp_path = "";
            string mdl_dir = "", mat_dir = "";

            mdl_files = null;
            mat_files = null;

            List<string> lstFilesFound = new List<string>();

            #region find models and materials root dirs

            string[] list_mdl_dirs = Directory.GetDirectories(tmp_loadout_dir, "models", System.IO.SearchOption.AllDirectories);
            string[] list_mat_dirs = Directory.GetDirectories(tmp_loadout_dir, "materials", System.IO.SearchOption.AllDirectories);

            // pick the model directory path, making sure its not "materials/models"
            for (int i = 0; i < list_mdl_dirs.Length; i++)
            {
                if (list_mdl_dirs[i].Contains("materials") == false)
                {
                    if (list_mdl_dirs[i] != "")
                    {
                        mdl_dir = list_mdl_dirs[i];
                    }
                }
            }

            // pick the materials directory path, making sure it IS "materials/models"
            for (int i = 0; i < list_mat_dirs.Length; i++)
            {
                if (list_mat_dirs[i].Contains("models") == false)
                {
                    if (list_mat_dirs[i] != "")
                    {
                        mat_dir = list_mat_dirs[i];
                    }
                }
            }



            #endregion

            #region move "models" & "materials" from "tmp_loadout\..\models\.." to "tmp_loadout\models\.."

            // if the extracted "models" & "materials" folders from a workshop ZIP or VPK are in a subfolder
            // we move them back to the root
            // i.e.:  "\tmp_loadout_dir\game\models\.." will be moved to "\tmp_loadout_dir\models\..  same for materials

            try
            {
                if ((mdl_dir != "") && ((mdl_dir + "\\").Replace("\\\\", "\\") != tmp_loadout_dir + "models\\"))
                {
                    miscFunc.move_dir_safe(mdl_dir, tmp_loadout_dir + "models\\");
                }
                if ((mat_dir != "") && ((mat_dir + "\\").Replace("\\\\", "\\") != tmp_loadout_dir + "materials\\"))
                {
                    miscFunc.move_dir_safe(mat_dir, tmp_loadout_dir + "materials\\");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            mdl_dir = tmp_loadout_dir + "models\\";
            mat_dir = tmp_loadout_dir + "materials\\";

            #endregion

            // region get files list of materials and models

            #region get workshop zip manifest info

            if (File.Exists(tmp_loadout_dir + "manifest.txt"))
            {
                TFMV.VDF_parser manifest = new TFMV.VDF_parser();
                manifest.file_path = tmp_loadout_dir + "manifest.txt";
                manifest.load_VDF_file();

                TFMV.VDF_parser.VDF_node item = manifest.sGet_NodePath("asset.ImportSession");

                TFMV.VDF_parser.VDF_node equip_region = manifest.sGet_NodePath("asset.ImportSession.equip_region");

                #region bodygroups

                // get bodygroups that should be disabled
                TFMV.VDF_parser.VDF_node bodygroups = manifest.sGet_NodePath("asset.ImportSession.bodygroup");
                List<string> bodygroups_off = new List<string>();

                foreach (var bodygroup in bodygroups.nSubNodes)
                {
                    if ((bodygroup.nkey != "") && (bodygroup.nvalue == "1"))
                    {
                        // add to loadout_bodygroups_off if it doesn't exist
                        if (!loadout_bodygroups_off.Contains(bodygroup.nkey))
                        {
                            loadout_bodygroups_off.Add(bodygroup.nkey);
                        }
                    }
                }

                #endregion

                // if not bodygroups add equip_region as bodygroup to hide
                if (!loadout_bodygroups_off.Contains(equip_region.nvalue) && loadout_bodygroups_off.Count == 0)
                {
                    loadout_bodygroups_off.Add(equip_region.nvalue);
                }

                string item_name = manifest.sGet_KeyVal(item, "name");
                string item_class = "";
                string model_path = "";

                #region get model depending on the player_class
                // loop through subnodes
                for (int i = 0; i < manifest.RootNode.nSubNodes.Count; i++)
                {
                    TFMV.VDF_parser.VDF_node n = manifest.RootNode.nSubNodes[i];

                    // get class
                    if (n.nkey == "class") { item_class = n.nvalue; }

                    // get item schema info
                    if (n.nkey == "ItemSchema")
                    {
                        TFMV.VDF_parser.VDF_node ItemSchema = manifest.RootNode.nSubNodes[i];
                        // loop through item schema subnodes
                        for (int s = 0; s < ItemSchema.nSubNodes.Count; s++)
                        {
                            TFMV.VDF_parser.VDF_node ns = ItemSchema.nSubNodes[s];

                            if (ns.nkey == "model_player") { model_path = ns.nvalue; }
                        }
                    }
                }
                #endregion

                #region set class and set pose
                // if not found guess class by model path
                if (item_class != "")
                {
                    // set slot to primary (loads faster)
                    item_type_name = "";

                    selected_item_slot = "primary";

                    // demoman exception
                    if (item_class == "demo")
                    {
                        if (!cb_ref_pose.Checked)
                        {
                            //cb_pose.Text = "Stand_SECONDARY";
                        }
                    }

                    if (item_class == "demo")
                    {
                        item_class = "demoman";
                    }

                    set_class(item_class, true);
                }
                #endregion
            }

            #endregion

            #region copy/setup files and add item to loadout list

            // if "models" directory exists find files in  TFMV\tmp_loadout\models\
            if (Directory.Exists(mdl_dir))
            {
                mdl_files = Directory.GetFiles(mdl_dir, "*.*", SearchOption.AllDirectories);
                List<string> mdls_list = new List<string>();

                for (int i = 0; i < mdl_files.Length; i++)
                {
                    if (mdl_files[i].Contains(".mdl"))
                    {
                        string[] mdl_internal_path = mdl_files[i].ToString().Replace("/", "\\").Split(new[] { "\\models" }, StringSplitOptions.None);

                        mdls_list.Add("models" + mdl_internal_path[mdl_internal_path.Length - 1]);
                    }
                }

                Image icon = null;
                #region get icon
                #region search tga file

                if (Directory.Exists(tmp_loadout_dir + "content\\materialsrc\\backpack"))
                {
                    string icon_path = "";
                    var tga_files = Directory.GetFiles(tmp_loadout_dir + "content\\materialsrc\\backpack", "*.tga", SearchOption.AllDirectories);
                    foreach (var tga_file in tga_files)
                    {
                        if (!tga_file.Contains("_large"))
                        {
                            icon_path = tga_file;

                            if (File.Exists(icon_path))
                            {
                                Functions.TargaImage tf = null;
                                try
                                {
                                    tf = new Functions.TargaImage(icon_path);
                                    icon = tf.Image;
                                }
                                catch //(Exception ex)
                                {
                                    //  MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                    }
                }
                #endregion

                if (icon == null) { icon = Properties.Resources.icon_workshop_item; }

                #endregion

                // if multiple models show dialogue to select which model(s) to add to the loadout list
                if (mdls_list.Count > 1)
                {
                    ModelSelect dlg = new ModelSelect(mdls_list);
                    var result = dlg.ShowDialog();

                    add_models_tolist(dlg.selected_models, icon);

                    if (dlg.selected_models.Count == 1)
                    {
                        string tf_class = miscFunc.findclass(dlg.selected_models[0]);

                        if (tf_class != "")
                        {
                            set_class(tf_class, true);
                        }
                    }
                }
                else if (mdls_list.Count == 1) // if only 1 model add to loadout list
                {
                    add_models_tolist(mdls_list, icon);

                    string tf_class = miscFunc.findclass(mdls_list[0]);

                    if (tf_class != "")
                    {
                        set_class(tf_class, true);
                    }
                }
            }
            else    // if "models" exist, find files in TFMV\tmp_loadout\models\
            {
                for (int i = 0; i < list_mdl_dirs.Length; i++)
                {
                    if (list_mdl_dirs[i].Contains("materials") == false)
                    {
                        if (list_mdl_dirs[i] != "") { miscFunc.copy_safe(list_mdl_dirs[i], steamGameConfig.tf_dir + "custom\\TFMV\\models\\"); }
                    }
                }
            }

            if (Directory.Exists(mat_dir))
            {
                mat_files = Directory.GetFiles(mat_dir, "*.*", SearchOption.AllDirectories);
            }
            else
            {
                for (int i = 0; i < list_mat_dirs.Length; i++)
                {
                    if (list_mat_dirs[i].Contains("models") == false)
                    {
                        if (list_mat_dirs[i] != "") { miscFunc.copy_safe(list_mat_dirs[i], steamGameConfig.tf_dir + "custom\\TFMV\\materials\\"); }
                    }
                }
            }

            #endregion

            CopySkins(mdl_dir, mat_dir);

            #region move "models" & "materials" from "tmp_loadout\models\.." to "tmp\models\.."

            // if the extracted "models" & "materials" folders from a workshop ZIP or VPK are in a subfolder
            // we move them back to the root
            // i.e.:  "\tmp_loadout_dir\game\models\.." will be moved to "\tmp_loadout_dir\models\..  same for materials

            try
            {
                if (Directory.Exists(mdl_dir))
                {
                    miscFunc.move_dir_safe(mdl_dir, tmp_dir + "models\\");
                }


                if (Directory.Exists(mat_dir))
                {
                    miscFunc.move_dir_safe(mat_dir, tmp_dir + "materials\\");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            mdl_dir = tmp_dir + "models\\";
            mat_dir = tmp_dir + "materials\\";

            #endregion

            tf_mdldir = mdl_dir;
            tf_matdir = mat_dir;
        }


        // copy models and materials to "tf/custom/TFMV/" 
        private void CopySkins(string mdldir, string matdir)
        {
            string models_path = steamGameConfig.tf_dir + "custom\\TFMV\\models\\";
            string mats_path = steamGameConfig.tf_dir + "custom\\TFMV\\materials\\";

            if (Directory.Exists(mdldir))
            {
                for (int i = 0; i < mdl_files.Length; i++)
                {
                    if ((File.Exists(mdl_files[i])) && (mdl_files[i].Contains("backpack") == false))
                    {
                        try
                        {

                            string targetfilemdl = models_path + mdl_files[i].Replace(mdldir, "");
                            if (!System.IO.Directory.Exists(Path.GetDirectoryName(targetfilemdl)))
                            {
                                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(targetfilemdl));
                            }
                            miscFunc.copy_safe(mdl_files[i], targetfilemdl);
                        }
                        catch { }
                    }
                }
            }


            if (Directory.Exists(matdir))
            {
                for (int e = 0; e < mat_files.Length; e++)
                {
                    if (File.Exists(mat_files[e]) && (mat_files[e].Contains("backpack") == false))
                    {
                        try
                        {

                            string targetfilemat = mats_path + mat_files[e].Replace(matdir, "");
                            if (!System.IO.Directory.Exists(Path.GetDirectoryName(targetfilemat)))
                            {
                                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(targetfilemat));
                            }

                            miscFunc.copy_safe(mat_files[e], mats_path + mat_files[e].Replace(matdir, ""));
                        }
                        catch { }
                    }
                }
            }
        }

        // load .mdl file, get material paths, try to find materials in tf/models/ copy to tf/custom/TFMV/
        // if files not found, returns false
        private bool preload_mdl(string mdl_path)
        {

            if (mdl_path == "") { return false; }
            #region copy model files to tmp_loadout_dir

            string mdl_name = Path.GetFileNameWithoutExtension(mdl_path);

            string mdl_in = Path.GetDirectoryName(mdl_path) + "\\" + mdl_name + ".mdl";
            string mdl_out = tmp_dir + mdl_name;

            // if .mdl is loaded from loadout list drag and drop, we get path like C:\something\stuff\models\workshop\player\items\mymodel.mdl
            // if not we expect // models\workshop\player\items\mymodel.mdl
            if (!File.Exists(mdl_in))
            {
                mdl_in = steamGameConfig.tf_dir + mdl_in;
            }

            // if model doesn't exist // load it from Steam game VPK files
            if (!File.Exists(mdl_in))
            {
                // MessageBox.Show("Could not find model files for " + mdl_name + ".mdl" + "\n\nTFMV is expecting the .mdl to be located in Team Fortress 2\\tf\\models\\");
                return true;
            }

            miscFunc.copy_safe(mdl_in, mdl_out + ".mdl");

            #endregion

            List<Object> materials = Get_MDL_Materials(mdl_name + ".mdl");

            if (materials == null) { return false; }
            if (materials.Count == 0) { return false; }

            List<String> mats = (List<String>)materials[0];
            if (mats.Count == 0) { return false; }

            foreach (string item in mats)
            {
                miscFunc.copy_safe(steamGameConfig.tf_dir + item, tmp_dir + item);
            }

            string mdl_game_path = Path.GetDirectoryName(mdl_path.Replace("/", "\\").ToLower().Replace(steamGameConfig.tf_dir.ToLower(), "")) + "\\";//mdl_path.Replace((mdl_path.Replace("/", "\\").Replace(steamGameConfig.tf_dir, "")), "");
            mdl_in = Path.GetDirectoryName(mdl_path) + "\\" + mdl_name;
            mdl_out = tmp_dir + mdl_game_path + mdl_name;

            miscFunc.copy_safe(mdl_in + ".mdl", mdl_out + ".mdl");

            miscFunc.copy_safe(mdl_in + ".dx80.vtx", mdl_out + ".dx80.vtx");
            miscFunc.copy_safe(mdl_in + ".dx90.vtx", mdl_out + ".dx90.vtx");
            miscFunc.copy_safe(mdl_in + ".sw.vtx", mdl_out + ".sw.vtx");
            miscFunc.copy_safe(mdl_in + ".sw.vtx", mdl_out + ".sw.vtx");
            miscFunc.copy_safe(mdl_in + ".vvd", mdl_out + ".vvd");
            miscFunc.copy_safe(mdl_in + ".phy", mdl_out + ".phy");

            return true;
        }


        #endregion

        #region Drag & Drop handles

        private void listb_cmdls_DragDrop(object sender, DragEventArgs e)
        {

            // if Drag&Drop was on the player icon (because its a Panel and not a FlowLayoutPanel)
            if (sender is Panel)
            {
                Panel p = (Panel)sender;
                if (p.Name == "main_model_panel")
                {
                    DragNDrop_on_MainModel = true;
                }
            }

            miscFunc.DeleteDirectoryContent(tmp_loadout_dir);

            string url = "";

            try
            {
                url = e.Data.GetData(DataFormats.Text).ToString();

            }
            catch //(Exception ex)
            {
            }

            // if its a tf2 wiki image url
            if (url != "")
            {
                // get_item_name_from_WikiURL(Uri.UnescapeDataString(url));
            }

            // if its a zip file
            try
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

                // make sure its a .zip and not null
                if ((a != null))
                {
                    string s = a.GetValue(0).ToString();

                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });

                    this.Activate();
                }

            }
            catch //(Exception ex)
            {
            }
        }

        private void listb_cmdls_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(DataFormats.Text)) || (e.Data.GetDataPresent(DataFormats.FileDrop)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        #endregion

        #region tf2 wiki icon drag and drop  + search item + load

        // wiki.teamfortress.com/w/images/thumb/b/bf/Painted_Physician%27s_Procedure_Mask_7E7E7E.png/71px-Painted_Physician%27s_Procedure_Mask_7E7E7E.png 
        private void get_item_name_from_WikiURL(string url)
        {
            if (loadout_list.Controls.Count >= 12)
            {
                MessageBox.Show("HLMV only allows up to 12 model attachments.");
                return;
            }

            if (url == "")
            {
                MessageBox.Show("Null path!");
                return;
            }

            string filename = Path.GetFileNameWithoutExtension(url);

            filename = filename.Replace("transparent", "");

            string[] words = filename.Split('_');
            string tfclass = "";

            bool painted = false;
            bool unpainted = false;
            // string teamcolor; 
            System.Drawing.Color col = new System.Drawing.Color();

            string item_name = "";
            string hex_paint = "";

            int nulled_out_words = 0;

            // find if its painted
            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i].ToLower();
                if ((w.Contains('-')) && (i == 0))
                {
                    words[i] = w.Split('-')[1];
                }

                if (w == ("painted"))
                {
                    painted = true;
                    words[i] = "";
                    nulled_out_words++;
                }

                if (w == ("unpainted"))
                {
                    unpainted = true;
                    painted = false;
                    words[i] = "";
                    nulled_out_words++;
                }

                if (w.Length == 6 && (OnlyHexInString(w)) && (IsAllUpper(w)))
                {
                    hex_paint = w;
                    col = System.Drawing.ColorTranslator.FromHtml("#" + hex_paint);
                    words[i] = "";
                    nulled_out_words++;
                }

                if (w == "red")
                {
                    // teamcolor = "red";
                    words[i] = "";
                    nulled_out_words++;
                }

                if ((w == "full") || (w == "yes") || (w == "item") || (w == "icon") || (w == "backpack") || (w == "formal") || (w == "color") || (w == "paint"))
                {
                    words[i] = "";
                    nulled_out_words++;
                }

                if (w == "red")
                {
                    // teamcolor = "red";
                    words[i] = "";
                    nulled_out_words++;
                }

                // find the class
                if (miscFunc.findclass(w) != "")
                {
                    tfclass = miscFunc.findclass(w);
                    words[i] = "";
                    nulled_out_words++;
                }
            }

            int matches = 0;
            int matches_max = 2;

            matches_max = words.Length - nulled_out_words;

            for (int i = 0; i < words.Length; i++)
            {
                if ((words[i] != "") && (matches < matches_max) && (OnlyHexInString(words[i]) == false) && (IsAllUpper(words[i]) == false))
                {
                    item_name = item_name + words[i] + " ";
                    matches++;
                }
            }

            item_name = item_name.TrimEnd();

            TF2.items_game.item myItem = search_item_byName(item_name.ToLower());

            if (myItem != null)
            {
                string model = "";

                if (tfclass == "")
                {
                    tfclass = selected_player_class.ToLower();
                }

                if (tfclass != "")
                {
                    for (int i = 0; i < myItem.model_player_per_class.Count; i++)
                    {
                        if (myItem.model_player_per_class[i].tfclass.ToLower() == tfclass.ToLower())
                        {
                            model = myItem.model_player_per_class[i].model;
                            break;
                        }
                    }
                }

                if (model == "")
                {
                    model = myItem.model_path;
                }


                bool found_mdl = false;
                // search item by mdl path
                foreach (Loadout_Item item in loadout_list.Controls)
                {

                    if (item.model_path == model)
                    {
                        found_mdl = true;
                    }

                }

                // search item by mdl path
                if (!found_mdl)
                {
                    loadout_addItem(null, "", model, 0, 0, -1, false);
                    // listb_cmdls.Items.Add(model);
                }
                else
                {
                    MessageBox.Show("The model is already in the list.");
                }

                SkinsManager_Form_reset();
            }
        }

        private TF2.items_game.item search_item_byName(string name)
        {
            foreach (var item in items_game)
            {
                if (item.item_name_var.ToLower().Contains(name))
                {
                    return item;
                }
            }

            // if we are here, we didn't find it, hence let's try removing part of the end of the string to see if we find it with a shorter name
            string[] split = name.Split(' ');
            if (split.Length > 1) // only if it has more than 1 word (bonk boy) << 2 words at least, separated by a space
            {
                string shorter_name = name.Split(' ')[0] + " " + name.Split(' ')[1];
                string shorter_name_0 = name.Split(' ')[0];

                foreach (var item in items_game)
                {
                    if ((item.item_name_var.ToLower().Contains(shorter_name)))
                    {
                        return item;
                    }
                }

                foreach (var item in items_game)
                {
                    if (item.item_name_var.ToLower().Contains(shorter_name_0))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsUpper(input[i]))
                    return false;
            }

            return true;
        }

        #endregion

        #endregion




        #region Events

        // on Form close do
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {


            close_hlmv();

            miscFunc.DeleteDirectoryContent(steamGameConfig.tf_dir + "custom\\TFMV\\");
            miscFunc.delete_safe(steamGameConfig.tf_dir + "custom\\TFMV\\");

            if (cb_disable_custom_mods.Checked)
            {
                restore_custom_mods();
            }

            miscFunc.DeleteDirectoryContent(steamGameConfig.tf_dir + "custom\\TFMV\\");
            miscFunc.delete_safe(steamGameConfig.tf_dir + "custom\\TFMV\\");

            miscFunc.DeleteDirectoryContent(tmp_dir);

        }


        private void label_model_MouseClick(object sender, MouseEventArgs e)
        {
            label_model.SelectAll();
        }


        // color dialog
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = colorDialog;

            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                panel_Bgcolor.BackColor = colorDlg.Color;
                bg_color = colorDlg.Color;
            }
        }

        // download schema
        private void btn_dl_schema_Click(object sender, EventArgs e)
        {
            download_schemas();
        }

        // define custom model to load
        private void btn_clist_add_Click(object sender, EventArgs e)
        {
            #region validate model path

            string mdl_path = (txtb_cmdl_path.Text).Replace("\\", "/");
            string cleanPath = String.Join("", mdl_path.Split(Path.GetInvalidPathChars()));

            if (cleanPath != mdl_path)
            {
                MessageBox.Show("Invalid path.");
                return;
            }

            if (mdl_path == "custom model path to attach to the player model in HLMV (i.e.: models\\player\\items\\scout\\batter_helmet.mdl)")
            { return; }

            if (loadout_list.Controls.Count >= 12)
            {
                MessageBox.Show("HLMV only allows up to 12 model attachments.");
                return;
            }

            if (((mdl_path == "")) || (mdl_path == null))
            {

                if ((mdl_path != "") || (mdl_path != null))
                {
                    MessageBox.Show("You must define a model path before adding it to the loadout list.");
                }
            }
            else
            {
                if ((!mdl_path.Contains(".mdl")) || (!mdl_path.Contains("/")))
                {
                    MessageBox.Show("Invalid model path, example: \n\n models\\player\\items\\scout\\batter_helmet.mdl");
                    return;
                }
            }

            #endregion

            if (!preload_mdl(mdl_path))
            {
                return;
            }

            SkinsManager_Form_reset();

            loadout_addItem(Properties.Resources.icon_mdl_item, Path.GetFileName(mdl_path), mdl_path, 0, 0, -1, false);
        }



        // reset camera position-rotation form values
        private void btn_reset_camposrot_Click(object sender, EventArgs e)
        {
            txtb_hlmv_campos_x.Text = "189";
            txtb_hlmv_campos_y.Text = "0";
            txtb_hlmv_campos_z.Text = "37";

            txtb_hlmv_camrot_x.Text = "19";
            txtb_hlmv_camrot_y.Text = "18";
            txtb_hlmv_camrot_z.Text = "6";
        }

        private void btn_reset_light_Click(object sender, EventArgs e)
        {

            txtb_hlmv_lightrot_x.Text = "25";
            txtb_hlmv_lightrot_y.Text = "-162";
            txtb_hlmv_lightrot_z.Text = "-16";

            panel_aColor.BackColor = Color.FromArgb(255, 75, 75, 75);
            aColor = Color.FromArgb(255, 75, 75, 75);

            panel_lColor.BackColor = Color.White;
            lColor = Color.White;
        }

        // set hlmv ambient color
        private void btn_aColor_Click(object sender, EventArgs e)
        {
            ColorPicker cp_dialog = new ColorPicker(panel_aColor.BackColor);

            DialogResult result = cp_dialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                panel_aColor.BackColor = cp_dialog.colorPanel.Value;
            }
        }

        // set hlmv light color
        private void btn_lColor_Click(object sender, EventArgs e)
        {
            ColorPicker cp_dialog = new ColorPicker(panel_lColor.BackColor);

            DialogResult result = cp_dialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                panel_lColor.BackColor = cp_dialog.colorPanel.Value;
            }
        }

        // set main model (instead of loading a player model as main model)
        private void btn_use_mainmodel_Click(object sender, EventArgs e)
        {
            set_main_model(null, txtb_main_model.Text, Properties.Resources.icon_mdl_item);
        }


        private void cb_hlmv_bg_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void btn_screenshot_Click(object sender, EventArgs e)
        {

            if ((proc_HLMV == null) || (proc_HLMV.HasExited))
            {
                //MessageBox.Show("Cannot resize window, HLMV is not running.");
                return;
            }

            miscFunc.create_missing_dir(txtb_screenshots_dir.Text);
            // verify screenshots dir path is set and is valid/exists
            if (!Directory.Exists(txtb_screenshots_dir.Text))
            {
                MessageBox.Show("Error: the screenshots directory is not set, define it in the settings.");
                tabControl.SelectedIndex = 1;
                return;
            }

            screenshot_capture(true, false);
        }


        // btn browse screenshot directory
        private void btn_browser_screensdir_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtb_screenshots_dir.Text))
            {

                Process.Start(txtb_screenshots_dir.Text);
            }
            else
            {
                MessageBox.Show("Error: invalid directory path.");
            }
        }


        // hotkeys events
        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    loadout_to_hlmv();
                    load_skins();
                    break;

                case Keys.F2:
                    load_skins();
                    break;

                case Keys.F3:
                    update_skins(true);
                    break;

                case Keys.F5:
                    screenshot_capture(true, false);
                    break;
            }

        }

        // adds models to the 4 item list, used for external form selection (ModelSelect.cs)
        private void add_models_tolist(List<string> items, Image icon)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (check_max_loadout_items()) { return; }
                Loadout_Item loadout_item_added = loadout_addItem(icon, Path.GetFileName(items[i].ToString()), items[i].ToString(), 0, 0, -1, false);

                loadout_item_added.Parent = loadout_list;
            }
        }

        public static void add_loadout_list_item(Loadout_Item loadout_item_added)
        {
            //loadout_item_added.Parent = loadout_list;
        }

        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private void cb_hlmv_antialias_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtb_searchitem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtb_searchitem.Text != "")
                {
                    load_items_to_listView(items_game, txtb_searchitem.Text, "", false);
                }
            }
        }


        private void launch_link(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel label = (LinkLabel)sender;

            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = label.Tag;

            Process.Start(link.LinkData as string);
        }

        private void txtb_delay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }

        // search selected item in the TF2 Wiki
        private void btn_tf2_wiki_Click(object sender, EventArgs e)
        {
            if (list_view.SelectedItems.Count == 0) { MessageBox.Show("Select an item in the list to search in the TF2 Wiki website."); return; }
            try
            {
                string SearchItemName = (list_view.SelectedItems[0].Text.Split('(')[0].Replace(" ", "_"));

                Process.Start("http://wiki.teamfortress.com/wiki/Special:Search/" + SearchItemName);

            }
            catch (Exception) { }
        }


        // checkbox HLMV anti aliasing
        private void cb_hlmv_antialias_CheckedChanged_1(object sender, EventArgs e)
        {
            settings_save(sender, e);

            if (cb_hlmv_antialias.Checked)
            {
                set_hlmv_antialias("8");
            } else {
                set_hlmv_antialias("0");
            }
        }

        private void panel_Bgcolor_Click(object sender, EventArgs e)
        {
            ColorPicker cp_dialog = new ColorPicker(panel_Bgcolor1.BackColor);

            DialogResult result = cp_dialog.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                panel_Bgcolor1.BackColor = cp_dialog.colorPanel.Value;
                panel_Bgcolor.BackColor = cp_dialog.colorPanel.Value;
            }
        }


        private void panel_Bgcolor_MouseHover(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            p.BorderStyle = BorderStyle.FixedSingle;
        }

        private void panel_Bgcolor_MouseLeave(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            p.BorderStyle = BorderStyle.None;
        }

        private void txtb_cmdl_path_Click(object sender, EventArgs e)
        {
            if (txtb_cmdl_path.Text.Contains("custom model")) { txtb_cmdl_path.Text = ""; }
        }


        // download schema
        private void btn_dl_schema_Click_1(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 0;
            download_schemas();
        }

        private void cb_refresh_SkinPaintChange_CheckedChanged(object sender, EventArgs e)
        {
            auto_refresh_paints = cb_refresh_SkinPaintChange.Checked;

            btn_update_paints.Visible = !cb_refresh_SkinPaintChange.Checked;
        }

        // reverse item list sorting order
        private void cb_sort_order_CheckedChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);
        }

        private void cb_ref_pose_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ref_pose.Checked)
            {
                txtb_pose.Text = "ref";
                txtb_pose.Enabled = false;
            }

            if (!cb_ref_pose.Checked)
            {
                txtb_pose.Enabled = true;
            }
            settings_save(sender, e);
        }


        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            //lock out API key whenever user changes tab
            if (chkHideAPIKey.Checked)
            {
                txtb_API_Key.UseSystemPasswordChar = true;
            }
            */

            int fheight = this_height;
            if (btn_expand_item_list.Text == "-") { fheight = this_height_extended; }


            if (tabControl.SelectedIndex == 0)
            {
                if (lab_expand_options.Text == "<<")
                    this.Size = new System.Drawing.Size(options_panel_open_width, fheight);
            }
            else
            {
                // if options panel is opened close it
                if (lab_expand_options.Text == "<<")
                    this.Size = new System.Drawing.Size(options_panel_closed_width, this_height);
            }
        }

        // re-cache schema button
        private void btn_recache_schema_Click(object sender, EventArgs e)
        {
            schema_save_cache();
            load_schema(false);

            if (cb_sort_order.Checked)
            {
                items_game.Reverse();
            }
        }


        private void linkLabel6_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void cb_pose_TextChanged(object sender, EventArgs e)
        {

        }


        // use TFMV background // checkbox event
        private void cb_screenshot_transparency_CheckedChanged(object sender, EventArgs e)
        {
            bool ctrl_checked = cb_screenshot_transparency.Checked;

            if (ctrl_checked) // && skins_manager_control.Controls.Count > 0 //  && !proc_HLMV.HasExited // && proc_HLMV != null
            {
                DialogResult dialogResult = MessageBox.Show("To enable screenshot transparency, the loadout needs to be reloaded first.", "Warning", MessageBoxButtons.OK); // \n\nReload loadout now?
                if (dialogResult == DialogResult.OK)
                {
                    install_tfmv_mods();

                    write_flat_mat(tfmv_dir + @"materials\models\TFMV\tfmv_bg.vmt", panel_Bgcolor.BackColor.R + " " + panel_Bgcolor.BackColor.G + " " + panel_Bgcolor.BackColor.B);
                }
            }

            btn_bg_color1.Visible = ctrl_checked;
            panel_Bgcolor1.Visible = ctrl_checked;
            lab_trsp_scrn.Visible = ctrl_checked;
            lab_trsp_scrn1.Visible = ctrl_checked;
            numUpDown_screenshot_delay.Visible = ctrl_checked;

            settings_save(sender, e);
        }

        #endregion




        #region set filter button sizes

        // sets the loadout filter buttons to the appropriate sizes and locations. also handles hiding or unhiding the medals button as appropriate.
        private void set_filter_button_sizes()
        {
            //if medals are checked, show the medal button
            if (cb_allow_tournament_medals.Checked)
            {
                btn_medal.Visible = true;

                //set the button scale appropriately depending on if workshop button is present
                if (btn_workshop_items.Visible)
                {
                    make_buttons_small();
                }
                else
                {
                    make_buttons_big();
                }
            }
            //medals aren't checked, use default big layout and no medal button
            else
            {
                make_buttons_big();
                btn_medal.Visible = false;
            }



            //hide/unhide startup button choices that shouldn't be there
            if (!btn_medal.Visible)
            {
                if (lstStartupTab_Slot.Items.Contains("Medal"))
                {
                    lstStartupTab_Slot.Items.Remove("Medal");
                }
            }
            else
            {
                if (!lstStartupTab_Slot.Items.Contains("Medal"))
                {
                    lstStartupTab_Slot.Items.Add("Medal");
                }
            }


            if (!btn_workshop_items.Visible)
            {
                if (lstStartupTab_Slot.Items.Contains("Workshop"))
                {
                    lstStartupTab_Slot.Items.Remove("Workshop");
                }
            }
            else
            {
                if (!lstStartupTab_Slot.Items.Contains("Workshop"))
                {
                    lstStartupTab_Slot.Items.Add("Workshop");
                }
            }

            //if you deleted the selected index, default back to 0
            if (lstStartupTab_Slot.Text == "")
            {
                lstStartupTab_Slot.SelectedIndex = 0;
            }

        }


        private void make_buttons_big()
        {
            //resize buttons back to their regular size.
            int y = 61;

            //PDA
            btn_pda.Location = new Point(410, y);
            btn_pda.Width = 75;

            //Taunt Prop
            btn_tauntprop.Location = new Point(491, y);
            btn_tauntprop.Width = 75;
            btn_tauntprop.Text = "Taunt Prop";

            //Medal
            btn_medal.Location = new Point(572, y);
            btn_medal.Width = 75;

            //Workshop
            btn_workshop_items.Location = new Point(571, y - 1);
            btn_workshop_items.Width = 77;
        }


        private void make_buttons_small()
        {
            //resize existing buttons to make room for medal button.
            int y = 61;

            //PDA Small
            btn_pda.Location = new Point(410, y);
            btn_pda.Width = 49;

            //Taunt Prop Small
            btn_tauntprop.Location = new Point(464, y);
            btn_tauntprop.Width = 49;
            btn_tauntprop.Text = "Prop";

            //Medal Small
            btn_medal.Location = new Point(518, y);
            btn_medal.Width = 49;

            //Workshop Small
            btn_workshop_items.Location = new Point(572, y - 1);
            btn_workshop_items.Width = 76;
        }




        #endregion


        #region cache player bodygroup mask textures

        // checks if player textures (for bodygroup masking) are cached
        private void check_player_texture_cache()
        {
            bool need_to_cache = false;
            if (!Directory.Exists(cached_dir)) { need_to_cache = true; }

            TF2.player_materials player_mats = new TF2.player_materials();
            string[] teams = new string[] { "red", "blue" };

            // search for the class material
            for (int i = 0; i < player_mats.players_mats.Count; i++)
            {
                for (int t = 0; t < teams.Length; t++)
                {
                    TF2.player_material player_mat = player_mats.players_mats[i];

                    // if player material was no found, skip to next
                    if ((player_mat == null) || (player_mat.tf_class == "") || (player_mat.mat_name == "") || (player_mat.mat_dir == "")) { continue; }

                    string mat_name = player_mat.mat_name + "_" + teams[t];
                    string path = (cached_dir + mat_name + "_mask" + ".vtf").Replace("\\/", "\\");

                    //todo: wip fix for head bodygroups not finding the cached copy. (medic_head)
                    string mat_name_noteamcolor = player_mat.mat_name;
                    string path_noteamcolor = (cached_dir + mat_name_noteamcolor + "_mask" + ".vtf").Replace("\\/", "\\");

                    // if VTF DXT5 doesn't exist in "/cached/mat_name_team_color_mask.vtf"
                    if (!File.Exists(path) && !File.Exists(path_noteamcolor))
                    {
                        need_to_cache = true;
                        break;
                    }
                }

                if (need_to_cache) { break; }
            }

            if (need_to_cache)
            {
                MessageBox.Show("Player bodygroup masks cache files are missing. \nPlease re-install TFMV to restore missing files.");
            }
        }

        private void check_player_grey_texture_cache()
        {
            if (!Directory.Exists(cached_dir)) { miscFunc.create_missing_dir(cached_dir); }

            // if one of the files missing, extract it
            if ((!File.Exists(cached_dir + "rgba_grey_1024_512.vtf")) || (!File.Exists(cached_dir + "rgba_grey_2048_1024.vtf")))
            {
                try
                {
                    // extract resource to cached dir 
                    WriteResourceToFile("TFMV.Files.textures.player_masks.zip", cached_dir + "player_masks.zip");

                    // extract zip file contents
                    ZipFile zip = ZipFile.Read(cached_dir + "player_masks.zip");
                    zip.ExtractAll(cached_dir);

                } catch {
                }
            }

            // miscFunc.delete_safe(cached_dir + "player_masks.zip");
        }

        private void numUpDown_screenshot_delay_ValueChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);
        }

        private void lab_trsp_scrn_Click(object sender, EventArgs e)
        {

        }

        private void lab_trsp_scrn1_Click(object sender, EventArgs e)
        {

        }

        private void panel_screen_paints_tool_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion

        private void btn_save_hlmv_cam_Click(object sender, EventArgs e)
        {
            btn_red_team.PerformClick();

            #region get HLMV window size and save it

            try
            {
                if ((proc_HLMV == null) || (proc_HLMV.HasExited))
                {
                    MessageBox.Show("HLMV is not running, cannot save window size.");
                    return;
                }

                var rect = new Rect();
                GetWindowRect(proc_HLMV.MainWindowHandle, ref rect);

                String size_x = ((rect.right - rect.left) - hlmv_padding.left).ToString();
                String size_y = ((rect.bottom - rect.top) - hlmv_padding.bottom).ToString();

                txtb_hlmv_wsize_x.Text = size_x;
                txtb_hlmv_wsize_y.Text = size_y;
                txtb_hlmv_def_wsize_x.Text = size_x;
                txtb_hlmv_def_wsize_y.Text = size_y;
            } catch {
            }

            #endregion

            #region get and save HLMV camera pos/rot

            try
            {
                // close HLMV.exe so camera position/rotation is saved in windows registry
                close_hlmv();

                string modelname = tfmv_dir + txtb_main_model.Text;

                modelname = modelname.Replace("\\", ".");
                modelname = modelname.Replace("/", ".");
                modelname = modelname.Replace(":", ".");

                // get cam pos/rot from registry
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Valve\\hlmv\\" + modelname);

                String pos = (String)key.GetValue("Trans");
                String rot = (String)key.GetValue("Rot");

                pos = pos.Replace("(", "").Replace(")", "");
                rot = rot.Replace("(", "").Replace(")", "");
                String[] POS = pos.Split(' ');
                String[] ROT = rot.Split(' ');

                // set pos/rot in control form textboxs
                txtb_hlmv_camrot_x.Text = ROT[0].Replace(".000000", "");
                txtb_hlmv_camrot_y.Text = ROT[1].Replace(".000000", "");
                txtb_hlmv_camrot_z.Text = ROT[2].Replace(".000000", "");

                txtb_hlmv_campos_x.Text = POS[0].Replace(".000000", "");
                txtb_hlmv_campos_y.Text = POS[1].Replace(".000000", "");
                txtb_hlmv_campos_z.Text = POS[2].Replace(".000000", "");


                //neodement: save light values too

                String lightrot = (String)key.GetValue("lightrot");
                String lcolor = (String)key.GetValue("lColor");
                String acolor = (String)key.GetValue("aColor");

                lightrot = lightrot.Replace("(", "").Replace(")", "");
                lcolor = lcolor.Replace("(", "").Replace(")", "");
                acolor = acolor.Replace("(", "").Replace(")", "");

                String[] LIGHTROT = lightrot.Split(' ');
                String[] LCOLOR = lcolor.Split(' ');
                String[] ACOLOR = acolor.Split(' ');

                // set light rot in control form textboxs
                txtb_hlmv_lightrot_x.Text = LIGHTROT[0].Replace(".000000", "");
                txtb_hlmv_lightrot_y.Text = LIGHTROT[1].Replace(".000000", "");
                txtb_hlmv_lightrot_z.Text = LIGHTROT[2].Replace(".000000", "");


                // set light/ambient colour to control form colour panels
                float aColor_Rf = float.Parse(ACOLOR[0].Replace(".000000", ""));
                float aColor_Gf = float.Parse(ACOLOR[1].Replace(".000000", ""));
                float aColor_Bf = float.Parse(ACOLOR[2].Replace(".000000", ""));

                int aColor_R = (int)(aColor_Rf * 255.0);
                int aColor_G = (int)(aColor_Gf * 255.0);
                int aColor_B = (int)(aColor_Bf * 255.0);

                panel_aColor.BackColor = Color.FromArgb(255, aColor_R, aColor_G, aColor_B);
                aColor = Color.FromArgb(255, aColor_R, aColor_G, aColor_B);

                float lColor_Rf = float.Parse(LCOLOR[0].Replace(".000000", ""));
                float lColor_Gf = float.Parse(LCOLOR[1].Replace(".000000", ""));
                float lColor_Bf = float.Parse(LCOLOR[2].Replace(".000000", ""));

                int lColor_R = (int)(lColor_Rf * 255.0);
                int lColor_G = (int)(lColor_Gf * 255.0);
                int lColor_B = (int)(lColor_Bf * 255.0);

                panel_lColor.BackColor = Color.FromArgb(255, lColor_R, lColor_G, lColor_B);
                lColor = Color.FromArgb(255, lColor_R, lColor_G, lColor_B);


                //also save bg colour

                String bgcolor = (String)key.GetValue("bgColor");

                bgcolor = bgcolor.Replace("(", "").Replace(")", "");

                String[] BGCOLOR = bgcolor.Split(' ');

                float bgColor_Rf = float.Parse(BGCOLOR[0].Replace(".000000", ""));
                float bgColor_Gf = float.Parse(BGCOLOR[1].Replace(".000000", ""));
                float bgColor_Bf = float.Parse(BGCOLOR[2].Replace(".000000", ""));

                int bgColor_R = (int)(bgColor_Rf * 255.0);
                int bgColor_G = (int)(bgColor_Gf * 255.0);
                int bgColor_B = (int)(bgColor_Bf * 255.0);

                //todo: what on earth is Bgcolor1?
                panel_Bgcolor1.BackColor = Color.FromArgb(255, bgColor_R, bgColor_G, bgColor_B);
                panel_Bgcolor.BackColor = Color.FromArgb(255, bgColor_R, bgColor_G, bgColor_B);
                bg_color = Color.FromArgb(255, bgColor_R, bgColor_G, bgColor_B);

            }
            catch
            {
            }
            #endregion

            // loadout to HLMV
            btn_loadout_to_HLMV.PerformClick();
        }


        //this method makes sure the user can't type invalid stuff into textboxes intended for numbers. allows one decimal point and negative numbers.
        public void textBoxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // allow negative numbers
            if ((e.KeyChar == '-'))
            {
                //if we already have a - anywhere in the number, ignore the keypress
                if (((sender as TextBox).Text.IndexOf('-') > -1))
                {
                    e.Handled = true;
                }
                else
                //if the user is attempting to put a - anywhere but the start of the string, ignore the keypress
                {
                    if ((sender as TextBox).SelectionStart != 0)
                    {
                        e.Handled = true;
                    }
                }
            }

        }

        //this method makes sure the user can't type invalid stuff into textboxes intended for simple numbers (window size, second count). doesn't allow any decimal points or negative numbers.
        private void textBoxNumericSimple_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox_equip_region_filter_MouseClick(object sender, MouseEventArgs e)
        {
            lab_region_filter.Visible = false;
        }

        private void btn_turntable_Click(object sender, EventArgs e)
        {
            Turntable_GIF_Generator turntable_gen = new Turntable_GIF_Generator(proc_HLMV, txtb_screenshots_dir.Text);
            turntable_gen.Location = new Point(0, 25);

            foreach (var c in tab_items.Controls)
            {
                if (c.GetType() == typeof(Turntable_GIF_Generator))
                {
                    ((Turntable_GIF_Generator)c).Dispose();
                }
            }

            tab_items.Controls.Add(turntable_gen);
            turntable_gen.BringToFront();
            turntable_gen.Show();
        }

        private void Settings_Click(object sender, EventArgs e)
        {

        }

        #region tf/custom/ mod handling

        // installs TFMV mods in "custom" folder, such as disabling the tiled fire overlay texture
        private void install_tfmv_mods()
        {

            //TODO: change these to use tfmv_dir variable instead of manually finding it?

            //create directory for fire overlay
            miscFunc.create_missing_dir(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\effects\\tiledfire\\");

            //create directory for fake bodygroups
            string out_dir = steamGameConfig.tf_dir + "custom\\TFMV\\models\\TFMV_bodygroups\\";
            miscFunc.create_missing_dir(out_dir);

            //create directory for purity fist material fix
            miscFunc.create_missing_dir(steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\TFMV_bodygroups\\");

            try
            {

                // write no fire overlay texture file
                WriteResourceToFile("TFMV.Files.textures.fireLayeredSlowTiled512.vtf", steamGameConfig.tf_dir + "custom\\TFMV\\materials\\effects\\tiledfire\\fireLayeredSlowTiled512.vtf");

                //write fake bodygroup files

                //GUNSLINGER
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.engineer_gunslinger_bodygroup.dx80.vtx", out_dir + "engineer_gunslinger_bodygroup.dx80.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.engineer_gunslinger_bodygroup.dx90.vtx", out_dir + "engineer_gunslinger_bodygroup.dx90.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.engineer_gunslinger_bodygroup.mdl", out_dir + "engineer_gunslinger_bodygroup.mdl");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.engineer_gunslinger_bodygroup.swx.vtx", out_dir + "engineer_gunslinger_bodygroup.sw.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.engineer_gunslinger_bodygroup.vvd", out_dir + "engineer_gunslinger_bodygroup.vvd");
                //SNIPER ARROWS
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_arrows_bodygroup.dx80.vtx", out_dir + "sniper_arrows_bodygroup.dx80.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_arrows_bodygroup.dx90.vtx", out_dir + "sniper_arrows_bodygroup.dx90.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_arrows_bodygroup.mdl", out_dir + "sniper_arrows_bodygroup.mdl");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_arrows_bodygroup.swx.vtx", out_dir + "sniper_arrows_bodygroup.sw.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_arrows_bodygroup.vvd", out_dir + "sniper_arrows_bodygroup.vvd");
                //SNIPER DARTS
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_darts_bodygroup.dx80.vtx", out_dir + "sniper_darts_bodygroup.dx80.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_darts_bodygroup.dx90.vtx", out_dir + "sniper_darts_bodygroup.dx90.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_darts_bodygroup.mdl", out_dir + "sniper_darts_bodygroup.mdl");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_darts_bodygroup.swx.vtx", out_dir + "sniper_darts_bodygroup.sw.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.sniper_darts_bodygroup.vvd", out_dir + "sniper_darts_bodygroup.vvd");
                //GENTLE MANNE'S SERVICE MEDAL
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.soldier_medal_bodygroup.dx80.vtx", out_dir + "soldier_medal_bodygroup.dx80.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.soldier_medal_bodygroup.dx90.vtx", out_dir + "soldier_medal_bodygroup.dx90.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.soldier_medal_bodygroup.mdl", out_dir + "soldier_medal_bodygroup.mdl");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.soldier_medal_bodygroup.swx.vtx", out_dir + "soldier_medal_bodygroup.sw.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.soldier_medal_bodygroup.vvd", out_dir + "soldier_medal_bodygroup.vvd");
                //PURITY FIST
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.heavy_purityfist_bodygroup.dx80.vtx", out_dir + "heavy_purityfist_bodygroup.dx80.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.heavy_purityfist_bodygroup.dx90.vtx", out_dir + "heavy_purityfist_bodygroup.dx90.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.heavy_purityfist_bodygroup.mdl", out_dir + "heavy_purityfist_bodygroup.mdl");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.heavy_purityfist_bodygroup.swx.vtx", out_dir + "heavy_purityfist_bodygroup.sw.vtx");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.heavy_purityfist_bodygroup.vvd", out_dir + "heavy_purityfist_bodygroup.vvd");
                //PURITY FIST MATERIALS (to fix masking bug with tfmv bodygroup method)
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.hvyweapon_red_purityfist.vmt", steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\TFMV_bodygroups\\hvyweapon_red_purityfist.vmt");
                WriteResourceToFile("TFMV.Files.models.fakebodygroups.hvyweapon_blue_purityfist.vmt", steamGameConfig.tf_dir + "custom\\TFMV\\materials\\models\\TFMV_bodygroups\\hvyweapon_blue_purityfist.vmt");

                if (cb_screenshot_transparency.Checked)
                {
                    out_dir = steamGameConfig.tf_dir + "custom\\TFMV\\models\\TFMV\\";
                    miscFunc.create_missing_dir(out_dir);
                    WriteResourceToFile("TFMV.Files.models.tfmv_bg.dx80.vtx", out_dir + "tfmv_bg.dx80.vtx");
                    WriteResourceToFile("TFMV.Files.models.tfmv_bg.dx90.vtx", out_dir + "tfmv_bg.dx90.vtx");
                    WriteResourceToFile("TFMV.Files.models.tfmv_bg.mdl", out_dir + "tfmv_bg.mdl");
                    WriteResourceToFile("TFMV.Files.models.tfmv_bg.swx.vtx", out_dir + "tfmv_bg.sw.vtx");
                    WriteResourceToFile("TFMV.Files.models.tfmv_bg.vvd", out_dir + "tfmv_bg.vvd");
                }
            }
            catch
            {
                MessageBox.Show("Error writing files to TFMV custom directory.");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start("https://steamcommunity.com/dev/apikey");
        }

        // checks tf/custom/ contents to see if custom mods are installed and warns user if they want to disable mods while using TFMV
        private void check_custom_mods()
        {
            // check if this was already asked
            string filepath = settings_dir + "params.ini";
            if (File.Exists(filepath))
            {
                // Read in lines from file.
                foreach (string line in File.ReadLines(filepath))
                {
                    if (line == "ask_disable_mods")
                    {
                        return;
                    }
                }
            }

            File.AppendAllText(filepath, Environment.NewLine + "ask_disable_mods");

            if ((Directory.Exists(steamGameConfig.tf_dir + "custom\\")) && (!cb_disable_custom_mods.Checked))

                if (Directory.GetFiles(steamGameConfig.tf_dir + "custom\\", "*.vpk").Length > 0)
                {
                    DialogResult result = MessageBox.Show("TFMV has detected you have mods installed in \\Team Fortress 2\\tf\\custom\\"
                        + "\nMods can interfere with TFMV team skin switching and bodygroup masking."

                         + "\n\nHowever, TFMV can temporarily disable the mods while using TFMV to avoid this."
                         + "\n\nWould you like TFMV to disable mods? mods will always be re-enabled again when TFMV is closed."
                         + "\nYou can disable this option in the Settings tab."
                        ,

                        "TFMV: Disable custom mods while using TFMV?", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        cb_disable_custom_mods.Checked = true;
                    }
                }
        }

        private void txtb_API_Key_TextChanged(object sender, EventArgs e)
        {

            string txtb_API_Key_Text = txtb_API_Key.Text;

            //check if it's a 32 character Alphanumerical string. as soon as it is, try to save it
            if (txtb_API_Key_Text.Length == 32 && Regex.IsMatch(txtb_API_Key_Text, "^[a-zA-Z0-9]*$"))
            {
                steam_api_key = txtb_API_Key.Text;
                SaveAPIKey();
            }

        }

        private void lbl_EasterEgg_Click(object sender, EventArgs e)
        {
            img_EasterEgg.Visible = true;
        }

        private void logoPictureBox_Click(object sender, EventArgs e)
        {
            logoPictureBox.Image = new Bitmap(TFMV.Properties.Resources.TFMV_neo_logo);
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cb_disable_background_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("With the background disabled, TFMV will not be able to take transparent screenshots.", "Warning", MessageBoxButtons.OK);
            //DisableBackgroundWarning.Show(); //neodement: todo: create a dialog with a "Don't warn me again" checkbox
        }

        private void btn_reset_background_Click(object sender, EventArgs e)
        {
            //todo: what on earth is Bgcolor1?
            panel_Bgcolor1.BackColor = Color.FromArgb(255, 63, 63, 63);
            panel_Bgcolor.BackColor = Color.FromArgb(255, 63, 63, 63);
            bg_color = Color.FromArgb(255, 63, 63, 63);

            cb_hlmv_bg.Checked = false;
        }

        private void btn_reset_window_Click(object sender, EventArgs e)
        {
            txtb_hlmv_def_wsize_x.Text = "800";
            txtb_hlmv_def_wsize_y.Text = "600";
        }

        private void steamGameConfig_Load(object sender, EventArgs e)
        {

        }

        private void cb_cubemap_CheckedChanged(object sender, EventArgs e)
        {
            settings_save(sender, e);

            // custom cubemaps
            //todo: allow user to select from a list instead of just using 2fort!
            if (cb_cubemap.Checked)
            {
                install_custom_cubemap("Maps/2fort.vtf");

                refresh_hlmv(false);
            }
            else
            {
                remove_custom_cubemap();

                refresh_hlmv(false);
            }

        }


        //this method stops the user typing anything non-numerical into a numUpDown (used for screenshot delays)
        private void numUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_allow_tournament_medals_CheckedChanged(object sender, EventArgs e)
        {

            settings_save(sender, e);


            //special variable so we don't trigger the dialog unless the user clicked it.
            if (cb_allow_tournament_medals_SupressCheckedChange)
            {
                cb_allow_tournament_medals_SupressCheckedChange = false;
                return;
            }

            if (cb_allow_tournament_medals.Checked)
            {

                var result = MessageBox.Show("Changing this setting will cause the schema to re-download.\n\nAre you sure you want to do this? ", "Warning", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    tabControl.SelectedIndex = 0;
                    download_schemas();
                    set_filter_button_sizes();
                }
                else
                {
                    //special variable so we know not to trigger the dialog as a user didn't click it.
                    cb_allow_tournament_medals_SupressCheckedChange = true;
                    cb_allow_tournament_medals.Checked = false;
                }
            }
            else
            {
                var result = MessageBox.Show("Changing this setting will cause the schema to re-download.\n\nAre you sure you want to do this? ", "Warning", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    tabControl.SelectedIndex = 0;
                    download_schemas();
                    set_filter_button_sizes();
                }
                else
                {
                    //special variable so we know not to trigger the dialog as a user didn't click it.
                    cb_allow_tournament_medals_SupressCheckedChange = true;
                    cb_allow_tournament_medals.Checked = true;
                }
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void btn_scout_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void lstStartupTab_Slot_SelectedIndexChanged(object sender, EventArgs e)
        {

            //disable class selection if Workshop is selected
            lstStartupTab_Class.Enabled = true;
            if (lstStartupTab_Slot.Text == "Workshop")
            {
                lstStartupTab_Class.Enabled = false;
            }

            settings_save(sender, e);
        }







        /*
        Public Class SourceMdlBodyPart

	'struct mstudiobodyparts_t
	'{
	'	DECLARE_BYTESWAP_DATADESC();
	'	int					sznameindex;
	'	inline char * const pszName( void ) const { return ((char *)this) + sznameindex; }
	'	int					nummodels;
	'	int					base;
	'	int					modelindex; // index into models array
	'	inline mstudiomodel_t *pModel( int i ) const { return (mstudiomodel_t *)(((byte *)this) + modelindex) + i; };
	'};

	'   offset from start of this struct
	'	int					sznameindex;
	Public nameOffset As Integer
	'	int					nummodels;

    Public modelCount As Integer
	'	int					base;

    Public base As Integer
	'	int					modelindex; // index into models array

    Public modelOffset As Integer


    Public theName As String

    Public theModels As List(Of SourceMdlModel)

    Public theModelCommandIsUsed As Boolean

    Public theEyeballOptionIsUsed As Boolean

    Public theFlexFrames As List(Of FlexFrame)

End Class
*/


        private string ReadNullTerminatedString(BinaryReader inputFileReader)
        {
            StringBuilder text = new StringBuilder();

            text.Length = 0;

            while (inputFileReader.PeekChar() > 0)
            {

                text.Append(inputFileReader.ReadChar());
            }

            //' Read the null character.

            inputFileReader.ReadChar();
            return text.ToString();

        }







        [Serializable]
        public class SourceMdlBodyPart
        {
            public int nameOffset { get; set; }
            public int modelCount { get; set; }
            public int Base { get; set; }

            public int modelOffset { get; set; }

            public string theName { get; set; }

            //Public theModels As List(Of SourceMdlModel)
            public List<SourceMdlModel> theModels { get; set; }

            //Public theModelCommandIsUsed As Boolean

            //Public theEyeballOptionIsUsed As Boolean

            //Public theFlexFrames As List(Of FlexFrame)
        }





        [Serializable]
        public class SourceMdlModel
        {

            //todo: possibly important
            /*
            	'	char				name[64];
	Public name(63) As Char
	'	int					type;
            */


            public char[] name { get; set; }

            public int type { get; set; }
            public Single boundingRadius { get; set; }
            public int meshCount { get; set; }

            //offset to the offset
            //public int meshOffset_Offset { get; set; }
            public int meshOffset { get; set; }


            public int vertexCount { get; set; }



            public int vertexOffset { get; set; }

            public int tangentOffset { get; set; }

            public int attachmentCount { get; set; }

            public int attachmentOffset { get; set; }


            public int eyeballCount { get; set; }

            public int eyeballOffset { get; set; }

            //these are a separate struct in crowbar but it's probably ok
            public int vertexDataP { get; set; }

            public int tangentDataP { get; set; }




            public List<SourceMdlMesh> theMeshes { get; set; }

            //todo: good chance we need the vertex data.

            /*

	'	int					numeyeballs;

    Public eyeballCount As Integer
	'	int					eyeballindex;

    Public eyeballOffset As Integer

	'	mstudio_modelvertexdata_t vertexdata;

    Public vertexData As SourceMdlModelVertexData

	'	int					unused[8];		// remove as appropriate

    Public unused(7) As Integer



    Public theSmdFileNames As List(Of String)

    Public theMeshes As List(Of SourceMdlMesh)

    Public theEyeballs As List(Of SourceMdlEyeball)

            */
        }




        [Serializable]
        public class SourceMdlMesh
        {
            public int materialIndex { get; set; }
            public int modelOffset { get; set; }
            public int vertexCount { get; set; }
            public int vertexIndexStart { get; set; }
            public int flexCount { get; set; }
            public int flexOffset { get; set; }
            public int materialType { get; set; }
            public int materialParam { get; set; }

            public int id { get; set; }

            public Single centerX { get; set; }

            public Single centerY { get; set; }

            public Single centerZ { get; set; }


            /*
            	'	mstudio_meshvertexdata_t vertexdata;
	Public vertexData As SourceMdlMeshVertexData

	'	int					unused[8]; // remove as appropriate

    Public unused(7) As Integer



    Public theFlexes As List(Of SourceMdlFlex)
                */

        }




            private void ReadMeshes(long modelInputFileStreamPosition, SourceMdlModel aModel)
        {


            if (aModel.meshCount > 0 && aModel.meshOffset != 0) {

                aModel.theMeshes = new List<SourceMdlMesh>(aModel.meshCount);
                long meshInputFileStreamPosition;
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;



                reader.BaseStream.Seek(modelInputFileStreamPosition + aModel.meshOffset, SeekOrigin.Begin);

                fileOffsetStart = reader.BaseStream.Position;

                for (int meshIndex = 0; meshIndex < aModel.meshCount; meshIndex++)
                {


                    meshInputFileStreamPosition = reader.BaseStream.Position;


                    SourceMdlMesh aMesh = new SourceMdlMesh();


                    aMesh.materialIndex = reader.ReadInt32();


                    MessageBox.Show("this one:" + reader.BaseStream.Position);
                    aMesh.modelOffset = reader.ReadInt32();


                    aMesh.vertexCount = reader.ReadInt32();

                    MessageBox.Show("btw, vert count was: " + aMesh.vertexCount);

                    aMesh.vertexIndexStart = reader.ReadInt32();


                    aMesh.flexCount = reader.ReadInt32();


                    aMesh.flexOffset = reader.ReadInt32();


                    aMesh.materialType = reader.ReadInt32();


                    aMesh.materialParam = reader.ReadInt32();


                    aMesh.id = reader.ReadInt32();
                    //MessageBox.Show("btw, id was: " + aMesh.id);

                    aMesh.centerX = reader.ReadSingle();


                    aMesh.centerY = reader.ReadSingle();

                    //bet we could fuck with this
                    /////////////////////////////////////////////////////////////////////
                    aMesh.centerZ = reader.ReadSingle();
                    ////////////////////////////////////////////////////////////////////

                    int MAX_NUM_LODS = 1;

                    //SourceMdlMeshVertexData meshVertexData = new SourceMdlMeshVertexData();

                    //do nothing instead
                    reader.ReadInt32();



                    for (int x = 0; x < MAX_NUM_LODS; x++)
                    {


                        //meshVertexData.lodVertexCount(x) = reader.ReadInt32();

                        //do nothing instead
                        reader.ReadInt32();


                    }

                    //aMesh.vertexData = meshVertexData;



                    for (int x = 0; x < 8; x++)
                    {

                        //do nothing instead
                        reader.ReadInt32();

                        //aMesh.unused(x) = Me.theInputFileReader.ReadInt32()



                    }


                    aModel.theMeshes.Add(aMesh);


                    /*
                    ' Fill-in eyeball texture index info.
    

                    If aMesh.materialType = 1 Then
    
                        aModel.theEyeballs(aMesh.materialParam).theTextureIndex = aMesh.materialIndex
    

                    End If
    */


                    inputFileStreamPosition = reader.BaseStream.Position;
    

                    /*
                    If aMesh.flexCount > 0 AndAlso aMesh.flexOffset <> 0 Then
    
                        Me.ReadFlexes(meshInputFileStreamPosition, aMesh)
    
                    End If
    

                    Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                    */
                }
                

                fileOffsetEnd = reader.BaseStream.Position - 1;

            //Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theMeshes " + aModel.theMeshes.Count.ToString())


            //Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aModel.theMeshes alignment")

        }

 }

            



        private void ReadModels(long bodyPartInputFileStreamPosition, SourceMdlBodyPart aBodyPart, int bodyPartIndex)
        {

            if (aBodyPart.modelCount > 0)
            {
                long modelInputFileStreamPosition;
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;


                string modelName = "";


                reader.BaseStream.Seek(bodyPartInputFileStreamPosition + aBodyPart.modelOffset, SeekOrigin.Begin);

                //'fileOffsetStart = Me.theInputFileReader.BaseStream.Position


                aBodyPart.theModels = new List<SourceMdlModel>(aBodyPart.modelCount);

                //For j As Integer = 0 To aBodyPart.modelCount - 1
                for (int j = 0; j < aBodyPart.modelCount; j++)
                {

                    modelInputFileStreamPosition = reader.BaseStream.Position;

                    //MessageBox.Show("HEY!!!!!!!! " + modelInputFileStreamPosition);

                    fileOffsetStart = reader.BaseStream.Position;


                    SourceMdlModel aModel = new SourceMdlModel();

                    aModel.name = reader.ReadChars(64);

                    aModel.type = reader.ReadInt32();

                    aModel.boundingRadius = reader.ReadSingle();

                    aModel.meshCount = reader.ReadInt32();


                    //offset to the offset
                    //aModel.meshOffset_Offset = Convert.ToInt32(reader.BaseStream.Position);

                    aModel.meshOffset = reader.ReadInt32();

                    aModel.vertexCount = reader.ReadInt32();

                    aModel.vertexOffset = reader.ReadInt32();

                    aModel.tangentOffset = reader.ReadInt32();

                    aModel.attachmentCount = reader.ReadInt32();

                    aModel.attachmentOffset = reader.ReadInt32();

                    aModel.eyeballCount = reader.ReadInt32();

                    aModel.eyeballOffset = reader.ReadInt32();

                    aModel.vertexDataP = reader.ReadInt32();

                    aModel.tangentDataP = reader.ReadInt32();

                    //todo: junk data
                    int temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();
                    temp = reader.ReadInt32();


                    /*
                    aModel.eyeballCount = reader.ReadInt32();

                    aModel.eyeballOffset = reader.ReadInt32();

                    Dim modelVertexData As New SourceMdlModelVertexData()
                     modelVertexData.vertexDataP = Me.theInputFileReader.ReadInt32()

                    modelVertexData.tangentDataP = Me.theInputFileReader.ReadInt32()

                    aModel.vertexData = modelVertexData

                    For x As Integer = 0 To 7

                        aModel.unused(x) = Me.theInputFileReader.ReadInt32()
                    Next
     */


                    aBodyPart.theModels.Add(aModel);

                    inputFileStreamPosition = reader.BaseStream.Position;


                    /*
                'NOTE: Call ReadEyeballs() before ReadMeshes() so that ReadMeshes can fill-in the eyeball.theTextureIndex values.


                Me.ReadEyeballs(modelInputFileStreamPosition, aModel)
                 Me.ReadMeshes(modelInputFileStreamPosition, aModel)
                    */

                    //dead end?
                    ReadMeshes(modelInputFileStreamPosition, aModel);

                reader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);

                        //todo: is this needed?
                //modelName = CStr(aModel.name).Trim(Chr(0))


                fileOffsetEnd = reader.BaseStream.Position - 1;


                //Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel Name = " + modelName)


            }
 

            //'fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
 
            //'Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aBodyPart.theModels " + aBodyPart.theModels.Count.ToString())
 
        }

    }
        

        //using (var stream = File.Open(filepath, FileMode.Open))
        //{
        //    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
        //    {

        private static BinaryReader reader;

        // this method edits the .mdl to disable bodygroups
        // reads .mdl file, gets bodygroup count and offset and blah blah
        // if found, it swaps the nameoffset and modeloffset with each other

        //this results in the bodygroup working in reverse,
        //being hidden when it would usually be visible and vice versa

        //(returns an array of bytes to write back to a file)

        //Public theBodyParts As List(Of SourceMdlBodyPart);

        private byte[] mdl_disable_bodygroup(string filepath)
        {


            if (!File.Exists(filepath)) { return null; }

            //TODO: uncomment this
            //try
            //{

                byte[] mdl_data = File.ReadAllBytes(filepath);

            // Offsets: 0xE8 (232), 0xEC (236)

                int bodyPartCount = BitConverter.ToInt32(mdl_data, 232);
                int bodyPartOffset = BitConverter.ToInt32(mdl_data, 236);

            //MessageBox.Show("body part count is " + bodyPartCount);

            //todo: reading file twice for no reason currently


            if (bodyPartCount > 0)
            {
                long bodyPartInputFileStreamPosition;
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                long fileOffsetStart2;
                long fileOffsetEnd2;


                //using (var stream = File.Open(filepath, FileMode.Open))
                //{
                //    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                //    {


                //load file into the stream
                FileStream stream = File.Open(filepath, FileMode.Open);

                reader = new BinaryReader(stream, Encoding.UTF8, false);

                reader.BaseStream.Seek(bodyPartOffset, SeekOrigin.Begin);

                        fileOffsetStart = reader.BaseStream.Position;

                        List<SourceMdlBodyPart> theBodyParts = new List<SourceMdlBodyPart>(bodyPartCount);


                //For bodyPartIndex As Integer = 0 To Me.theMdlFileData.bodyPartCount - 1
                for (int bodyPartIndex = 0; bodyPartIndex < bodyPartCount; bodyPartIndex++)
                {

                    bodyPartInputFileStreamPosition = reader.BaseStream.Position;

                    SourceMdlBodyPart aBodyPart = new SourceMdlBodyPart();


                    MessageBox.Show("OFFSET:  " + reader.BaseStream.Position);

                    aBodyPart.nameOffset = reader.ReadInt32();

                    aBodyPart.modelCount = reader.ReadInt32();

                    aBodyPart.Base = reader.ReadInt32();

                    aBodyPart.modelOffset = reader.ReadInt32();


                    //theBodyParts.Add(aBodyPart);


                    inputFileStreamPosition = reader.BaseStream.Position;


                    if (aBodyPart.nameOffset != 0)
                    {

                        reader.BaseStream.Seek(bodyPartInputFileStreamPosition + aBodyPart.nameOffset, SeekOrigin.Begin);

                        fileOffsetStart2 = reader.BaseStream.Position;

                        //TODO: reuse it for jigglebone disable and editor
                        aBodyPart.theName = ReadNullTerminatedString(reader);


                        fileOffsetEnd2 = reader.BaseStream.Position - 1;

                        //Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart2, fileOffsetEnd2, "aBodyPart.theName = " + aBodyPart.theName)

                    }
                    else
                    {

                        aBodyPart.theName = "";
                    }


                    theBodyParts.Add(aBodyPart);



                    ReadModels(bodyPartInputFileStreamPosition, aBodyPart, bodyPartIndex);


                    //MessageBox.Show("mesh offset: " + (bodyPartInputFileStreamPosition + aBodyPart.modelOffset));

                    //'NOTE: Aligned here because studiomdl aligns after reserving space for bodyparts and models.

                            /*
                            if (bodyPartIndex == Me.theMdlFileData.bodyPartCount - 1)
                            { 

                            Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, Me.theInputFileReader.BaseStream.Position - 1, 4, "theMdlFileData.theBodyParts + aBodyPart.theModels alignment")

                        }
                            */




                            reader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);

                        }

                string allNames = "Found these bodyparts on soldier.mdl:\n\n";
                        for (int i = 0; i < theBodyParts.Count; i++)
                        {
                            allNames += theBodyParts[i].theName + ":";
                            allNames += "\n";


                    for (int j = 0; j < theBodyParts[i].modelCount; j++)
                    {


                        string MyString = "";
                        for (int k = 0; k < 64; k++)
                        {
                            if (theBodyParts[i].theModels[j].name[k] != 0)
                            {
                                MyString += theBodyParts[i].theModels[j].name[k];
                            }
                        }


                        if (MyString == "")
                        {
                            MyString = "<blank>";
                        }

                        allNames += "-" + MyString + " (mesh offset: " + theBodyParts[i].theModels[j].meshOffset + ")";
                        allNames += "\n";

                        /*
                        allNames += "-" + MyString + " (vertex offset: " + theBodyParts[i].theModels[j].vertexOffset_Offset + ")";
                        allNames += "\n";
                        allNames += "-" + MyString + " (tangent offset: " + theBodyParts[i].theModels[j].tangentOffset_Offset + ")";
                        allNames += "\n";
                        */

                        /*
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        theBodyParts[i].theModels[0].meshOffset = theBodyParts[i].theModels[1].meshOffset;
                        */

                        //swap submodel 0 data entirely with submodel 1

                        //disabled for now
                        if (theBodyParts[i].modelCount == 2 && (true == false))
                        {
                            SourceMdlModel temporaryModel_0 = theBodyParts[i].theModels[0];
                            SourceMdlModel temporaryModel_1 = theBodyParts[i].theModels[1];

                            //correct the offsets. we're moving the entire model 148 bytes FORWARD,
                            //so adjust the offsets 148 bytes backwards
                            temporaryModel_0.attachmentOffset -= 148;
                            temporaryModel_0.eyeballOffset -= 148;
                            temporaryModel_0.meshOffset -= 148;
                            temporaryModel_0.tangentOffset -= 148;
                            temporaryModel_0.vertexOffset -= 148;

                            //correct the offsets. we're moving the entire model 148 bytes BACKWARD,
                            //so adjust the offsets 148 bytes forwards
                            temporaryModel_0.attachmentOffset += 148;
                            temporaryModel_0.eyeballOffset += 148;
                            temporaryModel_0.meshOffset += 148;
                            temporaryModel_0.tangentOffset += 148;
                            temporaryModel_0.vertexOffset += 148;

                            theBodyParts[i].theModels[0] = temporaryModel_1;
                            theBodyParts[i].theModels[1] = temporaryModel_0;
                        }

                    }
                    allNames += "\n";

                }
                        MessageBox.Show(allNames);
                    //}
                        /*
            fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1

            Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBodyParts " + Me.theMdlFileData.theBodyParts.Count.ToString())

        End If
                        */

//}
                }



                return mdl_data;
            //}
            //catch
            //{
            //    return null;
            //}
        }


        private void btnDisableSoldierHelm_Click(object sender, EventArgs e)
        {
            mdl_disable_bodygroup("C:\\Users\\jburn\\Desktop\\TFMV\\config\\soldier.mdl");
        }


        //TODO:

        //this successfully disables soldiers helmet bodygroup, but it's not perfect:

        //model1 decompiles as a mesh with some default name instead of as "none" in the qc (a quick look in crowbar for "body" shows it has invalid chars in filename?)
        //model2 is the medal bodygroup?

        //theory: maybe we need to change the bodygroup header instead of just the information inside.

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] mdl_data = File.ReadAllBytes("C:\\Users\\jburn\\Desktop\\TFMV\\config\\soldier.mdl");

            string filepath = "C:\\Users\\jburn\\Desktop\\TFMV\\config\\soldier.mdl";




            //mdl_data[101556]
            //BitConverter.ToInt32(mdl_data[10])


            //load file into the stream
            FileStream stream = File.Open(filepath, FileMode.Open);

            reader = new BinaryReader(stream, Encoding.UTF8, false);



            //go to first model data
//            reader.BaseStream.Seek(102000, SeekOrigin.Begin);
            reader.BaseStream.Position = 102000;
//            reader.BaseStream.Position = 101704;

            SourceMdlModel aModel = new SourceMdlModel();

            aModel.name = reader.ReadChars(64);

            aModel.type = reader.ReadInt32();

            aModel.boundingRadius = reader.ReadSingle();

            aModel.meshCount = reader.ReadInt32();

            //offset to the offset
            //aModel.meshOffset_Offset = Convert.ToInt32(reader.BaseStream.Position);

            aModel.meshOffset = reader.ReadInt32();

            aModel.vertexCount = reader.ReadInt32();

            aModel.vertexOffset = reader.ReadInt32();

            aModel.tangentOffset = reader.ReadInt32();

            aModel.attachmentCount = reader.ReadInt32();

            aModel.attachmentOffset = reader.ReadInt32();

            aModel.eyeballCount = reader.ReadInt32();

            aModel.eyeballOffset = reader.ReadInt32();

            aModel.vertexDataP = reader.ReadInt32();

            aModel.tangentDataP = reader.ReadInt32();

            //todo: junk data
            int temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();
            temp = reader.ReadInt32();



            //go to second model data
//            reader.BaseStream.Seek(102148, SeekOrigin.Begin);
            reader.BaseStream.Position = 102148;
//            reader.BaseStream.Position = 101852;


            SourceMdlModel bModel = new SourceMdlModel();

            bModel.name = reader.ReadChars(64);

            bModel.type = reader.ReadInt32();

            bModel.boundingRadius = reader.ReadSingle();

            bModel.meshCount = reader.ReadInt32();

            //offset to the offset
            //aModel.meshOffset_Offset = Convert.ToInt32(reader.BaseStream.Position);

            bModel.meshOffset = reader.ReadInt32();

            bModel.vertexCount = reader.ReadInt32();

            bModel.vertexOffset = reader.ReadInt32();

            bModel.tangentOffset = reader.ReadInt32();

            bModel.attachmentCount = reader.ReadInt32();

            bModel.attachmentOffset = reader.ReadInt32();

            bModel.eyeballCount = reader.ReadInt32();

            bModel.eyeballOffset = reader.ReadInt32();

            bModel.vertexDataP = reader.ReadInt32();

            bModel.tangentDataP = reader.ReadInt32();

            //todo: junk data
            int temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();
            temp2 = reader.ReadInt32();



            reader.Close();




            File.WriteAllBytes("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\custom\\!TFMV\\models\\player\\soldier.mdl", mdl_data);

            MessageBox.Show("hacky pause...");

            string filepath2 = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Team Fortress 2\\tf\\custom\\!TFMV\\models\\player\\soldier.mdl";

            using (var stream2 = File.Open(filepath2, FileMode.Open))
            {
                using (var writer = new BinaryWriter(stream2, Encoding.UTF8, false))
                {

                    //go to first model data
                    //                    writer.BaseStream.Seek(101556, SeekOrigin.Begin);
                    //                    writer.BaseStream.Seek(102148, SeekOrigin.Begin);
                    writer.BaseStream.Position = 102148;
//                    writer.BaseStream.Position = 101852;

                    writer.Write(aModel.name);

                    writer.Write(aModel.type);

                    writer.Write(aModel.boundingRadius);

                    writer.Write(aModel.meshCount);

                    //offset to the offset
                    //aModel.meshOffset_Offset = Convert.ToInt32(reader.BaseStream.Position);

                    writer.Write(aModel.meshOffset - 148 + 0);

                    writer.Write(aModel.vertexCount);

                    writer.Write(aModel.vertexOffset - 0);

                    writer.Write(aModel.tangentOffset - 0);

                    writer.Write(aModel.attachmentCount);

                    writer.Write(aModel.attachmentOffset - 0);

                    writer.Write(aModel.eyeballCount);

                    writer.Write(aModel.eyeballOffset - 0);

                    writer.Write(aModel.vertexDataP);

                    writer.Write(aModel.tangentDataP);

                    //todo: junk data
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);




                    //go to second model data
                    //                    writer.BaseStream.Seek(101704, SeekOrigin.Begin);
                    //                    writer.BaseStream.Seek(102000, SeekOrigin.Begin);
                    writer.BaseStream.Position = 102000;
//                    writer.BaseStream.Position = 101704;

                    writer.Write(bModel.name);

                    writer.Write(bModel.type);

                    writer.Write(bModel.boundingRadius);

                    writer.Write(bModel.meshCount);

                    //offset to the offset
                    //aModel.meshOffset_Offset = Convert.ToInt32(reader.BaseStream.Position);

                    writer.Write(bModel.meshOffset + 0); //doesnt matter?

                    writer.Write(bModel.vertexCount);

                    writer.Write(bModel.vertexOffset + 0);

                    writer.Write(bModel.tangentOffset + 0);

                    writer.Write(bModel.attachmentCount);

                    writer.Write(bModel.attachmentOffset + 0);

                    writer.Write(bModel.eyeballCount);

                    writer.Write(bModel.eyeballOffset + 0);

                    writer.Write(bModel.vertexDataP);

                    writer.Write(bModel.tangentDataP);

                    //todo: junk data
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);
                    writer.Write(temp);

                    writer.Close();




                }
            }
        }

        private void skins_manager_control_Paint(object sender, PaintEventArgs e)
        {
        }

        private void chk_API_Key_CheckedChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("make this work!");
            //txtb_API_Key.Text = "";

            if (chk_HideAPIKey.Checked)
            {
                txtb_API_Key.UseSystemPasswordChar = true;
                //txtb_API_Key.Enabled = true;
                //txtb_API_Key.BackColor = Color.WhiteSmoke;

                string txtb_API_Key_Text = txtb_API_Key.Text;

                //check if it's a 32 character Alphanumerical string. as soon as it is, try to save it
                //if (txtb_API_Key_Text.Length == 32 && Regex.IsMatch(txtb_API_Key_Text, "^[a-zA-Z0-9]*$"))
                //{
                //    steam_api_key = txtb_API_Key.Text;
                //    SaveAPIKey();
                //}

            }
            else
            {
                txtb_API_Key.UseSystemPasswordChar = false;
                //txtb_API_Key.Enabled = false;
                //txtb_API_Key.BackColor = Color.Gainsboro;
                //steam_api_key = internal_steam_api_key;
            }

            settings_save(sender, e);
        }

        private void panel_hlmv_settings_Paint(object sender, PaintEventArgs e)
        {

        }

        //unlock API key textbox when clicked
        private void txtb_API_Key_Enter(object sender, EventArgs e)
        {
            //txtb_API_Key.UseSystemPasswordChar = false;
        }

        private void list_view_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!supress_TF2Item_ContextMenu)
                {
                    ExtdListViewItem focusedItem = (ExtdListViewItem)list_view.FocusedItem;
                    if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                    {
                        //tag is used to store right clicked item
                        menu_TF2Item.Tag = focusedItem;
                        menu_TF2Item.Show(Cursor.Position);
                    }
                }
            }
        }


        // get checked/unchecked item
        //ExtdListViewItem item = (ExtdListViewItem)e.Item;


        /*
        // searches item by mdl_path and returns the index
        private int loadout_find_model(string search_mdl_path)
        {
            for (int i = 0; i < loadout_list.Controls.Count; i++)
            {

                if (((Loadout_Item)loadout_list.Controls[i]).model_path == search_mdl_path)
                {
                    return i;
                }
            }

            return -1;
        }
        */


        private void copyMDLPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtdListViewItem item = (ExtdListViewItem)menu_TF2Item.Tag;

            Clipboard.SetText(item.model_path);
        }

        private void viewOnTF2WikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtdListViewItem item = (ExtdListViewItem)menu_TF2Item.Tag;

            string SearchItemName = (item.Text.Split('(')[0].Replace(" ", "_"));

            Process.Start("http://wiki.teamfortress.com/wiki/Special:Search/" + SearchItemName);
        }

        private void disable_custom_mods()
        {
            miscFunc.move_dir_safe(steamGameConfig.tf_dir + "custom\\", steamGameConfig.tf_dir + "custom_disabled\\");
        }

        private void restore_custom_mods()
        {
            miscFunc.move_dir_safe(steamGameConfig.tf_dir + "custom_disabled\\", steamGameConfig.tf_dir + "custom\\");
        }


        // enable/disable custom mods checkbox event
        private void cb_disable_custom_mods_CheckedChanged(object sender, EventArgs e)
        {

            if (cb_disable_custom_mods.Checked)
            {
                disable_custom_mods();
                install_tfmv_mods();
            }
            else
            {
                restore_custom_mods();
            }

            settings_save(sender, e);
        }


        //neodement:
        //load API key from "api_key.ini"

        //(disabled error messages, just use the built-in one if it exists)
        private string LoadAPIKey()
        {
            try
            {
                string LoadedAPIKey = File.ReadAllText(app_data_dir + "api_key.ini");

                //check if it's a 32 character Alphanumerical string
                if (LoadedAPIKey.Length == 32 && Regex.IsMatch(LoadedAPIKey, "^[a-zA-Z0-9]*$"))
                {
                    return LoadedAPIKey;
                }
                else
                {
                    //MessageBox.Show("Failed to load API key from file (invalid key).\nPlease set your API Key at the bottom of the settings window.");
                    return "";
                }
            }
            catch
            {
                //MessageBox.Show("Failed to load API key from file (api_key.ini couldn't be loaded).\nPlease set your API Key at the bottom of the settings window.");
                return "";
            }
        }


        //neodement:
        //save API key to "api_key.ini"
        private void SaveAPIKey()
        {

            try
            {
                 File.WriteAllText(app_data_dir + "api_key.ini", steam_api_key);
                 boxL.Visible = false;
                 boxT.Visible = false;
                 boxR.Visible = false;
                 boxB.Visible = false;
            }
            catch
            {
                MessageBox.Show("Failed to save API key to file.");
            }
        }


        #endregion

    }
}
