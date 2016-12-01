using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAPBot
{
    class StoreEntry
    {
        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string appId;
        public string AppID
        {
            get { return appId; }
            set { appId = value; }
        }

        public StoreEntry() : this(0,0,"","") {}
        public StoreEntry(int price, int quantity, string name, string appId)
        {
            this.price = price;
            this.quantity = quantity;
            this.name = name;
            this.appId = appId;
        }

        public void SetPrice(int price)
        {
            this.price = price;
        }

        public int GetPrice()
        {
            return price;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public void SetAppID(string appId)
        {
            this.appId = appId;
        }

        public string GetAppID()
        {
            return appId;
        }

    }
}
