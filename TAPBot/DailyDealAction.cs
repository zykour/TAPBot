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
    class DailyDealAction {
        
        protected bool hasRanToday;
        protected DateTime currentDate; // represents the date when this was last called
        protected string gameName;
        protected int gameQuantity;
        protected int gamePrice;
        protected int discountAmnt;
        protected int reroll;
        protected string results;

        public DailyDealAction(string friendId, string chatId) 
        {
            hasRanToday = false;
            currentDate = DateTime.Today;
            gameName = "";
            gameQuantity = 0;
            gamePrice = 0;
            discountAmnt = 0;
            reroll = 1;
            results = "";
        }

        
        public void Execute()
        {
            if (DateTime.Compare(DateTime.Today, currentDate) != 0)
            {
                hasRanToday = false;
                reroll = 1;
            }

            if (!hasRanToday)
            {
                firstTime();
                currentDate = DateTime.Today;
                hasRanToday = true;
            }

            string fixedQuantity = (gameQuantity == 1) ? " copy remains." : " copies remain.";
            string fixedPrice = (gamePrice == 1) ? " point (" : " points (";

            results = "The Co-op Shop Special of the Day is \"" + gameName + ".\" The discounted price is " + gamePrice + fixedPrice + discountAmnt + "%), currently " + gameQuantity + fixedQuantity;
        }

        public String GetMessage()
        {
            return results;
        }

        public void Reroll()
        {
            reroll++;
            hasRanToday = false;
        }

        public void InventoryChange()
        {
            reroll = 1;
            hasRanToday = false;
        }

        public void firstTime()
        {
            
            // General format for inventory entry is: #     #      Name
            Regex inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+(.*)");

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\zykour\Dropbox\TAP Inventory 1.txt"))
                {
                    LinkedList<string> steamItems = new LinkedList<string>();
                    string line = "";
                    int numLines = 0;

                    while ((line = sr.ReadLine()) != null)
                    {

                        numLines++;

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

                    int day = Convert.ToInt32((DateTime.Today - new DateTime(2010, 1, 1)).TotalDays);
                    Random randomGen = new Random(day + numLines * reroll);
                    int dealNumber = randomGen.Next(1, steamItems.Count() + 1);

                    line = steamItems.ElementAt(dealNumber);

                    Match match = inventoryCmd.Match(line);
                    int originalPrice = 0;

                    if (match.Success)
                    {
                        gameQuantity = Int32.Parse(match.Groups[1].ToString().Trim());
                        gameName = match.Groups[3].ToString();
                        originalPrice = Int32.Parse(match.Groups[2].ToString().Trim());
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
