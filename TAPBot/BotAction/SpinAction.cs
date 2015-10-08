using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class SpinAction : BotAction
    {
        protected DealPicker dealPicker;

        protected SpinAction() { }
        public SpinAction(DealPicker dealPicker)
        {
            this.dealPicker = dealPicker;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.FriendID.ConvertToUInt64().ToString().CompareTo("76561198030277114") != 0)
            {
                return "";
            }

            int numDeals = 1;

            try
            {
                numDeals = Convert.ToInt32(botContext.Command.Trim().Substring(6));
            }
            catch (Exception e)
            {
                if (e is OverflowException || e is FormatException)
                {
                    numDeals = 1; // no-op, essentially. just making it explicit
                }
            }

            int max = (dealPicker.Count() > numDeals) ? numDeals : dealPicker.Count();
            LinkedList<DealEntry> spunDeals = new LinkedList<DealEntry>();

            int iterator = numDeals;

            while (iterator > 0)
            {
                DealEntry tempDeal = dealPicker.PickDeal();
                bool dealHasBeenAdded = false;

                for (int i = 0; i < spunDeals.Count; i++)
                {
                    if (spunDeals.ElementAt(i).Name.CompareTo(tempDeal.Name) == 0)
                    {
                        dealHasBeenAdded = true;
                        break;
                    }

                    if (spunDeals.ElementAt(i).Name.CompareTo(tempDeal.Name) > 0)
                    {
                        spunDeals.AddBefore(spunDeals.Find(spunDeals.ElementAt(i)), tempDeal);
                        dealHasBeenAdded = true;
                        iterator--;
                        break;
                    }
                }

                if (!dealHasBeenAdded)
                {
                    spunDeals.AddLast(tempDeal);
                    iterator--;
                }
            }

            StringBuilder outputBuffer = new StringBuilder();

            foreach (DealEntry deal in spunDeals)
            {
                string tempStr = deal.Quantity + "\t" + deal.Price + " (" + deal.DiscountAmount + "%)" + ((deal.Price < 10) ? "\t\t" : "\t") + deal.Name + "\n";

                if (tempStr.Length + outputBuffer.Length > 2048)
                {
                    SendMessage(botContext, outputBuffer.ToString());

                    // don't want to attempt to send too many messages in a row
                    // this will cause the entire bot to sleep for up to a few seconds but as only one privleged person can invoke this command, that is ok
                    
                    System.Threading.Thread.Sleep(1000);
                    outputBuffer.Clear();
                }

                outputBuffer.Append(tempStr);
            }

            return outputBuffer.ToString();
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.Trim().StartsWith("!spin"))
            {
                return true;
            }

            return false;
        }
    }
}
