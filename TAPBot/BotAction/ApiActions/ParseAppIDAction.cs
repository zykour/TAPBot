using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class ParseAppIDAction : BotAction
    {
        Regex steamAppRegex;
        List<String> appIdsParsed;
        
        public ParseAppIDAction(Func<ApiService> CreateApiService) : base(CreateApiService)
        {
            steamAppRegex = new Regex(@"(.*)?(store\.steampowered\.com/app/)([0-9]+)(.*)?", RegexOptions.IgnoreCase);
            appIdsParsed = new List<String>();
        }
        
        private void ParseChat(string remainingMessage)
        {
            Match steamAppMatch = steamAppRegex.Match(remainingMessage);

            if (steamAppMatch.Success)
            {
                appIdsParsed.Add(steamAppMatch.Groups[3].ToString());
                
                ParseChat(steamAppMatch.Groups[4].ToString());
            }
        }
        
        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            appIdsParsed = new List<String>();

            ParseChat(botContext.Command);

            BotContext sendContext = botContext.Clone();

            foreach (String appId in appIdsParsed)
            {
                int id = 0;

                Int32.TryParse(appId, out id);

                if (id != 0)
                {
                    Listing listing = apiService.GetListingByAppID(id);

                    if (listing != null && listing.Quantity > 0 && listing.ListingPrice > 0)
                    {

                        string listingEntry = String.Empty;

                        if (listing.HasDailyDeal() || listing.HasWeeklyDeal() || listing.HasOtherDeal())
                        {
                            listingEntry = "#" + listing.ListingID + " (SALE) " + listing.ListingName + " - " + listing.SaleOrDefaultPrice() + " point";
                            if (listing.SaleOrDefaultPrice() > 1)
                            {
                                listingEntry += "s";
                            }
                            listingEntry += " (" + Math.Round(listing.GetSalePercent() * 100) + "%)";
                        }
                        else
                        {
                            listingEntry = "#" + listing.ListingID + " " + listing.ListingName + " - " + listing.SaleOrDefaultPrice() + " point";
                            if (listing.SaleOrDefaultPrice() > 1)
                            {
                                listingEntry += "s";
                            }
                        }

                        if (listing.Quantity > 1)
                        {
                            listingEntry += " - " + listing.Quantity + " copies.";
                        }
                        else
                        {
                            listingEntry += " - " + listing.Quantity + " copy.";
                        }

                        if (listing.Quantity > 0)
                        {
                            SendMessage(botContext, listingEntry);
                        }
                    }
                }
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            Match steamAppMatch = steamAppRegex.Match(chatInput);

            return steamAppMatch.Success;
        }
    }
}
