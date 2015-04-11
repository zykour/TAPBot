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
    class RollAction : ChatBotAction
    {

        protected string msg;

        public RollAction(string friendId, string chatId, string msg) 
            : base(friendId, chatId)
        {
            this.msg = msg;
        }

        public override void Execute() 
        {
            Regex rollFormat = new Regex(@"[!/](roll )([0-9]+)\-([0-9]+)");
        
            Match match = rollFormat.Match(msg);

            int lower = 1;
            int upper = 100;

            if (match.Success)
            {
                lower = Int32.Parse(match.Groups[2].ToString().Trim());
                upper = Int32.Parse(match.Groups[3].ToString().Trim());
            }

            if (lower > upper)
            {
                int temp = lower;
                lower = upper;
                upper = temp;
            }

            Random rg = new Random();
            int randomNum = rg.Next(lower, upper + 1);

            results = "Rolled a " + randomNum;
            messageAvailable = true;
            success = true;
        }
    }
}
