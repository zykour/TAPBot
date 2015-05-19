using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class ConfirmBuyAction : BotAction
    {
        private LinkedList<UserEntry> pendingPurchases;

        // this should be a reference to the dealEntry in BuyDealAction
        private DealEntry dealEntry;

        private DateTimeWrapper currentDealDate;
        private DailyDealAction dailyDealAction;

        protected ConfirmBuyAction() {}
        public ConfirmBuyAction(LinkedList<UserEntry> pendingPurchases, DateTimeWrapper date, DailyDealAction dailyDealAction)
        {
            this.pendingPurchases = pendingPurchases;
            currentDealDate = date;
            this.dailyDealAction = dailyDealAction;
            dealEntry = dailyDealAction.Deal;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (DateTime.Compare(currentDealDate.Date, DateTime.Today) != 0)
            {
                pendingPurchases.Clear();
                currentDealDate.Date = DateTime.Today;
                dealEntry = dailyDealAction.Deal;
            }

            UserEntry buyer = CoopShopUtility.GetUserEntry(botContext);

            if (buyer == null)
            {
                return "";
            }

            foreach (UserEntry user in pendingPurchases)
            {
                if (user.Name.CompareTo(buyer.Name) == 0)
                {
                    pendingPurchases.Remove(user);

                    if (buyer.Balance < dealEntry.Price)
                    {
                        return buyer.Name + ", you no longer have enough points!";
                    }

                    string chatMessage = buyer.Name + " wishes to buy " + dealEntry.Name + " for " + dealEntry.Price + " points.";
                    string returnMessage = buyer.Name + ", a purchase request has been sent to Monukai for '" + dealEntry.Name + "'!";

                    if ((dealEntry.Quantity - 1) <= 0)
                    {
                        pendingPurchases.Clear();
                        dailyDealAction.Reroll();
                        dealEntry = dailyDealAction.Deal;
                    }
                    else
                    {
                        dealEntry.Quantity = dealEntry.Quantity - 1;
                    }

                    botContext.SteamFriend.SendChatMessage(new SteamID(Convert.ToUInt64("76561198030277114")), EChatEntryType.ChatMsg, chatMessage);

                    return returnMessage;
                }
            }

            return "It does not appear you have attempted to buy this deal, do so with '!buydeal'";
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("/confirm") == 0 ||
                chatInput.CompareTo("!confirm") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
