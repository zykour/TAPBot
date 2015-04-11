using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class CommandFactory
    {

        protected DailyDealAction dailyDeal;
        protected MilkshakeAction milkshakes;

        public CommandFactory()
        {
            dailyDeal = null;
        }

        public BotAction CreateBotAction(string command, string userId, string chatId)
        {
            if (command.Trim().CompareTo("!balance") == 0)
                return new BalanceBotAction(userId, chatId);
            if (command.Trim().CompareTo("!queen") == 0)
                return new QueenBotAction(userId, chatId);
            if (command.Trim().CompareTo("!roll") == 0 || command.Trim().CompareTo("/roll") == 0 || command.StartsWith("!roll ") || command.StartsWith("/roll "))
                return new RollAction(userId, chatId, command);
            if (command.Trim().CompareTo("!deal") == 0)
            {
                if (dailyDeal == null)
                {
                    dailyDeal = new DailyDealAction(null, chatId);
                    return dailyDeal;
                }
                else
                {
                    return dailyDeal;
                }
            }
            if (command.Trim().CompareTo("!milkshake") == 0)
            {
                if (milkshakes == null)
                {
                    milkshakes = new MilkshakeAction(null, null);
                }

                return new FriendChatWrapper(userId, chatId, milkshakes);
            }
            if (command.Trim().CompareTo("!reroll") == 0)
            {
                if (dailyDeal != null)
                {
                    BotAction temp;
                    if (userId.CompareTo("76561198030277114") == 0)
                    {
                        dailyDeal.Reroll();
                        temp = new ChatMsgBotAction(userId, chatId, "Reroll successful, master!");
                    }
                    else
                    {
                        temp = new ChatMsgBotAction(userId, chatId, "Reroll unsuccessful, not-Monukai");
                    }

                    return temp;
                }
            }
            if (command.Trim().CompareTo("!reset") == 0)
            {
                if (dailyDeal != null)
                {
                    BotAction temp;
                    if (userId.CompareTo("76561198030277114") == 0)
                    {
                        dailyDeal.InventoryChange();
                        temp = new ChatMsgBotAction(userId, chatId, "Reroll successful, master!");
                    }
                    else
                    {
                        temp = new ChatMsgBotAction(userId, chatId, "Reroll unsuccessful, not-Monukai");
                    }

                    return temp;
                }
            }

            return null;
        }

        public BotAction CreateBotAction(string command, string userId)
        {

            if (command.Trim().CompareTo("!balance") == 0)
                return new BalanceBotAction(userId, null);

            if (command.Trim().CompareTo("!deal") == 0)
            {
                if (dailyDeal == null)
                {
                    return new FriendChatWrapper(userId, null, new DailyDealAction(null, null));
                }

                return new FriendChatWrapper(userId, null, dailyDeal);
            }

            if (command.Trim().CompareTo("!milkshake") == 0)
            {
                if (milkshakes == null)
                {
                   milkshakes = new MilkshakeAction(null, null);
                }

                return new FriendChatWrapper(userId, null, milkshakes);
            }


            // friend msg actions here, if any, else see if there is a chat msg actions
            return CreateBotAction(command, userId, null);
        }
    }
}
