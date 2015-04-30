using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    abstract class BotAction
    {

        // Holds the context for this action to execute

        protected BotContext botContext;

        public BotAction()
        {
            botContext = null;
        }

        public BotAction(BotContext botContext)
        {
            this.botContext = botContext;
        }

        // A helper method for all bot actions that sends a message in response to the command
        // Any command that should send always send a response to a user, call SendFriendMessage directly

        public abstract void SendMessage();
        
        // SendGroupMessage and SendFriendMessage are called by SendMessage but can be called directly for different behavior

        public abstract void SendGroupMessage();
        public abstract void SendFriendMessage();



        public abstract bool ValidCommand();
        public abstract void Execute();
    }
}
