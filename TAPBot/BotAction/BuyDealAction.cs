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

        // this should be a reference to the dealEntry in BuyDealAction
        private DealWrapper dealEntry;

        private DateTimeWrapper currentDealDate;
        private DailyDealAction dailyDealAction;

        protected BuyDealAction() {}
        public BuyDealAction(LinkedList<UserEntry> pendingPurchases, DateTimeWrapper date, DailyDealAction dailyDealAction, DealWrapper dealEntry)
        {
            this.pendingPurchases = pendingPurchases;
            this.dailyDealAction = dailyDealAction;
            this.dealEntry = dealEntry;
            currentDealDate = date;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (DateTime.Compare(currentDealDate.Date, DateTime.Today) != 0)
            {
                pendingPurchases.Clear();
                currentDealDate.Date = DateTime.Today;
            }

            dealEntry.Deal = dailyDealAction.Deal;

            UserEntry buyer = CoopShopUtility.GetUserEntry(botContext);

            if (buyer == null)
            {
                return "";
            }

            foreach (UserEntry user in pendingPurchases)
            {
                if (user.Name.CompareTo(buyer.Name) == 0)
                {
                    return buyer.Name + ", please '!confirm' the purchase of '" + dealEntry.Name + "'";
                }
            }
            
            if (buyer.Balance < dealEntry.Price)
            {
                return buyer.Name + ", you do not have enough points!";
            }

            pendingPurchases.AddFirst(buyer);

            return buyer.Name + ", please '!confirm' the purchase of '" + dealEntry.Name + "'";
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
