using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{

    // A wrapper class for holding a minimal set of commonly used data for each bot action

    class BotContext
    {

        // Holds the SteamID of the Group Chat where the command was invoked

        private SteamID groupId;
        public SteamID GroupID
        {
            get { return groupId; }
            set { groupId = value; }
        }

        // Holds the SteamID of the Steam User who invoked the command

        private SteamID friendId;
        public SteamID FriendID
        {
            get { return friendId; }
            set { friendId = value; }
        }

        // Holds the actual command "as is" when it was invoked

        private string command;
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        // Holds the SteamFriend object for sending messages and joining chats

        private SteamFriends steamFriend;
        public SteamFriends SteamFriend
        {
            get { return steamFriend; }
        }

        public BotContext() : this(null, null, null, null) { }
        public BotContext(string command) : this(null, null, command, null) { }
        public BotContext(string command, SteamFriends steamFriend) : this(null, null, command, steamFriend) { }
        public BotContext(SteamID groupId, SteamID friendId) : this(groupId, friendId, null, null) { }
        public BotContext(SteamID groupId, SteamID friendId, SteamFriends steamFriend) : this(groupId, friendId, null, steamFriend) { }
        public BotContext(SteamID groupId, SteamID friendId, string command, SteamFriends steamFriend)
        {
            this.groupId = groupId;
            this.friendId = friendId;
            this.command = command;
            this.steamFriend = steamFriend;
        }
    }
}
