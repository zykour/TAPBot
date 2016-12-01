using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot.Context_and_Utility
{
    class BuyEntry
    {
        public BuyEntry(string userId, int listingId, int priceAtReservation)
        {
            UserID = userId;
            ListingID = listingId;
            PriceAtReservation = priceAtReservation;
        }

        public int ListingID { get; set; }
        public string UserID { get; set; }
        public int PriceAtReservation { get; set; }
    }
}
