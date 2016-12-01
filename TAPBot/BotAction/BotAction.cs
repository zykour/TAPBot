using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;
using TAPBot.Context_and_Utility;
using TheAfterParty.Domain.Services;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace TAPBot
{
    public class BotAction
    {
        // A helper method for all bot actions that sends a message in response to the command
        // Any command that should send always send a response to a user, call SendFriendMessage directly

        public ApiAuthorizer ApiAuthorizer { get; set; }
        protected Func<ApiService> CreateApiService;
        protected ApiService apiService;

        public BotAction()
        {
            CreateApiService = null;
        }
        public BotAction(Func<ApiService> CreateApiService)
        {
            this.CreateApiService = CreateApiService;
        }

        protected IApiService GetApiService()
        {
            return apiService;
        }

        protected virtual void SendMessage(BotContext botContext)
        {
            if (String.IsNullOrEmpty(botContext.OutgoingMessage) == false)
            {
                if (botContext.GroupID != null)
                {
                    SendGroupMessage(botContext, botContext.OutgoingMessage);
                }
                else if (botContext.FriendID != null)
                {
                    SendFriendMessage(botContext, botContext.OutgoingMessage);
                }
            }
        }

        protected virtual void SendMessage(BotContext botContext, string outgoingMessage)
        {
            if (String.IsNullOrEmpty(outgoingMessage) == false)
            {
                if (botContext.GroupID != null)
                {
                    SendGroupMessage(botContext, outgoingMessage);
                }
                else if (botContext.FriendID != null)
                {
                    SendFriendMessage(botContext, outgoingMessage);
                }
            }
        }
        
        /// <summary>
        /// Get the name of the game based on it's unique Steam ID (apps only, no packages)
        /// </summary>
        /// <returns> The name of the game, software, or movie or String.Empty otherwise </returns>
        protected static String GetSteamGameName(int appId)
        {
            if (appId <= 0)
            {
                return String.Empty;
            }

            String url = String.Format("http://store.steampowered.com/api/appdetails?appids={0}&filters=basic", appId.ToString());

            string result = new System.Net.WebClient().DownloadString(url);

            JObject jsonResult = JObject.Parse(result);

            string appID = appId.ToString();

            if (jsonResult == null || jsonResult[appID] == null || jsonResult[appID]["data"] == null)
            {
                return String.Empty;
            }

            JToken appData = jsonResult[appID]["data"];

            return (string)appData["name"] ?? String.Empty;
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

        public virtual bool IsValidCommand(string chatInput) { return false; }

        // This is where the logic is done for every action and the output is generated (and returned)

        protected virtual BotContext ProduceOutgoingMessage(BotContext botContext) { return new BotContext(); }

        public void Execute(BotContext botContext)
        {
            if (CreateApiService != null)
            {
                using (apiService = CreateApiService())
                {
                    SendMessage(ProduceOutgoingMessage(botContext));
                }
            }
            else
            {
                SendMessage(ProduceOutgoingMessage(botContext));
            }
        }
    }
}
