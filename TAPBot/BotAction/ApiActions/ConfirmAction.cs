using System;
using TheAfterParty.Domain.Services;
using TheAfterParty.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAPBot.Context_and_Utility;

namespace TAPBot
{
    class ConfirmAction : BotAction
    {
        private List<BuyEntry> pendingPurchases;

        public ConfirmAction(Func<ApiService> CreateApiService, List<BuyEntry> pendingPurchases) : base(CreateApiService)
        {
            this.pendingPurchases = pendingPurchases;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            BuyEntry targetEntry = null;

            AppUser user = apiService.GetUserBySteamID(botContext.FriendID.ConvertToUInt64());

            if (user == null)
            {
                return sendContext;
            }

            foreach (BuyEntry entry in pendingPurchases)
            {
                if (entry.UserID.CompareTo(user.Id) == 0)
                {
                    targetEntry = entry;
                }
            }

            pendingPurchases.Remove(targetEntry);

            Listing targetListing = apiService.GetListingByID(targetEntry.ListingID);

            // if the price for the item since being added to pending purchases, adjust the price to make it lower (store policy is okay to honor the price if the price has risen)
            if (targetListing.SaleOrDefaultPrice() < targetEntry.PriceAtReservation)
            {
                targetEntry.PriceAtReservation = targetListing.SaleOrDefaultPrice();
            }

            if (user.Balance < targetEntry.PriceAtReservation)
            {
                sendContext.OutgoingMessage = "You now have insufficient funds to purchase this item.";
                return sendContext;
            }

            Order order = apiService.BuyAndRevealListing(user, targetListing.ListingID, targetEntry.PriceAtReservation);

            string outputMsg = String.Empty;

            if (order != null)
            {
                ProductOrderEntry entry = order.ProductOrderEntries.SingleOrDefault();

                outputMsg = "Purchase of \"" + targetListing.ListingName + "\" successful!";

                if (entry.ClaimedProductKeys.Count == 1)
                {
                    ClaimedProductKey key = entry.ClaimedProductKeys.FirstOrDefault();

                    if (key.IsGift == false)
                    {
                        outputMsg += " Your key for \"" + targetListing.ListingName + "\": " + key.Key;
                    }
                    else
                    {
                        outputMsg += " Your product is a gift copy and will be sent to you shortly.";
                    }
                }
                else
                {
                    foreach (ClaimedProductKey key in entry.ClaimedProductKeys)
                    {
                        if (key.IsGift == false)
                        {
                            outputMsg += "\n\"" + key.Listing.ListingName + "\": " + key.Key;
                        }
                        else
                        {
                            outputMsg += "\n\"" + key.Listing.ListingName + "\": Gift copy";
                        }
                        
                    }
                }

                SendFriendMessage(botContext, outputMsg);
                
                if (sendContext.GroupID != null)
                {
                    sendContext.OutgoingMessage = "Purchase successful! Please check your private messages.";
                }
            }
            else
            {
                sendContext.OutgoingMessage = "Purchase errors, please contact an admin.";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("/confirm") == 0 || chatInput.CompareTo("/confirm") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
