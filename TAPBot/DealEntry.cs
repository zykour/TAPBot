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

        public DealEntry() { }
        public DealEntry(int price, int quantity, string name, string appId, DateTime expiration) : base(price, quantity, name, appId)
        {
            this.expiration = expiration;
        }

        public void SetExpiration(DateTime expiration)
        {
            this.expiration = expiration;
        }

        public DateTime GetExpiration()
        {
            return expiration;
        }
    }
}
