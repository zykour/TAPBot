using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;

namespace TAPBot
{
    class DiscordAction : BotAction
    {

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            sendContext.OutgoingMessage = ConfigurationManager.AppSettings.Get("Discord");

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if ( chatInput.CompareTo("!discord") == 0 || 
                 chatInput.CompareTo("/discord") == 0 )
            {
                return true;
            }

            return false;
        }
    }
}
