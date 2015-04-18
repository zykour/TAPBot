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
    // todo: rebuild DailyDeal and SpinAction to use components of a third class
    class SpinAction : ChatBotAction
    {
        protected string gameName;
        protected string gameId;        // represents the Steam AppID for this game, if present
        protected int gameQuantity;
        protected int gamePrice;
        protected int discountAmnt;
        protected int numDeals;

        public SpinAction(string friendId, string chatId, int numDeals) : base(friendId, null)
        {
            this.numDeals = numDeals;
            gameName = "";
            gameQuantity = 0;
            gamePrice = 0;
            discountAmnt = 0;
            results = "";
            gameId = "";
        }

        public override void Execute()
        {
            Regex inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)(\t)*(.+)?");
            LinkedList<string> steamItems = new LinkedList<string>();
            string line = "";

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\zykour\Dropbox\TAP Inventory 1.txt"))
                {

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().CompareTo("Origin") == 0)
                        {
                            // Steam games show up first in the list, don't want to parse past those
                            break;
                        }

                        Match parseMatch = inventoryCmd.Match(line);

                        if (parseMatch.Success)
                        {
                            steamItems.AddFirst(line);
                        }
                    }

                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            Random randomGen = new Random();

            for (int i = 1; i <= numDeals; i++)
            {
                int dealNumber = randomGen.Next(1, steamItems.Count() + 1);

                line = steamItems.ElementAt(dealNumber);

                Match match = inventoryCmd.Match(line);
                int originalPrice = 0;

                if (match.Success)
                {
                    gameQuantity = Int32.Parse(match.Groups[1].ToString().Trim());
                    gameName = match.Groups[3].ToString();
                    originalPrice = Int32.Parse(match.Groups[2].ToString().Trim());
                    if (match.Groups[5].Success)
                    {
                        gameId = match.Groups[5].ToString();
                    }
                }

                int discountNum = randomGen.Next(1, 36);

                if (discountNum < 13)
                {
                    discountAmnt = 33;
                }
                else if (discountNum < 18)
                {
                    discountAmnt = 40;
                }
                else if (discountNum < 26)
                {
                    discountAmnt = 50;
                }
                else if (discountNum < 30)
                {
                    discountAmnt = 66;
                }
                else if (discountNum < 35)
                {
                    discountAmnt = 75;
                }
                else
                {
                    discountAmnt = 85;
                }

                gamePrice = Convert.ToInt32(Math.Floor(originalPrice * (1 - (discountAmnt / 100.00))));

                if (gamePrice == 0)
                {
                    gamePrice = 1;
                }

                results = results + gameQuantity + "\t" + gamePrice + " (" + discountAmnt + "%)\t" + gameName + "\n";
            }

            messageAvailable = true;
            success = true;
        }
    }
}
