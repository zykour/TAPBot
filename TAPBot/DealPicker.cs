using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TAPBot
{
    
    // a basic Object to assist bot actions like DailyDeal and SpinAction
    // designed specifically for TAPBot, thus contains hardcoded values
    class DealPicker
    {
        private Random rng;
        private string appId;
        private string gameName;
        private int discountPrice;
        private int discountAmnt;
        private int gameQuantity;
        private int reroll;
        LinkedList<string> steamItems;
        Regex inventoryCmd;

        public DealPicker(int seed)
        {
            rng = new Random(seed);
            reroll = 1;
            steamItems = new LinkedList<string>();
            inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)(\t)*(.+)?");
        }

        public DealPicker()
        {
            rng = new Random();
            reroll = 1;
            steamItems = new LinkedList<string>();
            inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)(\t)*(.+)?");
        }
        
        public void PickDeal()
        {
            if ( steamItems.Count == 0 )
            {
                Initialize();
            }


            int dealNumber = rng.Next(1, steamItems.Count() + 1);

            String line = steamItems.ElementAt(dealNumber);

            Match match = inventoryCmd.Match(line);
            int originalPrice = 0;

            if (match.Success)
            {
                gameQuantity = Int32.Parse(match.Groups[1].ToString().Trim());
                gameName = match.Groups[3].ToString();
                originalPrice = Int32.Parse(match.Groups[2].ToString().Trim());
                if (match.Groups[5].Success)
                {
                    appId = match.Groups[5].ToString();
                }
            }

            int discountNum = rng.Next(1, 36);

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

            discountPrice = Convert.ToInt32(Math.Floor(originalPrice * (1 - (discountAmnt / 100.00))));

            if (discountPrice == 0)
            {
                discountPrice = 1;
            }
        }

        public void Initialize()
        {
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
        }

        public void Reset()
        {
            reroll = 1;
        }

        public void Reroll(int val)
        {
            reroll = val;
        }

        public void Reroll()
        {
            reroll++;
        }

        public string GetAppID()
        {
            return (appId != null) ? appId : "";
        }

        public string GetGameName()
        {
            return gameName;
        }

        public int GetSalePrice()
        {
            return discountPrice;
        }

        public int GetGameQuantity()
        {
            return gameQuantity;
        }

        public int GetDiscountAmount()
        {
            return discountAmnt;
        }
    }
}
