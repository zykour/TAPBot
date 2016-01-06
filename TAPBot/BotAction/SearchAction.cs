using System.Text;
using System.Threading.Tasks;
using System;
using SteamKit2;
using System.IO;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class SearchAction : BotAction
    {

        protected override string ProduceChatMessage(BotContext botContext)
        {
            Regex searchParam = new Regex(@"[!/](search )(.*)");
            Match searchMatch = searchParam.Match(botContext.Command);
            string searchName = "";

            if (!String.IsNullOrEmpty(searchMatch.Groups[2].ToString()))
            {
                searchName = searchMatch.Groups[2].ToString();
            }
            else
            {
                return "No search terms supplied.";
            }
            
            Regex replRegex = new Regex(@"[^a-z0-9]");
            string testName = searchName.ToLower();
            testName = replRegex.Replace(testName, "");

            Regex itemName = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)\t*(.*)?");

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\zykour\Dropbox\TAP Inventory 1.txt"))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Match match = itemName.Match(line);

                        if (match.Success)
                        {
                            string matchedString = match.Groups[3].ToString().Trim();
                            searchName = matchedString;
                            matchedString = matchedString.ToLower();
                            matchedString = replRegex.Replace(matchedString, "");
                            
                            if (matchedString.CompareTo(testName) == 0)
                            {
                                return searchName + " is available for " + match.Groups[2].ToString().Trim() + " points, just " + match.Groups[1].ToString().Trim() + " left!";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            return "Item not found in the master inventory list.";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!search ")||
                chatInput.StartsWith("/search "))
            {
                return true;
            }

            return false;
        }
    }
}
