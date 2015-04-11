using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class ChatBotAction : BotAction
    {
        protected string groupId;
        protected string friendId;

        public ChatBotAction(string friendId, string groupId) : base()
        {
            this.groupId = groupId;
            this.friendId = friendId;
        }

        public ChatBotAction(SteamID friendId, SteamID groupId) : base()
        {
            this.groupId = groupId.ToString();
            this.friendId = friendId.ToString();
        }

        public ChatBotAction(string friendId, string groupId, string msg)
        {
            this.groupId = groupId;
            this.friendId = friendId;
            results = msg;
            success = true;
            messageAvailable = true;
        }

        public override void SetFriendID(SteamID friendId)
        {
            this.friendId = friendId.ToString();
        }

        public override void SetFriendID(string friendId)
        {
            this.friendId = friendId;
        }

        public override bool HasFriendID()
        {
            return ( friendId != null ) ? true : false;
        }

        public override string GetFriendID()
        {
            return friendId;
        }

        public override SteamID GetFriendSteamID()
        {
            return new SteamID(UInt64.Parse(friendId));
        }

        public override void SetGroupChatSteamID(SteamID groupId)
        {
            this.groupId = groupId.ToString();
        }

        public override void SetGroupChatSteamID(string groupId)
        {
            this.groupId = groupId;
        }

        public override bool HasGroupChatID()
        {
            return (groupId != null) ? true : false;
        }

        public override string GetGroupChatID()
        {
            return groupId;
        }
        
        public override SteamID GetGroupChatSteamID()
        {
            return new SteamID(UInt64.Parse(groupId));
        }

        public override string GetMessage()
        {
            return results;
        }

        public override bool IsSuccessful()
        {
            return success;
        }

        public override bool HasMessage()
        {
            return messageAvailable;
        }

        public override void Execute()
        {
        }
    }
}
