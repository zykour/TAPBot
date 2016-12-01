using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class PrincessAction : BotAction
    {
        public PrincessAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            AppUser user = apiService.GetPOTW();

            if (user != null)
            {
                sendContext.OutgoingMessage = user.UserName + " is the pretty princess of the week!";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!princess") == 0 ||
                 chatInput.CompareTo("/princess") == 0 ||
                 chatInput.CompareTo("!potw") == 0 ||
                 chatInput.CompareTo("/potw") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
