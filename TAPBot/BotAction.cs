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
        protected string results;
        protected bool success;
        protected bool messageAvailable;

        public BotAction()
        {
            results = "";
            success = false;
            messageAvailable = false;
        }

        public virtual void SetFriendID(SteamID friendId)
        {
        }

        public virtual void SetFriendID(string friendId)
        {
        }

        public virtual bool HasFriendID()
        {
            return false;
        }

        public virtual string GetFriendID()
        {
            return null;
        }

        public virtual SteamID GetFriendSteamID()
        {
            return null;
        }

        public virtual void SetGroupChatSteamID(SteamID groupId)
        {
        }

        public virtual void SetGroupChatSteamID(string groupId)
        {
        }

        public virtual bool HasGroupChatID()
        {
            return false;
        }

        public virtual string GetGroupChatID()
        {
            return null;
        }

        public virtual SteamID GetGroupChatSteamID()
        {
            return null;
        }

        public override string ToString()
        {
            return results;
        }

        public bool IsSuccessful()
        {
            return success;
        }

        public bool HasMessage()
        {
            return messageAvailable;
        }

        public virtual void Execute()
        {
            //Console.WriteLine("Hello, from BotAction Execute!");
        }
    }
}
