using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class BuyDealAction : BotAction
    {
        private LinkedList<UserEntry> pendingPurchases;
        private DealEntry dealEntry;

        protected BuyDealAction() {}
        public BuyDealAction(LinkedList<UserEntry> pendingPurchases)
        {
            this.pendingPurchases = pendingPurchases;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            UserEntry buyer = CoopShopUtility.GetUserEntry(botContext);

            foreach (UserEntry user in pendingPurchases)
            {
                if (user.Name.CompareTo(buyer.Name) == 0)
                {
                    return buyer.Name + ", please '!confirm' the purchase of '" + dealEntry.Name + "'";
                }
            }

            return "";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("/buydeal") == 0 ||
                chatInput.CompareTo("!buydeal") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
