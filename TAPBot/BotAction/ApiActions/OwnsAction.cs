using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class OwnsAction : BotAction
    {
        private Regex OwnsRegex;
        private Regex OwnsRegexURL;

        public OwnsAction(Func<ApiService> CreateApiService) : base(CreateApiService)
        {
            OwnsRegexURL = new Regex(@"(?:!owns |/owns |!own |/own )(?:http|https)://(?:store\.steampowered\.com/)(?:agecheck/)?(?:app/)([0-9]+)[/]*", RegexOptions.IgnoreCase);
            OwnsRegex = new Regex(@"(?:!owns |/owns |!own |/own )([0-9]+)", RegexOptions.IgnoreCase);
        }
        
        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();
            
            int appId = 0;
            Match regexMatch;

            if (OwnsRegex.Match(botContext.Command).Success == true)
            {
                regexMatch = OwnsRegex.Match(botContext.Command);

                appId = Int32.Parse(regexMatch.Groups[1].ToString());
            }
            else
            {
                regexMatch = OwnsRegexURL.Match(botContext.Command);

                appId = Int32.Parse(regexMatch.Groups[1].ToString());
            }

            if (appId == 0)
            {
                sendContext.OutgoingMessage = "Invalid AppID provided.";

                return sendContext;
            }

            String[] owners = apiService.GetUsersWhoOwn(appId).ToArray();

            if (owners.Count() > 0)
            {
                StringBuilder output = new StringBuilder();

                string gameName = GetSteamGameName(appId);

                if (String.IsNullOrWhiteSpace(gameName) == false)
                {
                    output.Append("Own \"" + gameName + "\":");
                }
                else
                {
                    output.Append("Own " + appId + ":");
                }

                for (int i = 0; i < owners.Count() - 1; i++)
                {
                    output.Append(" " + owners[i] + ",");
                }

                if (owners.Count() > 1)
                {
                    output.Append(" and " + owners[owners.Count() - 1]);
                }
                else
                {
                    output.Append(" " + owners[owners.Count() - 1]);
                }

                sendContext.OutgoingMessage = output.ToString();
            }
            else
            {
                sendContext.OutgoingMessage = "No users own this game.";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            return OwnsRegex.Match(chatInput).Success || OwnsRegexURL.Match(chatInput).Success;
        }
    }
}
