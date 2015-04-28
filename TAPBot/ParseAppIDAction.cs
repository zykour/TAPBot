using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class ParseAppIDAction
    {
        // used to determine if we need to reparse
        private DateTime lastParse;

        // key-value pair is String-StoreEntry
        private BinaryTree<string, StoreEntry> apps;

        public ParseAppIDAction()
        {
            apps = new BinaryTree<string, StoreEntry>();
            InitializeApps();
        }

        public StoreEntry Execute(string appId)
        {
            return apps.Search(appId);
        }

        public void InitializeApps()
        {
            lastParse = DateTime.Today;
            Regex inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)(\t)+(http://store\.steampowered\.com/app/)([0-9]+)(.*)?");
            string line = "";

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\zykour\Dropbox\TAP Inventory 1.txt"))
                {

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().CompareTo("Origin") == 0)
                        {
                            // Steam games show up first in the list, don't want to parse past those
                            break;
                        }

                        Match parseMatch = inventoryCmd.Match(line);

                        if (parseMatch.Success)
                        {
                            StoreEntry sEntry = new StoreEntry(Int32.Parse(parseMatch.Groups[2].ToString()),
                                                                Int32.Parse(parseMatch.Groups[1].ToString()),
                                                                parseMatch.Groups[3].ToString(),
                                                                parseMatch.Groups[6].ToString()
                                                               );

                            apps.Insert(new BinaryTreeNode<string,StoreEntry>(sEntry.AppID,sEntry));
                        }
                    }

                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
