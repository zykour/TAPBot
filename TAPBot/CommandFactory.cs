using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SteamKit2;
using TAPBot.Context_and_Utility;
using System.Configuration;
using System.Collections.Specialized;
using TheAfterParty.Domain.Services;
using TheAfterParty.Domain.Concrete;

namespace TAPBot
{

    // A factory class to return the appropriate BotAction object based on the parsed chat command, if any

    public class CommandFactory
    {
        private List<BotAction> actions;
        private SteamFriends steamFriends;
        private ApiAuthorizer ApiAuthorizer { get; set; }

        public CommandFactory(SteamFriends steamFriends, ApiAuthorizer apiAuthorizer) : this()
        {
            this.steamFriends = steamFriends;
            this.ApiAuthorizer = apiAuthorizer;
        }
        protected CommandFactory()
        {
            actions = new List<BotAction>();
        }

        private static ApiService CreateApiService()
        {
            AppIdentityDbContext context = AppIdentityDbContext.Create();
            UnitOfWork unitOfWork = new UnitOfWork(context);
            UserRepository userRepository = new UserRepository(unitOfWork);
            ListingRepository listingRepository = new ListingRepository(unitOfWork);
            AuctionRepository auctionRepository = new AuctionRepository(unitOfWork);
            ObjectiveRepository objectiveRepository = new ObjectiveRepository(unitOfWork);
            SiteRepository siteRepository = new SiteRepository(unitOfWork);

            return new ApiService(auctionRepository, userRepository, listingRepository, objectiveRepository, siteRepository, unitOfWork);
        }

        private void AddAction(BotAction action)
        {
            action.ApiAuthorizer = this.ApiAuthorizer;
            actions.Add(action);
        }

        public void Initialize()
        {
            string admins = ConfigurationManager.AppSettings.Get("Admins");
            List<String> adminList = admins.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string mods = ConfigurationManager.AppSettings.Get("Admins");
            List<String> modList = mods.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new AddBalancesObjectiveAction(CreateApiService, adminList));
            
            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new AddBalancesAction(CreateApiService, adminList));

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new ToggleActiveAction(CreateApiService, adminList));

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new AddSiteNotificationAction(CreateApiService, modList));

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new OwnsAction(CreateApiService));

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new SetPOTWAction(CreateApiService, modList));

            //------------------------------------------------------------------------

            // allows one user to transfer points to another

            AddAction(new TransferAction(CreateApiService));

            //------------------------------------------------------------------------

            // finds the balance (co-op shop points) of the invoker
            // Specific to The After Party steam group

            AddAction(new BalanceAction(CreateApiService));

            //------------------------------------------------------------------------

            // finds the balance (co-op shop points) of the invoker
            // Specific to The After Party steam group

            AddAction(new BoostedAction(CreateApiService));

            //------------------------------------------------------------------------

            // determines which member has the most co-op shop points.
            // Specific to The After Party steam group

            AddAction(new QueenAction(CreateApiService));

            //------------------------------------------------------------------------

            // rolls a number between the specified values, or if no values are specified it rolls a random number between 1-100

            AddAction(new RollAction());

            //------------------------------------------------------------------------

            // queries the TAP website for the daily deal

            AddAction(new DealAction(CreateApiService));

            //------------------------------------------------------------------------

            // queries the TAP website for the daily deal

            AddAction(new WeeklyAction(CreateApiService));

            //------------------------------------------------------------------------

            // queries the TAP website for the daily deal

            AddAction(new OtherDealAction(CreateApiService));

            //------------------------------------------------------------------------

            // a command to force the server to pick a new deal
            // Specific to The After Party steam group

            AddAction(new RerollAction(CreateApiService, adminList));

            //------------------------------------------------------------------------

            // functions for buying the daily deal, buy games by ID, and confirming these purchases

            List<BuyEntry> buyEntries = new List<BuyEntry>();

            AddAction(new BuyDealAction(CreateApiService, buyEntries));
            AddAction(new ConfirmAction(CreateApiService, buyEntries));
            AddAction(new BuyGameAction(CreateApiService, buyEntries));

            //------------------------------------------------------------------------

            // parses steamApp links to see if the specified game/app is in the co-op shop
            // Specific to The After Party steam group

            AddAction(new ParseAppIDAction(CreateApiService));

            //------------------------------------------------------------------------

            // report the player of the week of the TAP group

            AddAction(new PrincessAction(CreateApiService));

            //------------------------------------------------------------------------

            // simple action to post directory URLs to help easily navigate users to certain pages

            AddAction(new DirectoryAction());

            //------------------------------------------------------------------------

            // simple action to tell the user who invokes the command their 64-bit Steam ID

            AddAction(new IDAction());

            //------------------------------------------------------------------------

            // simple action to search the store by item name

            AddAction(new SearchAction(CreateApiService));

            //------------------------------------------------------------------------

            // simple action to search objectives by title or name text

            AddAction(new SearchObjectiveAction(CreateApiService));
        }
       
        public void ParseChatText(BotContext botContext)
        {
            foreach (BotAction action in actions)
            {
                if (action.IsValidCommand(botContext.Command.Trim().ToLower()))
                {
                    action.Execute(botContext);
                    //break;
                }
            }
        }
    }
}
