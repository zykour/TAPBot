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
        // A helper method for all bot actions that sends a message in response to the command
        // Any command that should send always send a response to a user, call SendFriendMessage directly

        protected virtual void SendMessage(BotContext botContext, string chatMessage)
        {
            if (botContext.GroupID != null)
            {
                SendGroupMessage(botContext, chatMessage);
            }
            else if (botContext.FriendID != null)
            {
                SendFriendMessage(botContext, chatMessage);
            }
        }
        
        // SendGroupMessage and SendFriendMessage are called by SendMessage but can be called directly for different behavior   

        protected void SendGroupMessage(BotContext botContext, string chatMessage)
        {
            botContext.SteamFriend.SendChatRoomMessage(botContext.GroupID, EChatEntryType.ChatMsg, chatMessage);
        }

        protected void SendFriendMessage(BotContext botContext, string chatMessage)
        {
            botContext.SteamFriend.SendChatMessage(botContext.FriendID, EChatEntryType.ChatMsg, chatMessage);
        }
        
        // A function to determine if this the line of chat text is a valid command for this action

        public abstract bool IsValidCommand(string chatInput);

        // This is where the logic is done for every action and the output is generated (and returned)

        protected abstract string ProduceChatMessage(BotContext botContext);

        public void Execute(BotContext botContext)
        {
            SendMessage(botContext, ProduceChatMessage(botContext));
        }
    }
}
