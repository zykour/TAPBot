using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class TransferAction : BotAction
    {
        public TransferAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string helpText = "!transfer {points} {website username|nickname|steam 64-bit ID} , i.e. to transfer 10 points to imadous: !transfer 10 IMAD";

            if (sendContext.Command.Contains("-h"))
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string transferText = "!transfer ";

            string pointsText = botContext.Command.Substring(transferText.Length);

            //Contains(" "): we require two arguments separated by spaces, thus if there are no spaces remaining, this is a bad command
            if (pointsText.Length < 1 || pointsText.Contains(" ") == false)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            string userText = pointsText.Substring(pointsText.IndexOf(' ') + 1);
            pointsText = pointsText.Substring(0, pointsText.IndexOf(' '));

            int points = 0;
            Int32.TryParse(pointsText.Trim(), out points);

            AppUser recipient = apiService.GetUser(userText);
            AppUser sender = apiService.GetUserBySteamID(Convert.ToInt64(botContext.FriendID.ConvertToUInt64()));

            if (points <= 0 || recipient == null || sender == null)
            {
                sendContext.OutgoingMessage = helpText;
                return sendContext;
            }

            bool success = apiService.TransferPoints(sender, recipient, points);

            if (success)
            {
                sendContext.OutgoingMessage = "Transfer of " + points + " points by " + sender.UserName + " (" + sender.Balance + ") to " + recipient.UserName + " (" + recipient.Balance + ") successful!";
            }
            else
            {
                sendContext.OutgoingMessage = "Transfer failed, make sure you have sufficient funds for this transfer.";
            }
            
            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!transfer ") ||
                 chatInput.StartsWith("/transfer "))
            {
                return true;
            }

            return false;
        }
    }
}
