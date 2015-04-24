using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TAPBot
{

    // A factory class to return the appropriate BotAction object based on the parsed chat command, if any

    class CommandFactory
    {

        // These represent objects which require a persistent state across calls, thus they must be saved
        // ToDo: Create these as singletons as only one copy need exist, make them their own class, and create a base ChatMsgBotAction to handle them

        protected DailyDealAction dailyDeal;
        protected MilkshakeAction milkshakes;
        protected ParseAppIDAction appParser;

        public CommandFactory()
        {
            appParser = new ParseAppIDAction();
            dailyDeal = new DailyDealAction();
            milkshakes = new MilkshakeAction();
        }

        public BotAction CreateBotAction(string command, string userId, string chatId)
        {
            //------------------------------------------------------------------------

            // finds the balance (co-op shop points) of the invoker
            // Specific to The After Party steam group

            if (command.Trim().CompareTo("!balance") == 0)
                return new BalanceBotAction(userId, chatId);

            //------------------------------------------------------------------------

            // determines which member has the most co-op shop points.
            // Specific to The After Party steam group

            if (command.Trim().CompareTo("!queen") == 0)
                return new QueenBotAction(userId, chatId);

            //------------------------------------------------------------------------

            // rolls a number between the specified values, or if no values are specified it rolls a random number between 1-100

            if (command.Trim().CompareTo("!roll") == 0 || command.Trim().CompareTo("/roll") == 0 || command.StartsWith("!roll ") || command.StartsWith("/roll "))
                return new RollAction(userId, chatId, command);

            //------------------------------------------------------------------------

            // creates a daily deal for this day
            // Specific to The After Party steam group

            if (command.Trim().CompareTo("!deal") == 0)
            {
                dailyDeal.Execute();
                return new ChatBotAction(userId, chatId, dailyDeal.GetMessage());
            }

            //------------------------------------------------------------------------

            // a trivial action to count how many times !milkshakes has been invoked
            // no real purpose

            if (command.Trim().CompareTo("!milkshake") == 0)
            {
                milkshakes.Execute();
                return new ChatBotAction(userId, chatId, milkshakes.GetMessage());
            }

            //------------------------------------------------------------------------

            // if the current deal is bought, there is need to reroll the deal to get a new random deal
            // otherwise it uses the same seed (a value procurred via today's date times a reroll factor)
            // and will just pick the next item on the list after the purchased item is removed
            // Specific to The After Party steam group

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

            //------------------------------------------------------------------------

            // resets the reroll counter in the dailyDealAction 
            // only successfully executes if Monukai invokes it personally
            // Specific to The After Party steam group

            if (command.Trim().CompareTo("!reset") == 0)
            {
                if (dailyDeal != null)
                {
                    BotAction temp;
                    if (userId.CompareTo("76561198030277114") == 0)
                    {
                        dailyDeal.Reset();
                        temp = new ChatBotAction(userId, chatId, "Reset successful, master!");
                    }
                    else
                    {
                        temp = new ChatBotAction(userId, chatId, "Reset unsuccessful, not-Monukai");
                    }

                    return temp;
                }
            }

            //------------------------------------------------------------------------

            // The spin action "spins" a number of deals equal to the number specified in the command
            // -- uses current datetime to seed randomness
            // Specific to The After Party steam group

            if (command.Trim().StartsWith("!spin ") && command.Trim().Length > 6)
            {
                int numDeals = 1;

                try
                {
                    numDeals = Convert.ToInt32(command.Trim().Substring(6));
                }
                catch ( Exception e )
                {
                    if ( e is OverflowException || e is FormatException )
                    {
                        return new SpinAction(userId, chatId, 1);
                    }
                }
                
                return new SpinAction(userId, chatId, numDeals);
            }

            //------------------------------------------------------------------------

            // parses steamApp links to see if the specified game/app is in the co-op shop
            // Specific to The After Party steam group

            Regex steamAppRegex = new Regex(@"(.*)?(store\.steampowered\.com/app/)([0-9]+)(.*)?");
            Match steamAppMatch = steamAppRegex.Match(command);

            if (steamAppMatch.Success)
            {
                StoreEntry appFound = appParser.Execute(steamAppMatch.Groups[3].ToString());
                
                if (appFound != null) 
                {
                    string tempStr = "Co-op shop game/app: " + appFound.Name + ". Price: " + appFound.Price + ". Quantity: " + appFound.Quantity;
                    return new ChatBotAction(userId, chatId, tempStr);
                }
            }

            return null;
        }
       
    }
}
