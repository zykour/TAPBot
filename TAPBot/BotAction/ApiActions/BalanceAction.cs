using System;
using TheAfterParty.Domain.Services;
using TheAfterParty.Domain.Entities;

namespace TAPBot
{
    class BalanceAction : BotAction
    {
        public BalanceAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            AppUser user = apiService.GetUserBySteamID(Convert.ToInt64(botContext.FriendID.ConvertToUInt64()));

            if (user != null)
            {
                sendContext.OutgoingMessage = user.UserName + ", your balance is " + user.Balance.ToString();
            }
                 
            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!balance") == 0 ||
                 chatInput.CompareTo("/balance") == 0)
            {
                return true;
            }

            return false;
        }
    }
}
