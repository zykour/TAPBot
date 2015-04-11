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
    class MilkshakeAction {
        
        protected int milkshakes;
        protected string results;

        public MilkshakeAction(string friendId, string chatId) 
        {
            milkshakes = 0;
            results = "";
        }

        public void Execute()
        {
            milkshakes++;
            results = "Milkshakes: " + milkshakes;
        }

        public string GetMessage()
        {
            return results;
        }
    }
}
