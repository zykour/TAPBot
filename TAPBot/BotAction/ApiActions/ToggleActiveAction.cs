using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class ToggleActiveAction : BotAction
    {
        public List<String> admins;

        public ToggleActiveAction(Func<ApiService> CreateApiService, List<String> adminList) : base(CreateApiService)
        {
            admins = adminList;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            if (admins.Contains(botContext.FriendID.ConvertToUInt64().ToString()) == false)
            {
                sendContext.OutgoingMessage = "You have insufficient privileges to use this command.";
                return sendContext;
            }

            string helpText = "!toggleactive {objective id}";

            if (botContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string cmd = "!toggleactive ";

            string objIdStr = botContext.Command.Substring(cmd.Length);

            int objId = 0;

            Int32.TryParse(objIdStr, out objId);

            if (objId == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }
            
            Objective objective = apiService.GetObjectiveByID(objId);

            if (objective == null)
            {
                sendContext.OutgoingMessage = "Invalid objective ID detected";
                return sendContext;
            }

            bool isActive = objective.IsActive;

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    isActive = apiService.ToggleActiveObjective(objective);
                }
            }

            sendContext.OutgoingMessage = "Success! Objective \"" + objective.ObjectiveName + "\" is now ";

            if (isActive == true)
            {
                sendContext.OutgoingMessage += "active!";
            }
            else
            {
                sendContext.OutgoingMessage += "inactive!";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!toggleactive ") ||
                 chatInput.StartsWith("/toggleactive "))
            {
                return true;
            }

            return false;
        }
    }
}
