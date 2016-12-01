using System;
using TheAfterParty.Domain.Services;
using System.Collections.Generic;
using TheAfterParty.Domain.Entities;
using System.Linq;
using System.Text;

namespace TAPBot
{
    class AddSiteNotificationAction : BotAction
    {
        public List<String> admins;

        public AddSiteNotificationAction(Func<ApiService> CreateApiService, List<String> modList) : base(CreateApiService)
        {
            admins = modList;
        }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            foreach (String admin in admins)
            {
                if (admin.CompareTo(botContext.FriendID.ConvertToUInt64().ToString()) == 0)
                {
                    if (botContext.Command.StartsWith("!addsitenotification ") || botContext.Command.StartsWith("/addsitenotification "))
                    {
                        string command = "!addsitenotification ";

                        string text = botContext.Command.Substring(command.Length).Trim();

                        if (text.Length > 0)
                        {
                            apiService.AddSiteNotification(text);
                            sendContext.OutgoingMessage = "Site notification added successfully!";
                        }
                        else
                        {
                            sendContext.OutgoingMessage = "Add site notification usage: !addsitenotification {message}";
                        }
                    }
                    else if (botContext.Command.StartsWith("!asn ") || botContext.Command.StartsWith("/asn "))
                    {
                        string command = "!asn ";


                        string text = botContext.Command.Substring(command.Length).Trim();

                        if (text.Length > 0)
                        {
                            apiService.AddSiteNotification(text);
                            sendContext.OutgoingMessage = "Site notification added successfully!";
                        }
                        else
                        {
                            sendContext.OutgoingMessage = "Add site notification usage: !asn {message}";
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(sendContext.OutgoingMessage) == true)
            {
                sendContext.OutgoingMessage = "Insufficient privileges, PM Monukai to be added to the mod list for this command.";
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!addsitenotification ") ||
                 chatInput.StartsWith("/addsitenotification ") ||
                 chatInput.StartsWith("!asn ") ||
                 chatInput.StartsWith("/asn "))
            {
                return true;
            }

            return false;
        }
    }
}
