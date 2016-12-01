using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace TAPBot
{
    class UserEntry
    {
        private SteamID userId;
        public SteamID UserID
        {
            get { return userId; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private int balance;
        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public UserEntry() : this(null, 0, null) { }
        public UserEntry(string name, int balance, SteamID userId)
        {
            this.name = name;
            this.balance = balance;
            this.userId = userId;
        }
    }
}
