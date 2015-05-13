using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class BotAction
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
        // Any command that should always send a response to a user, call SendFriendMessage directly

        protected void SendMessage(string message)
        {
            if (botContext.GroupID != null)
            {
                SendGroupMessage(message);
            }
            else if(botContext.FriendID != null)
            {
                SendFriendMessage(message);
            }
        }
        
        // SendGroupMessage and SendFriendMessage are called by SendMessage but can be called directly for custom behavior

        protected void SendGroupMessage(string message)
        {
            botContext.SteamFriend.SendChatRoomMessage(botContext.GroupID, EChatEntryType.ChatMsg, message);
        }

        protected void SendFriendMessage(string message)
        {
            botContext.SteamFriend.SendChatMessage(botContext.FriendID, EChatEntryType.ChatMsg, message);
        }

        // Encapsulates the logic for deciding whether the line of chat text was a valid invocation for this action

        public virtual bool ValidCommand(string command)
        {
            return false;
        }

        public virtual void Execute();
    }
}
