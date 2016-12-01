using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class OtherDealAction : BotAction
    {
        public OtherDealAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            List<Listing> deals = apiService.GetOtherDeals().ToList();
            
            bool friendMsg = deals.Count > 1;

            StringBuilder output = new StringBuilder(String.Empty);

            if (deals.Count != 0)
            {
                for (int i = 0; i < deals.Count; i++)
                {
                    string listingEntry = String.Empty;
                    
                    listingEntry = "#" + deals.ElementAt(i).ListingID + " " + deals.ElementAt(i).ListingName + " - " + deals.ElementAt(i).SaleOrDefaultPrice() + " point";
                    if (deals.ElementAt(i).SaleOrDefaultPrice() > 1)
                    {
                        listingEntry += "s";
                    }
                    listingEntry += " (" + Math.Round(deals.ElementAt(i).GetSalePercent() * 100) + "%)";

                    if (deals.ElementAt(i).Quantity > 1)
                    {
                        listingEntry += " - " + deals.ElementAt(i).Quantity + " copies.";
                    }
                    else
                    {
                        listingEntry += " - " + deals.ElementAt(i).Quantity + " copy.";
                    }

                    if (output.Length + listingEntry.Length < 2048)
                    {
                        output.Append(listingEntry);
                    }
                    else
                    {
                        SendFriendMessage(botContext, output.ToString());
                        output = new StringBuilder(listingEntry);
                    }
                }
            }
            else
            {
                output.Append("No results found.");
            }

            if (friendMsg)
            {
                sendContext.SetAsFriendMessage();
            }

            sendContext.OutgoingMessage = output.ToString();

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!otherdeals") == 0 ||
                 chatInput.CompareTo("/otherdeals") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
