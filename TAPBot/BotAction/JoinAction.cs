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
    class JoinAction : BotAction
    {

        protected override string ProduceChatMessage(BotContext botContext)
        {
            Regex joinCmd = new Regex(@"(!join)(\s+)(.+)");
            Regex validSteamURL = new Regex(@"(http://)?(www\.)?(steamcommunity\.com/groups/)([a-zA-Z0-9_]+)");
            Match match = joinCmd.Match(botContext.Command);
            Match urlMatch;

            if (match.Success)
            {
                urlMatch = validSteamURL.Match(match.Groups[3].Value);
                Console.WriteLine(match.Groups[3].Value);

                if (urlMatch.Success)
                {
                    string html = new WebClient().DownloadString(match.Groups[3].Value);
                    Regex joinChatExpr = new Regex(@".*(joinchat/)([0-9]+).*");
                    Match htmlMatch = joinChatExpr.Match(html);

                    if (htmlMatch.Success)
                    {
                        ulong chatID = 0;
                        if (UInt64.TryParse(htmlMatch.Groups[2].Value, out chatID))
                        {
                            Console.WriteLine("Entering chat...");
                            SteamID groupChatID = new SteamID(chatID);
                            botContext.SteamFriend.JoinChat(groupChatID);
                        }
                    }
                }
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!join ") ||
                chatInput.StartsWith("/join "))
            {
                return true;
            }

            return false;
        }
    }
}
