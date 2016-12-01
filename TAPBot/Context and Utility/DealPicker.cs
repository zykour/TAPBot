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
        private LinkedList<string> steamItems;
        private Regex inventoryCmd;
        
        public DealPicker()
        {
            steamItems = new LinkedList<string>();
            inventoryCmd = new Regex(@"([0-9]+)\s+([0-9]+)\s+([^\t]+)(\t)*(.+)?");
        }

        public DealEntry PickDeal() { return PickDeal(new Random(), 1); }
        public DealEntry PickDeal(Random random) { return PickDeal(random, 1); }
        public DealEntry PickDeal(int reroll) { return PickDeal(new Random(), reroll); }

        public DealEntry PickDeal(Random random, int reroll)
        {
            if ( steamItems.Count == 0 )
            {
                Initialize();
            }

            DealEntry dealEntry = new DealEntry();

            // Pick the first deal, if there is a reroll factor to consider, we'll reroll as many times as required by the reroll factor

            int dealNumber = random.Next(1, steamItems.Count() + 1);
            
            // By default reroll is set to 1, meaning there is no reroll unless it's more than or equal to 2

            for (int i = 1; i < reroll; i++)
            {
                dealNumber = random.Next(1, steamItems.Count() + 1);
            }

            String line = steamItems.ElementAt(dealNumber - 1);

            Match match = inventoryCmd.Match(line);
            int originalPrice = 0;
            
            if (match.Success)
            {
                dealEntry.Quantity = Int32.Parse(match.Groups[1].ToString().Trim());
                dealEntry.Name = match.Groups[3].ToString();
                originalPrice = Int32.Parse(match.Groups[2].ToString().Trim());
                if (match.Groups[5].Success)
                {
                    dealEntry.AppID = match.Groups[5].ToString();
                }
            }

            int discountNum = random.Next(1, 36);

            // Uses the same logic as above for rerolling the deal, we want to make sure the discount percentage is also rerolled the same number of times

            for (int i = 1; i < reroll; i++)
            {
                discountNum = random.Next(1, 36);
            }

            if (discountNum < 13)
            {
                dealEntry.DiscountAmount = 33;
            }
            else if (discountNum < 18)
            {
                dealEntry.DiscountAmount = 40;
            }
            else if (discountNum < 26)
            {
                dealEntry.DiscountAmount = 50;
            }
            else if (discountNum < 30)
            {
                dealEntry.DiscountAmount = 66;
            }
            else if (discountNum < 35)
            {
                dealEntry.DiscountAmount = 75;
            }
            else
            {
                dealEntry.DiscountAmount = 85;
            }

            dealEntry.Price = Convert.ToInt32(Math.Floor(originalPrice * (1 - (dealEntry.DiscountAmount / 100.00))));

            if (dealEntry.Price == 0)
            {
                dealEntry.Price = 1;
            }

            dealEntry.Expiration = DateTime.Today;

            return dealEntry;
        }

        public void Initialize()
        {
            string line = "";
            steamItems = new LinkedList<string>();

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

        public int Count()
        {
            if (steamItems.Count < 1)
            {
                Initialize();
            }

            return steamItems.Count;
        }
    }
}
