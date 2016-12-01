using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class QueenAction : BotAction
    {
        public QueenAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            AppUser user = apiService.GetUserWithHighestBalance();

            if (user != null)
            {
                sendContext.OutgoingMessage = "Queen " + user.UserName + " is reigning with " + user.Balance + " points!";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!queen") == 0 ||
                 chatInput.CompareTo("/queen") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
