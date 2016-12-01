using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class AddBalancesAction : BotAction
    {
        public List<String> admins;

        public AddBalancesAction(Func<ApiService> CreateApiService, List<String> adminList) : base(CreateApiService)
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

            string helpText = "!addbal {points amount} {CSV of user nicknames} , i.e. to add 5 points for Nickolas and imadous: !addbal 5 IMAD,NICK";

            if (botContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string cmd = "!addbal ";

            string variables = botContext.Command.Substring(cmd.Length);

            if (variables.Contains(" ") == false)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string amountStr = variables.Substring(0, variables.IndexOf(" ")).Trim();

            string nicknamesStr = variables.Substring(variables.IndexOf(" ")).Trim();

            string[] nicknames = nicknamesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (nicknames.Count() == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            int points = 0;

            Int32.TryParse(amountStr, out points);

            if (points == 0)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            List<String> users = null;

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    users = apiService.AddBalance(points, nicknames);
                }
            }
            
            if (users == null)
            {
                sendContext.OutgoingMessage = "Failed to add balances for any users.";
                return sendContext;
            }

            StringBuilder namesString = new StringBuilder("Added " + points + " points to the following users' balance: ");

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
            if (chatInput.StartsWith("!addbal ") ||
                 chatInput.StartsWith("/addbal "))
            {
                return true;
            }

            return false;
        }
    }
}
