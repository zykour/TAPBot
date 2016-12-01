using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TheAfterParty.Domain.Entities;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class SearchObjectiveAction : BotAction
    {
        public SearchObjectiveAction(Func<ApiService> CreateApiService) : base(CreateApiService) { }

        protected override BotContext ProduceOutgoingMessage(BotContext botContext)
        {
            BotContext sendContext = botContext.Clone();

            string searchText = String.Empty;

            if (botContext.Command.StartsWith("!searchobjective ") || botContext.Command.StartsWith("/searchobjective "))
            {
                string searchCmd = "!searchobjective ";
                searchText = botContext.Command.Substring(searchCmd.Length).Trim();

            }
            else
            {
                string searchCmd = "!searchobj ";
                searchText = botContext.Command.Substring(searchCmd.Length).Trim();
            }
            
            if (String.IsNullOrWhiteSpace(searchText) == true)
            {
                return sendContext;
            }

            List<Objective> objectives = apiService.SearchObjectives(searchText, 6).ToList();

            bool friendMsg = objectives.Count > 1;

            StringBuilder output = new StringBuilder(String.Empty);
            string messageOverflow = "Showing partial results, please use more specific search terms.\n";
            bool overflowed = false;

            if (objectives.Count != 0)
            {
                for (int i = 0; i < objectives.Count; i++)
                {
                    if (i > 5)
                    {
                        overflowed = true;
                        break;
                    }

                    string objectiveEntry = String.Empty;

                    if (objectives.ElementAt(i).HasBoostedObjective())
                    {
                        objectiveEntry = "#" + objectives.ElementAt(i).ObjectiveID + " (" + objectives.ElementAt(i).FixedReward() + " points)* " + objectives.ElementAt(i).Title + " - \"" + objectives.ElementAt(i).ObjectiveName + "\" - " + objectives.ElementAt(i).Description + " (" + objectives.ElementAt(i).BoostedObjective.BoostAmount + "x boost)";
                    }
                    else
                    {
                        objectiveEntry = "#" + objectives.ElementAt(i).ObjectiveID + " (" + objectives.ElementAt(i).FixedReward() + " points) " + objectives.ElementAt(i).Title + " - \"" + objectives.ElementAt(i).ObjectiveName + "\" - " + objectives.ElementAt(i).Description;
                    }
                    
                    if (objectives.ElementAt(i).RequiresAdmin == true)
                    {
                        objectiveEntry = objectiveEntry + " (Monuplay)";
                    }

                    objectiveEntry += "\n";

                    if (output.Length + objectiveEntry.Length + messageOverflow.Length < 2048)
                    {
                        output.Append(objectiveEntry);
                    }
                    else
                    {
                        overflowed = true;
                        break;
                    }
                }
            }
            else
            {
                output.Append("No results found.");
            }

            if (overflowed)
            {
                sendContext.OutgoingMessage = messageOverflow + output.ToString();
            }
            else
            {
                sendContext.OutgoingMessage = output.ToString();
            }

            if (friendMsg)
            {
                sendContext.SetAsFriendMessage();
            }
            
            if (botContext.GroupID != null && friendMsg)
            {
                SendGroupMessage(botContext, "Multiple results. Results have been sent in a private message!");
            }

            return sendContext;
        }

        public override bool IsValidCommand(string chatInput)
        {
            if (chatInput.StartsWith("!searchobj ") ||
                 chatInput.StartsWith("/searchobj ") ||
                 chatInput.StartsWith("!searchobjective ") ||
                 chatInput.StartsWith("/searchobjective "))
            {
                return true;
            }

            return false;
        }
    }
}
