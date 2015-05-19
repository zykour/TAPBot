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
    class RollAction : BotAction
    {

        protected override string ProduceChatMessage(BotContext botContext)
        {
            Regex rollFormat = new Regex(@"[!/](roll )([0-9]+)\-([0-9]+)");
        
            Match match = rollFormat.Match(botContext.Command);

            int lower = 1;
            int upper = 100;

            if (match.Success)
            {
                try
                {
                    lower = Int32.Parse(match.Groups[2].ToString().Trim());
                }
                catch ( OverflowException e )
                {
                    lower = Int32.MaxValue - 1;
                }

                try 
                {
                    upper = Int32.Parse(match.Groups[3].ToString().Trim());
                }
                catch ( OverflowException e )
                {
                    upper = Int32.MaxValue - 1;
                }
            }

            if (lower > upper)
            {
                int temp = lower;
                lower = upper;
                upper = temp;
            }

            Random rg = new Random();
            int randomNum = rg.Next(lower, upper + 1);

            return "rolled a " + randomNum;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if ( chatInput.StartsWith("!roll") || 
                 chatInput.StartsWith("/roll") )
            {
                return true;
            }

            return false;
        }
    }
}
