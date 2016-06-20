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
        protected DateTime currentDate;
        protected string message;

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.GroupID == null)
            {
                return "";
            }

            if (botContext.GroupID.ConvertToUInt64().ToString().CompareTo("110338190875693447") != 0)
            {
                return "";
            }

            //if (currentDate.Date.CompareTo(DateTime.Today.Date) != 0 || String.IsNullOrEmpty(message))
            //{
                GetPOTW(botContext);
            //}

            return message;
        }
        
        protected void GetPOTW(BotContext botContext)
        {
            currentDate = DateTime.Today;

            string groupWebsite = new WebClient().DownloadString("http://steamcommunity.com/groups/Tap_gaming");    
            int position = groupWebsite.IndexOf("Group Player of the Week");

            Regex playerRegex = new Regex(@".*(a href="")(http://steamcommunity\.com/(id|profiles)/.*)("").*");
            Match htmlMatch = playerRegex.Match(groupWebsite, position);
            
            if (htmlMatch.Success)
            {   
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string playerWebsite = webClient.DownloadString(htmlMatch.Groups[2].Value);
                Regex nameRegex = new Regex(@".*(<title>Steam Community :: )(.*)(</title>)");
                Match nameMatch = nameRegex.Match(playerWebsite);

                if (nameMatch.Success)
                {
                    message = "The princess of the week is " + nameMatch.Groups[2].Value;
                }
            }

            return;
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
