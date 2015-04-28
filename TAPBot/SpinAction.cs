using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class SpinAction : ChatBotAction
    {
        protected int numDeals;
        protected DealPicker dp;

        public SpinAction(string friendId, string chatId, int numDeals) : base(friendId, null)
        {
            this.numDeals = numDeals;
            dp = new DealPicker();
        }

        public override void Execute()
        {
            for (int i = 1; i <= numDeals; i++)
            {
                dp.PickDeal();
                results = results + dp.GetGameQuantity() + "\t" + dp.GetSalePrice() + " (" + dp.GetDiscountAmount() +"%)\t" + dp.GetGameName() + ((dp.GetAppID() == "") ? "" : "\t" + dp.GetAppID()) + "\n";
            }

            messageAvailable = true;
            success = true;
        }
    }
}
