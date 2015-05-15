using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class RerollAction : BotAction
    {
        private DailyDealAction dealAction;

        protected RerollAction() { }
        public RerollAction(DailyDealAction dealAction)
        {
            this.dealAction = dealAction;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.FriendID.ConvertToUInt64().ToString().CompareTo("76561198030277114") == 0)
            {
                dealAction.Reroll();
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!reroll") == 0 ||
                chatInput.CompareTo("/reroll") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
