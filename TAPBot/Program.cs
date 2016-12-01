using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using TheAfterParty.Domain.Services;

namespace TAPBot
{
    class Program
    {
        static string username, password, apiLogin, apiPassword, apiUrl;
        static SteamBotManager steamBotManager;

        static void Main(string[] args)
        {
            username = ConfigurationManager.AppSettings.Get("SteamLogin");
            password = ConfigurationManager.AppSettings.Get("SteamPassword");
            apiLogin = ConfigurationManager.AppSettings.Get("ApiLogin");
            apiPassword = ConfigurationManager.AppSettings.Get("ApiPassword");
            apiUrl = ConfigurationManager.AppSettings.Get("ApiUrl");

            steamBotManager = new SteamBotManager();
            
            while (true)
            {
                steamBotManager.InitializeAndRun(username, password, apiUrl, apiLogin, apiPassword);
            }
        }
    }
}
