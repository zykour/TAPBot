using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace TAPBot
{
    class SetDiscordLinkAction : BotAction
    {
        public List<String> admins;

        public SetDiscordLinkAction(List<String> adminList) : base()
        {
            admins = adminList;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string discordLink = String.Empty;

            if (botContext.Command.StartsWith("!setprincess ") || botContext.Command.StartsWith("/setprincess "))
            {
                string cmd = "!setdiscord ";

                discordLink = botContext.Command.Substring(cmd.Length);
            }

            if (String.IsNullOrWhiteSpace(discordLink))
            {
                sendContext.OutgoingMessage = "Error, please use as follows: !setdiscord {invite link to discord}";
                return sendContext;
            }

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    ConfigurationManager.AppSettings.Set("Discord", discordLink);
                }
            }

            sendContext.OutgoingMessage = "Successfully updated the discord link!";

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!setdiscord ") ||
                 chatInput.StartsWith("/setdiscord ") )
            {
                return true;
            }

            return false;
        }
    }
}
