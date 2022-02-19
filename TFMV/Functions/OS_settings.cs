using System;
using System.Management;

namespace TFMV.Functions
{
    class OS_Settings
    {
        public string os_version { get; set; }

        Point4 win_xp_hlmv_padding = new Point4(18, 8, 50, 300);
        Point4 win_7_hlmv_padding = new Point4(18, 8, 50, 300);
        Point4 win_8_hlmv_padding = new Point4(18, 8, 51, 300);
        Point4 win_10_hlmv_padding = new Point4(18, 8, 51, 300);


        public OS_Settings()
        {
            this.os_version = get_os_ver();
        }

        public static string get_OsName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }


        // determines Os version and returns the padding for taking HLMV's render window screenshots (excluding the UI)
        public Point4 get_hlmv_padding()
        {
            Point4 hlmv_padding = new Point4(18, 8, 50, 300);

            switch (this.os_version)
            {
                case "win_xp":
                    return win_xp_hlmv_padding;
                case "win_7":
                    return win_7_hlmv_padding;
                case "win_8":
                    return win_8_hlmv_padding;
                case "win_10":
                    return win_10_hlmv_padding;
            }

            return hlmv_padding;
        }

        public string get_os_ver()
        {
            string OS_version = get_OsName();

            if (OS_version.Contains(" 7 ")) { OS_version = "win_7"; }
            if (OS_version.Contains(" 8 ")) { OS_version = "win_8"; }
            if (OS_version.Contains(" 8.1 ")) { OS_version = "win_8"; }
            if (OS_version.Contains(" 10 ")) { OS_version = "win_10"; }
            if (OS_version.ToLower().Contains(" xp")) { OS_version = "win_xp"; }

            return OS_version;
        }

        public class Point4
        {
            public Int16 left { get; set; }
            public Int16 right { get; set; }
            public Int16 top { get; set; }
            public Int16 bottom { get; set; }


            public Point4(Int16 _left, Int16 _right, Int16 _top, Int16 _bottom)
            {
                this.left = _left;
                this.right = _right;
                this.top = _top;
                this.bottom = _bottom;
            }

        }
    }
}
