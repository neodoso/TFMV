using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace TFMV.Functions
{
    public static class miscFunc
    {
        #region TF2

        public static string findclass(string s)
        {
            string return_string = "";
            if (s.Contains("demo")) { return_string = "demo"; }
            if (s.Contains("engineer")) { return_string = "engineer"; }
            if (s.Contains("heavy")) { return_string = "heavy"; }
            if (s.Contains("medic")) { return_string = "medic"; }
            if (s.Contains("pyro")) { return_string = "pyro"; }
            if (s.Contains("scout")) { return_string = "scout"; }
            if (s.Contains("sniper")) { return_string = "sniper"; }
            if (s.Contains("soldier")) { return_string = "soldier"; }

            return return_string;
        }

        public static bool is_valid_tf_class_name(string s)
        {
            s = s.ToLower();
            if (s.Contains("demo")) { return true; }
            if (s.Contains("engineer")) { return true; }
            if (s.Contains("heavy")) { return true; }
            if (s.Contains("medic")) { return true; }
            if (s.Contains("pyro")) { return true; }
            if (s.Contains("scout")) { return true; }
            if (s.Contains("sniper")) { return true; }
            if (s.Contains("soldier")) { return true; }
            if (s.Contains("spy")) { return true; }

            return false;
        }

        // used to check if a model path is a tf2 player model
        public static bool if_mdl_path_is_playermodel(string mdl_path)
        {
            mdl_path = mdl_path.Replace("\\", "/");
            switch (mdl_path)
            {
                case "models/player/scout.mdl":
                    return true;
                case "models/player/soldier.mdl":
                    return true;
                case "models/player/pyro.mdl":
                    return true;
                case "models/player/demo.mdl":
                    return true;
                case "models/player/heavy.mdl":
                    return true;
                case "models/player/engineer.mdl":
                    return true;
                case "models/player/medic.mdl":
                    return true;
                case "models/player/sniper.mdl":
                    return true;
                case "models/player/spy.mdl":
                    return true;


                case "models/player/hwm/scout.mdl":
                    return true;
                case "models/player/hwm/soldier.mdl":
                    return true;
                case "models/player/hwm/pyro.mdl":
                    return true;
                case "models/player/hwm/demo.mdl":
                    return true;
                case "models/player/hwm/heavy.mdl":
                    return true;
                case "models/player/hwm/engineer.mdl":
                    return true;
                case "models/player/hwm/medic.mdl":
                    return true;
                case "models/player/hwm/sniper.mdl":
                    return true;
                case "models/player/hwm/spy.mdl":
                    return true;
            }

            return false;
        }

        #endregion

        #region IO

        //converts long paths to short paths, for "dos" console commands
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
        [MarshalAs(UnmanagedType.LPTStr)] string path,
        [MarshalAs(UnmanagedType.LPTStr)]  StringBuilder shortPath, int shortPathLength);


        public static void create_missing_dir(string path)
        {
//todo: remove these comments
            //System.Windows.Forms.MessageBox.Show(path);
            try
            {
                if (path == "") { return; }
                if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                {
                    return;
                }

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show("mystery error");
            }
        }

        // copies file from one source to a destinaiton, checks if file exists etc
        public static void copy_safe(string source, string destination)
        {
            if (File.Exists(source))
            {
                if (!Directory.Exists(Path.GetDirectoryName(destination))) 
                { 
                    Directory.CreateDirectory(Path.GetDirectoryName(destination)); 
                }
                File.Copy(source, destination, true);
            }
        }

        public static void delete_safe(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Unable to delete file: " + file + "\nFile might be locked by another process \nor TFMV doesn't have the user rights to delete.");
                }
            }
            //neodement: this fixes the tfmv directory in custom not getting removed
            else
            {
                if (Directory.Exists(file))
                {
                    try
                    {
                        Directory.Delete(file);
                    }
                    catch
                    {
                        //System.Windows.Forms.MessageBox.Show("Unable to delete directory: " + file + "\nDirectory might be locked by another process \nor TFMV doesn't have the user rights to delete.");
                    }
                }
            }

        }

        public static void move_dir_safe(string source, string target)
        {
            if(!Directory.Exists(source))
            {
                return;
            }

            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            Directory.Delete(source, true);
        }

        public class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
        }

        public static void DeleteDirectoryContent(string target_dir)
        {
           if( !Directory.Exists(target_dir))
           {
                return;
           }

            System.IO.DirectoryInfo di = new DirectoryInfo(target_dir);

            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                } catch {
                
                }

                
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {

                try
                {
                    //neodement: delete stuff even if we're in debug mode. this was making testing difficult.
                   //#if !DEBUG
                        dir.Delete(true);
                   //#endif
                }
                catch
                {
                    //could warn the user if the directories weren't successfully deleted?
                }          
            }
        }


        public static Boolean IsValidPath(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (!driveCheck.IsMatch(path.Substring(0, 3))) return false;
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                return false;

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            if (!dir.Exists)
                dir.Create();
            return true;
        }

        // checks if file is being used by another process
        public static  bool IsFileLocked(string filepath)
        {
           // if (File.Exists(filepath)) { System.Windows.Forms.MessageBox.Show("File does not exist: \n" + filepath); return true; }
            FileInfo file = new FileInfo(filepath);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }



#endregion

        #region dynamic

        public static object getProperty(object containingObject, string propertyName)
        {
            return containingObject.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, containingObject, null);
        }

        public static void setProperty(object containingObject, string propertyName, object newValue)
        {
            containingObject.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, containingObject, new object[] { newValue });
        }

        #endregion

        #region string

        public static bool In(this object obj, params object[] objects)
        {
            if (objects == null || obj == null)
                return false;
            object found = objects.FirstOrDefault(o => o.GetType().Equals(obj.GetType()) && o.Equals(obj));
            return (found != null);
        }

        #endregion
   
    }
}
