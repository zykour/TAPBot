using System;
using TheAfterParty.Domain.Services;
using TheAfterParty.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAPBot.Context_and_Utility;

namespace TAPBot
{
    class BuyDealAction : BotAction
    {
        private List<BuyEntry> pendingPurchases;

        public BuyDealAction(Func<ApiService> CreateApiService, List<BuyEntry> pendingPurchases) : base(CreateApiService)
        {
            this.pendingPurchases = pendingPurchases;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();


            string helpText = "!buydeal - if you have sufficient funds, this command will add a pending purchase for today's daily day";

            if (sendContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }
            
            AppUser user = apiService.GetUserBySteamID(botContext.FriendID.ConvertToUInt64());

            if (user == null)
            {
                return sendContext;
            }

            Listing targetItem = apiService.GetDailyDeal();

            if (targetItem == null)
            {
                sendContext.OutgoingMessage = "Daily deal could not be found.";

                return sendContext;
            }

            if (user.Balance < targetItem.SaleOrDefaultPrice())
            {
                sendContext.OutgoingMessage = "You have insufficient funds to purchase this item.";
                return sendContext;
            }

            string output = String.Empty;
            BuyEntry toRemove = null;

            foreach (BuyEntry entry in pendingPurchases)
            {
                if (entry.UserID.CompareTo(user.Id) == 0)
                {
                    toRemove = entry;
                    break;
                }
            }

            if (toRemove != null)
            {
                Listing removedItem = apiService.GetListingByID(toRemove.ListingID);
                output = "Removed an old entry for \"" + removedItem.ListingName + "\". ";
                pendingPurchases.Remove(toRemove);
            }

            BuyEntry newEntry = new BuyEntry(user.Id, targetItem.ListingID, targetItem.SaleOrDefaultPrice());

            pendingPurchases.Add(newEntry);

            output += "Added a new pending purchase for \"" + targetItem.ListingName + "\". Please !confirm the purchase.";

            sendContext.OutgoingMessage = output;

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("/buydeal") == 0 || chatInput.CompareTo("/buydeal") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
