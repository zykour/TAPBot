using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class DealEntry : StoreEntry
    {
        private DateTime expiration;
        public DateTime Expiration
        {
            get { return expiration; }
            set { expiration = value; }
        }

        private int discountAmount;
        public int DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }

        public DealEntry() : this(0, 0, "", "", DateTime.Today, 0) { }
        public DealEntry(int price, int quantity, string name, string appId, DateTime expiration, int discountAmount) : base(price, quantity, name, appId)
        {
            this.expiration = expiration;
            this.discountAmount = discountAmount;
        }
    }
}
