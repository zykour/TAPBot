using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class DailyDealAction : BotAction {
        
        protected bool hasRanToday;
        protected DateTime currentDate; // represents the date when this was last called
        protected int reroll;
        string dealMessage;

        protected DealPicker dealPicker;

        // Holds the today's deal

        protected DealEntry dealEntry;
        public DealEntry Deal
        {
            get 
            {
                if (dealEntry == null || hasRanToday == false)
                {
                    ProduceChatMessage(null);
                }
                else if (DateTime.Compare(dealEntry.Expiration, DateTime.Today) != 0)
                {
                    ProduceChatMessage(null);
                }

                return dealEntry; 
            }
        }

        protected DailyDealAction() { }
        public DailyDealAction(DealPicker dealPicker) 
        {
            hasRanToday = false;
            currentDate = DateTime.Today;
            reroll = 1;
            dealMessage = "";
            this.dealPicker = dealPicker;
        }


        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (DateTime.Compare(DateTime.Today, currentDate) != 0)
            {
                hasRanToday = false;
                reroll = 1;
            }

            if (!hasRanToday)
            {
                int day = Convert.ToInt32((DateTime.Today - new DateTime(2010, 1, 1)).TotalDays);

                currentDate = DateTime.Today;
                hasRanToday = true;
                dealEntry = dealPicker.PickDeal(new Random(day), reroll);

                string fixedQuantity = (dealEntry.Quantity == 1) ? " copy remains." : " copies remain.";
                string fixedPrice = (dealEntry.Price == 1) ? " point (" : " points (";

                //results = "The Co-op Shop Special of the Day is \"" + gameName + ".\" The discounted price is " + gamePrice + fixedPrice + discountAmnt + "%), currently " + gameQuantity + fixedQuantity;
                dealMessage = "Daily deal: '" + dealEntry.Name + "' for " + dealEntry.Price + fixedPrice + dealEntry.DiscountAmount + "%), currently " + dealEntry.Quantity + fixedQuantity;

                if (dealEntry.AppID.CompareTo("") != 0)
                {
                    dealMessage = dealMessage + " " + dealEntry.AppID;
                }
            }

            return dealMessage;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("/deal") == 0 ||
                chatInput.CompareTo("!deal") == 0)
            {
                return true;
            }

            return false;
        }
        
        // Used to change the daily deal. Daily deal is selected by the day's current date as a seed for the random number generator multiplied by the reroll factor (1 by default)
        // thus by adding 1 to the reroll factor each time this is called, we'll get a new deal
        public void Reroll()
        {
            reroll++;
            hasRanToday = false;
        }

        // if we want to reset the daily deal
        public void Reset()
        {
            reroll = 1;
            hasRanToday = false;
        }

    }
}
