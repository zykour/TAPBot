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
    class MilkshakeAction : ChatMsgBotAction {
        
        protected int milkshakes;

        public MilkshakeAction(string friendId, string chatId) 
            : base(friendId, chatId)
        {
            milkshakes = 0;
        }

        public override void Execute()
        {
            milkshakes++;
            results = "Milkshakes: " + milkshakes;
            messageAvailable = true;
            success = true;
        }
    }
}
