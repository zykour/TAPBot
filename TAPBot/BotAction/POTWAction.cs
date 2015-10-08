using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using SteamKit2;

namespace TAPBot
{
    class POTWAction : BotAction
    {

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.GroupID.ToString().CompareTo("103582791434637703") != 0)
            {
                return "";
            }

            string groupWebsite = new WebClient().DownloadString("http://steamcommunity.com/groups/Tap_gaming");
            int position = groupWebsite.IndexOf("Group Player of the Week");

            Regex playerRegex = new Regex(@".*(a href="")(http://steamcommunity\.com/id/.*)("").*");
            Match htmlMatch = playerRegex.Match(groupWebsite, position);
            
            if (htmlMatch.Success)
            {
                string playerWebsite = new WebClient().DownloadString(htmlMatch.Groups[1].Value);
                Regex nameRegex = new Regex(@".*(<title>Steam Community :: )(.*)(</title>)");
                Match nameMatch = nameRegex.Match(playerWebsite);

                if (nameMatch.Success)
                {
                    return nameMatch.Groups[1].Value;
                }
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!princess") == 0 ||
                chatInput.CompareTo("/princess") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
