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
    class BalanceBotAction : ChatMsgBotAction
    {
        public BalanceBotAction(string friendId, string chatId) 
            : base(friendId, chatId)
        {
        }

        public override void Execute()
        {
            // General format for balances is: Name     ##      SteamID
            Regex balanceCmd = new Regex(@"([^0-9]*)([0-9]+)\s+([0-9]+)");

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\zykour\Dropbox\TAP balance.txt"))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Match match = balanceCmd.Match(line);

                        if (match.Success)
                        {
                            string matchedString = match.Groups[3].ToString().Trim();

                            if (matchedString.CompareTo(friendId.Trim()) == 0)
                            {
                                results = match.Groups[1].ToString().Trim() + ", your Co-op Shop balance is: " + match.Groups[2].ToString().Trim();
                                messageAvailable = true;
                                success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
