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
    class MilkshakeAction : BotAction {
        
        protected int milkshakes;

        public MilkshakeAction() 
        {
            milkshakes = 0;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!milkshake") == 0 ||
                chatInput.CompareTo("!milkshakes") == 0 ||
                chatInput.CompareTo("/milkshake") == 0 ||
                chatInput.CompareTo("/milkshakes") == 0)
            {
                return true;
            }

            return false;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            milkshakes += 1;

            return "Milkshakes: " + milkshakes;
        }
    }
}
