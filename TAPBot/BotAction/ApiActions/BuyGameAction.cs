using System;
using TheAfterParty.Domain.Services;
using TheAfterParty.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAPBot.Context_and_Utility;

namespace TAPBot
{
    class BuyGameAction : BotAction
    {
        private List<BuyEntry> pendingPurchases;

        public BuyGameAction(Func<ApiService> CreateApiService, List<BuyEntry> pendingPurchases) : base(CreateApiService)
        {
            this.pendingPurchases = pendingPurchases;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string helpText = "!buygame {TAP game ID} , i.e. to buy a game with an id of 5: !buygame 5";

            if (sendContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string buyCommand = "!buygame ";

            string gameId = botContext.Command.Substring(buyCommand.Length);

            int id = 0;

            Int32.TryParse(gameId.Trim(), out id);

            AppUser user = apiService.GetUserBySteamID(botContext.FriendID.ConvertToUInt64());
            
            if (id == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }
            else if (user == null)
            {
                return sendContext;
            }

            Listing targetItem = apiService.GetListingByID(id);

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

            BuyEntry newEntry = new BuyEntry(user.Id, id, targetItem.SaleOrDefaultPrice());

            pendingPurchases.Add(newEntry);

            output += "Added a new pending purchase for \"" + targetItem.ListingName + "\". Please !confirm the purchase.";

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!buygame ") || chatInput.StartsWith("/buygame "))
            {
                return true;
            }

            return false;
        }
    }
}
