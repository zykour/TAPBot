using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;
using System.IO;
using System.Text.RegularExpressions;

namespace TAPBot
{
    class DirectoryAction : BotAction
    {
        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.CompareTo("!directory") == 0 ||
                chatInput.CompareTo("/directory") == 0 ||
                chatInput.CompareTo("!overview") == 0 ||
                chatInput.CompareTo("/overview") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!events") == 0 ||
                chatInput.CompareTo("/events") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!coopshop") == 0 ||
                chatInput.CompareTo("/coopshop") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!forums") == 0 ||
                chatInput.CompareTo("/forums") == 0 ||
                chatInput.CompareTo("!discussions") == 0 ||
                chatInput.CompareTo("/discussions") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!balances") == 0 ||
                chatInput.CompareTo("/balances") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!inventory") == 0 ||
                chatInput.CompareTo("/inventory") == 0)
            {
                return true;
            }

            if (chatInput.CompareTo("!sales") == 0 ||
                chatInput.CompareTo("/sales") == 0 ||
                chatInput.CompareTo("!sale") == 0 ||
                chatInput.CompareTo("/sale") == 0)
            {
                return true;
            }

            return false;
        }

        protected override string ProduceChatMessage(BotContext botContext)
        {
            if (botContext.Command.CompareTo("!directory") == 0 ||
                botContext.Command.CompareTo("/directory") == 0 ||
                botContext.Command.CompareTo("!overview") == 0 ||
                botContext.Command.CompareTo("/overview") == 0)
            {
                return "Group overview: http://steamcommunity.com/groups/TAP_Gaming";
            }

            if (botContext.Command.CompareTo("!events") == 0 ||
                botContext.Command.CompareTo("/events") == 0)
            {
                return "Group events: http://steamcommunity.com/groups/TAP_Gaming#events";
            }

            if (botContext.Command.CompareTo("!coopshop") == 0 ||
                botContext.Command.CompareTo("/coopshop") == 0)
            {
                return "The Co-op Shop: http://steamcommunity.com/groups/TAP_Gaming/discussions/3/618458030693142262/";
            }

            if (botContext.Command.CompareTo("!forums") == 0 ||
                botContext.Command.CompareTo("/forums") == 0 ||
                botContext.Command.CompareTo("!discussions") == 0 ||
                botContext.Command.CompareTo("/discussions") == 0)
            {
                return "Group forums: http://steamcommunity.com/groups/TAP_Gaming/discussions";
            }

            if (botContext.Command.CompareTo("!balances") == 0 ||
                botContext.Command.CompareTo("/balances") == 0)
            {
                return "User balances: http://steamcommunity.com/groups/TAP_Gaming/discussions/3/618458030693142262/#c618458030693142415";
            }

            if (botContext.Command.CompareTo("!inventory") == 0 ||
                botContext.Command.CompareTo("/inventory") == 0)
            {
                return "Co-op Shop inventory: http://steamcommunity.com/groups/TAP_Gaming/discussions/3/618458030693142262/#c618458030693142528";
            }

            if (botContext.Command.CompareTo("!sales") == 0 ||
                botContext.Command.CompareTo("/sales") == 0 ||
                botContext.Command.CompareTo("!sale") == 0 ||
                botContext.Command.CompareTo("/sale") == 0)
            {
                return "Co-op Shop sales: http://steamcommunity.com/groups/TAP_Gaming/discussions/3/618458030693142262/#c618458030693142805";
            }

            return "";
        }
    }
}
