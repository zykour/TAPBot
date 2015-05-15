using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class ResetAction : BotAction
    {
        private DailyDealAction dealAction;

        protected ResetAction() { }
        public ResetAction(DailyDealAction dealAction)
        {
            this.dealAction = dealAction;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.FriendID.ConvertToUInt64().ToString().CompareTo("76561198030277114") == 0)
            {
                dealAction.Reset();
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!reset") == 0 ||
                chatInput.CompareTo("/reset") == 0)
            {
                return true;
            }

            return false;
        }

    }
}
