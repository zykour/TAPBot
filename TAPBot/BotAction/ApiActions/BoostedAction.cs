using System;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class BoostedAction : BotAction
    {
        public BoostedAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            Objective boostedObjective = apiService.GetObjectiveWithBoostedDaily();
            
            if (boostedObjective == null)
            {
                return sendContext;
            }

            string reqAdmin = String.Empty;

            if (boostedObjective.RequiresAdmin == true)
            {
                reqAdmin = " (Monuplay)";
            }

            TimeSpan timeLeft = boostedObjective.BoostedObjective.EndDate - DateTime.Now;
            String timeStr = String.Format("{0}h {1}m", (int)Math.Floor(timeLeft.TotalHours), timeLeft.Minutes);

            sendContext.OutgoingMessage = "#" + boostedObjective.ObjectiveID + " (" + boostedObjective.FixedReward() + " points)* " + boostedObjective.Title + " - \"" + boostedObjective.ObjectiveName + "\" - " + boostedObjective.Description + " (" + boostedObjective.BoostedObjective.BoostAmount + "x boost)" + reqAdmin + " (" + timeStr + ")";

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!boosted") == 0 ||
                 chatInput.CompareTo("/boosted") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
