using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class BalanceBotAction : BotAction
    {

        protected override string ProduceChatMessage(BotContext botContext)
        {
            UserEntry userEntry = CoopShopUtility.GetUserEntry(botContext);

            if (userEntry != null)
            {
                return userEntry.Name + ", your balance is " + userEntry.Balance;
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!balance") == 0 ||
                chatInput.CompareTo("/balance") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
