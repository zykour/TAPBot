using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class FriendMsgBotAction : BotAction
    {
        protected string friendId;

        public FriendMsgBotAction(string friendId)
        {
            this.friendId = friendId;
        }

        public FriendMsgBotAction(SteamID groupId, SteamID friendId)
        {
            this.friendId = friendId.ToString();
        }

        public FriendMsgBotAction()
        {
            friendId = null;
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
            return (friendId != null) ? true : false;
        }

        public override string GetFriendID()
        {
            return friendId;
        }

        public override SteamID GetFriendSteamID()
        {
            return new SteamID(UInt64.Parse(friendId));
        }
    }
}
