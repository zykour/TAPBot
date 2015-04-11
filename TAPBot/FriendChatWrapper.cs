using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class FriendChatWrapper : ChatMsgBotAction
    {
        ChatMsgBotAction action;

        public FriendChatWrapper(string friendId, string groupId, ChatMsgBotAction action)
        {
            this.friendId = friendId;
            this.groupId = groupId;
            this.action = action;
        }

        public override void Execute()
        {
            action.Execute();
            results = action.ToString();
            messageAvailable = true;
            success = true;
        }
    }
}
