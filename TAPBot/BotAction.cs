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
        protected string results;
        protected bool success;
        protected bool messageAvailable;

        public BotAction()
        {
            results = "";
            success = false;
            messageAvailable = false;
        }

        public abstract void SetFriendID(SteamID friendId);
        public abstract void SetFriendID(string friendId);
        public abstract bool HasFriendID();
        public abstract string GetFriendID();
        public abstract SteamID GetFriendSteamID();

        public abstract void SetGroupChatSteamID(SteamID groupId);
        public abstract void SetGroupChatSteamID(string groupId);
        public abstract bool HasGroupChatID();
        public abstract string GetGroupChatID();
        public abstract SteamID GetGroupChatSteamID();

        public abstract string GetMessage();
        public abstract bool IsSuccessful();
        public abstract bool HasMessage();

        public abstract void Execute();
    }
}
