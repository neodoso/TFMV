// by/from https://github.com/Shaun1i00/tf2itemseditor

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace TFMV
{
    public class VDF_parser
    {


        public class VDF_node
        {

            public VDF_node nparent { get; set; }
            public List<VDF_node> nSubNodes { get; set; }

            public string nkey { get; set; }
            public string nvalue { get; set; }



            /// <summary>
            /// Initializes a DataNode without a key a value or subnodes.
            /// </summary>
            public VDF_node()
            {
                nSubNodes = new List<VDF_node>();
            }


            /// <summary>
            /// Initializes a DataNode with a key and a value
            /// </summary>
            /// <param name="key">The key</param>
            /// <param name="value">The value</param>
            /// <param name="parent">The parent of this DataNode</param>
            public VDF_node(string key, string value, VDF_node parent)
                : this(key, value)
            {
                nparent = parent;
            }


            /// <summary>
            /// Initializes a DataNode with a key and a value
            /// </summary>
            /// <param name="key">The key</param>
            /// <param name="value">The value</param>
            public VDF_node(string key, string value)
                : this()
            {
                nkey = key;
                nvalue = value;
            }



            /// <summary>
            /// Initializes a DataNode with only a key (this DataNode can contain SubNodes instead of a value)
            /// </summary>
            /// <param name="key">The key</param>
            /// <param name="parent">The parent of this DataNode</param>
            public VDF_node(string key, VDF_node parent)
                : this(key)
            {
                nparent = parent;
            }


            /// <summary>
            /// Initializes a DataNode with only a key (this DataNode can contain SubNodes instead of a value)
            /// </summary>
            /// <param name="key">The key</param>
            public VDF_node(string key) : this()
            {
                nkey = key;
            }

            /// <summary>
            /// Gets a string containing the required number of \t for correct indentation in a file.
            /// </summary>
            /// <returns>A string with \t repeated the node's level times (see also <seealso cref="find_node_level_num"/>)</returns>
            public string get_ident_str()
            {
                int lvl = find_node_level_num();
                return String.Concat(Enumerable.Repeat("\t", lvl));
            }

            /// <summary>
            /// Finds the level that this DataNode has (how many Parents until the root node)
            /// </summary>
            /// <returns>The level number</returns>
            public int find_node_level_num()
            {
                VDF_node cur = this;
                int ret = 0;
                while (true)
                {
                    cur = cur.nparent;
                    if (cur != null) ret++;
                    else return ret;
                }
            }

            public int find_node_parent() // bs
            {
                VDF_node cur = this;
                int ret = 0;

                int lvl = find_node_level_num();

                while (true)
                {
                    cur = cur.nparent;
                    if (cur == null) ret = (ret--) + lvl;
                    else return ret;
                }
            }


            public override string ToString()
            {
                string s = "";
                foreach (VDF_node n in nSubNodes)
                {
                    s += n.get_ident_str() + n;
                }
                return string.Format(nkey + " = " + (nvalue == null ? null : nvalue.Replace("\r\n", "\r\n" + get_ident_str())) + "\r\n" + s);
            }
        }


        #region vdf parser

        public string name { get; set; } //bs
        public string file_path { get; set; }

		bool _indent = true;
        public VDF_node RootNode { get; set; }

        /// <summary>
        /// Creates a new Valve Format Parser
        /// </summary>
        public VDF_parser()
        {
            
        }


        /// <summary>
        /// Creates a new Valve Format Parser
        /// </summary>
        /// <param name="path">File to parse</param>
        public VDF_parser(string _path, string _name)
        {
            file_path = _path;
            name = _name;
        }


        /// <summary>
        /// Clears all nodes and loads them from <see cref="file_path"/>
        /// </summary>
        public void load_VDF_file()
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(file_path);
            } catch {
                return;
            }

            RootNode = new VDF_node();
            Regex regex;
            MatchCollection matches;
            var currentNode = new VDF_node();
            bool multiLineValue = false;
            VDF_node multiLineNode = new VDF_node();
            foreach (string _line in lines)
            {
                string line = _line.TrimEnd(new[] { '\t', '\n', '\r' }).TrimStart(new[] { '\t', '\n', '\r' }).Replace("\r\n", ""); //Can't change the value of _line, trim whitespace chars and assign to line
                if (multiLineValue)
                {
                    regex = new Regex("(.*)\"");
                    matches = regex.Matches(line);
                    if (matches.Count == 1)
                    {
                        multiLineNode.nvalue += matches[0].Groups[1];
                        multiLineValue = false;
                        currentNode.nSubNodes.Add(multiLineNode);
                        Console.Write(matches[0].Groups[1] + "\r\n");
                        continue;
                    }
                    regex = new Regex("(.*)");
                    matches = regex.Matches(line);
                    if (matches.Count == 1)
                    {
                        multiLineNode.nvalue += matches[0].Groups[1] + "\r\n";
                        Console.Write(matches[0].Groups[1] + "\r\n");
                        continue;
                    }
                }
                regex = new Regex("\"(.*?)\"");
                matches = regex.Matches(line);
                if (matches.Count == 1 && line.Count(f => f == '"') == 2)
                {
                    string k = matches[0].Groups[1].Value;
                    if (currentNode.nkey == null)
                        currentNode = new VDF_node(k);
                    else
                    {
                        var no = new VDF_node(k, currentNode);
                        currentNode.nSubNodes.Add(no);
                        currentNode = no;
                    }
                    continue;
                }

                if (line.Trim(new[] { ' ', '\r', '\n', '\t' }) == "{")
                {
                    continue;
                }
                if (line.Trim(new[] { ' ', '\r', '\n', '\t' }) == "}")
                {
                    currentNode = currentNode.nparent ?? currentNode;
                    continue;
                }
                regex = new Regex(@"""(.*?)""\s*""(.*?)""");
                matches = regex.Matches(line);




                if (matches.Count > 1)
                {
                    throw new KVs_exception(line);
                }
                if (matches.Count == 1)
                {
                    var k = matches[0].Groups[1].Value;
                    var v = matches[0].Groups[2].Value;
                    currentNode.nSubNodes.Add(new VDF_node(k, v, currentNode));
                    continue;
                }
                regex = new Regex(@"""(.*?)""\s*""(.*)");
                matches = regex.Matches(line);
                if (matches.Count == 1)
                {
                    multiLineValue = true;
                    multiLineNode = new VDF_node(matches[0].Groups[1].Value, matches[0].Groups[2].Value + "\r\n", currentNode);
                    continue;
                }

            }
            RootNode = currentNode;

        }


        /// <summary>
        /// Thrown when multiple key-value pairs have been found on a single line
        /// </summary>
        private class KVs_exception : Exception
        {
            public string _line { get; set; }
            public KVs_exception(string line)
            {
                _line = line;
            }
        }


    	/// <summary>
    	/// Saves the file
    	/// </summary>
    	/// <param name="path">The path to save the file</param>
    	/// <param name="indent">Defines whether to indent the nodes or not</param>
    	public void save_VDF_file(string path, bool indent = true)
    	{
    		_indent = indent;
            File.WriteAllText(path, write_line(RootNode, (path.Split('.')[0]).ToString())); //path.Split('.')[0] is bs
        }


        public VDF_node sGet_NodePath(string path)
        {

            VDF_node node = this.RootNode;

            string[] keys = path.Split('.');

            if (node.nkey.ToLower() == keys[0].ToLower())
            {
                for (int k = 1; k < keys.Length; k++)
                {
                    node = sGet_Node(node, keys[k]);
                }
            }

            return node;
        }



        private string write_line(VDF_node node, string _name)
        {
            string append = "";

            if (!String.IsNullOrEmpty(node.nkey))
            {
                append += (_indent ? node.get_ident_str() : "") + "\"" + node.nkey + "\"";
            }


            if (node.nvalue != null) 
            {
                append += "\t\"" + node.nvalue + "\"\r\n";
            }
            else
            {
				append += "\r\n" + (_indent ? node.get_ident_str() : "") +"{\r\n";
            }
            foreach (VDF_node sub in node.nSubNodes)
            {
                append += write_line(sub, _name);
            }


            if (node.nvalue == null)
            {
				append += (_indent ? node.get_ident_str() : "") + "}\r\n";
            }

            return append;
        }

        // FindInSubNodes
        public VDF_node Find_SubNode(VDF_node in_node, VDF_node search_node)
        {
            if ((in_node == null) || (search_node == null))
            {
                return null;
            }

            foreach (VDF_node the_node in in_node.nSubNodes)
            {
                if (the_node == search_node) return the_node;
                var subN = Find_SubNode(search_node, the_node);
                if (subN != null) return subN;
            }
            return null;
        }

        public string sGet_KeyVal(VDF_node node, string key)
        {
            string value = "";
            for (int i = 0; i < node.nSubNodes.Count; i++)
            {
                if (node.nSubNodes[i].nkey == key)
                {
                    value = node.nSubNodes[i].nvalue;
                }
                
            }

            return value;
        }

        public VDF_node sGet_Node(VDF_node node, string key)
        {
            VDF_node result = new VDF_node();

            for (int i = 0; i < node.nSubNodes.Count; i++)
            {
                if (node.nSubNodes[i].nkey.ToLower() == key.ToLower())
                {
                    result = node.nSubNodes[i];
                }
            }

            return result;
        }


        /// <summary>
        /// Finds the parent node of the given
        /// </summary>
        /// <param name="node">The node to find the parent of</param>
        /// <returns>The parent node or null when the node has no parent</returns>
        public VDF_node Find_Parent_Node(VDF_node node)
        {
            foreach (VDF_node n in RootNode.nSubNodes)
            {
                if (n == node) return n.nparent;
                var subN = Find_SubNode(node, n);
                if (subN != null) return subN;
            }
            return null;
        }

        #endregion
    }
}
