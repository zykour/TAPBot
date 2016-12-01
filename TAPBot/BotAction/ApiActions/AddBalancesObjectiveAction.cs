using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class AddBalancesObjectiveAction : BotAction
    {
        public List<String> admins;

        public AddBalancesObjectiveAction(Func<ApiService> CreateApiService, List<String> adminList) : base(CreateApiService)
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

            string helpText = "!abo {objective id} {CSV of user nicknames} , i.e. to award Nickolas and imadous for objective #4: !abo 4 IMAD,NICK";

            if (botContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string cmd = "!abo ";

            string variables = botContext.Command.Substring(cmd.Length);

            if (variables.Contains(" ") == false)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string objIdStr = variables.Substring(0, variables.IndexOf(" ")).Trim();

            string nicknamesStr = variables.Substring(variables.IndexOf(" ")).Trim();

            string[] nicknames = nicknamesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (nicknames.Count() == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            int objId = 0;

            Int32.TryParse(objIdStr, out objId);

            if (objId == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            List<String> users = null;

            Objective objective = apiService.GetObjectiveByID(objId);

            if (objective == null)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    users = apiService.AddBalanceForObjective(objective, nicknames);
                }
            }

            if (users == null)
            {
                sendContext.OutgoingMessage = "Failed to add balances for any users.";
                return sendContext;
            }

            StringBuilder namesString = new StringBuilder(objective.ObjectiveName + " completed! " + objective.FixedReward() + " points awarded to the following users' balance: ");

            for (int i = 0; i < users.Count - 1; i++)
            {
                namesString.Append(users.ElementAt(i) + ", ");
            }

            namesString.Append(users.ElementAt(users.Count - 1));

            sendContext.OutgoingMessage = namesString.ToString();

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!abo") ||
                 chatInput.StartsWith("/abo"))
            {
                return true;
            }

            return false;
        }
    }
}
