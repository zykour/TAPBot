using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class RerollAction : BotAction
    {
        public List<String> admins;
        
        public RerollAction(Func<ApiService> CreateApiService, List<String> adminList) : base(CreateApiService)
        {
            admins = adminList;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    apiService.RolloverDailyDeal();
                }
            }
                        
            sendContext.OutgoingMessage = String.Empty;

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!reroll") == 0 ||
                 chatInput.CompareTo("/reroll") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
