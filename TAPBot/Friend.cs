using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    // A class that can be extended to support on-going states of the current chat session
    class Friend
    {
        string friendName;
        SteamID friendId;

        public Friend(string friendName, SteamID friendId)
        {
            this.friendName = friendName;
            this.friendId = friendId;
        }

        public void SetFriendName(string friendName)
        {
            this.friendName = friendName;
        }

        public string GetFriendName()
        {
            return friendName;
        }

        public void SetSteamID(SteamID friendId)
        {
            this.friendId = friendId;
        }

        public SteamID GetSteamID()
        {
            return friendId;
        }
    }
}
