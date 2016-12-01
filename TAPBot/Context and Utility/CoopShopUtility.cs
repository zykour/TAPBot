using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TAPBot
{
    static class CoopShopUtility
    {
        public static UserEntry GetUserEntry(BotContext botContext)
        {
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

                            if (matchedString.CompareTo(botContext.FriendID.ConvertToUInt64().ToString().Trim()) == 0)
                            {
                                return new UserEntry(match.Groups[1].ToString().Trim(), Convert.ToInt32(match.Groups[2].ToString().Trim()), botContext.FriendID);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
