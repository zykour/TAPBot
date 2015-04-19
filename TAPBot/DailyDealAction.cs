using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class DailyDealAction {
        
        protected bool hasRanToday;
        protected DateTime currentDate; // represents the date when this was last called
        protected int reroll;
        protected string results;

        public DailyDealAction() 
        {
            hasRanToday = false;
            currentDate = DateTime.Today;
            reroll = 1;
            results = "";
        }

        
        public void Execute()
        {
            if (DateTime.Compare(DateTime.Today, currentDate) != 0)
            {
                hasRanToday = false;
                reroll = 1;
            }

            if (!hasRanToday)
            {
                int day = Convert.ToInt32((DateTime.Today - new DateTime(2010, 1, 1)).TotalDays);
                //Random randomGen = new Random((day + numLines * Convert.ToInt32(DateTime.Today.Day)) * reroll);
                DealPicker dp = new DealPicker(day * reroll);

                currentDate = DateTime.Today;
                hasRanToday = true;
                dp.PickDeal();

                string fixedQuantity = (dp.GetGameQuantity() == 1) ? " copy remains." : " copies remain.";
                string fixedPrice = (dp.GetSalePrice() == 1) ? " point (" : " points (";

                //results = "The Co-op Shop Special of the Day is \"" + gameName + ".\" The discounted price is " + gamePrice + fixedPrice + discountAmnt + "%), currently " + gameQuantity + fixedQuantity;
                results = "Daily deal: '" + dp.GetGameName() + "' for " + dp.GetSalePrice() + fixedPrice + dp.GetDiscountAmount() + "%), currently " + dp.GetGameQuantity() + fixedQuantity;

                if (dp.GetAppID().CompareTo("") != 0)
                {
                    results = results + " " + dp.GetAppID();
                }
            }
        }

        public String GetMessage()
        {
            return results;
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
