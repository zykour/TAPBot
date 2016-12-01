using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class SetPOTWAction : BotAction
    {
        public List<String> admins;

        public SetPOTWAction(Func<ApiService> CreateApiService, List<String> adminList) : base(CreateApiService)
        {
            admins = adminList;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string user = String.Empty;

            if (botContext.Command.StartsWith("!setprincess ") || botContext.Command.StartsWith("/setprincess "))
            {
                string cmd = "!setprincess ";

                user = botContext.Command.Substring(cmd.Length);
            }
            else
            {
                string cmd = "!setpotw ";

                user = botContext.Command.Substring(cmd.Length);
            }

            if (String.IsNullOrWhiteSpace(user))
            {
                sendContext.OutgoingMessage = "Error, please use as follows: !setpotw {TAP website username|nickname|Steam 64bit ID}";
                return sendContext;
            }

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    AppUser potw = apiService.GetUser(user);
                    apiService.SetPOTW(potw);
                }
            }

            sendContext.OutgoingMessage = String.Empty;

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!setprincess ") ||
                 chatInput.StartsWith("/setprincess ") ||
                 chatInput.StartsWith("!setpotw ") ||
                 chatInput.StartsWith("/setpotw "))
            {
                return true;
            }

            return false;
        }
    }
}
