using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{

    // small class to support DealEntries that is shared among many classes and can be changed by any one of them

    class DealWrapper
    {
        private DealEntry dealEntry;
        public DealEntry Deal
        {
            get { return dealEntry; }
            set { this.dealEntry = value; }
        }

        private DealWrapper() {}
        public DealWrapper(DealEntry dealEntry)
        {
            this.dealEntry = dealEntry;
        }

        // forwarding the following request to the encapsulated DealEntry object

        public DateTime Expiration
        {
            get { return dealEntry.Expiration; }
            set { dealEntry.Expiration = value; }
        }

        public int DiscountAmount
        {
            get { return dealEntry.DiscountAmount; }
            set { dealEntry.DiscountAmount = value; }
        }

        public int Price
        {
            get { return dealEntry.Price; }
            set { dealEntry.Price = value; }
        }

        public int Quantity
        {
            get { return dealEntry.Quantity; }
            set { dealEntry.Quantity = value; }
        }

        public string Name
        {
            get { return dealEntry.Name; }
            set { dealEntry.Name = value; }
        }

        public string AppID
        {
            get { return dealEntry.AppID; }
            set { dealEntry.AppID = value; }
        }

    }
}
