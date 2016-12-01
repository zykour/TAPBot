using System;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class DealAction : BotAction
    {
        public DealAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            Listing deal = apiService.GetDailyDeal();

            if (deal == null)
            {
                return sendContext;
            }

            string listingEntry = String.Empty;
            
            listingEntry = "#" + deal.ListingID + " (SALE) " + deal.ListingName + " - " + deal.SaleOrDefaultPrice() + " point";
            if (deal.SaleOrDefaultPrice() > 1)
            {
                listingEntry += "s";
            }
            listingEntry += " (" + Math.Round(deal.GetSalePercent() * 100) + "%)";
            

            if (deal.Quantity > 1)
            {
                listingEntry += " - " + deal.Quantity + " copies.";
            }
            else
            {
                listingEntry += " - " + deal.Quantity + " copy.";
            }

            sendContext.OutgoingMessage = listingEntry;

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!deal") == 0 ||
                 chatInput.CompareTo("/deal") == 0 ||
                 chatInput.CompareTo("!daily") == 0 ||
                 chatInput.CompareTo("/daily") == 0 ||
                 chatInput.CompareTo("!dailydeal") == 0 ||
                 chatInput.CompareTo("/dailydeal") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
