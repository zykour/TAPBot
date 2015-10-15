using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

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
        static string authCode, twoFactorAuth;

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
            // todo: rename to BotActionManager
            // may use a state pattern in the future to support a bot with different actions depending on state

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

            // this callback is triggered when the steam servers wish for the client to store the sentry file
            new Callback<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth, callbackManager);

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

            byte[] sentryHash = null;
            if (File.Exists("sentry.bin"))
            {
                // if we have a saved sentry file, read and sha-1 hash it
                byte[] sentryFile = File.ReadAllBytes("sentry.bin");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = username,
                Password = password,

                // in this sample, we pass in an additional authcode
                // this value will be null (which is the default) for our first logon attempt
                AuthCode = authCode,

                // if the account is using 2-factor auth, we'll provide the two factor code instead
                // this will also be null on our first logon attempt
                TwoFactorCode = twoFactorAuth,

                // our subsequent logons use the hash of the sentry file as proof of ownership of the file
                // this will also be null for our first (no authcode) and second (authcode only) logon attempts
                SentryFileHash = sentryHash,
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
            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLogonDeniedNeedTwoFactorCode;

            if (isSteamGuard || is2FA)
            {
                Console.WriteLine("This account is SteamGuard protected!");

                if (is2FA)
                {
                    Console.Write("Please enter your 2 factor auth code from your authenticator app: ");
                    twoFactorAuth = Console.ReadLine();
                }
                else
                {
                    Console.Write("Please enter the auth code sent to the email at {0}: ", callback.EmailDomain);
                    authCode = Console.ReadLine();
                }

                return;
            }


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
            // use the factory to get an appropriate object correlating to the action
            commandFactory.ParseChatText(new BotContext(callback.ChatRoomID, callback.ChatterID, callback.Message.Trim(), steamFriends));
        }

        static void OnChatInvite(SteamFriends.ChatInviteCallback callback) 
        { 
            SteamID chatId = callback.ChatRoomID;
            Console.WriteLine("Attempting to join " + callback.ChatRoomName + "...");
            steamFriends.JoinChat(chatId);
        }

        static void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {
                // use the factory to get an appropriate object correlating to the action
                commandFactory.ParseChatText(new BotContext(null, callback.Sender, callback.Message.Trim(), steamFriends));
            }            
        }

        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            Console.WriteLine("Updating sentryfile...");

            byte[] sentryHash = CryptoHelper.SHAHash(callback.Data);

            // write out our sentry file
            // ideally we'd want to write to the filename specified in the callback
            // but then this sample would require more code to find the correct sentry file to read during logon
            // for the sake of simplicity, we'll just use "sentry.bin"
            File.WriteAllBytes("sentry.bin", callback.Data);

            // inform the steam servers that we're accepting this sentry file
            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = callback.Data.Length,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });

            Console.WriteLine("Done!");
        }
    }
}
