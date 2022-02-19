using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TFMV.SourceEngine
{
    public static class VMT
    {
        // reads VMT file
        // searches for $parameter_name 
        // if found changes value to input param_value and re-writes VMT file
        public static void set_parameter(string vmt_path, string param_name, string param_value)
        {
            if (!File.Exists(vmt_path) || param_name.Trim() == "") { return; }

            // replace comma by dot
            param_value = param_value.Replace(",", ".");

            List<string> vmt_lines = new List<string>();
            bool param_exists = false;

            try
            {

                #region read vmt into vmt_lines string list

                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(vmt_path);
                while ((rline = file.ReadLine()) != null)
                {
                    vmt_lines.Add(rline);
                }

                file.Close();
                #endregion

                // remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    // replace double spaces by single
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    //make sure its not a line that's commented out
                    if (!line.StartsWith("//") && line != "")
                    {
                        string[] line_split = line.Split('"');

                        if (line_split.Length > 1)
                        {
                            // if its parameter_name $something
                            if (line_split[1].Equals("$" + param_name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                param_exists = true;
                                vmt_lines[i] = "\t\"$" + param_name + "\" \"" + param_value + "\"";
                            }


                        }
                    }
                }


                // write new vmt IF the parameter was found
                if (param_exists)
                {
                    File.WriteAllLines(vmt_path, vmt_lines);
                }

            }
            catch (IOException)
            {
            }

        }

        // returns lists of vaules for the given list (param_list) of $parameterNames
        public static string[,] get_parameters(string vmt_path, List<string> param_list)
        {
            if (File.Exists(vmt_path) == false) { return null; }
            string[,] results = new string[param_list.Count, 2];


            try
            {
                #region read vmt lines to list

                List<string> vmt_lines = new List<string>();
                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(vmt_path);
                while ((rline = file.ReadLine()) != null)
                {
                    vmt_lines.Add(rline);
                }

                file.Close();

                #endregion

                //remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    // ignore comment lines or empty lines
                    if (!line.StartsWith("//") && line != "")
                    {
                        //string[] line_split = ((line.ToLower()).Replace("  ", string.Empty)).Split('"');

                        List<String> line_split = ((line.ToLower()).Replace("  ", string.Empty)).Split('"').ToList();

                        line_split = line_split.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                        for (int p = 0; p < param_list.Count; p++)
                        {
                            // parameter name
                            string param_name = "$" + param_list[p].ToLower();

                            // check if param/value is already in results
                            bool result_exists = false;
                            for (int r = 0; r < param_list.Count; r++)
                            {
                                if (results[r, 0] != null)
                                {
                                    if (results[r, 0] == param_list[p])
                                    {
                                        result_exists = true;
                                    }
                                }
                            }

                            // skip param value if it already exists in results array
                            if (result_exists) { continue; }

                            if (line.Contains('"'))
                            {
                                if ((line_split[0].Contains(param_name)))
                                {
                                    results[p, 0] = param_list[p];
                                    results[p, 1] = line_split[1].Replace("{", "").Replace("}", "");
                                }
                            }


                            if (line_split.Count > 1)
                            {
                                if (line_split[1] == param_name)
                                {
                                    results[p, 0] = param_list[p];
                                    results[p, 1] = line_split[3].Replace("{", "").Replace("}", "");
                                }
                            }
                        }

                    }
                }

            }
            catch (IOException)
            {
            }

            return results;


        }

        // returns true if VMT contains parameters for painting
        public static bool check_paintability(string filepath)
        {
            string[] vmt_lines = null;

            try
            {

                if (!File.Exists(filepath))
                {
                    // MessageBox.Show("Error: invalid vmt path " + filepath);
                    return false;
                }
                vmt_lines = File.ReadAllLines(filepath);

                //clean multiple spaces by a single space to avoid parsing issues
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"[ ]{2,}", options);

                //cleanup file
                for (int i = 0; i < vmt_lines.Length; i++)
                {
                    string line = regex.Replace(vmt_lines[i], @" ").Replace("  ", " ");

                    //make sure its not a line that's commented out
                    if (!line.StartsWith("//") && !line.StartsWith("\t//") && line != "" && !line.Contains("srcvar"))
                    {
                        //find color and get it
                        if ((line.Contains("colortint_base")) || (line.Contains("colortintbase"))) // || (line.Contains("color2")
                        {
                            string[] tmpx = line.Split('"');

                            return true;
                        }

                    }
                }

            }
            catch (IOException)
            {
            }

            return false;
        }

        // sets paint color2
        public static void set_color2(string filepath, string color)
        {
            if (!File.Exists(filepath) || color == "" || color == " ") { return; }

            List<string> vmt_lines = new List<string>();

            int colorbase_line = -1;
            Boolean color2_exists = false;
            string colortint_base = "";

            try
            {
                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((rline = file.ReadLine()) != null)
                {
                    //vmt_raw_list.Add(rline);
                    vmt_lines.Add(rline);
                }

                file.Close();

                //remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    //make sure its not a line that's commented out
                    if (!line.StartsWith("//") && line != "")
                    {
                        string[] line_split = line.Split('"');

                        if (line_split.Length > 1)
                        {
                            //find color2 and get it
                            if (line_split[1] == "$color2")
                            {
                                color2_exists = true;
                                vmt_lines[i] = "\t\"$color2\" \"{" + color + "}\"";
                            }



                            if (line.Contains('"'))
                            {
                                if ((line_split[0].Contains("$colortint_base")) || (line_split[0].Contains("$colortintbase")))
                                {
                                    colorbase_line = i;
                                    if (color == "")
                                    {
                                        color = colortint_base;
                                    }
                                    else
                                    {
                                        colortint_base = line_split[1];
                                    }
                                }
                            }
                            //find colortint_base and get it
                            if ((line_split[1] == "$colortint_base") || (line_split[1] == "$colortintbase"))
                            {
                                colorbase_line = i;
                                if (color == "")
                                {
                                    color = colortint_base;
                                }
                                else
                                {
                                    colortint_base = line_split[3];
                                }
                            }

                        }
                    }
                }

                if ((color2_exists == false) && (colorbase_line > -1))
                {
                    vmt_lines.Insert(colorbase_line, "\t\"$color2\" \"{" + color + "}\"");
                }

                File.WriteAllLines(filepath, vmt_lines);


            }
            catch (IOException)
            {
            }




        }

        // returns colorbase value
        public static string get_colorbase(string filepath)
        {
            if (File.Exists(filepath) == false) { return ""; }

            string colortint_base = "";
            List<string> vmt_lines = new List<string>();

            try
            {
                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((rline = file.ReadLine()) != null)
                {
                    vmt_lines.Add(rline);
                }

                file.Close();

                //remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    if (!line.StartsWith("//") && line != "")
                    {
                        string[] line_split = line.ToLower().Split('"');
                        if (line.Contains('"'))
                        {
                            if ((line_split[0].Contains("$colortint_base")) || (line_split[0].Contains("$colortintbase")))
                            {
                                colortint_base = line_split[1].Replace("{", "").Replace("}", "");
                            }
                        }

                        if (line_split.Length > 1)
                        {
                            //find colortint_base and get it
                            if ((line_split[1] == "$colortint_base") || (line_split[1] == "$colortintbase"))
                            {
                                colortint_base = line_split[3].Replace("{", "").Replace("}", "");
                            }
                        }
                    }
                }




            }
            catch (IOException)
            {
            }

            if (colortint_base == "") { colortint_base = "255 255 255"; }

            return colortint_base.Trim();
        }

        // removes VMT parameter and value $color2
        public static void remove_color2(string filepath)
        {
            if (File.Exists(filepath) == false) { return; }

            List<string> vmt_lines = new List<string>();

            try
            {
                string rline;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((rline = file.ReadLine()) != null)
                {
                    vmt_lines.Add(rline);
                }

                file.Close();

                //remove all tabs, white spaces etc
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"\s*?$", options);

                for (int i = 0; i < vmt_lines.Count; i++)
                {
                    string line = ((regex.Replace(vmt_lines[i], @" ")).Trim()).ToLower().Replace("  ", " ");

                    //make sure its not a line that's commented out
                    if (!line.StartsWith("//") && line != "")
                    {
                        string[] line_split = line.ToLower().Split('"');

                        if (line_split.Length > 1)
                        {
                            //find color2 and get it
                            if (line_split[1] == "$color2")
                            {
                                vmt_lines.RemoveAt(i);
                                i--;
                            }
                        }

                    }
                }

                File.WriteAllLines(filepath, vmt_lines);
                file.Close();

            }
            catch (IOException)
            {
            }
        }
    }
}
