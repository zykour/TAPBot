using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;
using System.IO;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class IDAction : BotAction
    {
        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!myid") == 0 ||
                chatInput.CompareTo("!id") == 0 ||
                chatInput.CompareTo("/myid") == 0 ||
                chatInput.CompareTo("/id") == 0)
            {
                return true;
            }

            return false;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext outgoingContext = botContext.Clone();
            outgoingContext.OutgoingMessage = "Your 64-bit SteamID is: " + botContext.FriendID.ConvertToUInt64().ToString();

            return outgoingContext;
        }
    }
}
