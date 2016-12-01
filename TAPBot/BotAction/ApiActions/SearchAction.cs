using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class SearchAction : BotAction
    {
        public SearchAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string searchCmd = "!search ";

            string searchText = botContext.Command.Substring(searchCmd.Length);

            List<Listing> entries = apiService.SearchListings(searchText, 6).ToList();

            bool friendMsg = entries.Count > 1;
            bool overflowed = false;

            StringBuilder output = new StringBuilder(String.Empty);
            string messageOverflow = "Showing partial results, please use more specific search terms.\n";

            if (entries.Count != 0)
            {
                // we take 6 listings to see if more than 5 exists (inexpensive to get one extra, but don't actually use it)
                for (int i = 0; i < entries.Count; i++)
                {
                    if (i > 5)
                    {
                        overflowed = true;
                        break;
                    }

                    string listingEntry = String.Empty;

                    if (entries.ElementAt(i).HasDailyDeal() || entries.ElementAt(i).HasWeeklyDeal() || entries.ElementAt(i).HasOtherDeal())
                    {
                        listingEntry = "#" + entries.ElementAt(i).ListingID + " (SALE) " + entries.ElementAt(i).ListingName + " - " + entries.ElementAt(i).SaleOrDefaultPrice() + " point";
                        if (entries.ElementAt(i).SaleOrDefaultPrice() > 1)
                        {
                            listingEntry += "s";
                        }
                        listingEntry += " (" + Math.Round(entries.ElementAt(i).GetSalePercent() * 100) + "%)";
                    }
                    else
                    {
                        listingEntry = "#" + entries.ElementAt(i).ListingID + " " + entries.ElementAt(i).ListingName + " - " + entries.ElementAt(i).ListingPrice + " point";
                        if (entries.ElementAt(i).ListingPrice > 1)
                        {
                            listingEntry += "s";
                        }
                    }
                    
                    if (entries.ElementAt(i).Quantity > 1)
                    {
                        listingEntry += " - " + entries.ElementAt(i).Quantity + " copies.";
                    }
                    else
                    {
                        listingEntry += " - " + entries.ElementAt(i).Quantity + " copy.";
                    }

                    listingEntry += "\n";

                    if (output.Length + listingEntry.Length + messageOverflow.Length < 2048)
                    {
                        output.Append(listingEntry);
                    }
                    else
                    {
                        overflowed = true;
                        break;
                    }
                }
            }
            else
            {
                output.Append("No results found.");
            }

            if (overflowed)
            {
                sendContext.OutgoingMessage = messageOverflow + output.ToString();
            }
            else
            {
                sendContext.OutgoingMessage = output.ToString();
            }

            if (friendMsg)
            {
                sendContext.SetAsFriendMessage();
            }

            if (botContext.GroupID != null && friendMsg)
            {
                SendGroupMessage(botContext, "Multiple results. Results have been sent in a private message!");
            }
            
            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!search ") ||
                 chatInput.StartsWith("/search "))
            {
                return true;
            }

            return false;
        }
    }
}
