using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{

    // A factory class to return the appropriate BotAction object based on the parsed chat command, if any

    class CommandFactory
    {

        // These represent objects which require a persistent state across calls, thus they must be saved
        // ToDo: Create these as singletons as only one copy need exist, make them their own class, and create a base ChatMsgBotAction to handle them

        protected DailyDealAction dailyDeal;
        protected MilkshakeAction milkshakes;

        public CommandFactory()
        {
            dailyDeal = new DailyDealAction(null, null);
            milkshakes = new MilkshakeAction(null, null);
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
                dailyDeal.Execute();
                return new ChatBotAction(userId, chatId, dailyDeal.GetMessage());
            }
            if (command.Trim().CompareTo("!milkshake") == 0)
            {
                milkshakes.Execute();
                return new ChatBotAction(userId, chatId, milkshakes.GetMessage());
            }
            if (command.Trim().CompareTo("!reroll") == 0)
            {
                if (dailyDeal != null)
                {
                    BotAction temp;
                    if (userId.CompareTo("76561198030277114") == 0)
                    {
                        dailyDeal.Reroll();
                        temp = new ChatBotAction(userId, chatId, "Reroll successful, master!");
                    }
                    else
                    {
                        temp = new ChatBotAction(userId, chatId, "Reroll unsuccessful, not-Monukai");
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
                        temp = new ChatBotAction(userId, chatId, "Reset successful, master!");
                    }
                    else
                    {
                        temp = new ChatBotAction(userId, chatId, "Reset unsuccessful, not-Monukai");
                    }

                    return temp;
                }
            }

            return null;
        }
    }
}
