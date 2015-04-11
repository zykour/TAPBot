using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    // A class that can be extended to support on-going states of the current chat session
    class GroupChat
    {
        protected string chatName;
        protected SteamID chatId;
    
        public GroupChat(string chatName, SteamID chatId)
        {
            this.chatName = chatName;
            this.chatId = chatId;
        }

        public void SetChatName(string chatName)
        {
            this.chatName = chatName;
        }

        public string GetchatName()
        {
            return chatName;
        }

        public void SetSteamID(SteamID chatId)
        {
            this.chatId = chatId;
        }

        public SteamID GetSteamID()
        {
            return chatId;
        }
    }
}
