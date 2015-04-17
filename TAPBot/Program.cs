using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

using SteamKit2;

namespace TAPBot
{
    class Program
    {
        static CommandFactory commandFactory;
        static SteamClient steamClient;
        static SteamUser steamUser;
        static string username, password;
        static CallbackManager callbackManager;
        static SteamFriends steamFriends;

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect number of arguments: ");

                Console.WriteLine("Username: ");
                while ((username = Console.ReadLine()) != null) ;
                Console.WriteLine("Password: ");
                while (( password = Console.ReadLine() ) != null) ;
            }

            // instantiate commandFactory used to create bot actions

            commandFactory = new CommandFactory();

            // grab command line arguments for logging into the bot's steam account

            username = args[0];
            password = args[1];

            // initialize SteamKit related variables and a CallbackManager to handle callbacks

            steamClient = new SteamClient();
            steamUser = steamClient.GetHandler<SteamUser>();
            callbackManager = new CallbackManager(steamClient);
            steamFriends = steamClient.GetHandler<SteamFriends>();

            // register callbacks we are interested in

            new Callback<SteamClient.ConnectedCallback>(OnConnected, callbackManager);
            new Callback<SteamClient.DisconnectedCallback>(OnDisconnected, callbackManager);
            
            new Callback<SteamUser.LoggedOnCallback>(OnLoggedOn, callbackManager);
            new Callback<SteamUser.LoggedOffCallback>(OnLoggedOff, callbackManager);
            new Callback<SteamUser.AccountInfoCallback>(OnAccountInfo, callbackManager);

            new Callback<SteamFriends.ChatInviteCallback>(OnChatInvite, callbackManager);
            new Callback<SteamFriends.ChatMsgCallback>(OnChatMsg, callbackManager);
            new Callback<SteamFriends.FriendMsgCallback>(OnFriendMsg, callbackManager);

            // establish a connection with the Steam servers

            Console.WriteLine("Attempting to connect to Steam...");
            steamClient.Connect();

            while (true)
            {
                callbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
            
        }


        // Simple method to handle sending messages to the appropriate location

        private static void HandleMessage(BotAction botAction)
        {

            // First check whether the Action object has a groupID, if so then the original command was invoked in a group chat
            // If for any reason a command should result in the bot messaging a user isntead of posting it to chat
            // Then handle that in the CommandFactory by instantiating an object with a null chatID

            if (botAction.HasGroupChatID())
            {
                steamFriends.SendChatRoomMessage(botAction.GetGroupChatSteamID(), EChatEntryType.ChatMsg, botAction.GetMessage());
            }
            else
            {
                steamFriends.SendChatMessage(botAction.GetFriendSteamID(), EChatEntryType.ChatMsg, botAction.GetMessage());
            }
        }

        static void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            // Set the Steam bots status to Online

            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                Console.WriteLine("Unable to connect to Steam: {0}", callback.Result);
                System.Threading.Thread.Sleep(5000);
                steamClient.Connect();
                return;
            }
            Console.WriteLine("Connected to Steam! Logging in '{0}'...", username);
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = username,
                Password = password,
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("Disconnected from Steam");
            System.Threading.Thread.Sleep(5000);
            steamClient.Connect();
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    // If the account is SteamGuard protected, do not attempt to reconnect
                    Console.WriteLine("Unable to logon to Steam: This account is SteamGuard protected.");
                    System.Environment.Exit(0);
                }
                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);

                // Attempt to log back on

                System.Threading.Thread.Sleep(5000);
                steamClient.Disconnect();

                return;
            }

            Console.WriteLine("Successfully logged on!");

            // This bot is designed specifically for The After Party, it can join other chats, but will automatically join TAP chat.

            steamFriends.JoinChat(new SteamID(103582791434637703));
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("Logged off of Steam: {0}", callback.Result);

            System.Threading.Thread.Sleep(5000);
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = username,
                Password = password,
            });

            return;
        }

        static void OnChatMsg(SteamFriends.ChatMsgCallback callback)
        {
            // Write the incoming message to the console
            Console.WriteLine(callback.Message);
            // use the factory to get an appropriate object correlating to the action
            BotAction botAction = commandFactory.CreateBotAction(callback.Message.Trim(), callback.ChatterID.ConvertToUInt64().ToString(), callback.ChatRoomID.ConvertToUInt64().ToString());

            // Join is a special command, we could send it to the commandFactory but unlike every other action this bot performs, it needs access to some of the variables from main
            // ToDo: Possible workaround would be to have an Action that returns the ChatID as a string

            if (callback.Message.StartsWith("!join "))
            {
                JoinChat(callback.Message.Trim());
                return;
            }

            // if we successfully got an object, run the overridden Execute method and print any messages if applicable

            if (botAction != null)
            {
                botAction.Execute();

                if (botAction.IsSuccessful() && botAction.HasMessage())
                {
                    HandleMessage(botAction);
                }
            }
        }

        static void OnChatInvite(SteamFriends.ChatInviteCallback callback) 
        { 
            SteamID chatId = callback.ChatRoomID;
            Console.WriteLine("Attempting to join " + callback.ChatRoomName + "...");
            steamFriends.JoinChat(chatId);
        }

        static void JoinChat(string msg)
        {
            Regex joinCmd = new Regex(@"(!join)(\s+)(.+)");
            Regex validSteamURL = new Regex(@"(http://)?(www\.)?(steamcommunity.com/groups/)([a-zA-Z0-9_]+)");
            Match match = joinCmd.Match(msg);
            Match urlMatch;

            if (match.Success)
            {
                urlMatch = validSteamURL.Match(match.Groups[3].Value);
                Console.WriteLine(match.Groups[3].Value);

                if (urlMatch.Success)
                {
                    string html = new WebClient().DownloadString(match.Groups[3].Value);
                    Regex joinChatExpr = new Regex(@".*(joinchat/)([0-9]+).*");
                    Match htmlMatch = joinChatExpr.Match(html);

                    if (htmlMatch.Success)
                    {
                        ulong chatID = 0;
                        if (UInt64.TryParse(htmlMatch.Groups[2].Value, out chatID))
                        {
                            Console.WriteLine("Entering chat...");
                            SteamID groupChatID = new SteamID(chatID);
                            steamFriends.JoinChat(groupChatID);
                        }
                    }
                }
            }
        }

        static void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg) {

                if (callback.Message.StartsWith("!join "))
                {
                    JoinChat(callback.Message.Trim());
                    return;
                }

                BotAction botAction = commandFactory.CreateBotAction(callback.Message.ToString(), callback.Sender.ConvertToUInt64().ToString(), null);

                if (botAction != null)
                {
                    botAction.Execute();
                    if (botAction.IsSuccessful() && botAction.HasMessage())
                    {
                        HandleMessage(botAction);
                    }
                }
            }            
        }
    }
}
