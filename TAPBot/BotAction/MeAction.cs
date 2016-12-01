using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class MeAction : BotAction
    {
        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("/me ") ||
                chatInput.StartsWith("!me ") )
            {
                return true;
            }

            return false;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            return "*" + botContext.Command.Substring(3).Trim() + "*";
        }
    }
}
